using Solitons.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Solitons
{
    sealed class DomainSerializer : IDomainSerializer
    {
        //TODO: Change record to struct
        sealed record SerializationKey(Type DtoType, string ContentType);

        //TODO: Change record to struct
        sealed record DeserializationKey(Guid TypeId, string ContentType);

        #region Private Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<SerializationKey, IDataTransferObjectSerializer> _serializers = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<DeserializationKey, IDataTransferObjectSerializer> _deserializers = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<Guid, Type> _dtoTypeById = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<Guid, string[]> _supportedContentTypes = new();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<Guid, IDataTransferObjectSerializer> _defaultSerializers = new();

        private readonly Dictionary<HttpEventArgsAttribute, Type> _httpEventArgs;
        #endregion

        private DomainSerializer(DomainContext domainContext)
        {
            var dtoTypes = domainContext.GetDataTransferObjectTypes();
            
            foreach (var type in dtoTypes.Keys)
            {
                if (_dtoTypeById.ContainsKey(type.GUID))
                {
                    var duplicate = _dtoTypeById[type.GUID];
                    throw new InvalidOperationException(new StringBuilder("Data Transfer Object type id conflict.")
                        .Append($" See types {duplicate} and {type}.")
                        .Append($" Make sure that all Data Transfer Objects are uniquely annotated with {typeof(GuidAttribute)}.")
                        .ToString());
                }

                _dtoTypeById.Add(type.GUID, type);
                var attributes = dtoTypes[type];

                var contentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var attribute in attributes)
                {
                    var serializer = domainContext.GetDataTransferObjectSerializer(attribute.SerializerType);
                    if(false == contentTypes.Add(serializer.ContentType))
                    {
                        throw new InvalidOperationException($"Duplicate content type declarations. See {type}");
                    }
                    _serializers.Add(new SerializationKey(type, serializer.ContentType), serializer);
                    _deserializers.Add(new DeserializationKey(type.GUID, serializer.ContentType), serializer);
                    if (attribute.IsDefault)
                    {
                        _defaultSerializers.Add(type.GUID, serializer);
                    }
                }
                _supportedContentTypes.Add(type.GUID, contentTypes.ToArray());
            }
        }

        internal static IDomainSerializer Create(DomainContext domainContext) => new DomainSerializer(domainContext);

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

        public DomainWebRequest AsDomainWebRequest(IWebRequest request)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));


            throw new NotImplementedException();
        }
    }
}
