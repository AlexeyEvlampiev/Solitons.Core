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

        private readonly Dictionary<SerializationKey, IDataContractSerializer> _serializers = new();
        private readonly Dictionary<DeserializationKey, IDataContractSerializer> _deserializers = new();
        private readonly Dictionary<Guid, Type> _dtoTypeById = new();
        private readonly Dictionary<Guid, string[]> _supportedContentTypes = new();
        private readonly Dictionary<Guid, IDataContractSerializer> _defaultSerializers = new();

        private DomainSerializer(Domain domain)
        {
            var dtoTypes = domain.GetDataTransferObjectTypes();
            foreach (var type in dtoTypes)
            {
                _dtoTypeById.Add(type.GUID, type);
                var attributes = domain.GetDataTransferAttributes(type);
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

        public static IDomainSerializer Create(Domain domain) => new DomainSerializer(domain);

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

        private string Serialize(object dto, string contentType)
        {
            if (_serializers.TryGetValue(new SerializationKey(dto.GetType(), contentType), out var callback))
            {
                return callback.Serialize(dto);
            }

            throw new NotSupportedException($"'{contentType}' content type is not suported for {dto.GetType()}");
        }

        public string Serialize(object dto, out string contentType)
        {
            if(_defaultSerializers.TryGetValue(dto.GetType().GUID, out var serializer))
            {
                contentType = serializer.ContentType;
                return serializer.Serialize(dto);
            }

            throw new ArgumentException($"Required a domain annotated DTO type. Actual: {dto.GetType()}");
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
        string IDomainSerializer.Serialize(object dto, string contentType)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            return Serialize(dto, contentType);
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
