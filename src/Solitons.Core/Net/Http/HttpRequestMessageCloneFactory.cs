using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace Solitons.Net.Http;

/// <summary>
/// Represents a factory for creating clones of HttpRequestMessage instances.
/// </summary>
public sealed class HttpRequestMessageCloneFactory
{
    private readonly Func<HttpRequestMessage> _cloneBuilder;

    /// <summary>
    /// Initializes a new instance of the HttpRequestMessageCloneFactory class.
    /// </summary>
    /// <param name="factory">A delegate representing the factory method for creating clones of HttpRequestMessage instances.</param>
    private HttpRequestMessageCloneFactory(Func<HttpRequestMessage> factory)
    {
        _cloneBuilder = factory;
    }

    /// <summary>
    /// Asynchronously creates a new HttpRequestMessageCloneFactory from a given HttpRequestMessage.
    /// </summary>
    /// <param name="message">The HttpRequestMessage to clone.</param>
    /// <returns>A Task resulting in a new HttpRequestMessageCloneFactory which can be used to create clones of the given HttpRequestMessage.</returns>
    public static async Task<HttpRequestMessageCloneFactory> FromAsync(HttpRequestMessage message)
    {
        var memory = new MemoryStream();
        if (message.Content != null)
        {
            await message.Content.CopyToAsync(memory);
        }

        HttpMethod method = message.Method;
        Uri requestUri = message.RequestUri;
        byte[] content = memory.ToArray();
        HttpHeaders headers = message.Headers;
        Version version = message.Version;

        HttpRequestMessage Factory()
        {
            var clone = new HttpRequestMessage(method, requestUri)
            {
                Version = version
            };

            foreach (var header in headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            if (message.Content != null)
            {
                clone.Content = new ByteArrayContent(content);
                foreach (var header in message.Content.Headers)
                {
                    clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            return clone;
        }

        return new HttpRequestMessageCloneFactory(Factory);
    }

    /// <summary>
    /// Creates a new HttpRequestMessage using the clone builder function.
    /// </summary>
    /// <returns>A clone of the HttpRequestMessage object.</returns>
    [DebuggerStepThrough]
    public HttpRequestMessage Create() => _cloneBuilder.Invoke();
}
