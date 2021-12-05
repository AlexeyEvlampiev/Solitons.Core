using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class QueueServiceProvider : IQueueServiceProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxMessageSizeInBytes"></param>
        /// <param name="maxMessageTtl"></param>
        protected QueueServiceProvider(int maxMessageSizeInBytes, TimeSpan maxMessageTtl)
        {
            MessageMaxSizeInBytes = maxMessageSizeInBytes
                .ThrowIfArgumentLessThan(0, nameof(maxMessageSizeInBytes));
            MessageMaxTimeToLive = maxMessageTtl
                .ThrowIfArgumentLessThan(TimeSpan.Zero, nameof(maxMessageTtl));
        }


        public int MessageMaxSizeInBytes { get; }
        public TimeSpan MessageMaxTimeToLive { get; }
        public TimeSpan MinMessageVisibilityTimeout { get; }
        public TimeSpan MaxMessageVisibilityTimeout { get; }

        protected abstract Task SendAsync(
            byte[] body,
            IQueueMessageOptions options,
            CancellationToken cancellation);

        public Task<IQueueMessage> ReceiveAsync(TimeSpan visibilityTimeout, QueueConsumerBehaviour callbackRequiredBehaviour,
            CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public IObservable<IQueueMessage> ReceiveBatchAsync(
            int maxBatchSize, 
            TimeSpan visibilityTimeout, 
            QueueConsumerBehaviour behaviour,
            CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }


        [DebuggerStepThrough]
        async Task IQueueServiceProvider.SendAsync(
            byte[] body, 
            IQueueMessageOptions options,
            CancellationToken cancellation)
        {
            if (body == null) throw new ArgumentNullException(nameof(body));
            if (options == null) throw new ArgumentNullException(nameof(options));
            cancellation.ThrowIfCancellationRequested();

            await SendAsync(body, options, cancellation);
        }
    }
}
