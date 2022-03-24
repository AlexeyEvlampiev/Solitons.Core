using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using Solitons.Data.Common;

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
            public IMediaTypeSerializer DefaultSerializer { get; set; }
            public HashSet<string> SupportedContentTypes { get; }
        }


        sealed class BasicDataTransferPackage : DataTransferPackageWriter, IDataTransferPackage
        {
            private readonly Dictionary<string, string> _data;

            public BasicDataTransferPackage() 
                : this(new Dictionary<string, string>())
            {
            }

            private BasicDataTransferPackage(Dictionary<string, string> data)
            {
                _data = data;
            }

            public string ContentType
            {
                get => _data.TryGetValue("sys.contentType", out var value) ? value : "text/plain";
                set => _data["sys.contentType"] = value.DefaultIfNullOrWhiteSpace("text/plain").Trim();
            }

            public Guid TypeId
            {
                get => _data.TryGetValue("sys.tid", out var value) ? Guid.TryParse(value, out var guid) ? guid : Guid.Empty : Guid.Empty;
                set => _data["sys.tid"] = value.ThrowIfEmpty(()=> new InvalidOperationException("Type Id is required")).ToString("N");
            }

            public byte[] Content
            {
                get => _data.TryGetValue("sys.body", out var value) ? Convert.FromBase64String(value) : Array.Empty<byte>();
                set => _data["sys.body"] = value
                    .ThrowIfNull(() => new InvalidOperationException("Content is required"))
                    .ToBase64String();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public bool TryGetProperty(string key, out string? value)
            {
                return _data.TryGetValue($"user.{key}", out value);
            }

            protected override void SetContent(byte[] content)
            {
                Content = content;
            }

            protected override void SetContentType(string contentType)
            {
                ContentType = contentType;
            }

            protected override void SetTypeGuid(Guid guid)
            {
                TypeId = guid;
            }


            protected override void SetProperty(string key, string value)
            {
                _data[$"user.{key}"] = value;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString() => JsonSerializer.Serialize(_data);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="package"></param>
            /// <returns></returns>
            public static BasicDataTransferPackage Parse(string package)
            {
                return new BasicDataTransferPackage(JsonSerializer
                    .Deserialize<Dictionary<string, string>>(package)
                    .ThrowIfNull(() => new InvalidOperationException()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Flags]
        protected enum DataContractSerializerBehaviour
        {
            /// <summary>
            /// 
            /// </summary>
            Default = 0,

            /// <summary>
            /// 
            /// </summary>
            RequireGuidAnnotation = 1
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


            var key = new SerializerKey(type.GUID, serializer.ContentType);
            var value = new SerializerValue(type, serializer);
            _serializers[key] = value;
            if (_metadata.TryGetValue(type.GUID, out var metadata) == false)
            {
                metadata = new Metadata(type, serializer);
                _metadata.Add(type.GUID, metadata);
            }

            metadata.SupportedContentTypes.Add(serializer.ContentType);
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
                contentType = metadata.DefaultSerializer.ContentType;
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
                contentType = metadata.DefaultSerializer.ContentType;
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
        public object Deserialize(Type targetType, string contentType, string content)
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
        public T Deserialize<T>(string contentType, string content) => (T)Deserialize(typeof(T), contentType, content);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="contentType"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public object Deserialize(Guid typeId, string contentType, string content)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="writer"></param>
        public void Pack(object dto, IDataTransferPackageWriter writer)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (writer == null) throw new ArgumentNullException(nameof(writer));
            var content = Serialize(dto, out var contentType).ToUtf8Bytes();
            writer.SetContentType(contentType);
            writer.SetTypeGuid(dto.GetType().GUID);
            writer.SetContent(content);
            writer.SetProperty("type", dto.GetType().Name);
            writer.SetProperty("createdUtc", DateTime.UtcNow.ToString("O"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public string Pack(object dto)
        {
            var writer = new BasicDataTransferPackage();
            Pack(dto, writer);
            return writer.ToString()!
                .ThrowIfNullOrWhiteSpace(()=> new InvalidOperationException());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public object Unpack(string package)
        {
            var obj = BasicDataTransferPackage.Parse(package);
            return Deserialize(obj.TypeId, obj.ContentType, obj.Content.ToUtf8String());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetSupportedTypes() => _metadata.Values.Select(metadata => metadata.DataContractType);
    }
}
