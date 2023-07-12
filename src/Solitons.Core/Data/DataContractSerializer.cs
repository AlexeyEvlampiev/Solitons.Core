using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
sealed class DataContractSerializer : IDataContractSerializer
{
    #region Types

    readonly struct SerializerKey
    {
        private readonly string _contentType;

        [DebuggerNonUserCode]
        public SerializerKey(Type type, string contentType) 
            : this(type.GUID, contentType)
        {
        }

        [DebuggerNonUserCode]
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

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Dictionary<string, string> _contentTypeNames
        = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["application/json"] = "application/json",
            ["application/xml"] = "application/xml",
            ["text/html"] = "text/html",
            ["text/plain"] = "text/plain",
            ["text/csv"] = "text/csv",
        };

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private static string _typeRegistrationMethods = string.Join(", ", new[]
    {
        nameof(IDataContractSerializerBuilder.Add),
        nameof(IDataContractSerializerBuilder.AddAssemblyTypes),
    });

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Lazy<string> _registeredContentTypesCsv;


    #endregion

    internal DataContractSerializer()
    {
        _registeredContentTypesCsv = new Lazy<string>(() => _metadata.Values
            .SelectMany(v => v.SupportedContentTypes)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(_ => _, StringComparer.OrdinalIgnoreCase)
            .Join(", "));
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="serializer"></param>
    /// <exception cref="ArgumentNullException"></exception>
    internal void Register(Type type, IMediaTypeSerializer serializer)
    {
        type = ThrowIf.ArgumentNull(type, nameof(type));

        var targetContentType = _contentTypeNames
            .GetOrAdd(
                serializer.TargetContentType, 
                serializer.TargetContentType);

        serializer = ThrowIf
            .ArgumentNull(serializer, nameof(serializer))
            .Convert(MediaTypeSerializerProxy.Wrap);

        var key = new SerializerKey(type, targetContentType);
        var value = new SerializerValue(type, serializer);
        _serializers[key] = value;

        if (_metadata.TryGetValue(type.GUID, out var metadata) == false)
        {
            metadata = new Metadata(type, serializer);
            _metadata.Add(type.GUID, metadata);
        }

        metadata.SupportedContentTypes.Add(targetContentType);
    }

    private static string BuildDataTypeOutOfRangeMessage(Type type)
    {
        return new StringBuilder($"{type} data type is not registered.")
            .Append($" Use the following {typeof(IDataContractSerializerBuilder)} methods to configure the serializer: {_typeRegistrationMethods}.")
            .ToString();
    }

    private string BuildDataTypeOutOfRangeMessage(Guid typeId)
    {
        return new StringBuilder($"{typeId} data type is not registered.")
            .Append($" Use the following {typeof(IDataContractSerializerBuilder)} methods to configure the serializer: {_typeRegistrationMethods}.")
            .ToString();
    }

    private static string BuildContentTypeOutOfRangeMessage(Type type, string contentType)
    {
        return new StringBuilder($"The {contentType.Trim()} serialization is not configured for {type}.")
            .Append($" Use the following {typeof(IDataContractSerializerBuilder)} methods to configure the serializer: {_typeRegistrationMethods}.")
            .ToString();
    }


    private string BuildContentTypeOutOfRangeMessage(string contentType)
    {
        return new StringBuilder($"The {contentType.Trim()} serialization is not configured.")
            .Append($" Configured content types: {_registeredContentTypesCsv}.")
            .Append($" Use the following {typeof(IDataContractSerializerBuilder)} methods if applicable: {_typeRegistrationMethods}.")
            .ToString();
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
            contentType = _contentTypeNames[metadata.DefaultSerializer.TargetContentType];
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
            foreach (var contentType in metadata.SupportedContentTypes)
            {
                yield return contentType;
            }
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public IEnumerable<string> GetSupportedContentTypes(Type type) => 
        GetSupportedContentTypes(type.GUID);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="contentType"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public TextMediaContent Serialize(object obj, string contentType)
    {
        obj = ThrowIf
            .ArgumentNull(obj, nameof(obj))
            .Convert(dto =>
            {
                if (_metadata.ContainsKey(dto.GetType().GUID))
                {
                    return dto;
                }
                var message = BuildDataTypeOutOfRangeMessage(dto.GetType());
                throw new ArgumentOutOfRangeException(nameof(obj), message);
            });

        contentType = ThrowIf
            .ArgumentNullOrWhiteSpace(contentType, nameof(contentType))
            .Convert(ct =>
            {
                if (_contentTypeNames.TryGetValue(ct, out var actualContentType))
                {
                    return actualContentType;
                }

                string message = BuildContentTypeOutOfRangeMessage(obj.GetType(), ct);
                throw new ArgumentOutOfRangeException(nameof(contentType), message);
            });


        var key = new SerializerKey(obj.GetType(), contentType);

        if (_serializers.TryGetValue(key, out var value))
        {
            var content = value.Serializer.Serialize(obj);

            return new TextMediaContent(content, contentType);
        }

        string message = BuildContentTypeOutOfRangeMessage(obj.GetType(), contentType);
        throw new ArgumentOutOfRangeException(nameof(contentType), message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public TextMediaContent Serialize(object obj)
    {
        obj = ThrowIf.ArgumentNull(obj, nameof(obj));
        if (_metadata.TryGetValue(obj.GetType().GUID, out var metadata))
        {
            var contentType = _contentTypeNames[metadata.DefaultSerializer.TargetContentType];
            var content = metadata.DefaultSerializer.Serialize(obj);
            return new TextMediaContent(content, contentType);
        }

        var message = BuildDataTypeOutOfRangeMessage(obj.GetType());
        throw new ArgumentOutOfRangeException(nameof(obj), message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="typeId"></param>
    /// <param name="mediaContent"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public object Deserialize(Guid typeId, TextMediaContent mediaContent)
    {
        typeId = ThrowIf.ArgumentNullOrEmpty(typeId, nameof(typeId));
        if (_metadata.TryGetValue(typeId, out var metadata) == false)
        {
            throw new ArgumentOutOfRangeException(
                nameof(mediaContent),
                BuildDataTypeOutOfRangeMessage(typeId));
        }

        mediaContent = ThrowIf
            .ArgumentNull(mediaContent, nameof(mediaContent))
            .Convert(md =>
            {
                if (_contentTypeNames.TryGetValue(md.ContentType, out var actualContentType))
                {
                    return new TextMediaContent(md.Content, actualContentType);
                }

                var message = BuildContentTypeOutOfRangeMessage(md.ContentType);
                throw new ArgumentOutOfRangeException(nameof(mediaContent), message);
            });

        var key = new SerializerKey(typeId, mediaContent.ContentType);
        if (_serializers.TryGetValue(key, out var value))
        {
            return value.Serializer
                .Deserialize(mediaContent.Content, value.DataContractType);
        }

        var message = BuildContentTypeOutOfRangeMessage(
            metadata.DataContractType, 
            mediaContent.ContentType);

        throw new ArgumentOutOfRangeException(nameof(mediaContent), message);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="contentType"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public T Deserialize<T>(string content, string contentType) => (T)Deserialize(typeof(T).GUID, content, contentType);

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
        typeId = ThrowIf.ArgumentNullOrEmpty(typeId, nameof(typeId));
        if (_metadata.TryGetValue(typeId, out var metadata) == false)
        {
            throw new ArgumentOutOfRangeException(
                nameof(typeId), 
                BuildDataTypeOutOfRangeMessage(typeId));
        }

        content = ThrowIf.ArgumentNull(content, nameof(content));
        contentType = ThrowIf.ArgumentNullOrWhiteSpace(contentType, nameof(contentType));

        var key = new SerializerKey(typeId, contentType);
        if (_serializers.TryGetValue(key, out var value))
        {
            return value.Serializer.Deserialize(content, value.DataContractType);
        }

        var message = BuildContentTypeOutOfRangeMessage(
            metadata.DataContractType,
            contentType);
        throw new ArgumentOutOfRangeException(nameof(contentType), message);
    }

    public DataTransferPackage Pack(object dto)
    {
        var content = Serialize(dto);
        var package = new DataTransferPackage(dto.GetType().GUID, content, Encoding.UTF8);
        if (dto is IRemoteTriggerArgs args)
        {
            package.IntentId = args.IntentId;
        }
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

    public Type GetType(Guid dtoTypeId)
    {
        return GetTypeIfRegistered(dtoTypeId) ?? throw new ArgumentOutOfRangeException(nameof(dtoTypeId));
    }

    public Type? GetTypeIfRegistered(Guid dtoTypeId)
    {
        return _metadata.TryGetValue(dtoTypeId, out var metadata)
            ? metadata.DataContractType
            : null;
    }
}