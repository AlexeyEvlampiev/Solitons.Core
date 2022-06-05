using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatabaseRpcCommand
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        bool CanAccept(MediaContent request);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<MediaContent> InvokeAsync(MediaContent request, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task SendAsync(MediaContent request, CancellationToken cancellation = default);
    }
}
