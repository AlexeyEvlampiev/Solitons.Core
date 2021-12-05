using System.Net;

namespace Solitons.Web
{
    public record ContentWebResponse : IWebResponse
    {
        private const string DefaultContentType = "text/plain";
        public ContentWebResponse(HttpStatusCode statusCode, string content, string contentType = DefaultContentType)
        {
            StatusCode = statusCode;
            Content = content ?? string.Empty;
            ContentType = contentType.DefaultIfNullOrWhiteSpace(DefaultContentType);
        }

        public HttpStatusCode StatusCode { get; }
        public string Content { get; }
        public string ContentType { get; }
    }
}
