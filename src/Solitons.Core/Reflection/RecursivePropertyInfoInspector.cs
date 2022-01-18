using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Solitons.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class RecursivePropertyInfoInspector
    {
        private readonly TypePropertiesDictionary _properties;
        private readonly Lazy<HashSet<PropertyInfo>> _targetProperties;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        protected RecursivePropertyInfoInspector(TypePropertiesDictionary properties) 
        {
            _properties = properties ?? throw new ArgumentNullException(nameof(properties));
            _targetProperties = new Lazy<HashSet<PropertyInfo>>(() => properties
                .SelectMany(kvp=> kvp.Value)
                .Where(IsTarget)
                .ToHashSet(), LazyThreadSafetyMode.PublicationOnly);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        protected abstract void Inspect(object target, PropertyInfo property);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected abstract bool IsTarget(PropertyInfo property);
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public bool Inspect(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (_properties.ContainsPropertiesDeclaredBy(obj.GetType()))
            {
                Inspect(obj, new HashSet<object>());
                return true;
            }
            
            return false;
        }


        private void Inspect(object obj, HashSet<object> inspected)
        {
            if (obj is null ||
                inspected.Contains(obj) ||
                false == _properties.TryGetValue(obj.GetType(), out var properties))
            {
                Debug.WriteLine($"Already inspected");
                return;
            }

            foreach (var property in properties)
            {
                if (_targetProperties.Value.Contains(property))
                {
                    Inspect(obj, property);
                }
                else
                {
                    var value = property.GetValue(obj);
                    Inspect(value, inspected);
                }
            }
        }

    }
}
