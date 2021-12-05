using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues
{
    sealed class QueueServiceProviderProxy : IQueueServiceProvider
    {
        private readonly IQueueServiceProvider _innerProvider;

        public QueueServiceProviderProxy(IQueueServiceProvider innerProvider)
        {
            _innerProvider = innerProvider;
        }

        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IQueueServiceProvider Wrap(IQueueServiceProvider innerProvider)
        {
            if (innerProvider == null) throw new ArgumentNullException(nameof(innerProvider));
            return innerProvider is QueueServiceProviderProxy proxy
                ? proxy
                : new QueueServiceProviderProxy(innerProvider);
        }


        public TimeSpan MaxMessageVisibilityTimeout => _innerProvider.MaxMessageVisibilityTimeout;

        public Task SendAsync(byte[] body, IQueueMessageOptions options, CancellationToken cancellation)
        {
            return _innerProvider.SendAsync(body, options, cancellation);
        }


        public Task<IQueueMessage> ReceiveAsync(TimeSpan visibilityTimeout, QueueConsumerBehaviour callbackRequiredBehaviour,
            CancellationToken cancellation)
        {
            return _innerProvider.ReceiveAsync(visibilityTimeout, callbackRequiredBehaviour, cancellation);
        }

        public IObservable<IQueueMessage> ReceiveBatchAsync(int maxBatchSize, TimeSpan visibilityTimeout, QueueConsumerBehaviour behaviour,
            CancellationToken cancellation)
        {
            return _innerProvider.ReceiveBatchAsync(maxBatchSize, visibilityTimeout, behaviour, cancellation);
        }

        public TimeSpan MessageMaxTimeToLive => _innerProvider.MessageMaxTimeToLive;
        public TimeSpan MinMessageVisibilityTimeout => _innerProvider.MinMessageVisibilityTimeout;

        public int MessageMaxSizeInBytes => _innerProvider.MessageMaxSizeInBytes;


        [DebuggerStepThrough]
        public override string ToString() => _innerProvider.ToString();

        [DebuggerStepThrough]
        public override bool Equals(object? obj) => _innerProvider.Equals(obj);

        [DebuggerStepThrough]
        public override int GetHashCode() => _innerProvider.GetHashCode();
    }
}
