using System;

namespace Solitons.Queues
{
    public sealed class QueueMessageOperationException : InvalidOperationException
    {

        private readonly IQueueMessage _queueMessage;

        public QueueMessageOperationException(IQueueMessage queueMessage,
            QueueServiceErrorCodes errorCode, Exception innerException) : base(innerException.Message, innerException)
        {
            ErrorCode = errorCode;
            _queueMessage = queueMessage ?? throw new ArgumentNullException(nameof(queueMessage));
        }

        public QueueServiceErrorCodes ErrorCode { get; }

        public bool CausedBy(IQueueMessage queueMessage)
        {
            return queueMessage != null && ReferenceEquals(queueMessage, _queueMessage);
        }
    }
}
