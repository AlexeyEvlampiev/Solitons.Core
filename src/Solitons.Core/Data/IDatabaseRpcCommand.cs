using System;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        Task SendAsync(object dto, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="dto"></param>
        /// <param name="config"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task SendViaAsync(
            ILargeObjectQueueProducer queue,
            object dto,
            Action<DataTransferPackage> config,
            CancellationToken cancellation = default);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="dto"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task SendViaAsync(
            ILargeObjectQueueProducer queue,
            object dto,
            CancellationToken cancellation = default);
    }
}
