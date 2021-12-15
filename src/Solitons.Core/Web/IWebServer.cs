using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Web
{
    public interface IWebServer
    {
        Task<WebResponse> InvokeAsync(
            IWebRequest request,
            IAsyncLogger logger, 
            CancellationToken cancellation = default);
    }
}
