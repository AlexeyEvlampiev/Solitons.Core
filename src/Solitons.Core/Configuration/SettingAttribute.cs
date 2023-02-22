using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Solitons.Configuration;

/// <summary>
/// Annotates flat configuration set properties with metadata defining serialization and parsing rules and constraints.
/// </summary>
/// <remarks>
/// The attribute takes effect when applied on properties of <see cref="SettingsGroup"/> sub-classes.
/// </remarks>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class SettingAttribute : Attribute
{
    private Regex? _nameRegex; // backing field for the NameRegex property

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingAttribute"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <exception cref="ArgumentNullException">Thrown when the name parameter is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the name parameter is empty or contains only white space characters.</exception>
    public SettingAttribute(string name)
    {
        Name = (name ?? throw new ArgumentNullException(nameof(name))).Trim();
        if (string.IsNullOrWhiteSpace(Name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SettingAttribute"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="position">The position.</param>
    public SettingAttribute(string name, int position) : this(name)
    {
        Position = position;
    }

    /// <summary>
    /// Gets the position of the setting.
    /// </summary>
    public int? Position { get; }

    /// <summary>
    /// Gets the name of the setting.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets the regular expression pattern for the setting name.
    /// </summary>
    public string Pattern
    {
        get => NameRegex.ToString();
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _nameRegex = new Regex(Name);
            }
            else
            {
                value = value.Trim();
                _nameRegex = new Regex($"^(?:{value})$");
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether the setting is required.
    /// </summary>
    public bool IsRequired { get; set; } = true;

    /// <summary>
    /// Gets the regular expression for the setting name.
    /// </summary>
    internal Regex NameRegex => _nameRegex ?? new Regex(Name);

    /// <summary>
    /// Returns a value that indicates whether this instance is equal to a specified object.
    /// </summary>
    /// <param name="obj">An <see cref="T:System.Object" /> to compare with this instance or <see langword="null" />.</param>
    /// <returns>
    ///   <see langword="true" /> if <paramref name="obj" /> and this instance are of the same type and have identical field values; otherwise, <see langword="false" />.
    /// </returns>
    public override bool Equals(object? obj)
    {
        return obj is SettingAttribute other && string.Equals(Name, other.Name, StringComparison.Ordinal);
    }

    /// <summary>
    /// Returns a hash code for this instance.
    /// </summary>
    /// <returns>
    /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table
    /// </returns>
    public override int GetHashCode() => Name.GetHashCode(StringComparison.Ordinal);

    /// <summary>
    /// Discovers properties in the specified type that are annotated with the SettingAttribute and returns a dictionary that maps each SettingAttribute to its corresponding PropertyInfo.
    /// </summary>
    /// <param name="type">The type to discover properties from.</param>
    /// <returns>A dictionary that maps each SettingAttribute to its corresponding PropertyInfo.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the type parameter is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the type is not a subclass of SettingsGroup.</exception>
    internal static Dictionary<SettingAttribute, PropertyInfo> DiscoverProperties(Type type)
    {
        if (type.IsSubclassOf(typeof(SettingsGroup)) == false)
            throw new ArgumentOutOfRangeException($"Expected a subtype of {typeof(SettingsGroup)}", nameof(type));

        var result = new Dictionary<SettingAttribute, PropertyInfo>();
        foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty))
        {
            var att = property.GetCustomAttribute<SettingAttribute>();
            if (att is null) continue;
            result.Add(att, property);
        }

        return result;
    }

}