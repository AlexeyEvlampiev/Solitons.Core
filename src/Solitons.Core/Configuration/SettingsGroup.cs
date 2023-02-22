using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Solitons.Collections;

namespace Solitons.Configuration;

/// <summary>
/// Represents a group of settings.
/// </summary>
/// <remarks>
/// The serialization format is a plain-text semicolon-separated list of key-value pairs constructed for properties annotated with <see cref="SettingAttribute"/>.
/// Use the <see cref="SettingsGroup.ToString"/> method for serialization. Use the <see cref="Parse{T}"/> generic parser method to implement custom deserialization.
/// </remarks>
/// <seealso cref="SettingAttribute"/>
public abstract class SettingsGroup : IEnumerable<KeyValuePair<string, string>>
{
    sealed record Item(PropertyInfo Property, SettingAttribute Setting, DictionaryKeyAttribute DictionaryKey);

    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly Lazy<Item[]> _items;

    /// <summary>
    /// 
    /// </summary>
    protected SettingsGroup()
    {
        _items = new Lazy<Item[]>(() =>
        {
            var items = new List<Item>();
            var attributes = new List<Attribute>();
            foreach (var property in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty))
            {
                attributes.Clear();
                attributes.AddRange(property.GetCustomAttributes());
                var setting = attributes.OfType<SettingAttribute>().SingleOrDefault();
                if (setting is null) continue;
                var key = attributes.OfType<DictionaryKeyAttribute>().SingleOrDefault() ?? new DictionaryKeyAttribute(setting.Name);
                items.Add(new Item(property, setting, key));
            }

            return items.ToArray();
        });
    }

    /// <summary>
    /// Converts a property value to a string representation.
    /// </summary>
    /// <param name="property">The property to convert.</param>
    /// <param name="value">The value to convert.</param>
    /// <returns>The string representation of the property value.</returns>
    protected virtual string? ToString(PropertyInfo property, object? value) => value?.ToString();

    /// <summary>
    /// Pre-processes the input string before parsing.
    /// </summary>
    /// <param name="input">The input string.</param>
    /// <returns>The pre-processed input string.</returns>
    protected virtual string PreProcess(string input) => input;

    /// <summary>
    /// Performs additional deserialization logic after parsing.
    /// </summary>
    /// <exception cref="FormatException">Thrown if a required property is missing.</exception>
    protected virtual void OnDeserialized()
    {
        var items = _items.Value;
        foreach (var item in items)
        {
            var (property, setting) = (item.Property, Setting: item.Setting);
            if (setting.IsRequired)
            {
                var value = property.GetValue(this);
                var isMissing = value is null;
                isMissing |= property.PropertyType == typeof(string) && string.IsNullOrWhiteSpace(value as string);
                if (isMissing)
                {
                    throw new FormatException($"{property.Name} is required.");
                }
            }
        }
    }

    /// <summary>
    /// Sets the value of a property.
    /// </summary>
    /// <param name="property">The property to set.</param>
    /// <param name="value">The value to set.</param>
    protected virtual void SetProperty(PropertyInfo property, string value)
    {
        var converter = TypeDescriptor.GetConverter(property.PropertyType);
        var propertyValue = converter.ConvertFrom(value);
        property.SetValue(this, propertyValue);
    }

    /// <summary>
    /// Determines whether two property values are equal.
    /// </summary>
    /// <param name="property">The property to compare.</param>
    /// <param name="lhs">The first value to compare.</param>
    /// <param name="rhs">The second value to compare.</param>
    /// <returns>true if the values are equal; otherwise, false.</returns>
    protected virtual bool Equals(PropertyInfo property, object? lhs, object? rhs)
    {
        if (lhs is null && rhs is null) return true;
        if (lhs is null || rhs is null) return false;
        if (property.PropertyType == typeof(string))
        {
            return string.Equals(lhs.ToString(), rhs.ToString(), StringComparison.Ordinal);
        }

        return lhs.Equals(rhs);
    }


    [DebuggerStepThrough]
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();



    /// <summary>
    /// Parses a plain-text semicolon-separated list of key-value pairs into a new instance of the specified <see cref="SettingsGroup"/> type.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="SettingsGroup"/> to parse.</typeparam>
    /// <param name="input">The input string to parse.</param>
    /// <returns>A new instance of the <see cref="SettingsGroup"/> type.</returns>
    /// <exception cref="ArgumentException">Thrown if the input string is null or whitespace.</exception>
    /// <exception cref="NullReferenceException">Thrown if the <see cref="PreProcess"/> method returns null.</exception>
    /// <exception cref="FormatException">Thrown if the input string is not in the correct format or a required property is missing.</exception>
    protected static T Parse<T>(string input) where T : SettingsGroup
    {
        if (input.IsNullOrWhiteSpace()) throw new ArgumentException($"Input string is required. {GetSynopsis<T>()}", nameof(input));

        var settings = (T?)Activator.CreateInstance(typeof(T), true)!;
        if (settings is null)
        {
            throw new InvalidOperationException($"{typeof(T)} parameterless constructor is missing");
        }

        input = settings.PreProcess(input);
        if (input is null) throw new NullReferenceException($"{settings.GetType()}.{nameof(settings.PreProcess)} returned null.");

        var properties = settings._items.Value;
        var propertiesByPosition = properties
            .Where(p => p.Setting.Position.HasValue)
            .ToDictionary(p => p.Setting.Position.GetValueOrDefault(), p => p.Property);
            
            
        var equations = Regex
            .Split(input, @";")
            .Skip(string.IsNullOrWhiteSpace);

        var equationRegex = new Regex(
            @"\s*(?:(?<lhs>@key)\s*[=])?\s*(?<rhs>.+?)\s*$"
                .Replace("@key", @"[^\s=](?:\s*-?\s*[^\s=])*"));
        var position = 0;
        foreach (var equation in equations)
        {
            var match = equationRegex.Match(equation);
            //TODO: add exception message 
            if (!match.Success) throw new FormatException();
            var (lhs, rhs) = (match.Groups["lhs"].Value.Trim(), match.Groups["rhs"].Value);
            var matchedItems = properties
                .Where(p=> p.Setting.NameRegex.IsMatch(lhs))
                .ToList();
            if (matchedItems.Count > 1)
            {
                var csv = matchedItems.Select(i => i.Setting.Name).Join(",");
                var message = new StringBuilder("Ambigous setting pattern declaration.")
                    .Append($" The '{lhs}' setting is matched by multiple setting patterns.")
                    .Append($" See settings {csv}.")
                    .ToString();
                throw new FormatException(message);
            }
            if (lhs.IsNullOrWhiteSpace())
            {
                if (propertiesByPosition.TryGetValue(position, out var positionedProperty))
                {
                    settings.SetProperty(positionedProperty, rhs);
                }
                else
                {
                    throw new FormatException($"Property name is missing at position {position}. {GetSynopsis<T>()}");
                }
            }
            else if(matchedItems.Count == 1)
            {
                var item = matchedItems.Single();
                var property = item.Property;
                settings.SetProperty(property, rhs);
            }
            else
            {
                throw new FormatException($"Unexpected key. Key: {lhs}. {GetSynopsis<T>()}");
            }

            position++;
        }

        settings.OnDeserialized();
        return settings;
    }

    /// <summary>
    /// Returns an enumerator that iterates through the key-value pairs in the settings group.
    /// </summary>
    /// <returns>An enumerator that can be used to iterate through the key-value pairs in the settings group.</returns>

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        foreach (var item in _items.Value)
        {
            var value = item.Property.GetValue(this, Array.Empty<object>())?.ToString();
            if(value.IsNullOrWhiteSpace())continue;
            var key = item.DictionaryKey.Name;
            yield return KeyValuePair.Create(key, value!);
        }
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>

    public sealed override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        var properties = _items
            .Value
            .Select(item=> item.Property);

        foreach (var p in properties)
        {
            var lhs = p.GetValue(this);
            var rhs = p.GetValue(obj);
            if (lhs is null && rhs is null) continue;
            if (lhs is null || rhs is null) return false;
            if(Equals(p, lhs, rhs))continue;
            return false;
        }

        return true;
    }


    /// <summary>
    /// Returns a hash code for this settings group.
    /// </summary>
    /// <returns>A hash code for this settings group.</returns>
    public override int GetHashCode()
    {
        var properties = _items
            .Value
            .Select(item => item.Property);

        long sum = 0;
        foreach (var p in properties)
        {
            sum += p.GetValue(this)?.GetHashCode() ?? 0;
        }

        return sum.GetHashCode();
    }

    /// <summary>
    /// Returns a string representation of this settings group.
    /// </summary>
    /// <returns>A string representation of this settings group.</returns>
    public sealed override string ToString()
    {
        var items = _items.Value;
        var parts = new List<string>();
        foreach (var item in items)
        {
            var (property, setting) = (item.Property, Setting: item.Setting);
            var value = property.GetValue(this);
            var valueString = ToString(property, value);
            if (valueString is null) continue;

            parts.Add($"{setting.Name}={valueString}");
        }
        return parts.Join(";");
    }


    /// <summary>
    /// Gets the synopsis for the specified <see cref="SettingsGroup"/> type.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="SettingsGroup"/>.</typeparam>
    /// <returns>A string representing the synopsis for the specified <see cref="SettingsGroup"/> type.</returns>

    protected static string GetSynopsis<T>() where T : SettingsGroup
    {
        var metadata = SettingAttribute.DiscoverProperties(typeof(T));
        return metadata.Keys
            .Select(att => $"{att.Name}={{{att.Name}}}")
            .Join(";");
    }
}