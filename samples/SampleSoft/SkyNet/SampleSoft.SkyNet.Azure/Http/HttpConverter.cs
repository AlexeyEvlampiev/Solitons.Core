using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace SampleSoft.SkyNet.Azure.Http;

public static class HttpConverter
{
    public static HttpRequestMessage Convert(Microsoft.AspNetCore.Http.HttpRequest aspNetRequest)
    {
        var requestMessage = new HttpRequestMessage
        {
            Method = new HttpMethod(aspNetRequest.Method),
            RequestUri = new Uri(aspNetRequest.Scheme + "://" + aspNetRequest.Host + aspNetRequest.Path + aspNetRequest.QueryString),
            Content = new StreamContent(aspNetRequest.Body)
        };

        if (Version.TryParse(aspNetRequest.Protocol, out Version? version))
        {
            requestMessage.Version = version;
        }

        foreach (var header in aspNetRequest.Headers)
        {
            if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()) && requestMessage.Content != null)
            {
                requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
            }
        }

        return requestMessage;
    }

    public static async Task PopulateAsync(HttpResponse aspNetResponse, HttpResponseMessage response)
    {
        // Set the status code and reason phrase
        aspNetResponse.StatusCode = (int)response.StatusCode;
        var aspNetResponseFeature = aspNetResponse.HttpContext.Features.Get<IHttpResponseFeature>();
        if (aspNetResponseFeature != null)
        {
            aspNetResponseFeature.ReasonPhrase = response.ReasonPhrase;
        }

        // Copy the headers
        foreach (var header in response.Headers)
        {
            aspNetResponse.Headers[header.Key] = header.Value.ToArray();
        }

        foreach (var header in response.Content.Headers)
        {
            aspNetResponse.Headers[header.Key] = header.Value.ToArray();
        }

        // Copy the response body
        await response.Content.CopyToAsync(aspNetResponse.Body);
    }
}