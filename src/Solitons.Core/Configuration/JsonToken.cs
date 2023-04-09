using System;
using System.Text;
using System.Text.Json;

namespace Solitons.Configuration;

/// <summary>
/// Provides methods for serializing and deserializing JSON objects to and from Base64-encoded strings.
/// </summary>
public abstract class JsonToken
{
    /// <inheritdoc/>
    public override string ToString()
    {
        var json = JsonSerializer.Serialize(this);
        return json.ToBase64(Encoding.UTF8);
    }

    /// <summary>
    /// Converts a <see cref="JsonToken"/> object to a Base64-encoded JSON string.
    /// </summary>
    /// <param name="token">The <see cref="JsonToken"/> object to convert.</param>
    /// <returns>The Base64-encoded JSON string.</returns>
    public static implicit operator string(JsonToken token) => token.ToString();

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
            var obj = (T?)JsonSerializer.Deserialize<T>(json);
            return obj ?? throw new FormatException($"Deserialization of {typeof(T)} failed: the JSON string was null or empty.");
        }
        catch (JsonException e)
        {
            throw new FormatException($"Deserialization of {typeof(T)} failed: {e.Message}", e);
        }
        catch (Exception e) when(e is not FormatException)
        {
            throw new FormatException($"Deserialization of {typeof(T)} failed: an unexpected error occurred.", e);
        }
    }
}