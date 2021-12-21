using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Web
{
    public interface IDomainHttpEventHandler
    {
        bool CanProcess(WebRequest domainWebRequest);
        Task<WebResponse> InvokeAsync(WebRequest domainWebRequest, IAsyncLogger logger, CancellationToken cancellation);
    }
}
