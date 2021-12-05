using System.Diagnostics;
using System.Net;

namespace Solitons.Web
{
    public class StatusCodeWebResponse : IWebResponse
    {
        [DebuggerNonUserCode]
        public StatusCodeWebResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }

        public override string ToString() => StatusCode.ToString();
    }
}
