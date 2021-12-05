using System;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Queues;

namespace Solitons.Azure.Referencelmpl
{
    public sealed class AzureStorageQueueServiceProvider : IQueueServiceProvider
    {


        public AzureStorageQueueServiceProvider(string connectionString, string v)
        {

        }

        public int MessageMaxSizeInBytes { get; }
        public TimeSpan MessageMaxTimeToLive { get; }
        public TimeSpan MinMessageVisibilityTimeout { get; }
        public TimeSpan MaxMessageVisibilityTimeout { get; }
        public Task SendAsync(byte[] body, IQueueMessageOptions options, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public Task<IQueueMessage> ReceiveAsync(TimeSpan visibilityTimeout, QueueConsumerBehaviour callbackRequiredBehaviour,
            CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        public IObservable<IQueueMessage> ReceiveBatchAsync(int maxBatchSize, TimeSpan visibilityTimeout, QueueConsumerBehaviour behaviour,
            CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}
