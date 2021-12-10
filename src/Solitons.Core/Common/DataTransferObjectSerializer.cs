using System;
using System.Diagnostics;
using System.Runtime.Serialization;

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
            var callback = obj as ISerializationCallback;
            callback?.OnSerializing(this);
            var content = Serialize(obj);
            callback?.OnSerialized(this);
            return content;
        }

        [DebuggerStepThrough]
        object IDataTransferObjectSerializer.Deserialize(string content, Type targetType)
        {
            var obj = Deserialize(
                content.ThrowIfNullArgument(nameof(content)), 
                targetType.ThrowIfNullArgument(nameof(targetType)));
            if(obj is IDeserializationCallback callback)
                callback.OnDeserialization(this);
            return obj;
        }
    }
}
