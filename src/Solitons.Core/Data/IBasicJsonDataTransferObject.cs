using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Solitons.Data;

/// <summary>
/// Facilitates JSON serialization and deserialization via the <see cref="System.Text.Json"/> library in .NET. 
/// Additionally, serves as a marker for <see cref="IDataContractSerializer"/> serializers to detect and handle data contract classes.
/// </summary>
public interface IBasicJsonDataTransferObject
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
    public sealed TextMediaContent ToJsonMediaContent() => BasicJsonDataTransferObject.Serialize(this);

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="options">The options to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public sealed TextMediaContent ToJsonMediaContent(JsonSerializerOptions options) => BasicJsonDataTransferObject.Serialize(this, options);

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="context">The context to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public sealed TextMediaContent ToJsonMediaContent(JsonSerializerContext context) => BasicJsonDataTransferObject.Serialize(this, context);

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified input type.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public sealed TextMediaContent ToJsonMediaContent(Type inputType) => BasicJsonDataTransferObject.Serialize(this, inputType);

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified input type and <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <param name="options">The options to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public sealed TextMediaContent ToJsonMediaContent(Type inputType, JsonSerializerOptions options) => BasicJsonDataTransferObject.Serialize(this, inputType, options);


    /// <summary>
    /// Converts this instance to its JSON string representation using the specified input type and <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <param name="context">The context to use during serialization.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    [DebuggerStepThrough]
    public sealed TextMediaContent ToJsonMediaContent(Type inputType, JsonSerializerContext context) => BasicJsonDataTransferObject.Serialize(this, inputType, context);

    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the default <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public sealed byte[] ToUtf8Bytes() => BasicJsonDataTransferObject.SerializeToUtf8Bytes(this);


    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="options">The options to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public sealed byte[] ToUtf8Bytes(JsonSerializerOptions options) => BasicJsonDataTransferObject.SerializeToUtf8Bytes(this, options);

    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="context">The context to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public sealed byte[] ToUtf8Bytes(JsonSerializerContext context) => BasicJsonDataTransferObject.SerializeToUtf8Bytes(this, context);



    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified input type.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public sealed byte[] ToUtf8Bytes(Type inputType) => BasicJsonDataTransferObject.SerializeToUtf8Bytes(this, inputType);


    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified input type and <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <param name="options">The options to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public sealed byte[] ToUtf8Bytes(Type inputType, JsonSerializerOptions options) =>
        BasicJsonDataTransferObject.SerializeToUtf8Bytes(this, inputType, options);


    /// <summary>
    /// Converts this instance to its UTF-8 encoded JSON byte array representation using the specified input type and <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="inputType">The input type to use during serialization.</param>
    /// <param name="context">The context to use during serialization.</param>
    /// <returns>The UTF-8 encoded JSON byte array representation of this instance.</returns>
    [DebuggerStepThrough]
    public sealed byte[] ToUtf8Bytes(Type inputType, JsonSerializerContext context) =>
        BasicJsonDataTransferObject.SerializeToUtf8Bytes(this, inputType, context);


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
    public static T Parse<T>(string jsonString) where T : IBasicJsonDataTransferObject
    {
        return (T)Parse(jsonString, typeof(T));
    }

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
    public static T Parse<T>(string jsonString, JsonSerializerOptions options) where T : IBasicJsonDataTransferObject
    {
        return (T)Parse(jsonString, typeof(T), options);
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
    public static T Parse<T>(string jsonString, JsonSerializerContext context) where T : IBasicJsonDataTransferObject
    {
        return (T)Parse(jsonString, typeof(T), context);
    }

    /// <summary>
    /// Deserializes the specified JSON string to an object of the specified <paramref name="returnType"/>.
    /// </summary>
    /// <param name="jsonString">The JSON string to deserialize.</param>
    /// <param name="returnType">The type of the object to deserialize to.</param>
    /// <returns>The deserialized object.</returns>
    /// <exception cref="System.FormatException">Thrown if deserialization fails.</exception>
    /// <remarks>
    /// This method deserializes the specified JSON string to an object of the specified <paramref name="returnType"/>.
    /// After deserialization, this method calls the <see cref="IDeserializationCallback.OnDeserialization"/> method of the object, if it implements the <see cref="IDeserializationCallback"/> interface.
    /// </remarks>
    [DebuggerStepThrough]
    public static object Parse(string jsonString, Type returnType)
    {
        var obj = JsonSerializer.Deserialize(jsonString, returnType);
        if (obj is IDeserializationCallback callback)
            callback.OnDeserialization(null);
        return obj ?? throw new FormatException();
    }

    /// <summary>
    /// Deserializes the specified JSON string to an object of the specified <paramref name="returnType"/> using the specified <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="jsonString">The JSON string to deserialize.</param>
    /// <param name="returnType">The type of the object to deserialize to.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> used to customize the deserialization process.</param>
    /// <returns>The deserialized object.</returns>
    /// <exception cref="System.FormatException">Thrown if deserialization fails.</exception>
    /// <remarks>
    /// This method deserializes the specified JSON string to an object of the specified <paramref name="returnType"/> using the specified <see cref="JsonSerializerOptions"/>.
    /// After deserialization, this method calls the <see cref="IDeserializationCallback.OnDeserialization"/> method of the object, if it implements the <see cref="IDeserializationCallback"/> interface.
    /// </remarks>
    [DebuggerStepThrough]
    public static object Parse(string jsonString, Type returnType, JsonSerializerOptions options)
    {
        var obj = JsonSerializer.Deserialize(jsonString, returnType, options);
        if (obj is IDeserializationCallback callback)
            callback.OnDeserialization(null);
        return obj ?? throw new FormatException();
    }

    /// <summary>
    /// Deserializes the specified JSON string to an object of the specified <paramref name="returnType"/> using the specified <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="jsonString">The JSON string to deserialize.</param>
    /// <param name="returnType">The type of the object to deserialize to.</param>
    /// <param name="context">The <see cref="JsonSerializerContext"/> used to customize the deserialization process.</param>
    /// <returns>The deserialized object.</returns>
    /// <exception cref="System.FormatException">Thrown if deserialization fails.</exception>
    /// <remarks>
    /// This method deserializes the specified JSON string to an object of the specified <paramref name="returnType"/> using the specified <see cref="JsonSerializerContext"/>.
    /// After deserialization, this method calls the <see cref="IDeserializationCallback.OnDeserialization"/> method of the object, if it implements the <see cref="IDeserializationCallback"/> interface.
    /// </remarks>
    [DebuggerStepThrough]
    public static object Parse(string jsonString, Type returnType, JsonSerializerContext context)
    {
        var obj = JsonSerializer.Deserialize(jsonString, returnType, context);
        if (obj is IDeserializationCallback callback)
            callback.OnDeserialization(null);
        return obj ?? throw new FormatException();
    }
}

public static partial class Extensions
{
    /// <summary>
    /// Converts the object implementing the <see cref="IBasicJsonDataTransferObject"/> interface to a <see cref="TextMediaContent"/> object with JSON content.
    /// </summary>
    /// <param name="self">The object implementing the <see cref="IBasicJsonDataTransferObject"/> interface.</param>
    /// <returns>A <see cref="TextMediaContent"/> object with JSON content.</returns>
    /// <remarks>
    /// This extension method converts the object to its JSON string representation using the <see cref="IBasicJsonDataTransferObject.ToJsonString"/> method.
    /// The JSON string is then used to create a new <see cref="TextMediaContent"/> object with "application/json" as the content type using the <see cref="TextMediaContent.CreateJson"/> method.
    /// </remarks>
    [DebuggerStepThrough]
    public static TextMediaContent AsJsonMediaContent(this IBasicJsonDataTransferObject self)
    {
        string jsonString = self.ToJsonMediaContent();
        return TextMediaContent.CreateJson(jsonString);
    }
}