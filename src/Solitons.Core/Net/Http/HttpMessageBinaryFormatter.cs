using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

/// <summary>
/// Provides functionality for serializing HTTP requests and responses to a binary format, and deserializing them from a binary format.
/// </summary>
/// <remarks>
/// This class is useful when you need to store HTTP messages for later use or transmit them over a non-HTTP protocol.
/// </remarks>
public static class HttpMessageBinaryFormatter
{
    /// <summary>
    /// Converts an <see cref="HttpRequestMessage"/> instance to a binary format.
    /// </summary>
    /// <param name="request">The <see cref="HttpRequestMessage"/> to convert.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A byte array representing the serialized <see cref="HttpRequestMessage"/>.</returns>
    /// <exception cref="HttpMessageBinaryFormatterException">Thrown when an error occurs during serialization and the operation was not cancelled.</exception>
    /// <remarks>
    /// This method serializes the HTTP method, URI, version, headers, and content of the <see cref="HttpRequestMessage"/> to a byte array.
    /// </remarks>
    public static async Task<byte[]> ToByteArrayAsync(
        HttpRequestMessage request,
        CancellationToken cancellation = default)
    {
        await using var memoryStream = new MemoryStream();
        await WriteAsync(request, memoryStream, cancellation);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Converts an <see cref="HttpResponseMessage"/> instance to a binary format.
    /// </summary>
    /// <param name="response">The <see cref="HttpResponseMessage"/> to convert.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A byte array representing the serialized <see cref="HttpResponseMessage"/>.</returns>
    /// <exception cref="HttpMessageBinaryFormatterException">Thrown when an error occurs during serialization and the operation was not cancelled.</exception>
    /// <remarks>
    /// This method serializes the status code, version, reason phrase, headers, and content of the <see cref="HttpResponseMessage"/> to a byte array.
    /// </remarks>
    public static async Task<byte[]> ToByteArrayAsync(
        HttpResponseMessage response,
        CancellationToken cancellation = default)
    {
        await using var memoryStream = new MemoryStream();
        await WriteAsync(response, memoryStream, cancellation);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// Converts a byte array to an <see cref="HttpRequestMessage"/> instance.
    /// </summary>
    /// <param name="bytes">The byte array to convert.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>An <see cref="HttpRequestMessage"/> constructed from the binary data.</returns>
    /// <exception cref="HttpMessageBinaryFormatterException">Thrown when an error occurs during deserialization and the operation was not cancelled.</exception>
    /// <remarks>
    /// This method constructs an <see cref="HttpRequestMessage"/> from a byte array. It expects the array to be in the format produced by <see cref="ToByteArrayAsync"/>.
    /// </remarks>
    public static async Task<HttpRequestMessage> ToRequestAsync(
        byte[] bytes,
        CancellationToken cancellation = default)
    {
        await using var memoryStream = new MemoryStream(bytes);
        memoryStream.Position = 0;
        return await ReadRequestAsync(memoryStream, cancellation);
    }

    /// <summary>
    /// Converts a byte array to an <see cref="HttpResponseMessage"/> instance.
    /// </summary>
    /// <param name="bytes">The byte array to convert.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>An <see cref="HttpResponseMessage"/> constructed from the binary data.</returns>
    /// <exception cref="HttpMessageBinaryFormatterException">Thrown when an error occurs during deserialization and the operation was not cancelled.</exception>
    /// <remarks>
    /// This method constructs an <see cref="HttpResponseMessage"/> from a byte array. It expects the array to be in the format produced by <see cref="ToByteArrayAsync"/>.
    /// </remarks>
    public static async Task<HttpResponseMessage> ToResponseAsync(
        byte[] bytes,
        CancellationToken cancellation = default)
    {
        using var memoryStream = new MemoryStream(bytes);
        return await ReadResponseAsync(memoryStream, cancellation);
    }

    /// <summary>
    /// Writes the specified <see cref="HttpRequestMessage"/> to a <see cref="Stream"/> in binary format.
    /// </summary>
    /// <param name="request">The <see cref="HttpRequestMessage"/> to write.</param>
    /// <param name="stream">The stream to which the <see cref="HttpRequestMessage"/> should be written.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <exception cref="HttpMessageBinaryFormatterException">Thrown when an error occurs during serialization and the operation was not cancelled.</exception>
    /// <remarks>
    /// This method serializes the HTTP method, URI, version, headers, and content of the <see cref="HttpRequestMessage"/> to a binary format and writes it to the specified stream.
    /// This is a lower-level operation than <see cref="ToByteArrayAsync(HttpRequestMessage, CancellationToken)"/>, and you would typically use it when you want to serialize the message directly to a stream, such as a network stream or a file stream, rather than first to a byte array.
    /// </remarks>
    public static async Task WriteAsync(
        HttpRequestMessage request, 
        Stream stream,
        CancellationToken cancellation = default)
    {
        try
        {
            var writer = new BinaryWriter(stream);
            writer.Write(request.Method.ToString());
            writer.Write(request.RequestUri?.ToString() ?? String.Empty);
            writer.Write(request.Version.ToString());
            writer.Write(request.Headers.Count());
            var values = new List<string>();
            foreach (var header in request.Headers)
            {
                values.Clear();
                values.AddRange(header.Value);
                writer.Write(header.Key);
                writer.Write(values.Count);
                foreach (var value in values)
                {
                    writer.Write(value);
                }
            }

            if (request.Content != null)
            {
                writer.Write(true);
                var trailingHeaders = request.Content.Headers;
                writer.Write(trailingHeaders.Count());
                foreach (var header in trailingHeaders)
                {
                    values.Clear();
                    values.AddRange(header.Value);
                    writer.Write(header.Key);
                    writer.Write(values.Count);
                    foreach (var value in values)
                    {
                        writer.Write(value);
                    }
                }
                writer.Flush();
                await request.Content.CopyToAsync(stream, cancellation);
            }
            else
            {
                writer.Write(false);
                writer.Flush();
            }
        }
        catch (Exception ex) when(cancellation.IsCancellationRequested == false)
        {
            throw new HttpMessageBinaryFormatterException("Error occurred during serialization", ex);
        }
    }

    /// <summary>
    /// Reads a <see cref="HttpRequestMessage"/> from a <see cref="Stream"/> that contains a serialized HttpRequestMessage in binary format.
    /// </summary>
    /// <param name="stream">The stream from which the <see cref="HttpRequestMessage"/> should be read.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous read operation. The value of the <see cref="Task{TResult}.Result"/> parameter contains the deserialized <see cref="HttpRequestMessage"/>.</returns>
    /// <exception cref="HttpMessageBinaryFormatterException">Thrown when an error occurs during deserialization and the operation was not cancelled.</exception>
    /// <remarks>
    /// This method reads a serialized <see cref="HttpRequestMessage"/> in binary format from a stream and deserializes it. The method expects the data in the stream to be in the format produced by the <see cref="WriteAsync(HttpRequestMessage, Stream, CancellationToken)"/> method.
    /// This is a lower-level operation than <see cref="ToRequestAsync(byte[], CancellationToken)"/>, and you would typically use it when you want to deserialize a message directly from a stream, such as a network stream or a file stream, rather than from a byte array.
    /// </remarks>
    public static async Task<HttpRequestMessage> ReadRequestAsync(
        Stream stream,
        CancellationToken cancellation = default)
    {
        try
        {
            var reader = new BinaryReader(stream);

            var method = new HttpMethod(reader.ReadString());
            var requestUri = reader.ReadString();
            var version = Version.Parse(reader.ReadString());

            var message = new HttpRequestMessage
            {
                Method = method,
                Version = version,
                RequestUri = !string.IsNullOrWhiteSpace(requestUri)
                    ? new Uri(requestUri) : null
            };

            int headersCount = reader.ReadInt32();
            for (int i = 0; i < headersCount; i++)
            {
                var key = reader.ReadString();
                var valueCount = reader.ReadInt32();
                for (int j = 0; j < valueCount; j++)
                {
                    var value = reader.ReadString();
                    message.Headers.TryAddWithoutValidation(key, value);
                }
            }

            bool hasContent = reader.ReadBoolean();
            if (hasContent == false)
            {
                return message;
            }
            int trailingHeadersCount = reader.ReadInt32();
            for (int i = 0; i < trailingHeadersCount; i++)
            {
                var key = reader.ReadString();
                var valueCount = reader.ReadInt32();
                for (int j = 0; j < valueCount; j++)
                {
                    var value = reader.ReadString();
                    message.Content?.Headers.TryAddWithoutValidation(key, value);
                }
            }

            var memoryStream = new MemoryStream();
            await reader.BaseStream.CopyToAsync(memoryStream, cancellation);
            memoryStream.Position = 0;
            message.Content = new StreamContent(memoryStream);

            return message;
        }
        catch (Exception ex) when(cancellation.IsCancellationRequested == false)
        {
            throw new HttpMessageBinaryFormatterException("Error occurred during deserialization", ex);
        }
    }

    /// <summary>
    /// Writes a <see cref="HttpResponseMessage"/> to a <see cref="Stream"/> in a binary format.
    /// </summary>
    /// <param name="response">The <see cref="HttpResponseMessage"/> to write.</param>
    /// <param name="stream">The stream to which the <see cref="HttpResponseMessage"/> should be written.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous write operation.</returns>
    /// <exception cref="HttpMessageBinaryFormatterException">Thrown when an error occurs during serialization and the operation was not cancelled.</exception>
    /// <remarks>
    /// This method writes a <see cref="HttpResponseMessage"/> to a stream in binary format. The format includes the HTTP status code, headers, and content.
    /// This is a lower-level operation than <see cref="ToByteArrayAsync(HttpResponseMessage, CancellationToken)"/>, and you would typically use it when you want to serialize a message directly to a stream, such as a network stream or a file stream, rather than to a byte array.
    /// </remarks>
    public static async Task WriteAsync(
        HttpResponseMessage response,
        Stream stream,
        CancellationToken cancellation = default)
    {
        try
        {
            var writer = new BinaryWriter(stream);

            writer.Write((int)response.StatusCode);
            writer.Write(response.Version.ToString());
            writer.Write(response.ReasonPhrase ?? String.Empty);
            writer.Write(response.Headers.Count());

            var values = new List<string>();
            foreach (var header in response.Headers)
            {
                values.Clear();
                values.AddRange(header.Value);
                writer.Write(header.Key);
                writer.Write(values.Count);
                foreach (var value in values)
                {
                    writer.Write(value);
                }
            }

            var trailingHeaders = response.Content.Headers;
            writer.Write(trailingHeaders.Count());
            foreach (var header in trailingHeaders)
            {
                values.Clear();
                values.AddRange(header.Value);
                writer.Write(header.Key);
                writer.Write(values.Count);
                foreach (var value in values)
                {
                    writer.Write(value);
                }
            }
            writer.Flush();
            await response.Content.CopyToAsync(stream, cancellation);
        }
        catch (Exception ex) when (cancellation.IsCancellationRequested == false)
        {
            throw new HttpMessageBinaryFormatterException("Error occurred during serialization", ex);
        }
    }

    /// <summary>
    /// Reads a <see cref="HttpResponseMessage"/> from a <see cref="Stream"/> that contains a serialized HttpResponseMessage in binary format.
    /// </summary>
    /// <param name="stream">The stream from which the <see cref="HttpResponseMessage"/> should be read.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous read operation. The value of the <see cref="Task{TResult}.Result"/> parameter contains the deserialized <see cref="HttpResponseMessage"/>.</returns>
    /// <exception cref="HttpMessageBinaryFormatterException">Thrown when an error occurs during deserialization and the operation was not cancelled.</exception>
    /// <remarks>
    /// This method reads a serialized <see cref="HttpResponseMessage"/> in binary format from a stream and deserializes it. The method expects the data in the stream to be in the format produced by the <see cref="WriteAsync(HttpResponseMessage, Stream, CancellationToken)"/> method.
    /// This is a lower-level operation than <see cref="ToResponseAsync(byte[], CancellationToken)"/>, and you would typically use it when you want to deserialize a message directly from a stream, such as a network stream or a file stream, rather than from a byte array.
    /// </remarks>
    public static async Task<HttpResponseMessage> ReadResponseAsync(
        Stream stream,
        CancellationToken cancellation = default)
    {
        try
        {
            var reader = new BinaryReader(stream);

            var statusCode = (HttpStatusCode)reader.ReadInt32();
            var version = Version.Parse(reader.ReadString());

            var message = new HttpResponseMessage
            {
                StatusCode = statusCode,
                Version = version,
                ReasonPhrase = reader.ReadString()
            };

            int headersCount = reader.ReadInt32();
            for (int i = 0; i < headersCount; i++)
            {
                var key = reader.ReadString();
                var valueCount = reader.ReadInt32();
                for (int j = 0; j < valueCount; j++)
                {
                    var value = reader.ReadString();
                    message.Headers.TryAddWithoutValidation(key, value);
                }
            }

            int trailingHeadersCount = reader.ReadInt32();

            for (int i = 0; i < trailingHeadersCount; i++)
            {
                var key = reader.ReadString();
                var valueCount = reader.ReadInt32();
                for (int j = 0; j < valueCount; j++)
                {
                    var value = reader.ReadString();
                    message.Content?.Headers.TryAddWithoutValidation(key, value);
                }
            }

            var memoryStream = new MemoryStream();
            await reader.BaseStream.CopyToAsync(memoryStream, cancellation);
            memoryStream.Position = 0;
            message.Content = new StreamContent(memoryStream);

            return message;
        }
        catch (Exception ex) when (cancellation.IsCancellationRequested == false)
        {
            throw new HttpMessageBinaryFormatterException("Error occurred during deserialization", ex);
        }
    }
}