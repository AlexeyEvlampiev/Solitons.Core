using System;
using System.Threading.Tasks;

namespace Solitons.Queues.Common
{
    public abstract class CloudQueueProvider : ICloudQueueProvider
    {
        protected CloudQueueProvider(int maxMessageSize)
        {
            MaxMessageSize = maxMessageSize;
        }

        protected abstract Task SendAsync(byte[] body, TimeSpan? visibilityTimeout, TimeSpan? messageTtl);

        /// <summary>
        /// 
        /// </summary>
        public int MaxMessageSize { get; }

        Task ICloudQueueProvider.SendAsync(byte[] body, TimeSpan? visibilityTimeout, TimeSpan? messageTtl)
        {
            if (body == null) throw new ArgumentNullException(nameof(body));
            visibilityTimeout?.ThrowIfArgumentLessThan(TimeSpan.Zero, nameof(visibilityTimeout));
            messageTtl?.ThrowIfArgumentLessThan(TimeSpan.Zero, nameof(messageTtl));
            return SendAsync(body, visibilityTimeout, messageTtl);
        }

    }
}
