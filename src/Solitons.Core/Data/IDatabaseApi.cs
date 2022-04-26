using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatabaseApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<MediaContent> InvokeAsync(
            Guid commandId, 
            MediaContent request,
            IDatabaseApiCallback callback,
            CancellationToken cancellation = default);
    }

}
