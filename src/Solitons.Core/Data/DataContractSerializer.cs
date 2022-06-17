using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    sealed class DataContractSerializer : IDataContractSerializer
    {
        #region Types

        readonly struct SerializerKey
        {
            private readonly string _contentType;

            public SerializerKey(Guid typeId, string contentType)
            {
                TypeId = typeId;
                _contentType = contentType;
            }

            public Guid TypeId { get; }

            public override bool Equals(object? obj)
            {
                if (obj is SerializerKey other)
                {
                    return TypeId == other.TypeId &&
                           StringComparer.OrdinalIgnoreCase.Equals(_contentType, other._contentType);
                }

                return false;
            }

            public override int GetHashCode() => TypeId.GetHashCode();
        }

        sealed record SerializerValue(Type DataContractType, IMediaTypeSerializer Serializer);

        sealed class Metadata
        {
            public Metadata(Type dataContractType,
                IMediaTypeSerializer defaultSerializer)
            {
                DataContractType = dataContractType;
                DefaultSerializer = defaultSerializer;
                SupportedContentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            }

            public Type DataContractType { get; }
            public IMediaTypeSerializer DefaultSerializer { get; }
            public HashSet<string> SupportedContentTypes { get; }
        }


        /// <summary>
        /// Serializer builder
        /// </summary>

        #endregion

        #region Private Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<SerializerKey, SerializerValue> _serializers = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<Guid, Metadata> _metadata = new();

        #endregion


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IDataContractSerializerBuilder CreateBuilder() => new DataContractSerializerBuilder(false);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal void Register(Type type, IMediaTypeSerializer serializer)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            serializer = MediaTypeSerializerProxy.Wrap(serializer
                .ThrowIfNullArgument(nameof(serializer)));


            var key = new SerializerKey(type.GUID, serializer.TargetContentType);
            var value = new SerializerValue(type, serializer);
            _serializers[key] = value;
            if (_metadata.TryGetValue(type.GUID, out var metadata) == false)
            {
                metadata = new Metadata(type, serializer);
                _metadata.Add(type.GUID, metadata);
            }

            metadata.SupportedContentTypes.Add(serializer.TargetContentType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public bool CanSerialize(Type type, string contentType) => _serializers.ContainsKey(new SerializerKey(type.GUID, contentType));


        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public bool CanSerialize(Type type, out string? contentType)
        {
            if (_metadata.TryGetValue(type.GUID, out var metadata))
            {
                contentType = metadata.DefaultSerializer.TargetContentType;
                return true;
            }

            contentType = null;
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public bool CanDeserialize(Guid typeId, string contentType)
        {
            if (_metadata.TryGetValue(typeId, out var metadata))
            {
                return metadata.SupportedContentTypes.Contains(contentType);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanDeserialize(Type type, string contentType) => CanDeserialize(type.GUID, contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public IEnumerable<string> GetSupportedContentTypes(Guid typeId)
        {
            if (_metadata.TryGetValue(typeId, out var metadata))
            {
                return metadata.SupportedContentTypes.AsEnumerable();
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<string> GetSupportedContentTypes(Type type)
        {
            if (_metadata.TryGetValue(type.GUID, out var metadata))
            {
                return metadata.SupportedContentTypes.AsEnumerable();
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string Serialize(object obj, string contentType)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));

            var key = new SerializerKey(obj.GetType().GUID, contentType
                .ThrowIfNullOrWhiteSpaceArgument(nameof(contentType)));

            if (_serializers.TryGetValue(key, out var value))
            {
                return value.Serializer.Serialize(obj);
            }

            throw new ArgumentOutOfRangeException($"{obj.GetType()} data contract type is not supported.", nameof(obj));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public string Serialize(object obj, out string contentType)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            if (_metadata.TryGetValue(obj.GetType().GUID, out var metadata))
            {
                contentType = metadata.DefaultSerializer.TargetContentType;
                return metadata.DefaultSerializer.Serialize(obj);
            }

            throw new ArgumentOutOfRangeException($"{obj.GetType()} data contract type is not supported.", nameof(obj));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="contentType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public object Deserialize(Type targetType, string content, string contentType)
        {
            if (targetType == null) throw new ArgumentNullException(nameof(targetType));
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));
            if (content == null) throw new ArgumentNullException(nameof(content));

            var key = new SerializerKey(targetType.GUID, contentType);
            if (_serializers.TryGetValue(key, out var value))
            {
                return value.Serializer.Deserialize(content, value.DataContractType);
            }

            throw new ArgumentOutOfRangeException($"'{targetType}' data contract type is not supported.", nameof(targetType));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="contentType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public T Deserialize<T>(string content, string contentType) => (T)Deserialize(typeof(T), content, contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="contentType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public object Deserialize(Guid typeId, string content, string contentType)
        {
            if (contentType == null) throw new ArgumentNullException(nameof(contentType));
            if (content == null) throw new ArgumentNullException(nameof(content));
            var key = new SerializerKey(typeId, contentType);
            if (_serializers.TryGetValue(key, out var value))
            {
                return value.Serializer.Deserialize(content, value.DataContractType);
            }

            throw new ArgumentOutOfRangeException($"'{typeId}' data contract type is not supported.", nameof(typeId));
        }

        public DataTransferPackage Pack(object dto)
        {
            var content = Serialize(dto, out var contentType);
            var package = new DataTransferPackage(dto.GetType().GUID, content, contentType, Encoding.UTF8);
            if (dto is ITransactionArgs args)
                package.TransactionTypeId = args.TransactionTypeId;
            return package;
        }

        public object Unpack(DataTransferPackage package)
        {
            string content = package.Encoding.GetString(package.Content.ToArray());
            return Deserialize(package.TypeId, content, package.ContentType);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetSupportedTypes() => _metadata.Values.Select(metadata => metadata.DataContractType);
    }
}
