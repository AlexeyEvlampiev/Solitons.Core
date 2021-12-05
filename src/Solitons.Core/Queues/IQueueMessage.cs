using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IQueueMessage : IAsyncDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        byte[] Body { get; }
        

        /// <summary>
        /// Determines whether the specified exception is a transient exception.
        /// </summary>
        /// <param name="exception">The exception to check.</param>
        /// <returns>
        ///   <c>true</c> if the exception is a transient exception and can be retried; otherwise, <c>false</c>.
        /// </returns>
        bool IsTransientError(Exception exception);
    }

    public partial interface IQueueMessage
    {
        /// <summary>
        /// Completes if active asynchronous.
        /// </summary>
        /// <param name="throwIfAlreadyCompleted">if set to <c>true</c> [throw if already completed].</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        public async Task<bool> CompleteIfActiveAsync(
            bool throwIfAlreadyCompleted = true, CancellationToken cancellation = default)
        {
            if (this is IQueueActiveMessage activeMessage)
            {
                await activeMessage.CompleteAsync(throwIfAlreadyCompleted, cancellation);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IQueueMessage AsQueueMessage() => QueueMessageProxy.Wrap(this);
    }
}
