using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IDomainQueueProducer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="config"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<DomainTransientStorageReceipt> SendMessageAsync(
            object dto,
            Action<DomainQueueMessageOptions> config = default,
            CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dtoType"></param>
        /// <returns></returns>
        bool CanSend(Type dtoType);
    }

    public partial interface IDomainQueueProducer
    {
        public bool CanSend<T>() => CanSend(typeof(T));

        public bool CanSend(object dto) => dto != null && CanSend(dto.GetType());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IDomainQueueProducer AsDomainQueueProducer() => DomainQueueProducerProxy.Wrap(this);
    }
}
