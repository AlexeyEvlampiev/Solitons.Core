using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

        public static WebResponse Create(HttpStatusCode status) => new WebResponse(status);

        public static ContentWebResponse Create(HttpStatusCode status, string content, string contentType = "text/plain") => new ContentWebResponse(status, content, contentType);

        public static ObjectWebResponse Create(HttpStatusCode status, object obj) => new ObjectWebResponse(status, obj);    
    }
}
