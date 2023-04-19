using System;
using System.Diagnostics;

namespace Solitons.Data;

/// <summary>
/// Represents a serializer that supports serialization and deserialization of objects with a specified content type.
/// </summary>
public partial interface IMediaTypeSerializer
{
    /// <summary>
    /// Gets the content type that this serializer can handle.
    /// </summary>
    string TargetContentType { get; }

    /// <summary>
    /// Serializes the specified object into a string representation encoded according to the declared content type specifications (see <see cref="TargetContentType"/>).
    /// </summary>
    /// <param name="obj">The object to be serialized.</param>
    /// <returns>A string representation of the serialized object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="obj"/> is null.</exception>
    /// <exception cref="NotSupportedException">Thrown when serialization of <paramref name="obj"/> is not supported by this serializer.</exception>
    string Serialize(object obj);

    /// <summary>
    /// Deserializes the specified string representation into an object of the specified type.
    /// </summary>
    /// <param name="content">A string representation of the serialized object.</param>
    /// <param name="targetType">The type of object to deserialize into.</param>
    /// <returns>The deserialized object.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="content"/> or <paramref name="targetType"/> is not valid.</exception>
    object Deserialize(string content, Type targetType);
}


public partial interface IMediaTypeSerializer
{
    /// <summary>
    /// Default basic JSON serializer instance.
    /// </summary>
    public static readonly IMediaTypeSerializer BasicJsonSerializer = new BasicJsonMediaTypeSerializer();

    /// <summary>
    /// Default basic XML serializer instance.
    /// </summary>
    public static readonly IMediaTypeSerializer BasicXmlSerializer = new BasicXmlMediaTypeSerializer();

    /// <summary>
    /// Extends this object behaviour with additional assertions for methods and properties.
    /// </summary>
    /// <returns>A proxy instance of <see cref="IMediaTypeSerializer"/>.</returns>
    [DebuggerNonUserCode]
    public IMediaTypeSerializer AsMediaTypeSerializer() =>
        MediaTypeSerializerProxy.Wrap(this);
}