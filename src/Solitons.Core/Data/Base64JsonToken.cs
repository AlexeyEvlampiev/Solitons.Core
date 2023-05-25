using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;

namespace Solitons.Data;

/// <summary>
/// Provides methods for serializing and deserializing JSON objects to and from Base64-encoded strings.
/// </summary>
public abstract record Base64JsonToken :
    ISerializationCallback,
    IBasicJsonDataTransferObject
{
    /// <summary>
    /// Returns a Base64-encoded JSON string representation of the current <see cref="Base64JsonToken"/> object.
    /// </summary>
    /// <returns>A Base64-encoded JSON string.</returns>
    public sealed override string ToString() => JsonSerializer
        .Serialize<object>(this)
        .ToBase64(Encoding.UTF8);


    /// <summary>
    /// Converts a <see cref="Base64JsonToken"/> object to a Base64-encoded JSON string.
    /// </summary>
    /// <param name="token">The <see cref="Base64JsonToken"/> object to convert.</param>
    /// <returns>The Base64-encoded JSON string.</returns>
    public static implicit operator string(Base64JsonToken token) => token.ToString();

    /// <summary>
    /// Deserializes a Base64-encoded JSON string to an object of type T.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize.</typeparam>
    /// <param name="base64">The Base64-encoded JSON string to deserialize.</param>
    /// <returns>The deserialized object of type T.</returns>
    /// <exception cref="FormatException">Thrown when deserialization fails for any reason.</exception>
    protected static T Parse<T>(string base64)
    {
        try
        {
            var json = Encoding.UTF8.GetString(base64.AsBase64Bytes());
            var obj = JsonSerializer.Deserialize<T>(json);
            return obj ?? throw new FormatException($"Deserialization of {typeof(T)} failed: the JSON string was null or empty.");
        }
        catch (JsonException e)
        {
            throw new FormatException($"Deserialization of {typeof(T)} failed: {e.Message}", e);
        }
        catch (Exception e) when (e is not FormatException)
        {
            throw new FormatException($"Deserialization of {typeof(T)} failed: an unexpected error occurred.", e);
        }
    }

    /// <summary>
    /// Invoked when the deserialization process has completed.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    protected virtual void OnDeserialization(object? sender) { }

    /// <summary>
    /// Invoked before the serialization process begins.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    protected virtual void OnSerializing(object? sender) { }

    /// <summary>
    /// Invoked after the serialization process has completed.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    protected virtual void OnSerialized(object? sender) { }

    [DebuggerStepThrough]
    void IDeserializationCallback.OnDeserialization(object? sender) => OnDeserialization(sender);

    [DebuggerStepThrough]
    void ISerializationCallback.OnSerializing(object? sender) => OnSerializing(sender);

    [DebuggerStepThrough]
    void ISerializationCallback.OnSerialized(object? sender) => OnSerialized(sender);
}