using System;

namespace Solitons.Collections
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class DictionaryKeyAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public DictionaryKeyAttribute(string name)
        {
            Name = ThrowIf.ArgumentNullOrWhiteSpace(name, "Name is required", nameof(name));
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }
    }
}
