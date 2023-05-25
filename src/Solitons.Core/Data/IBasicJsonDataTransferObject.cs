using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Solitons.Data;

/// <summary>
/// Provides methods for serializing and deserializing objects to and from JSON format using the <see cref="System.Text.Json"/> library in .NET.
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
    public sealed string ToJsonString()
    {
        var callback = this as ISerializationCallback;
        callback?.OnSerializing(null);
        var json = JsonSerializer.Serialize(this, this.GetType());
        callback?.OnSerialized(null);
        return json;
    }

    /// <summary>
    /// Converts this instance to its JSON UTF-8 encoded byte array representation using the default <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <returns>The JSON UTF-8 encoded byte array representation of this instance.</returns>
    /// <remarks>
    /// This method calls the <see cref="ISerializationCallback.OnSerializing"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, before serialization.
    /// This method calls the <see cref="ISerializationCallback.OnSerialized"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, after serialization.
    /// </remarks>
    [DebuggerStepThrough]
    public sealed byte[] ToJsonUtf8Bytes()
    {
        var callback = this as ISerializationCallback;
        callback?.OnSerializing(null);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(this, this.GetType());
        callback?.OnSerialized(null);
        return bytes;
    }

    /// <summary>
    /// Converts this instance to its JSON UTF-8 encoded byte array representation using the specified <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> used to customize the serialization process.</param>
    /// <returns>The JSON UTF-8 encoded byte array representation of this instance.</returns>
    /// <remarks>
    /// This method calls the <see cref="ISerializationCallback.OnSerializing"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, before serialization.
    /// This method calls the <see cref="ISerializationCallback.OnSerialized"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, after serialization.
    /// </remarks>
    [DebuggerStepThrough]
    public sealed byte[] ToJsonUtf8Bytes(JsonSerializerOptions options)
    {
        var callback = this as ISerializationCallback;
        callback?.OnSerializing(null);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(this, this.GetType(), options);
        callback?.OnSerialized(null);
        return bytes;
    }

    /// <summary>
    /// Converts this instance to its JSON UTF-8 encoded byte array representation using the specified <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="context">The <see cref="JsonSerializerContext"/> used to customize the serialization process.</param>
    /// <returns>The JSON UTF-8 encoded byte array representation of this instance.</returns>
    /// <remarks>
    /// This method calls the <see cref="ISerializationCallback.OnSerializing"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, before serialization.
    /// This method calls the <see cref="ISerializationCallback.OnSerialized"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, after serialization.
    /// </remarks>
    [DebuggerStepThrough]
    public sealed byte[] ToJsonUtf8Bytes(JsonSerializerContext context)
    {
        var callback = this as ISerializationCallback;
        callback?.OnSerializing(null);
        var bytes = JsonSerializer.SerializeToUtf8Bytes(this, this.GetType(), context);
        callback?.OnSerialized(null);
        return bytes;
    }

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> used to customize the serialization process.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    /// <remarks>
    /// This method serializes the object to a JSON string using the specified <see cref="JsonSerializerOptions"/>.
    /// Before serialization, this method calls the <see cref="ISerializationCallback.OnSerializing"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface.
    /// After serialization, this method calls the <see cref="ISerializationCallback.OnSerialized"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface.
    /// </remarks>
    [DebuggerStepThrough]
    public sealed string ToJsonString(JsonSerializerOptions options)
    {
        var callback = this as ISerializationCallback;
        callback?.OnSerializing(null);
        var json = JsonSerializer.Serialize(this, this.GetType(), options);
        callback?.OnSerialized(null);
        return json;
    }

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="context">The <see cref="JsonSerializerContext"/> used to customize the serialization process.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    /// <remarks>
    /// This method serializes the object to a JSON string using the specified <see cref="JsonSerializerContext"/>.
    /// Before serialization, this method calls the <see cref="ISerializationCallback.OnSerializing"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface.
    /// After serialization, this method calls the <see cref="ISerializationCallback.OnSerialized"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface.
    /// </remarks>
    [DebuggerStepThrough]
    public sealed string ToJsonString(JsonSerializerContext context)
    {
        var callback = this as ISerializationCallback;
        callback?.OnSerializing(null);
        var json = JsonSerializer.Serialize(this, this.GetType(), context);
        callback?.OnSerialized(null);
        return json;
    }

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
    /// Converts this instance to its JSON string representation using the default <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="self">The object to serialize.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    /// <remarks>
    /// This method calls the <see cref="ISerializationCallback.OnSerializing"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, before serialization.
    /// This method calls the <see cref="ISerializationCallback.OnSerialized"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, after serialization.
    /// </remarks>
    [DebuggerStepThrough]
    public static string ToJsonString(this IBasicJsonDataTransferObject self) => self.ToJsonString();

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="self">The object to serialize.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> used to customize the serialization process.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    /// <remarks>
    /// This method calls the <see cref="ISerializationCallback.OnSerializing"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, before serialization.
    /// This method calls the <see cref="ISerializationCallback.OnSerialized"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, after serialization.
    /// </remarks>
    [DebuggerStepThrough]
    public static string ToJsonString(this IBasicJsonDataTransferObject self, JsonSerializerOptions options) => self.ToJsonString(options);

    /// <summary>
    /// Converts this instance to its JSON string representation using the specified <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="self">The object to serialize.</param>
    /// <param name="context">The <see cref="JsonSerializerContext"/> used to customize the serialization process.</param>
    /// <returns>The JSON string representation of this instance.</returns>
    /// <remarks>
    /// This method calls the <see cref="ISerializationCallback.OnSerializing"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, before serialization.
    /// This method calls the <see cref="ISerializationCallback.OnSerialized"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, after serialization.
    /// </remarks>
    [DebuggerStepThrough]
    public static string ToJsonString(this IBasicJsonDataTransferObject self, JsonSerializerContext context) => self.ToJsonString(context);

    /// <summary>
    /// Converts this instance to its JSON UTF-8 encoded byte array representation using the default <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="self">The object to serialize.</param>
    /// <returns>The JSON UTF-8 encoded byte array representation of this instance.</returns>
    /// <remarks>
    /// This method calls the <see cref="ISerializationCallback.OnSerializing"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, before serialization.
    /// This method calls the <see cref="ISerializationCallback.OnSerialized"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, after serialization.
    /// </remarks>
    [DebuggerStepThrough]
    public static byte[] ToJsonUtf8Bytes(this IBasicJsonDataTransferObject self) => self.ToJsonUtf8Bytes();

    /// <summary>
    /// Converts this instance to its JSON UTF-8 encoded byte array representation using the specified <see cref="JsonSerializerOptions"/>.
    /// </summary>
    /// <param name="self">The object to serialize.</param>
    /// <param name="options">The <see cref="JsonSerializerOptions"/> used to customize the serialization process.</param>
    /// <returns>The JSON UTF-8 encoded byte array representation of this instance.</returns>
    /// <remarks>
    /// This method calls the <see cref="ISerializationCallback.OnSerializing"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, before serialization.
    /// This method calls the <see cref="ISerializationCallback.OnSerialized"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, after serialization.
    /// </remarks>
    [DebuggerStepThrough]
    public static byte[] ToJsonUtf8Bytes(this IBasicJsonDataTransferObject self, JsonSerializerOptions options) => self.ToJsonUtf8Bytes(options);

    /// <summary>
    /// Converts this instance to its JSON UTF-8 encoded byte array representation using the specified <see cref="JsonSerializerContext"/>.
    /// </summary>
    /// <param name="self">The object to serialize.</param>
    /// <param name="context">The <see cref="JsonSerializerContext"/> used to customize the serialization process.</param>
    /// <returns>The JSON UTF-8 encoded byte array representation of this instance.</returns>
    /// <remarks>
    /// This method calls the <see cref="ISerializationCallback.OnSerializing"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, before serialization.
    /// This method calls the <see cref="ISerializationCallback.OnSerialized"/> method of the object, if it implements the <see cref="ISerializationCallback"/> interface, after serialization.
    /// </remarks>
    [DebuggerStepThrough]
    public static byte[] ToJsonUtf8Bytes(this IBasicJsonDataTransferObject self, JsonSerializerContext context) => self.ToJsonUtf8Bytes(context);
}