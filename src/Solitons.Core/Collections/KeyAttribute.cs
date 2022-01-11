using System;

namespace Solitons.Collections
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class KeyAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public KeyAttribute(string name)
        {
            Name = name.ThrowIfNullOrWhiteSpaceArgument(nameof(name));
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }
    }
}
