using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

public static class HttpMessageSerializer
{
    public static async Task<byte[]> SerializeAsync(
        HttpRequestMessage request,
        CancellationToken cancellation = default)
    {
        await using var memoryStream = new MemoryStream();
        await SerializeAsync(request, memoryStream, cancellation);
        return memoryStream.ToArray();
    }

    public static async Task<byte[]> SerializeAsync(
        HttpResponseMessage response,
        CancellationToken cancellation = default)
    {
        await using var memoryStream = new MemoryStream();
        await SerializeAsync(response, memoryStream, cancellation);
        return memoryStream.ToArray();
    }

    public static async Task<HttpRequestMessage> DeserializeRequestAsync(
        byte[] bytes,
        CancellationToken cancellation = default)
    {
        await using var memoryStream = new MemoryStream(bytes);
        return await DeserializeRequestAsync(memoryStream, cancellation);
    }

    public static async Task<HttpResponseMessage> DeserializeResponseAsync(
        byte[] bytes,
        CancellationToken cancellation = default)
    {
        using var memoryStream = new MemoryStream(bytes);
        return await DeserializeResponseAsync(memoryStream, cancellation);
    }

    public static async Task SerializeAsync(
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
            throw new HttpMessageSerializationException("Error occurred during serialization", ex);
        }
    }

    public static async Task<HttpRequestMessage> DeserializeRequestAsync(
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
            throw new HttpMessageSerializationException("Error occurred during deserialization", ex);
        }
    }

    public static async Task SerializeAsync(
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
            throw new HttpMessageSerializationException("Error occurred during serialization", ex);
        }
    }

    public static async Task<HttpResponseMessage> DeserializeResponseAsync(
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
            throw new HttpMessageSerializationException("Error occurred during deserialization", ex);
        }
    }
}