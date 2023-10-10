using System;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Solitons.Common;

namespace Solitons.Data;

/// <summary>
/// Represents a JSON-based Data Transfer Object (DTO) with built-in serialization, deserialization, and cloning functionality.
/// This class serves as a base class for creating DTOs that can be easily serialized to and deserialized from JSON format.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="BasicJsonDataTransferObject"/> class provides a foundation for creating DTOs that are designed to work with JSON data.
/// It inherits the serialization callback functionality from the <see cref="SerializationCallback"/> class and implements the <see cref="IBasicJsonDataTransferObject"/> and <see cref="ICloneable"/> interfaces.
/// </para>
/// <para>
/// By extending this class, derived DTOs can take advantage of the following features:
/// - Automatic JSON serialization and deserialization using the <see cref="IBasicJsonDataTransferObject"/> methods.
/// - Cloning support through the <see cref="ICloneable"/> interface and the <see cref="Clone"/> method.
/// - Convenient JSON string representation retrieval using the overridden <see cref="ToString"/> method.
/// </para>
/// </remarks>
public abstract class BasicJsonDataTransferObject : SerializationCallback, IBasicJsonDataTransferObject, ICloneable
{
    /// <summary>
    /// Converts this instance to its JSON string representation using the default <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <returns>The JSON string representation of this instance.</returns>
    /// <remarks>
    /// This method serializes the object to a JSON string using the default <see cref="JsonSerializerOptions"/>.
    /// Before serialization, this method calls the <see cref="ISerializationCallback.OnSerializing"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface.
    /// After serialization, this method calls the <see cref="ISerializationCallback.OnSerialized"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface.
    /// </remarks>
    [DebuggerStepThrough]
    public TextMediaContent ToJsonMediaContent() => Serialize(this);

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="options">The options to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public TextMediaContent ToJsonMediaContent(JsonSerializerOptions options) => Serialize(this, options);

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="context">The context to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public TextMediaContent ToJsonMediaContent(JsonSerializerContext context) => Serialize(this, context);

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified input type.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public TextMediaContent ToJsonMediaContent(Type inputType) => Serialize(this, inputType);

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified input type and <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <param name="options">The options to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public TextMediaContent ToJsonMediaContent(Type inputType, JsonSerializerOptions options) => Serialize(this, inputType, options);

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified input type and <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <param name="context">The context to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public TextMediaContent ToJsonMediaContent(Type inputType, JsonSerializerContext context) => Serialize(this, inputType, context);

    /// <summary>
    /// Returns the JSON string representation of the current instance.
    /// </summary>
    /// <returns>The JSON string representation of the object.</returns>
    [DebuggerNonUserCode]
    public sealed override string ToString() => Serialize(this);

    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the default <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public byte[] ToUtf8Bytes() => SerializeToUtf8Bytes(this);


    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="options">The options to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public byte[] ToUtf8Bytes(JsonSerializerOptions options) => SerializeToUtf8Bytes(this, options);

    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="context">The context to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public byte[] ToUtf8Bytes(JsonSerializerContext context) => SerializeToUtf8Bytes(this, context);



    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified input type.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public byte[] ToUtf8Bytes(Type inputType) => SerializeToUtf8Bytes(this, inputType);


    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified input type and <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <param name="options">The options to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public byte[] ToUtf8Bytes(Type inputType, JsonSerializerOptions options) =>
        SerializeToUtf8Bytes(this, inputType, options);

    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified input type and <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <param name="context">The context to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public byte[] ToUtf8Bytes(Type inputType, JsonSerializerContext context) =>
        SerializeToUtf8Bytes(this, inputType, context);

    /// <summary>
    /// Creates a clone of the current instance using JSON serialization and deserialization.
    /// </summary>
    /// <returns>A clone of the current instance.</returns>
    /// <remarks>
    /// This method creates a clone by serializing the current instance to a JSON string and then
    /// deserializing it back to a new object of the same type.
    /// </remarks>
    [DebuggerNonUserCode]
    protected BasicJsonDataTransferObject Clone() => ((BasicJsonDataTransferObject)JsonSerializer.Deserialize(ToString(), GetType())!)!;

    [DebuggerStepThrough]
    object ICloneable.Clone() => Clone();


    /// <summary>
    /// Deserializes the specified JSON string to an object of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="jsonString">The JSON string to deserialize.</param>
    /// <returns>The deserialized object.</returns>
    /// <exception cref="System.FormatException">Thrown if deserialization fails.</exception>
    /// <remarks>
    /// This method deserializes the specified JSON string to an object of type <typeparamref name="T"/>.
    /// After deserialization, this method calls the <see cref="IDeserializationCallback.OnDeserialization"/> method of the object, if it implements the <see cref="IDeserializationCallback"/> interface.
    /// </remarks>
    [DebuggerNonUserCode]
    protected static T Parse<T>(string jsonString) where T : BasicJsonDataTransferObject => IBasicJsonDataTransferObject.Parse<T>(jsonString);

    /// <summary>
    /// Deserializes the specified JSON string to an object of type <typeparamref name="T"/> using the specified <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="jsonString">The JSON string to deserialize.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> used to customize the deserialization process.</param>
    /// <returns>The deserialized object.</returns>
    /// <exception cref="System.FormatException">Thrown if deserialization fails.</exception>
    /// <remarks>
    /// This method deserializes the specified JSON string to an object of type <typeparamref name="T"/> using the specified <see cref="JsonSerializerOptions"/>.
    /// After deserialization, this method calls the <see cref="IDeserializationCallback.OnDeserialization"/> method of the object, if it implements the <see cref="IDeserializationCallback"/> interface.
    /// </remarks>
    [DebuggerNonUserCode]
    protected static T Parse<T>(string jsonString, JsonSerializerOptions options) where T : IBasicJsonDataTransferObject
    {
        return IBasicJsonDataTransferObject.Parse<T>(jsonString, options);
    }

    /// <summary>
    /// Deserializes the specified JSON string to an object of type <typeparamref name="T"/> using the specified <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
    /// <param name="jsonString">The JSON string to deserialize.</param>
    /// <param name="context">The <see cref="JsonSerializerContext"/> used to customize the deserialization process.</param>
    /// <returns>The deserialized object.</returns>
    /// <exception cref="System.FormatException">Thrown if deserialization fails.</exception>
    /// <remarks>
    /// This method deserializes the specified JSON string to an object of type <typeparamref name="T"/> using the specified <see cref="JsonSerializerContext"/>.
    /// After deserialization, this method calls the <see cref="IDeserializationCallback.OnDeserialization"/> method of the object, if it implements the <see cref="IDeserializationCallback"/> interface.
    /// </remarks>
    [DebuggerNonUserCode]
    protected static T Parse<T>(string jsonString, JsonSerializerContext context) where T : IBasicJsonDataTransferObject
    {
        return IBasicJsonDataTransferObject.Parse<T>(jsonString, context);
    }

    /// <summary>
    /// Implicitly converts the instance to a <see cref="TextMediaContent"/> object by serializing it to JSON.
    /// </summary>
    /// <param name="dto">The instance of <see cref="BasicJsonDataTransferObject"/> to convert.</param>
    /// <returns>A <see cref="TextMediaContent"/> object containing the JSON representation of the instance.</returns>
    public static implicit operator TextMediaContent(BasicJsonDataTransferObject dto) => dto.ToJsonMediaContent();

    /// <summary>
    /// Implicitly converts the instance to a <see cref="HttpContent"/> object by serializing it to JSON and wrapping it in a <see cref="TextMediaContent"/> object.
    /// </summary>
    /// <param name="dto">The instance of <see cref="BasicJsonDataTransferObject"/> to convert.</param>
    /// <returns>An <see cref="HttpContent"/> object containing the JSON representation of the instance.</returns>
    public static implicit operator HttpContent(BasicJsonDataTransferObject dto) => dto
        .ToJsonMediaContent()
        .ToHttpContent();

    [DebuggerStepThrough]
    internal static TextMediaContent Serialize(IBasicJsonDataTransferObject dto)
    {
        var callback = dto as ISerializationCallback;
        callback?.OnSerializing(null);
        var json = JsonSerializer.Serialize(dto, dto.GetType());
        callback?.OnSerialized(null);
        return TextMediaContent.CreateJson(json);
    }


    [DebuggerStepThrough]
    internal static TextMediaContent Serialize(IBasicJsonDataTransferObject dto, JsonSerializerOptions options)
    {
        var callback = dto as ISerializationCallback;
        callback?.OnSerializing(null);
        var json = JsonSerializer.Serialize(dto, dto.GetType(), options);
        callback?.OnSerialized(null);
        return TextMediaContent.CreateJson(json);
    }

    [DebuggerStepThrough]
    internal static TextMediaContent Serialize(IBasicJsonDataTransferObject dto, JsonSerializerContext context)
    {
        var callback = dto as ISerializationCallback;
        callback?.OnSerializing(null);
        var json = JsonSerializer.Serialize(dto, dto.GetType(), context);
        callback?.OnSerialized(null);
        return TextMediaContent.CreateJson(json);
    }

    [DebuggerStepThrough]
    internal static TextMediaContent Serialize(IBasicJsonDataTransferObject dto, Type inputType)
    {
        var callback = dto as ISerializationCallback;
        callback?.OnSerializing(null);
        var json = JsonSerializer.Serialize(dto, inputType);
        callback?.OnSerialized(null);
        return TextMediaContent.CreateJson(json);
    }

    [DebuggerStepThrough]
    internal static TextMediaContent Serialize(IBasicJsonDataTransferObject dto, Type inputType, JsonSerializerOptions options)
    {
        var callback = dto as ISerializationCallback;
        callback?.OnSerializing(null);
        var json = JsonSerializer.Serialize(dto, inputType, options);
        callback?.OnSerialized(null);
        return TextMediaContent.CreateJson(json);
    }

    [DebuggerStepThrough]
    internal static TextMediaContent Serialize(IBasicJsonDataTransferObject dto, Type inputType, JsonSerializerContext context)
    {
        var callback = dto as ISerializationCallback;
        callback?.OnSerializing(null);
        var json = JsonSerializer.Serialize(dto, inputType, context);
        callback?.OnSerialized(null);
        return TextMediaContent.CreateJson(json);
    }

    [DebuggerStepThrough]
    internal static byte[] SerializeToUtf8Bytes(IBasicJsonDataTransferObject dto)
    {
        var callback = dto as ISerializationCallback;
        callback?.OnSerializing(null);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(dto, dto.GetType());
        callback?.OnSerialized(null);
        return bytes;
    }

    [DebuggerStepThrough]
    internal static byte[] SerializeToUtf8Bytes(IBasicJsonDataTransferObject dto, JsonSerializerOptions options)
    {
        var callback = dto as ISerializationCallback;
        callback?.OnSerializing(null);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(dto, dto.GetType(), options);
        callback?.OnSerialized(null);
        return bytes;
    }

    [DebuggerStepThrough]
    internal static byte[] SerializeToUtf8Bytes(IBasicJsonDataTransferObject dto, JsonSerializerContext context)
    {
        var callback = dto as ISerializationCallback;
        callback?.OnSerializing(null);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(dto, dto.GetType(), context);
        callback?.OnSerialized(null);
        return bytes;
    }

    [DebuggerStepThrough]
    internal static byte[] SerializeToUtf8Bytes(IBasicJsonDataTransferObject dto, Type inputType)
    {
        var callback = dto as ISerializationCallback;
        callback?.OnSerializing(null);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(dto, inputType);
        callback?.OnSerialized(null);
        return bytes;
    }

    [DebuggerStepThrough]
    internal static byte[] SerializeToUtf8Bytes(IBasicJsonDataTransferObject dto, Type inputType, JsonSerializerOptions options)
    {
        var callback = dto as ISerializationCallback;
        callback?.OnSerializing(null);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(dto, inputType, options);
        callback?.OnSerialized(null);
        return bytes;
    }

    [DebuggerStepThrough]
    internal static byte[] SerializeToUtf8Bytes(IBasicJsonDataTransferObject dto, Type inputType, JsonSerializerContext context)
    {
        var callback = dto as ISerializationCallback;
        callback?.OnSerializing(null);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(dto, inputType, context);
        callback?.OnSerialized(null);
        return bytes;
    }
}