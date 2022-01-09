using System;
using System.Reflection;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter)]
    public class StoredProcedureRequestAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public StoredProcedureRequestAttribute()
        {
            ContentType = "application/json";
        }

        public string ContentType { get; init; }
        public Type ParameterType { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static StoredProcedureRequestAttribute? Get(ParameterInfo parameter)
        {
            var attribute = parameter
                .ThrowIfNullArgument(nameof(parameter))
                .GetCustomAttribute<StoredProcedureRequestAttribute>();
            if (attribute != null)
            {
                attribute.ParameterType = parameter.ParameterType;
            }
            return attribute;
        }
    }
}
