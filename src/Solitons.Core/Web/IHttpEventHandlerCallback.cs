using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Web
{
    public interface IHttpEventHandlerCallback
    {
        void OnFoundMultipleRoutes(DomainWebRequest domainWebRequest, IAsyncLogger logger);
        void OnFoundNoRoutes(DomainWebRequest domainWebRequest, IAsyncLogger logger);
        void OnNullResponseObject(DomainWebRequest domainWebRequest, IAsyncLogger logger);
    }
}
