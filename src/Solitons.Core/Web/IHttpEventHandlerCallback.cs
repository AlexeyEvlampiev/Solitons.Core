using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Web
{
    public interface IHttpEventHandlerCallback
    {
        void OnFoundMultipleRoutes(WebRequest domainWebRequest, IAsyncLogger logger);
        void OnFoundNoRoutes(WebRequest domainWebRequest, IAsyncLogger logger);
        void OnNullResponseObject(WebRequest domainWebRequest, IAsyncLogger logger);
    }
}
