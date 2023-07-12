using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Solitons.Data;

/// <summary>
/// Represents a serializer for serializing and deserializing objects into media content.
/// </summary>
public partial interface IDataContractSerializer
{
    /// <summary>
    /// Determines whether the serializer can serialize an object of the specified type and content type.
    /// </summary>
    /// <param name="type">The type of the object to serialize.</param>
    /// <param name="contentType">The content type to serialize the object into.</param>
    /// <returns>true if the serializer can serialize the object; otherwise, false.</returns>
    bool CanSerialize(Type type, string contentType);

    /// <summary>
    /// Determines whether the serializer can serialize an object of the specified type and gets the content type.
    /// </summary>
    /// <param name="type">The type of the object to serialize.</param>
    /// <param name="contentType">When this method returns, contains the content type to serialize the object into, or null if the serializer cannot serialize the object.</param>
    /// <returns>true if the serializer can serialize the object; otherwise, false.</returns>
    bool CanSerialize(Type type, out string? contentType);

    /// <summary>
    /// Determines whether the serializer can deserialize an object of the specified type and content type.
    /// </summary>
    /// <param name="typeId">The unique identifier of the type of the object to deserialize.</param>
    /// <param name="contentType">The content type of the serialized object.</param>
    /// <returns>true if the serializer can deserialize the object; otherwise, false.</returns>
    bool CanDeserialize(Guid typeId, string contentType);

    /// <summary>
    /// Determines whether the serializer can deserialize an object of the specified type and content type.
    /// </summary>
    /// <param name="type">The type of the object to deserialize.</param>
    /// <param name="contentType">The content type of the serialized object.</param>
    /// <returns>true if the serializer can deserialize the object; otherwise, false.</returns>
    bool CanDeserialize(Type type, string contentType);

    /// <summary>
    /// Gets the supported content types for serializing objects of the specified type.
    /// </summary>
    /// <param name="typeId">The unique identifier of the type of the object to get the supported content types for.</param>
    /// <returns>An enumerable of supported content types.</returns>
    IEnumerable<string> GetSupportedContentTypes(Guid typeId);

    /// <summary>
    /// Gets the supported content types for serializing objects of the specified type.
    /// </summary>
    /// <param name="type">The type of the object to get the supported content types for.</param>
    /// <returns>An enumerable of supported content types.</returns>
    IEnumerable<string> GetSupportedContentTypes(Type type);

    /// <summary>
    /// Serializes an object into media content using the specified content type.
    /// </summary>
    /// <param name="obj">The object to serialize.</param>
    /// <param name="contentType">The content type to serialize the object into.</param>
    /// <returns>The serialized object as media content.</returns>
    /// <exception cref="ArgumentException">The content type is not supported.</exception>
    /// <exception cref="ArgumentNullException">The object to serialize is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">The content type is null or empty.</exception>
    TextMediaContent Serialize(object obj, string contentType);

    /// <summary>
    /// Serializes the specified object to a new instance of the <see cref="TextMediaContent"/> class, using the default content type.
    /// </summary>
    /// <param name="obj">The object to be serialized.</param>
    /// <returns>A new instance of the <see cref="TextMediaContent"/> class containing the serialized data.</returns>
    /// <exception cref="ArgumentException">Thrown when the specified object cannot be serialized.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the specified object is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the serialized data exceeds the maximum length allowed for the content.</exception>
    TextMediaContent Serialize(object obj);


    /// <summary>
    /// Deserializes the provided content string into an instance of the specified type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize the content into.</typeparam>
    /// <param name="content">The content to deserialize.</param>
    /// <param name="contentType">The content type of the serialized data.</param>
    /// <returns>An instance of the specified type <typeparamref name="T"/> deserialized from the provided content string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="content"/> is null or empty.</exception>
    /// <exception cref="ArgumentException">Thrown if <paramref name="contentType"/> is null or empty.</exception>
    /// <exception cref="SerializationException">Thrown if there was an error during deserialization.</exception>
    T Deserialize<T>(string content, string contentType);


    /// <summary>
    /// Deserializes the specified <paramref name="content"/> into an object of the type specified by the provided <paramref name="typeId"/>.
    /// </summary>
    /// <param name="typeId">The identifier of the type to deserialize the content into.</param>
    /// <param name="content">The content to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    /// <exception cref="ArgumentException">Thrown when the provided <paramref name="typeId"/> is not registered.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the provided <paramref name="content"/> is null.</exception>
    /// <exception cref="SerializationException">Thrown when an error occurs during the deserialization process.</exception>
    object Deserialize(Guid typeId, TextMediaContent content);

    /// <summary>
    /// Serializes an object into a DataTransferPackage.
    /// </summary>
    /// <param name="dto">The object to serialize.</param>
    /// <returns>A DataTransferPackage containing the serialized object.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input object is null.</exception>
    DataTransferPackage Pack(object dto);

    /// <summary>
    /// Unpacks a <see cref="DataTransferPackage"/> into an object of the corresponding type.
    /// </summary>
    /// <param name="package">The <see cref="DataTransferPackage"/> to unpack.</param>
    /// <returns>The unpacked object.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="package"/> is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the type of the object cannot be determined or if it is not registered.</exception>
    object Unpack(DataTransferPackage package);

    /// <summary>
    /// Gets the collection of types that this serializer can handle.
    /// </summary>
    /// <returns>The collection of supported types.</returns>
    IEnumerable<Type> GetSupportedTypes();

    /// <summary>
    /// Gets the <see cref="Type"/> associated with the given DTO type ID.
    /// </summary>
    /// <param name="dtoTypeId">The ID of the DTO type.</param>
    /// <returns>The <see cref="Type"/> associated with the given DTO type ID, if registered; otherwise, <see langword="null"/>.</returns>

    Type GetType(Guid dtoTypeId);

    /// <summary>
    /// Gets the registered type for the specified DTO type identifier, or null if the type is not registered.
    /// </summary>
    /// <param name="dtoTypeId">The identifier of the DTO type.</param>
    /// <returns>The registered type for the specified DTO type identifier, or null if the type is not registered.</returns>
    Type? GetTypeIfRegistered(Guid dtoTypeId);
}

public partial interface IDataContractSerializer
{
    /// <summary>
    /// Serializes an object and returns the serialized content as a string along with its content type.
    /// </summary>
    /// <param name="obj">The object to be serialized.</param>
    /// <param name="contentType">The content type of the serialized content.</param>
    /// <returns>The serialized content as a string.</returns>
    [DebuggerStepThrough]
    public string Serialize(object obj, out string contentType)
    {
        var content = Serialize(obj);
        contentType = content.ContentType;
        return content.Content;
    }


    /// <summary>
    /// Deserializes an object from the provided serialized content and content type identified by its GUID.
    /// </summary>
    /// <param name="typeId">The GUID of the type to be deserialized.</param>
    /// <param name="content">The serialized content to be deserialized.</param>
    /// <param name="contentType">The content type of the serialized content.</param>
    /// <returns>The deserialized object.</returns>
    [DebuggerStepThrough]
    public object Deserialize(Guid typeId, string content, string contentType) => 
        Deserialize(typeId, new TextMediaContent(content, contentType));

    /// <summary>
    /// Deserializes an object from the provided serialized content and content type identified by its type.
    /// </summary>
    /// <param name="type">The type of the object to be deserialized.</param>
    /// <param name="content">The serialized content to be deserialized.</param>
    /// <param name="contentType">The content type of the serialized content.</param>
    /// <returns>The deserialized object.</returns>
    [DebuggerStepThrough]
    public object Deserialize(Type type, string content, string contentType) =>
        Deserialize(type.GUID, new TextMediaContent(content, contentType));

    /// <summary>
    /// Deserializes an object from the provided serialized content identified by its type.
    /// </summary>
    /// <param name="type">The type of the object to be deserialized.</param>
    /// <param name="content">The serialized content to be deserialized.</param>
    /// <returns>The deserialized object.</returns>
    [DebuggerStepThrough]
    object Deserialize(Type type, TextMediaContent content) => Deserialize(type.GUID, content);

    /// <summary>
    /// Transforms the provided serialized content of an object to a new content type identified by its GUID.
    /// </summary>
    /// <param name="typeId">The GUID of the type to be transformed.</param>
    /// <param name="input">The serialized content to be transformed.</param>
    /// <param name="contentType">The new content type of the transformed content.</param>
    /// <returns>The transformed serialized content.</returns>
    public TextMediaContent Transform(
        Guid typeId, 
        TextMediaContent input, 
        string contentType)
    {
        var dto = Deserialize(typeId, input);
        var content = Serialize(dto, contentType);
        return new TextMediaContent(content, contentType);
    }


    /// <summary>
    /// Deserializes an object from the provided serialized content identified by its type and content type.
    /// </summary>
    /// <typeparam name="T">The type of the object to be deserialized.</typeparam>
    /// <param name="mediaContent">The serialized content to be deserialized.</param>
    /// <returns>The deserialized object.</returns>
    [DebuggerStepThrough]
    public T Deserialize<T>(TextMediaContent mediaContent) =>
        (T)Deserialize(typeof(T), mediaContent.Content, mediaContent.ContentType);


    /// <summary>
    /// Builds and returns an instance of IDataContractSerializer.
    /// </summary>
    /// <param name="config">The configuration to be used to build the instance.</param>
    /// <returns>An instance of IDataContractSerializer.</returns>
    [DebuggerStepThrough]
    public static IDataContractSerializer Build(Action<IDataContractSerializerBuilder> config)
    {
        var builder = new DataContractSerializerBuilder();
        config?.Invoke(builder);
        return builder.Build();
    }
}
