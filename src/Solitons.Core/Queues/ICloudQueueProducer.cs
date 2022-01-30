using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICloudQueueProducer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="config"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<TransientStorageReceipt> SendAsync(
            object dto,
            Action<QueueMessageOptions>? config = null,
            CancellationToken cancellation = default);
    }
}
