using System;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Runtime.Serialization;

namespace Solitons.Data;

/// <summary>
/// 
/// </summary>
public abstract record BasicJsonDataTransferRecord : IBasicJsonDataTransferObject
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
    public TextMediaContent ToJsonMediaContent() => BasicJsonDataTransferObject.Serialize(this);

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="options">The options to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public TextMediaContent ToJsonMediaContent(JsonSerializerOptions options) => BasicJsonDataTransferObject.Serialize(this, options);

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="context">The context to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public TextMediaContent ToJsonMediaContent(JsonSerializerContext context) => BasicJsonDataTransferObject.Serialize(this, context);

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified input type.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public TextMediaContent ToJsonMediaContent(Type inputType) => BasicJsonDataTransferObject.Serialize(this, inputType);

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified input type and <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <param name="options">The options to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public TextMediaContent ToJsonMediaContent(Type inputType, JsonSerializerOptions options) => BasicJsonDataTransferObject.Serialize(this, inputType, options);


    /// <summary>
    /// Converts this instance to its JSON string representation using the specified input type and <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <param name="context">The context to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public TextMediaContent ToJsonMediaContent(Type inputType, JsonSerializerContext context) => BasicJsonDataTransferObject.Serialize(this, inputType, context);

    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the default <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public byte[] ToUtf8Bytes() => BasicJsonDataTransferObject.SerializeToUtf8Bytes(this);


    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="options">The options to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public byte[] ToUtf8Bytes(JsonSerializerOptions options) => BasicJsonDataTransferObject.SerializeToUtf8Bytes(this, options);

    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="context">The context to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public byte[] ToUtf8Bytes(JsonSerializerContext context) => BasicJsonDataTransferObject.SerializeToUtf8Bytes(this, context);



    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified input type.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public byte[] ToUtf8Bytes(Type inputType) => BasicJsonDataTransferObject.SerializeToUtf8Bytes(this, inputType);


    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified input type and <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <param name="options">The options to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public byte[] ToUtf8Bytes(Type inputType, JsonSerializerOptions options) =>
        BasicJsonDataTransferObject.SerializeToUtf8Bytes(this, inputType, options);


    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified input type and <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <param name="context">The context to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public byte[] ToUtf8Bytes(Type inputType, JsonSerializerContext context) =>
        BasicJsonDataTransferObject.SerializeToUtf8Bytes(this, inputType, context);


    /// <summary>
    /// Converts the instance to its JSON representation as a string.
    /// </summary>
    /// <returns>A string containing the JSON representation of the instance.</returns>
    public override string ToString() => BasicJsonDataTransferObject.Serialize(this);

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
}