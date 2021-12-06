using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solitons
{
    sealed class DomainSerializer : IDomainSerializer
    {
        sealed record SerializationKey(Type DtoType, string ContentType);
        sealed record DeserializationKey(Guid TypeId, string ContentType);

        private readonly Dictionary<SerializationKey, IDataTransferObjectSerializer> _serializers = new();
        private readonly Dictionary<DeserializationKey, IDataTransferObjectSerializer> _deserializers = new();
        private readonly Dictionary<Guid, Type> _dtoTypeById = new();
        private readonly Dictionary<Guid, string[]> _supportedContentTypes = new();
        private readonly Dictionary<Guid, IDataTransferObjectSerializer> _defaultSerializers = new();

        private DomainSerializer(DomainContext domainContext)
        {
            var dtoTypes = domainContext.GetDataTransferObjectTypes();
            foreach (var type in dtoTypes)
            {
                _dtoTypeById.Add(type.GUID, type);
                var attributes = domainContext.GetDataTransferAttributes(type);
                var defaultSerializer = attributes
                    .Where(a => a.IsDefault)
                    .Select(a => a.Serializer)
                    .Single();
                _defaultSerializers.Add(type.GUID, defaultSerializer);

                _supportedContentTypes.Add(type.GUID, attributes
                    .Select(a=> a.Serializer.ContentType)
                    .ToArray());
                foreach (var attribute in attributes)
                {
                    var serializer = attribute.Serializer;
                    _serializers.Add(new SerializationKey(type, serializer.ContentType), serializer);
                    _deserializers.Add(new DeserializationKey(type.GUID, serializer.ContentType), serializer);
                }
            }
        }

        public static IDomainSerializer Create(DomainContext domainContext) => new DomainSerializer(domainContext);

        private bool CanSerialize(object dto, string contentType) =>
            _serializers.ContainsKey(new SerializationKey(dto.GetType(), contentType));

        [DebuggerStepThrough]
        public bool CanSerialize(object dto, out string contentType)
        {
            if (dto is not null && 
                _defaultSerializers.TryGetValue(dto.GetType().GUID, out var defaultSerializer))
            {
                contentType = defaultSerializer.ContentType;
                return true;
            }

            contentType = null;
            return false;
        }

        public bool CanSerialize(Type type, string contentType)
        {
            if (type is not null &&
                contentType.IsNullOrWhiteSpace() == false &&
                _serializers.ContainsKey(new SerializationKey(type, contentType)))
            {
                return true;
            }

            contentType = null;
            return false;
        }

        public bool CanSerialize(Type type, out string contentType)
        {
            if (type is not null &&
                _defaultSerializers.TryGetValue(type.GUID, out var defaultSerializer))
            {
                contentType = defaultSerializer.ContentType;
                return true;
            }

            contentType = null;
            return false;
        }

        private bool CanDeserialize(Guid typeId, string contentType) =>
            _deserializers.ContainsKey(new DeserializationKey(typeId, contentType));

        public IEnumerable<string> GetSupportedContentTypes(Guid typeId)
        {
            return _supportedContentTypes.TryGetValue(typeId, out var contentTypes)
                ? contentTypes.AsEnumerable()
                : Array.Empty<string>();
        }

        private string Serialize(object obj, string contentType)
        {
            if (_serializers.TryGetValue(new SerializationKey(obj.GetType(), contentType), out var callback))
            {
                return callback.Serialize(obj);
            }

            throw new NotSupportedException($"'{contentType}' content type is not suported for {obj.GetType()}");
        }

        public string Serialize(object obj, out string contentType)
        {
            if(_defaultSerializers.TryGetValue(obj.GetType().GUID, out var serializer))
            {
                contentType = serializer.ContentType;
                return serializer.Serialize(obj);
            }

            throw new ArgumentException($"Required a domain annotated DTO type. Actual: {obj.GetType()}");
        }

        private object Deserialize(Guid typeId, string contentType, string content)
        {
            if (false == _dtoTypeById.TryGetValue(typeId, out var type))
            {
                throw new NotSupportedException($"Unknown DTO type. Type identifier: '{typeId}'.");
            }

            if (_deserializers.TryGetValue(new DeserializationKey(typeId, contentType), out var callback))
            {
                return callback.Deserialize(content, type);
            }

            throw new NotSupportedException($"'{contentType}' content type not supported.");
        }


        [DebuggerStepThrough]
        bool IDomainSerializer.CanSerialize(object dto, string contentType) => dto != null && CanSerialize(dto,  contentType);

        [DebuggerStepThrough]
        bool IDomainSerializer.CanDeserialize(Guid typeId, string contentType)
        {
            if (typeId == Guid.Empty || contentType.IsNullOrWhiteSpace()) return false;
            return CanDeserialize(typeId, contentType);
        }

        [DebuggerStepThrough]
        string IDomainSerializer.Serialize(object obj, string contentType)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return Serialize(obj, contentType);
        }

        [DebuggerStepThrough]
        object IDomainSerializer.Deserialize(Guid typeId, string contentType, string content)
        {
            if (typeId == Guid.Empty) throw new ArgumentException($"Type GUID is required.", nameof(typeId));
            contentType.ThrowIfNullOrWhiteSpaceArgument(nameof(contentType));
            return Deserialize(
                typeId.ThrowIfEmptyArgument(nameof(typeId)), 
                contentType.ThrowIfNullOrWhiteSpaceArgument(nameof(contentType)), 
                content);
        }
    }
}
