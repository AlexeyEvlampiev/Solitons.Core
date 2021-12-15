using System.Net;

namespace Solitons.Web
{
    public class ContentWebResponse : WebResponse
    {
        private const string DefaultContentType = "text/plain";
        internal ContentWebResponse(HttpStatusCode statusCode, string content, string contentType = DefaultContentType) : base(statusCode)
        {
            Content = content ?? string.Empty;
            ContentType = contentType.DefaultIfNullOrWhiteSpace(DefaultContentType);
        }

        public string Content { get; }
        public string ContentType { get; }
    }
}
