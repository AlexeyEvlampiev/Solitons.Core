using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatabaseRpcModule
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <returns></returns>
        bool Contains(Guid commandId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        Task<MediaContent> InvokeAsync(Guid commandId, MediaContent request, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandId"></param>
        /// <param name="content"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        Task SendAsync(Guid commandId, MediaContent content, CancellationToken cancellation = default);
    }
}
