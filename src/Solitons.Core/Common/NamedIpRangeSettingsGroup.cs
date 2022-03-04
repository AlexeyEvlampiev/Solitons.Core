using System;
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using Solitons.Configuration;
using Solitons.Net;

namespace Solitons.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="SettingsGroup" />
    public sealed class NamedIpRangeSettingsGroup : SettingsGroup
    {
        private IPAddress? _endAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedIpRangeSettingsGroup"/> class.
        /// </summary>
        public NamedIpRangeSettingsGroup()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamedIpRangeSettingsGroup"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <exception cref="ArgumentException">IP range name is required. - name</exception>
        /// <exception cref="ArgumentNullException">
        /// start
        /// or
        /// end
        /// </exception>
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
        /// Parses the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">input</exception>
        [DebuggerStepThrough]
        public static NamedIpRangeSettingsGroup Parse(string input)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            return Parse<NamedIpRangeSettingsGroup>(input);
        }

        /// <summary>
        /// 
        /// </summary>
        [Setting("name", 0, Pattern = "(?i)(name|id|rule|firewall)")]
        public string? Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Setting("start", 1, Pattern = "(?i)(start|from)")]
        public IPAddress? StartAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Setting("end", 2, Pattern = "(?i)(end|to)")]
        public IPAddress? EndAddress
        {
            get => _endAddress ?? StartAddress;
            set => _endAddress = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
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
        /// 
        /// </summary>
        /// <exception cref="FormatException"></exception>
        protected override void OnDeserialized()
        {
            base.OnDeserialized();
            if (Name.IsNullOrWhiteSpace())
                throw new FormatException("Range name is missing.");
            if (IpAddressComparer.Default.Compare(StartAddress, EndAddress) > 0)
                throw new FormatException("Start IP < end IP");
        }
    }
}
