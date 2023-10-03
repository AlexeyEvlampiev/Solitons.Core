using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using Solitons.Configuration;
using Solitons.Net;

namespace Solitons.Common;

/// <summary>
/// Represents a named range of IP addresses.
/// </summary>
/// <remarks>
/// This class provides functionalities to define a named range of IP addresses.
/// It inherits from the <see cref="SettingsGroup"/> class and overrides specific methods to handle IP address settings.
/// </remarks>
public sealed class NamedIpRangeSettingsGroup : SettingsGroup
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private IPAddress? _endAddress;

    /// <summary>
    /// Initializes a new instance of the <see cref="NamedIpRangeSettingsGroup"/> class with default values.
    /// </summary>
    public NamedIpRangeSettingsGroup()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NamedIpRangeSettingsGroup"/> class with the specified name, start, and end IP addresses.
    /// </summary>
    /// <param name="name">The name of the IP range.</param>
    /// <param name="start">The starting IP address of the range.</param>
    /// <param name="end">The ending IP address of the range.</param>
    /// <exception cref="ArgumentException">Thrown when the name is null or whitespace.</exception>
    /// <exception cref="ArgumentNullException">Thrown when either the start or end IP address is null.</exception>
    public NamedIpRangeSettingsGroup(string name, IPAddress start, IPAddress end)
    {
        if (name.IsNullOrWhiteSpace())
            throw new ArgumentException($"IP range name is required.", nameof(name));
        Name = name;
        StartAddress = start ?? throw new ArgumentNullException(nameof(start));
        EndAddress = end ?? throw new ArgumentNullException(nameof(end));
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NamedIpRangeSettingsGroup"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="start">The start.</param>
    public NamedIpRangeSettingsGroup(string name, IPAddress start) : this(name, start, start)
    {
            
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NamedIpRangeSettingsGroup"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="start">The start.</param>
    /// <param name="end">The end.</param>
    public NamedIpRangeSettingsGroup(string name, string start, string end) 
        : this(name, IPAddress.Parse(start ?? throw new ArgumentNullException(nameof(start))), IPAddress.Parse(end ?? throw new ArgumentNullException(nameof(end))))
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NamedIpRangeSettingsGroup"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="start">The start.</param>
    public NamedIpRangeSettingsGroup(string name, string start) : this(name, start, start)
    {
            
    }
    /// <summary>
    /// Parses a string to create a new instance of the <see cref="NamedIpRangeSettingsGroup"/> class.
    /// </summary>
    /// <param name="input">The string representation of the named IP range.</param>
    /// <returns>A new instance of the <see cref="NamedIpRangeSettingsGroup"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input string is null.</exception>
    [DebuggerStepThrough]
    public static NamedIpRangeSettingsGroup Parse(string input)
    {
        if (input == null) throw new ArgumentNullException(nameof(input));
        return Parse<NamedIpRangeSettingsGroup>(input);
    }

    /// <summary>
    /// Gets or sets the name of the IP range.
    /// </summary>
    [Setting("name", 0, Pattern = "(?i)(name|id|rule|firewall)")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the starting IP address of the range.
    /// </summary>
    [Setting("start", 1, Pattern = "(?i)(start|from)")]
    public IPAddress? StartAddress { get; set; }

    /// <summary>
    /// Gets or sets the ending IP address of the range. Defaults to the start address if not set.
    /// </summary>
    [Setting("end", 2, Pattern = "(?i)(end|to)")]
    public IPAddress? EndAddress
    {
        get => _endAddress ?? StartAddress;
        set => _endAddress = value;
    }

    /// <summary>
    /// Overrides the base <see cref="SettingsGroup.SetProperty"/> method to handle IP address settings.
    /// </summary>
    /// <param name="property">The property to set.</param>
    /// <param name="value">The value to set.</param>
    protected override void SetProperty(PropertyInfo property, string value)
    {
        if (property.PropertyType == typeof(IPAddress))
        {
            var address = IPAddress.Parse(value);
            property.SetValue(this, address);
            return;
        }
        base.SetProperty(property, value);
    }

    /// <summary>
    /// Pre-processes the input string before parsing.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>The pre-processed input string.</returns>
    protected override string PreProcess(string input)
    {
        input = input
            .Replace(@"[;\s]+$", m => string.Empty)
            .Trim();
        var pattern = @"(?<=;)  \s ( (?:range \s = \s )? (?<start>@ip) \s - \s (?<end>@ip) \s $)"
            .Replace(@"\s+", m=> String.Empty)
            .Replace(@"\s", @"\s*")
            .Replace("@ip", @"[\.\d]{4,15}");
        var regex = new Regex(pattern);
        input = regex.Replace(input, m =>
        {
            var start = m.Groups["start"];
            var end = m.Groups["end"];
            return $"start={start};end={end}";
        });
        return base.PreProcess(input);
    }

    /// <summary>
    /// Performs additional validation after deserialization.
    /// </summary>
    /// <exception cref="FormatException">Thrown when either the name is missing or the start IP address is greater than the end IP address.</exception>
    protected override void OnDeserialized()
    {
        base.OnDeserialized();
        if (Name.IsNullOrWhiteSpace())
            throw new FormatException("Range name is missing.");
        if (IpAddressComparer.Default.Compare(StartAddress, EndAddress) > 0)
            throw new FormatException("Start IP < end IP");
    }
}