using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Solitons.Collections;

namespace Solitons.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public static class RecursivePropertyInfoEnumerator
    {
        /// <summary>
        /// 
        /// </summary>
        public const BindingFlags DefaultBindingFlags = 
            BindingFlags.Instance | 
            BindingFlags.Public | 
            BindingFlags.GetProperty;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static IEnumerable<PropertyInfo> GetProperties(
            IEnumerable<Assembly> assemblies,
            BindingFlags bindingFlags)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
            var types = assemblies
                .Distinct()
                .SkipNulls()
                .SelectMany(a => a.GetTypes());
            return GetProperties(types, bindingFlags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<PropertyInfo> GetProperties(
            IEnumerable<Assembly> assemblies) => GetProperties(assemblies, DefaultBindingFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public static IEnumerable<PropertyInfo> GetProperties(
            Assembly assembly,
            BindingFlags bindingFlags)
        {
            if (assembly == null) throw new ArgumentNullException(nameof(assembly));
            return GetProperties(assembly.GetTypes(), bindingFlags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<PropertyInfo> GetProperties(
            Assembly assembly) => GetProperties(assembly, DefaultBindingFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetProperties(
            IEnumerable<Type> types, 
            BindingFlags bindingFlags)
        {
            var propertiesCache = new HashSet<PropertyInfo>();
            var typesCache = new HashSet<Type>();
            return types
                .SelectMany(type=> 
                    GetProperties(type, bindingFlags, typesCache.Add, propertiesCache.Add));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<PropertyInfo> GetProperties(IEnumerable<Type> types) =>
            GetProperties(types, DefaultBindingFlags);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> GetProperties(
            Type type,
            BindingFlags bindingFlags) =>
            GetProperties(FluentEnumerable.Yield(type.ThrowIfNullArgument(nameof(type))), bindingFlags);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IEnumerable<PropertyInfo> GetProperties(
            Type type) =>
            GetProperties(type, DefaultBindingFlags);


        private static IEnumerable<PropertyInfo> GetProperties(
            Type type,
            BindingFlags bindingFlags,
            Func<Type, bool> typeSelector,
            Func<PropertyInfo, bool> propertySelector)
        {
            if (false == typeSelector.Invoke(type))
            {
                yield break;
            }

            var directProperties = type.GetProperties(bindingFlags);

            foreach (var directProperty in directProperties)
            {
                if (propertySelector.Invoke(directProperty) == false) continue;

                yield return directProperty;
                if (typeSelector.Invoke(directProperty.DeclaringType) == false)
                {
                    continue;
                }

                var nestedProperties = GetProperties(
                    directProperty.DeclaringType,
                    bindingFlags,
                    typeSelector,
                    propertySelector);

                foreach (var nestedProperty in nestedProperties)
                {
                    yield return nestedProperty;
                }
            }
        }
    }
}
