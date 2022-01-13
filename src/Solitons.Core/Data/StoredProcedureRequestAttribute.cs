using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

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
        public ParameterInfo ParameterInfo { get; private set; }

        

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
                attribute.ParameterInfo = parameter;
            }
            return attribute;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [return: NotNull]
        internal static StoredProcedureRequestAttribute Get(ParameterInfo[] parameters, IDomainSerializer serializer)
        {
            Debug.Assert(parameters != null);
            Debug.Assert(serializer != null);
            StoredProcedureRequestAttribute result = null;
            foreach (var parameter in parameters)
            {
                var att = Get(parameter);
                if(att == null)continue;
                if (result != null) 
                    throw new InvalidOperationException("Ambiguous stored procedure request parameter declaration.");
                result = att;
    
                if (false == serializer.CanSerialize(parameter.ParameterType, result.ContentType))
                {
                    var guidAtt = parameter.ParameterType.GetCustomAttribute<GuidAttribute>();
                    throw new InvalidOperationException(new StringBuilder("Required content type serialization is not supported.")
                        .Append($" {parameter.ParameterType} cannot be serialized applying the '{result.ContentType}' content type formatting.")
                        .Append(guidAtt is null ? $" Did you forget {typeof(GuidAttribute)} declaration?" : string.Empty)
                        .ToString());
                }
            }

            if (result is null)
            {
                foreach (var parameter in parameters)
                {
                    if(false == serializer.CanSerialize(parameter.ParameterType, out var contentType))continue;
                    if (result != null)
                        throw new InvalidOperationException("Ambiguous stored procedure request parameter declaration.");
                    result = new StoredProcedureRequestAttribute()
                    {
                        ContentType = contentType, 
                        ParameterInfo = parameter
                    };
                }
            }

            if (result is null)
                throw new InvalidOperationException($"Stored procedure request parameter could not be identified.");

            return result;
        }
    }
}
