using System;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.ReturnValue)]
    public class StoredProcedureResponseAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public StoredProcedureResponseAttribute()
        {
            ContentType = "application/json";
        }

        public string ContentType { get; init; }
        public Type AsyncResultType { get; private set; }

        internal static StoredProcedureResponseAttribute? Get(MethodInfo method, IDomainSerializer serializer)
        {
            var returnType = method
                .ThrowIfNullArgument(nameof(method))
                .ReturnType;
            
            if (typeof(Task).IsAssignableFrom(returnType))
            {
                if (returnType.IsGenericType)
                {
                    var asyncResultType = returnType.GetGenericArguments().Single();
                    var responseAttribute = method
                        .ReturnTypeCustomAttributes
                        .GetCustomAttributes(false)
                        .OfType<StoredProcedureResponseAttribute>()
                        .SingleOrDefault();
                    if (responseAttribute is null)
                    {
                        if (serializer.CanSerialize(asyncResultType, out var contentType))
                        {
                            return new StoredProcedureResponseAttribute()
                            {
                                AsyncResultType = asyncResultType, 
                                ContentType = contentType
                            };
                        }

                        var guidAtt = asyncResultType.GetCustomAttribute<GuidAttribute>();
                        throw new InvalidOperationException(new StringBuilder("Stored procedure response type cannot be serialized.")
                            .Append($" {asyncResultType} serialization is not supported.")
                            .Append(guidAtt is null ? $" Did you forget {typeof(GuidAttribute)} declaration?" : string.Empty)
                            .ToString());
                    }

                    responseAttribute.AsyncResultType = asyncResultType;
                    if (false == serializer.CanSerialize(asyncResultType, responseAttribute.ContentType))
                    {
                        var guidAtt = asyncResultType.GetCustomAttribute<GuidAttribute>();
                        throw new InvalidOperationException(new StringBuilder("Required content type serialization is not supported.")
                            .Append($" {responseAttribute.AsyncResultType} cannot be serialized applying the '{responseAttribute.ContentType}' content type formatting.")
                            .Append(guidAtt is null ? $" Did you forget {typeof(GuidAttribute)} declaration?" : string.Empty)
                            .ToString());
                    }
                    return responseAttribute;
                }

                throw new InvalidOperationException();
            }

            throw new InvalidOperationException();
        }
    }
}
