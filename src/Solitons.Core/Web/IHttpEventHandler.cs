using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHttpEventHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="logger"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IWebResponse> InvokeAsync(
            IWebRequest request, 
            IAsyncLogger logger, 
            CancellationToken cancellation);
    }
}