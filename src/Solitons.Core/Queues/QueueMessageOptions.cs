using System;

namespace Solitons.Queues
{
    public sealed class QueueMessageOptions
    {

        public QueueMessageOptions(int maxMessageSize)
        {
            MaxMessageSize = maxMessageSize;
        }

        public int MaxMessageSize { get; }

        public string ContentType { get; set; }
 
        public TimeSpan MessageTimeToLive { get; set; }

        public TimeSpan MessageVisibilityTimeout { get; set; }
        public Guid IntentId { get; set; }
        public string IntentName { get; set; }
    }
}
