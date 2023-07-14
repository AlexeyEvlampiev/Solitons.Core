
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

/// <summary>
/// A factory class to create clones of HttpResponseMessage objects.
/// </summary>
/// <remarks>
/// This class is useful for situations where identical HTTP responses need to be created 
/// multiple times, and creating these clones will save on the overhead of recreating the 
/// responses from scratch each time.
/// </remarks>
public sealed class HttpResponseMessageCloneFactory 
{
    private readonly Func<HttpResponseMessage> _cloneBuilder;


    private HttpResponseMessageCloneFactory(Func<HttpResponseMessage> factory)
    {
        _cloneBuilder = factory;
    }

    /// <summary>
    /// Asynchronously creates a new HttpResponseMessageCloneFactory from a given HttpResponseMessage.
    /// </summary>
    /// <param name="message">The HttpResponseMessage to clone.</param>
    /// <returns>A Task resulting in a new HttpResponseMessageCloneFactory which can be used to create clones of the given HttpResponseMessage.</returns>
    public static async Task<HttpResponseMessageCloneFactory> FromAsync(HttpResponseMessage message)
    {
        using var memory = new MemoryStream();
        await message.Content.CopyToAsync(memory);

        HttpStatusCode statusCode = message.StatusCode;
        byte[] content = memory.ToArray(); 
        HttpHeaders headers = message.Headers;
        HttpHeaders trailingHeaders = message.Content.Headers;
        Version version = message.Version;


        HttpResponseMessage Factory()
        {
            var clone = new HttpResponseMessage(statusCode)
            {
                Version = version
            };

            foreach (var header in headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            clone.Content = new ByteArrayContent(content);

            foreach (var header in trailingHeaders)
            {
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        return new HttpResponseMessageCloneFactory(Factory);
    }

    /// <summary>
    /// Creates a new HttpResponseMessage using the clone builder function.
    /// </summary>
    /// <returns>A clone of the HttpResponseMessage object.</returns>
    [DebuggerStepThrough]
    public HttpResponseMessage Create() => _cloneBuilder.Invoke();
}
