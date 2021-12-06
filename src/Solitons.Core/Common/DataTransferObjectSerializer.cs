using System;
using System.Diagnostics;

namespace Solitons.Common
{
    public abstract class DataTransferObjectSerializer : IDataTransferObjectSerializer
    {
        protected DataTransferObjectSerializer(string contentType)
        {
            ContentType = contentType;
        }

        public string ContentType { get; }

        protected abstract string Serialize(object obj);

        protected abstract object Deserialize(string content, Type targetType);

        [DebuggerStepThrough]
        string IDataTransferObjectSerializer.Serialize(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return Serialize(obj);
        }

        [DebuggerStepThrough]
        object IDataTransferObjectSerializer.Deserialize(string content, Type targetType)
        {
            return Deserialize(
                content.ThrowIfNullArgument(nameof(content)), 
                targetType.ThrowIfNullArgument(nameof(targetType)));
        }
    }
}
