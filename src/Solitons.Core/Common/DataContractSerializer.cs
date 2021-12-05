using System;
using System.Diagnostics;

namespace Solitons.Common
{
    public abstract class DataContractSerializer : IDataContractSerializer
    {
        protected DataContractSerializer(string contentType)
        {
            ContentType = contentType;
        }

        public string ContentType { get; }

        protected abstract string Serialize(object obj);

        protected abstract object Deserialize(string content, Type targetType);

        [DebuggerStepThrough]
        string IDataContractSerializer.Serialize(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return Serialize(obj);
        }

        [DebuggerStepThrough]
        object IDataContractSerializer.Deserialize(string content, Type targetType)
        {
            return Deserialize(
                content.ThrowIfNullArgument(nameof(content)), 
                targetType.ThrowIfNullArgument(nameof(targetType)));
        }
    }
}
