using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IQueueServiceProvider
    {
        /// <summary>
        /// 
        /// </summary>
        int MessageMaxSizeInBytes { get; }

        /// <summary>
        /// 
        /// </summary>
        TimeSpan MessageMaxTimeToLive { get; }

        /// <summary>
        /// 
        /// </summary>
        TimeSpan MinMessageVisibilityTimeout { get; }

        /// <summary>
        /// 
        /// </summary>
        TimeSpan MaxMessageVisibilityTimeout { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="options"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task SendAsync(byte[] body, IQueueMessageOptions options, CancellationToken cancellation);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="visibilityTimeout"></param>
        /// <param name="callbackRequiredBehaviour"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<IQueueMessage> ReceiveAsync(TimeSpan visibilityTimeout, QueueConsumerBehaviour callbackRequiredBehaviour, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxBatchSize"></param>
        /// <param name="visibilityTimeout"></param>
        /// <param name="behaviour"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        IObservable<IQueueMessage> ReceiveBatchAsync(
            int maxBatchSize,
            TimeSpan visibilityTimeout,
            QueueConsumerBehaviour behaviour,
            CancellationToken cancellation);
    }

    public partial interface IQueueServiceProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IQueueServiceProvider AsQueueServiceProvider() => QueueServiceProviderProxy.Wrap(this);
    }

}
