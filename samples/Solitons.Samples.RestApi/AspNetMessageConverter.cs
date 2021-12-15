using Solitons.Web;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Extensions;
using Solitons.Samples.Domain;
using System.Text.RegularExpressions;

namespace Solitons.Samples.RestApi
{
    public class AspNetMessageConverter
    {
        private readonly IDomainSerializer _domainSerialzer;

        sealed class ContentResult : IResult
        {
            public ContentResult(int status, string content, string contentType)
            {
                Status = status;
                Content = content;
                ContentType = contentType;
            }

            public int Status { get; }
            public string Content { get; }
            public string ContentType { get; }

            public Task ExecuteAsync(HttpContext httpContext)
            {
                httpContext.Response.StatusCode = Status;
                httpContext.Response.ContentType = ContentType;
                return httpContext.Response.WriteAsync(Content);
            }
        }

        public AspNetMessageConverter()
        {
            _domainSerialzer = SampleDomainContext.GetOrCreate().GetSerializer();
        }
        public IResult ToAspNetResult(WebResponse response, IWebRequest request)
        {
            if (response is ObjectWebResponse objResponse)
            {
                var body = objResponse.Object;
                var supportedTypes = _domainSerialzer.GetSupportedContentTypes(body.GetType().GUID);
                var accept = request.Accept.ToList();
                var contentType = supportedTypes
                    .Intersect(accept, StringComparer.OrdinalIgnoreCase)
                    .FirstOrDefault();


                var content = contentType is null
                        ? _domainSerialzer.Serialize(objResponse.Object, out contentType)
                        : _domainSerialzer.Serialize(objResponse.Object, contentType);

                var result = Results.Content(content, contentType);

                return result;
            }
            else if (response is ContentWebResponse contentResponse) 
            {
                return new ContentResult((int)contentResponse.Status, contentResponse.Content, contentResponse.ContentType);

            }

            return Results.StatusCode((int)response.Status);
        }

        public IWebRequest ToWebRequest(HttpRequest source, ClaimsPrincipal caller)
        {
            return new AspNetWebRequest(source, caller);
        }

        sealed class AspNetWebRequest : IWebRequest
        {
            private readonly HttpRequest _request;
            private readonly ClaimsPrincipal _caller;

            public AspNetWebRequest(HttpRequest request, ClaimsPrincipal caller)
            {
                _request = request;
                _caller = caller;
            }

            public string Uri => _request.GetEncodedUrl();

            public string Method => _request.Method;

            public Version ClientVersion => Version.Parse("1.0.0");

            public ClaimsPrincipal Caller => _caller;

            public IEnumerable<string> QueryParameterNames => throw new NotImplementedException();

            public System.Net.IPAddress? IPAddress => _request.HttpContext?.Connection?.RemoteIpAddress;

            public string ContentType => _request.ContentType;

            public IEnumerable<string> Accept
            {
                get
                {

                    return _request.Headers.Accept
                        .SelectMany(value => Regex.Split(value, @"\s*[,;]\s*"))
                        .Where(s=> s.IsNullOrWhiteSpace() == false);
                        
                }
            }

            public Stream GetBody()
            {
                return _request.Body;
            }

            public IEnumerable<string> GetQueryParameterValues(string name)
            {
                throw new NotImplementedException();
            }
        }
    }
}
