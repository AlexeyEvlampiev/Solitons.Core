using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Solitons.Data.Common;

/// <summary>
/// Represents a base implementation for media-type-specific serialization and deserialization.
/// This class simplifies the creation of custom media type serializers, facilitating integration with the <see cref="IDataContractSerializer"/> interface.
/// </summary>
public abstract class MediaTypeSerializer : IMediaTypeSerializer
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MediaTypeSerializer"/> class with the specified content type.
    /// </summary>
    /// <param name="contentType">The content type this serializer can handle.</param>
    protected MediaTypeSerializer(string contentType)
    {
        TargetContentType = contentType;
    }

    /// <summary>
    /// Gets the content type that this serializer can handle.
    /// </summary>
    public string TargetContentType { get; }

    /// <summary>
    /// Serializes the given object into its string representation.
    /// </summary>
    /// <param name="obj">The object to be serialized.</param>
    /// <returns>A string representation of the serialized object.</returns>
    protected abstract string Serialize(object obj);

    /// <summary>
    /// Deserializes the provided string representation back into an object of the specified type.
    /// </summary>
    /// <param name="content">The string representation of the serialized object.</param>
    /// <param name="targetType">The type of object to deserialize into.</param>
    /// <returns>The deserialized object.</returns>
    protected abstract object? Deserialize(string content, Type targetType);

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
                ThrowIf.ArgumentNull(content, nameof(content)),
                ThrowIf.ArgumentNull(targetType, nameof(targetType)))
            .ThrowIfNull(
                $"Could not deserialize from the '{TargetContentType}' content." +
                $" Target type: {targetType}");
        if (obj is IDeserializationCallback callback)
            callback.OnDeserialization(this);
        return obj;
    }
}