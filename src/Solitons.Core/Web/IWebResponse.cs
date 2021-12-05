using System.Diagnostics;
using System.Net;

namespace Solitons.Web
{
    public interface IWebResponse
    {
        HttpStatusCode StatusCode { get; }

        public bool IsSuccessStatusCode => StatusCode.IsSuccessStatusCode();
        public bool IsRedirectStatusCode => StatusCode.IsRedirectStatusCode();

        [DebuggerNonUserCode]
        public static IWebResponse Create(HttpStatusCode statusCode) => new StatusCodeWebResponse(statusCode);

        [DebuggerNonUserCode]
        public static IWebResponse Create(HttpStatusCode statusCode, string content, string contentType="text/plain") 
            => new ContentWebResponse(statusCode, content, contentType);
    }
}
