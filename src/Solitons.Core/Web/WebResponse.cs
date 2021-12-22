using System.Net;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class WebResponse
    {
        internal WebResponse(HttpStatusCode status)
        {
            Status = status;
        }

        public HttpStatusCode Status { get; }

        public static WebResponse Create(HttpStatusCode status) => new(status);

        public static ContentWebResponse Create(HttpStatusCode status, string content, string contentType = "text/plain") => new(status, content, contentType);

        public static ObjectWebResponse Create(HttpStatusCode status, object obj) => new(status, obj);    
    }
}
