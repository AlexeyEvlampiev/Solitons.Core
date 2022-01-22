﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;


namespace Solitons.Reflection.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class DeclarativePropertyInspector<T> : PropertyInspector where T : Attribute
    {
        private readonly Dictionary<PropertyInfo, T> _attributes = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        /// <param name="attribute"></param>
        protected abstract void Inspect(object target, PropertyInfo property, T attribute);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="property"></param>
        [DebuggerStepThrough]
        protected sealed override void Inspect(object target, PropertyInfo property)
        {
            if (_attributes.TryGetValue(property, out var attribute))
            {
                Inspect(target, property, attribute);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected override bool IsTargetProperty(PropertyInfo property)
        {
            var attribute = property.GetCustomAttribute<T>();
            if (attribute is null) return false;
            _attributes.Add(property, attribute);
            return true;
        }
    }
}
