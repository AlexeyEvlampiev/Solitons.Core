using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues.Common
{
    public sealed class CloudQueue : CloudQueueBase
    {
        private readonly IDomainContractSerializer _serializer;
        private readonly ICloudQueueProvider _queueProvider;
        private readonly ITransientStorage _transientStorage;

        public CloudQueue(
            IDomainContractSerializer serializer,
            ICloudQueueProvider queueProvider,
            ITransientStorage transientStorage)
        {
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _queueProvider = queueProvider ?? throw new ArgumentNullException(nameof(queueProvider));
            _transientStorage = transientStorage ?? throw new ArgumentNullException(nameof(transientStorage));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override QueueMessageOptions CreateMessageOptions()
        {
            return new QueueMessageOptions(_queueProvider.MaxMessageSize);
        }


        protected override Task SendAsync(byte[] body, TimeSpan? visibilityTimeout, TimeSpan? messageTtl)
        {
            return _queueProvider.SendAsync(body, visibilityTimeout, messageTtl);
        }

        protected override string Serialize(object dto, string contentType)
        {
            return _serializer.Serialize(dto, contentType);
        }

        protected override string Serialize(object dto, out string contentType)
        {
            return _serializer.Serialize(dto, out contentType);
        }

        protected override Task<TransientStorageReceipt> StoreAsync(byte[] bytes, TimeSpan expiresAfter, CancellationToken cancellation)
        {
            return _transientStorage.UploadAsync(bytes, expiresAfter, cancellation);
        }
    }
}
