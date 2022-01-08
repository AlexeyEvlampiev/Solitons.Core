using System;
using System.Linq;
using System.Reflection;
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

        internal static StoredProcedureResponseAttribute? Get(MethodInfo method)
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
                    responseAttribute ??= new StoredProcedureResponseAttribute();
                    responseAttribute.AsyncResultType = asyncResultType;
                    return responseAttribute;
                }

                throw new InvalidOperationException();
            }

            throw new InvalidOperationException();
        }
    }
}
