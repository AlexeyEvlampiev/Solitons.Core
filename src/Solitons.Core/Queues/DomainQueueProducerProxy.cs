using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues
{
    sealed class DomainQueueProducerProxy : IDomainQueueProducer
    {
        private readonly IDomainQueueProducer _innerProducer;

        private DomainQueueProducerProxy(IDomainQueueProducer innerProducer)
        {
            _innerProducer = innerProducer;
        }

        [DebuggerStepThrough]
        public static IDomainQueueProducer Wrap(IDomainQueueProducer innerProducer)
        {
            if (innerProducer == null) throw new ArgumentNullException(nameof(innerProducer));
            return innerProducer is DomainQueueProducerProxy proxy
                ? proxy
                : new DomainQueueProducerProxy(innerProducer);
        }

        [DebuggerStepThrough]
        public override string ToString() => _innerProducer.ToString();

        [DebuggerStepThrough]
        public override bool Equals(object? obj) => _innerProducer.Equals(obj);

        [DebuggerStepThrough]
        public override int GetHashCode() => _innerProducer.GetHashCode();

        public Task<DomainTransientStorageReceipt> SendMessageAsync(object dto, Action<DomainQueueMessageOptions> config = default, CancellationToken cancellation = default)
        {
            return _innerProducer.SendMessageAsync(dto, config, cancellation);
        }

        public bool CanSend(Type dtoType)
        {
            return _innerProducer.CanSend(dtoType);
        }
    }
}
