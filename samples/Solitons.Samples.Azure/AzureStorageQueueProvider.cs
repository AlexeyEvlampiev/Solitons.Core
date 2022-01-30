using Azure.Storage.Queues;
using Solitons.Queues.Common;

namespace Solitons.Samples.Azure
{
    public sealed class AzureStorageQueueProvider : CloudQueueProvider
    {
        private readonly QueueClient _queue;
        private const int MaxStorageQueueMessageSize = 64000;

        public AzureStorageQueueProvider(QueueClient queue) 
            : base(MaxStorageQueueMessageSize)
        {
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        protected override Task SendAsync(byte[] body, TimeSpan? visibilityTimeout, TimeSpan? messageTtl)
        {
            return _queue.SendMessageAsync(new BinaryData(body), visibilityTimeout, messageTtl);
        }
    }
}
