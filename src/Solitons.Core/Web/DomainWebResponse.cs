using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Web
{
    public abstract class DomainWebResponse : IWebResponse
    {
        public HttpStatusCode StatusCode => throw new NotImplementedException();
    }
}
