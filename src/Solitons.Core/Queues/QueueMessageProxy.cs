using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Solitons.Queues
{
    sealed class QueueMessageProxy : IQueueMessage
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IQueueMessage _innerMessage;

        [DebuggerNonUserCode]
        private QueueMessageProxy(IQueueMessage innerMessage)
        {
            _innerMessage = innerMessage ?? throw new ArgumentNullException(nameof(innerMessage));
        }

        [DebuggerNonUserCode]
        public static IQueueMessage Wrap(IQueueMessage innerMessage)
        {
            if (innerMessage == null) throw new ArgumentNullException(nameof(innerMessage));
            return innerMessage is QueueMessageProxy proxy
                ? proxy
                : new QueueMessageProxy(innerMessage);
        }


        public byte[] Body => _innerMessage.Body;


        [DebuggerStepThrough]
        public ValueTask DisposeAsync() => _innerMessage.DisposeAsync();


        [DebuggerStepThrough]
        public bool IsTransientError(Exception exception) => _innerMessage.IsTransientError(exception);

        [DebuggerStepThrough]
        public override string ToString() => _innerMessage.ToString();

        [DebuggerStepThrough]
        public override bool Equals(object? obj) => _innerMessage.Equals(obj);

        [DebuggerStepThrough]
        public override int GetHashCode() => _innerMessage.GetHashCode();
    }
}
