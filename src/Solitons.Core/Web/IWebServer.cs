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
