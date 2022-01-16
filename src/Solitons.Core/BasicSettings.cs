using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Solitons.Collections;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BasicSettings : IEnumerable<KeyValuePair<string, string>>
    {
        sealed record Item(PropertyInfo Property, BasicSettingAttribute Setting, DictionaryKeyAttribute DictionaryKey);

        private readonly Lazy<Item[]> _items;

        /// <summary>
        /// 
        /// </summary>
        protected BasicSettings()
        {
            _items = new Lazy<Item[]>(() =>
            {
                var items = new List<Item>();
                var attributes = new List<Attribute>();
                foreach (var property in GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty))
                {
                    attributes.Clear();
                    attributes.AddRange(property.GetCustomAttributes());
                    var setting = attributes.OfType<BasicSettingAttribute>().SingleOrDefault();
                    if (setting is null) continue;
                    var key = attributes.OfType<DictionaryKeyAttribute>().SingleOrDefault() ?? new DictionaryKeyAttribute(setting.Name);
                    items.Add(new Item(property, setting, key));
                }

                return items.ToArray();
            });
        }

        protected virtual string ToString(PropertyInfo property, object value) => value?.ToString();

        protected virtual string PreProcess(string input) => input;

        protected virtual void OnDeserialized()
        {
            var items = _items.Value;
            foreach (var item in items)
            {
                var (property, setting, key) = (item.Property, item.Setting, Key: item.DictionaryKey);
                if (setting.IsRequired)
                {
                    var value = property.GetValue(this);
                    if (value is null)
                    {
                        throw new FormatException($"{property.Name} is required.");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="value"></param>
        protected virtual void SetProperty(PropertyInfo property, string value)
        {
            var converter = TypeDescriptor.GetConverter(property.PropertyType);
            var propertyValue = converter.ConvertFrom(value);
            property.SetValue(this, propertyValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
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
            var items = _items.Value;

            var parts = new List<string>();
            foreach (var item in items)
            {
                var (property, setting, key) = (item.Property, item.Setting, Key: item.DictionaryKey);
                var value = property.GetValue(this);
                var valueString = ToString(property, value);
                if(valueString is null)continue;
                
                parts.Add($"{setting.Name}={valueString}");
            }
            return parts.Join(";");
        }

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();


        protected bool NameEquals(PropertyInfo property, string name)
        {
            return property != null &&
                   name != null &&
                   name.Equals(property.Name, StringComparison.Ordinal);
        }

        protected static T Parse<T>(string input) where T : BasicSettings, new()
        {
            if (input.IsNullOrWhiteSpace()) throw new ArgumentException($"Input string is required. {GetSynopsis<T>()}", nameof(input));

            var settings = new T();
            input = settings.PreProcess(input);
            if (input is null) throw new NullReferenceException($"{settings.GetType()}.{nameof(settings.PreProcess)} returned null.");

            var properties = settings._items.Value;
            var propertiesByPosition = properties
                .Where(p => p.Setting.Position.HasValue)
                .ToDictionary(p => p.Setting.Position.GetValueOrDefault(), p => p.Property);
            
            
            var equations = Regex
                .Split(input, @";")
                .Skip(string.IsNullOrWhiteSpace);

            var equationRegex = new Regex(@"\s*(?:(?<lhs>\w+)\s*[=])?\s*(?<rhs>.+?)\s*$");
            var position = 0;
            foreach (var equation in equations)
            {
                var match = equationRegex.Match(equation);
                //TODO: add exception message 
                if (!match.Success) throw new FormatException();
                var sides = Regex.Split(equation, @"(?=[=])(?<=^\s*\w+\s*)=");
                var (lhs, rhs) = (match.Groups["lhs"].Value.Trim(), match.Groups["rhs"].Value);
                var matchedItems = properties
                    .Where(p=> p.Setting.NameRegex.IsMatch(lhs))
                    .ToList();
                if (matchedItems.Count > 1)
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

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            foreach (var item in _items.Value)
            {
                var value = item.Property.GetValue(this, Array.Empty<object>())?.ToString();
                if(value.IsNullOrWhiteSpace())continue;
                var key = item.DictionaryKey.Name;
                yield return KeyValuePair.Create(key, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected static string GetSynopsis<T>() where T : BasicSettings
        {
            var metadata = BasicSettingAttribute.DiscoverProperties(typeof(T));
            return metadata.Keys
                .Select(att => $"{att.Name}={{{att.Name}}}")
                .Join(";");
        }
    }

    
}
