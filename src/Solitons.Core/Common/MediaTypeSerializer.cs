using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using Solitons.Data;

namespace Solitons.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MediaTypeSerializer : IMediaTypeSerializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        protected MediaTypeSerializer(string contentType)
        {
            ContentType = contentType;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ContentType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected abstract string Serialize(object obj);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        protected abstract object Deserialize(string content, Type targetType);

        [DebuggerStepThrough]
        string IMediaTypeSerializer.Serialize(object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            var callback = obj as ISerializationCallback;
            callback?.OnSerializing(this);
            var content = Serialize(obj);
            callback?.OnSerialized(this);
            return content;
        }

        [DebuggerStepThrough]
        object IMediaTypeSerializer.Deserialize(string content, Type targetType)
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
