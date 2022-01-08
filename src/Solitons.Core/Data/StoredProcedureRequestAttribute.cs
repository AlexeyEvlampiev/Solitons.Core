using System;
using System.Reflection;

namespace Solitons.Data
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class StoredProcedureRequestAttribute : Attribute
    {
        public StoredProcedureRequestAttribute()
        {
            ContentType = "application/json";
        }

        public string ContentType { get; init; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        internal static StoredProcedureRequestAttribute? Get(ParameterInfo parameter)
        {
            return parameter
                .ThrowIfNullArgument(nameof(parameter))
                .GetCustomAttribute<StoredProcedureRequestAttribute>();
        }
    }
}
