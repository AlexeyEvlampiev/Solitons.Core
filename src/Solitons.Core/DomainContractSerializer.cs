using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Solitons
{
    sealed class DomainContractSerializer : IDomainContractSerializer
    {
        //TODO: Change record to struct
        sealed record SerializerKey(Guid TypeId, string ContentType);
        sealed record SerializerValue(Type DtoType, IDataTransferObjectSerializer Serializer);

        sealed record DtoMetadata(            
            Type DtoType,
            IDataTransferObjectSerializer DefaultSerializer,
            HashSet<string> SupportedContentTypes);        

        #region Private Fields

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<SerializerKey, SerializerValue> _serializers = new();        

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<Guid, DtoMetadata> _metadata = new();


        

        #endregion

        private DomainContractSerializer(DomainContext context)
        {

            var dtoTypes = context.GetDataTransferObjectTypes();
            
            foreach (var type in dtoTypes.Keys)
            {
                if (_metadata.ContainsKey(type.GUID))
                {
                    var duplicate = _metadata[type.GUID];
                    throw new InvalidOperationException(new StringBuilder("Data Transfer Object type id conflict.")
                        .Append($" See types {duplicate} and {type}.")
                        .Append($" Make sure that all Data Transfer Objects are uniquely annotated with {typeof(GuidAttribute)}.")
                        .ToString());
                }
                                
                var attributes = dtoTypes[type];
                Debug.Assert(attributes.Count(a=> a.IsDefault) == 1);
                IDataTransferObjectSerializer defaultSerializer = null;

                var contentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var attribute in attributes)
                {
                    var serializer = context.GetDataTransferObjectSerializer(attribute.SerializerType);
                    if(false == contentTypes.Add(serializer.ContentType))
                    {
                        throw new InvalidOperationException($"Duplicate content type declarations. See {type}");
                    }
                    _serializers.Add(
                        new SerializerKey(type.GUID, serializer.ContentType), 
                        new SerializerValue(type, serializer));                    
                    if (attribute.IsDefault)
                    {
                        defaultSerializer = serializer;
                    }
                }
                Debug.Assert(defaultSerializer != null);
                _metadata.Add(type.GUID, new DtoMetadata(type, defaultSerializer, contentTypes));
            }
        }

        internal static IDomainContractSerializer Create(DomainContext domainContext) => new DomainContractSerializer(domainContext);

        private bool CanSerialize(object dto, string contentType) =>
            _serializers.ContainsKey(new SerializerKey(dto.GetType().GUID, contentType));

        [DebuggerStepThrough]
        public bool CanSerialize(object dto, out string contentType)
        {
            if (dto is not null && 
                _metadata.TryGetValue(dto.GetType().GUID, out var metadata))
            {
                contentType = metadata.DefaultSerializer.ContentType;
                return true;
            }

            contentType = null;
            return false;
        }

        public bool CanSerialize(Type type, string contentType)
        {
            if (type is not null &&
                contentType.IsNullOrWhiteSpace() == false &&
                _serializers.ContainsKey(new SerializerKey(type.GUID, contentType)))
            {
                return true;
            }

            contentType = null;
            return false;
        }

        public bool CanSerialize(Type type, out string contentType)
        {
            if (type is not null &&
                _metadata.TryGetValue(type.GUID, out var metadata))
            {
                contentType = metadata.DefaultSerializer.ContentType;
                return true;
            }

            contentType = null;
            return false;
        }

        private bool CanDeserialize(Guid typeId, string contentType) =>
            _serializers.ContainsKey(new SerializerKey(typeId, contentType));

        public IEnumerable<string> GetSupportedContentTypes(Guid typeId)
        {
            return _metadata.TryGetValue(typeId, out var metadata)
                ? metadata.SupportedContentTypes.AsEnumerable()
                : Array.Empty<string>();
        }

        private string Serialize(object obj, string contentType)
        {
            if (_serializers.TryGetValue(new SerializerKey(obj.GetType().GUID, contentType), out var value))
            {
                return value.Serializer.Serialize(obj);
            }

            throw new NotSupportedException($"'{contentType}' content type is not suported for {obj.GetType()}");
        }

        public string Serialize(object obj, out string contentType)
        {
            if(_metadata.TryGetValue(obj.GetType().GUID, out var metadata))
            {
                var serializer = metadata.DefaultSerializer;
                contentType = serializer.ContentType;
                return serializer.Serialize(obj);
            }

            throw new ArgumentException($"Required a domain annotated DTO type. Actual: {obj.GetType()}");
        }

        private object Deserialize(Guid typeId, string contentType, string content)
        {
            if (false == _metadata.TryGetValue(typeId, out var type))
            {
                throw new NotSupportedException($"Unknown DTO type. Type identifier: '{typeId}'.");
            }

            if (_serializers.TryGetValue(new SerializerKey(typeId, contentType), out var value))
            {
                return value.Serializer.Deserialize(content, value.DtoType);
            }

            throw new NotSupportedException($"'{contentType}' content type not supported.");
        }


        [DebuggerStepThrough]
        bool IDomainContractSerializer.CanSerialize(object dto, string contentType) => dto != null && CanSerialize(dto,  contentType);

        [DebuggerStepThrough]
        bool IDomainContractSerializer.CanDeserialize(Guid typeId, string contentType)
        {
            if (typeId == Guid.Empty || contentType.IsNullOrWhiteSpace()) return false;
            return CanDeserialize(typeId, contentType);
        }

        [DebuggerStepThrough]
        string IDomainContractSerializer.Serialize(object obj, string contentType)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));
            return Serialize(obj, contentType);
        }

        [DebuggerStepThrough]
        object IDomainContractSerializer.Deserialize(Guid typeId, string contentType, string content)
        {
            if (typeId == Guid.Empty) throw new ArgumentException($"Type GUID is required.", nameof(typeId));
            contentType.ThrowIfNullOrWhiteSpaceArgument(nameof(contentType));
            return Deserialize(
                typeId.ThrowIfEmptyArgument(nameof(typeId)), 
                contentType.ThrowIfNullOrWhiteSpaceArgument(nameof(contentType)), 
                content);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IEnumerable<Type> GetTypes() => _metadata.Values
            .Select(v => v.DtoType);
    }
}
