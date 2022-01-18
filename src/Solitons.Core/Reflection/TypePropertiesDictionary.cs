using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Solitons.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class TypePropertiesDictionary : IReadOnlyDictionary<Type,IReadOnlyList<PropertyInfo>>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<Type, IReadOnlyList<PropertyInfo>> _propertiesByType;

        internal TypePropertiesDictionary(IEnumerable<PropertyInfo> properties)
        {
            _propertiesByType = properties
                .ToLookup(p => p.DeclaringType)
                .ToDictionary(
                    l => l.Key, 
                    l => (IReadOnlyList<PropertyInfo>)l.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        /// <param name="bindingFlags"></param>
        public TypePropertiesDictionary(IEnumerable<Type> types, BindingFlags bindingFlags)
            : this(RecursivePropertyInfoEnumerator.GetProperties(types, bindingFlags))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bindingFlags"></param>
        public TypePropertiesDictionary(Type type, BindingFlags bindingFlags)
            : this(RecursivePropertyInfoEnumerator.GetProperties(type, bindingFlags))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        public TypePropertiesDictionary(IEnumerable<Type> types)
            : this(RecursivePropertyInfoEnumerator.GetProperties(types))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public TypePropertiesDictionary(Type type)
            : this(RecursivePropertyInfoEnumerator.GetProperties(type))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="bindingFlags"></param>
        public TypePropertiesDictionary(IEnumerable<Assembly> assemblies, BindingFlags bindingFlags)
            : this(RecursivePropertyInfoEnumerator.GetProperties(assemblies, bindingFlags))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        public TypePropertiesDictionary(IEnumerable<Assembly> assemblies)
            : this(RecursivePropertyInfoEnumerator.GetProperties(assemblies))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="bindingFlags"></param>
        public TypePropertiesDictionary(Assembly assembly, BindingFlags bindingFlags)
            : this(RecursivePropertyInfoEnumerator.GetProperties(assembly, bindingFlags))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        public TypePropertiesDictionary(Assembly assembly)
            : this(RecursivePropertyInfoEnumerator.GetProperties(assembly))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool ContainsPropertiesDeclaredBy(Type type) => _propertiesByType.ContainsKey(type);

        public IEnumerator<KeyValuePair<Type, IReadOnlyList<PropertyInfo>>> GetEnumerator()
        {
            return _propertiesByType.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_propertiesByType).GetEnumerator();
        }

        public int Count => _propertiesByType.Count;

        public bool ContainsKey(Type key)
        {
            return _propertiesByType.ContainsKey(key);
        }

        public bool TryGetValue(Type key, out IReadOnlyList<PropertyInfo> value)
        {
            return _propertiesByType.TryGetValue(key, out value);
        }

        public IReadOnlyList<PropertyInfo> this[Type key] => _propertiesByType[key];

        public IEnumerable<Type> Keys => ((IReadOnlyDictionary<Type, IReadOnlyList<PropertyInfo>>)_propertiesByType).Keys;

        public IEnumerable<IReadOnlyList<PropertyInfo>> Values => ((IReadOnlyDictionary<Type, IReadOnlyList<PropertyInfo>>)_propertiesByType).Values;
    }
}
