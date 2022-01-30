using System;
using System.Collections.Generic;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface ICloudQueueConsumer
    {
        /// <summary>
        /// Converts to observable.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        IObservable<object> ToObservable(IQueueStreamConsumerCallback callback, IAsyncLogger logger);

        /// <summary>
        /// Converts to observable.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="logger">The logger.</param>
        /// <returns></returns>
        IObservable<IReadOnlyList<object>> ToObservable(IQueueBatchConsumerCallback callback, IAsyncLogger logger);
    }

    public partial interface ICloudQueueConsumer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="logger"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task ConsumeAsync(
            IQueueStreamConsumerCallback callback,
            IAsyncLogger logger,
            CancellationToken cancellation = default)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            cancellation.ThrowIfCancellationRequested();
            var dtoStream = ToObservable(callback, logger);
            await dtoStream
                .ToTask(cancellation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="logger"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task ConsumeAsync(IQueueBatchConsumerCallback callback, IAsyncLogger logger, CancellationToken cancellation = default)
        {
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            cancellation.ThrowIfCancellationRequested();
            var dtoStream = ToObservable(callback, logger);
            await dtoStream
                .ToTask(cancellation);
        }
    }
}
