using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQueueActiveMessage : IQueueMessage
    {        /// <summary>
        /// Asynchronously completes the receive operation of this message and indicates that the message should be marked as processed and deleted.
        /// </summary>
        /// <param name="throwIfAlreadyCompleted"></param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task<bool> CompleteAsync(bool throwIfAlreadyCompleted = true, CancellationToken cancellation = default);

        /// <summary>
        /// Asynchronously abandons the message lock.
        /// </summary>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task<bool> AbandonAsync(CancellationToken cancellation = default);
    }
}
