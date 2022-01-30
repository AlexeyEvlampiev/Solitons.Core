using System;
using System.Threading.Tasks;

namespace Solitons.Queues.Common
{
    public interface ICloudQueueProvider
    {
        int MaxMessageSize { get; }
        Task SendAsync(byte[] body, TimeSpan? visibilityTimeout, TimeSpan? messageTtl);
    }
}
