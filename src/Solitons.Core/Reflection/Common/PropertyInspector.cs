using System.Collections.Generic;
using System.Reflection;

namespace Solitons.Reflection.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PropertyInspector : IPropertyInspector
    {
        private readonly Dictionary<PropertyInfo, bool> _properties = new();

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
        protected abstract bool IsTargetProperty(PropertyInfo property);

        void IPropertyInspector.Inspect(object target, PropertyInfo property)
        {
            if(target is null || property is null)return;
            if (false == _properties.TryGetValue(property, out var isTargetProperty))
            {
                isTargetProperty = IsTargetProperty(property);
                _properties.Add(property, isTargetProperty);
            }

            if (isTargetProperty)
            {
                Inspect(target, property);
            }
        }
    }
}
