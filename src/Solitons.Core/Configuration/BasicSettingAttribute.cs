using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Solitons.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class BasicSettingAttribute : Attribute
    {
        private Regex? _nameRegex;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicSettingAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException">name</exception>
        /// <exception cref="ArgumentException">Name is required. - name</exception>
        public BasicSettingAttribute(string name)
        {
            Name = (name ?? throw new ArgumentNullException(nameof(name))).Trim();
            if (Name.IsNullOrWhiteSpace()) throw new ArgumentException($"Name is required.", nameof(name));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicSettingAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="position">The position.</param>
        public BasicSettingAttribute(string name, int position) : this(name)
        {
            Position = position;
        }

        /// <summary>
        /// 
        /// </summary>
        public int? Position { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Pattern
        {
            get => NameRegex.ToString();
            set => _nameRegex = value.IsNullOrWhiteSpace() ? null : new Regex(value.Trim());
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRequired { get; set; } = true;

        internal Regex NameRegex => _nameRegex ??= new Regex(Name);



        /// <summary>
        /// Returns a value that indicates whether this instance is equal to a specified object.
        /// </summary>
        /// <param name="obj">An <see cref="T:System.Object" /> to compare with this instance or <see langword="null" />.</param>
        /// <returns>
        ///   <see langword="true" /> if <paramref name="obj" /> and this instance are of the same type and have identical field values; otherwise, <see langword="false" />.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return obj is BasicSettingAttribute other &&
                   string.Equals(Name, other.Name, StringComparison.Ordinal);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => Name.GetHashCode(StringComparison.Ordinal);

        internal static Dictionary<BasicSettingAttribute, PropertyInfo> DiscoverProperties(Type type)
        {
            if (type.IsSubclassOf(typeof(BasicSettings)) == false)
                throw new ArgumentOutOfRangeException($"Expected a subtype of {typeof(BasicSettings)}", nameof(type));

            var result = new Dictionary<BasicSettingAttribute, PropertyInfo>();
            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.SetProperty))
            {
                var att = property.GetCustomAttribute<BasicSettingAttribute>();
                if (att is null) continue;
                result.Add(att, property);
            }

            return result;
        }

    }
}
