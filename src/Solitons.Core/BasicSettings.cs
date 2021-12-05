using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Solitons
{
    public abstract class BasicSettings
    {
        private readonly Lazy<Dictionary<BasicSettingAttribute, PropertyInfo>> _properties;

        protected BasicSettings()
        {
            _properties =
                new Lazy<Dictionary<BasicSettingAttribute, PropertyInfo>>(() =>
                    BasicSettingAttribute.DiscoverProperties(GetType()));
        }

        protected virtual string ToString(PropertyInfo property, object value) => value?.ToString();

        protected virtual string PreProcess(string input) => input;

        protected virtual void OnDeserialized()
        {
            var properties = _properties.Value;
            foreach (var att in properties.Keys)
            {
                if (att.IsRequired)
                {
                    var property = properties[att];
                    var value = property.GetValue(this);
                    if (value is null)
                    {
                        throw new FormatException($"{property.Name} is required.");
                    }
                }
            }
        }

        protected virtual void SetProperty(PropertyInfo property, string value)
        {
            var converter = TypeDescriptor.GetConverter(property.PropertyType);
            var propertyValue = converter.ConvertFrom(value);
            property.SetValue(this, propertyValue);
        }

        protected virtual bool Equals(PropertyInfo property, object lhs, object rhs)
        {
            if (lhs is null && rhs is null) return true;
            if (lhs is null || rhs is null) return false;
            if (property.PropertyType == typeof(string))
            {
                return string.Equals(lhs.ToString(), rhs.ToString(), StringComparison.Ordinal);
            }

            return lhs.Equals(rhs);
        }


        public sealed override string ToString()
        {
            var properties = _properties.Value;

            var parts = new List<string>();
            foreach (var pair in properties)
            {
                var (attribute, property) = (pair.Key, pair.Value);
                var value = property.GetValue(this);
                var valueString = ToString(property, value);
                if(valueString is null)continue;
                
                parts.Add($"{attribute.Name}={valueString}");
            }
            return parts.Join(";");
        }


        protected bool NameEquals(PropertyInfo property, string name)
        {
            return property != null &&
                   name != null &&
                   name.Equals(property.Name, StringComparison.Ordinal);
        }

        protected static T Parse<T>(string input) where T : BasicSettings, new()
        {
            if (input.IsNullOrWhiteSpace()) throw new ArgumentException($"Input string is required.", nameof(input));

            var settings = new T();
            input = settings.PreProcess(input);
            if (input is null) throw new NullReferenceException($"{settings.GetType()}.{nameof(settings.PreProcess)} returned null.");

            var properties = settings._properties.Value;
            var propertiesByPosition = properties
                .Where(p => p.Key.Position.HasValue)
                .ToDictionary(p => p.Key.Position.GetValueOrDefault(), p => p.Value);
            
            
            var equations = Regex.Split(input, @";");
            var equationRegex = new Regex(@"\s*(?:(?<lhs>\w+)\s*[=])?\s*(?<rhs>.+?)\s*$");
            var position = 0;
            foreach (var equation in equations)
            {
                var match = equationRegex.Match(equation);
                //TODO: add exception message 
                if (!match.Success) throw new FormatException();
                var sides = Regex.Split(equation, @"(?=[=])(?<=^\s*\w+\s*)=");
                var (lhs, rhs) = (match.Groups["lhs"].Value.Trim(), match.Groups["rhs"].Value);
                var matchedAttributes = properties
                    .Where(p=> p.Key.NameRegex.IsMatch(lhs))
                    .Select(p=> p.Key)
                    .ToList();
                if (matchedAttributes.Count > 1)
                {
                    //...
                    throw new FormatException();
                }
                if (lhs.IsNullOrWhiteSpace())
                {
                    if (propertiesByPosition.TryGetValue(position, out var positionedProperty))
                    {
                        settings.SetProperty(positionedProperty, rhs);
                    }
                    else
                    {
                        throw new FormatException($"Property name is missing at position {position}.");
                    }
                }
                else if(matchedAttributes.Count == 1)
                {
                    var attribute = matchedAttributes.Single();
                    var property = properties[attribute];
                    settings.SetProperty(property, rhs);
                }
                else
                {
                    throw new FormatException($"Unexpected key. Key: {lhs}");
                }

                position++;
            }

            settings.OnDeserialized();
            return settings;
        }

        public sealed override bool Equals(object? obj)
        {
            if (obj is null) return false;
            if (obj.GetType() != GetType()) return false;
            var properties = _properties
                .Value
                .Select(pair=> pair.Value);

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
    }

    
}
