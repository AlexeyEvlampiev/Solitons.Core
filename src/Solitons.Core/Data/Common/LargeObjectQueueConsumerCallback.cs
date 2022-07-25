using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class LargeObjectQueueConsumerCallback : ILargeObjectQueueConsumerCallback
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected virtual Task OnStartingAsync(CancellationToken cancellation)
        {
            Debug.WriteLine($"Starting {GetType()}");
            return Task.CompletedTask;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <param name="dto"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task ProcessAsync(
            DataTransferPackage package, 
            object dto, 
            CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="exception"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task OnMalformedMessageAsync(
            string messageId, 
            Exception exception, 
            CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="package"></param>
        /// <param name="exception"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task OnUnpackingErrorAsync(
            string messageId, 
            DataTransferPackage package, 
            Exception exception,
            CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="package"></param>
        /// <param name="dto"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task OnFailedDeletingMessageAsync(
            string messageId, 
            DataTransferPackage package, 
            object dto,
            CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected virtual bool CanDeleteFailedMessage(Exception exception) => false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected virtual Task OnQueueIsEmptyAsync(CancellationToken cancellation)
        {
            Debug.WriteLine($"{nameof(OnQueueIsEmptyAsync)}");
            return Task.CompletedTask;
        }


        [DebuggerStepThrough]
        Task ILargeObjectQueueConsumerCallback.OnStartingAsync(CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return OnStartingAsync(cancellation);
        }

        [DebuggerStepThrough]
        Task ILargeObjectQueueConsumerCallback.ProcessAsync(DataTransferPackage package, object dto, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return ProcessAsync(package, dto, cancellation);
        }

        [DebuggerStepThrough]
        Task ILargeObjectQueueConsumerCallback.OnQueueIsEmptyAsync(CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return OnQueueIsEmptyAsync(cancellation);
        }

        [DebuggerStepThrough]
        Task ILargeObjectQueueConsumerCallback.OnMalformedMessageAsync(string messageId, Exception exception, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return OnMalformedMessageAsync(messageId, exception, cancellation);
        }

        [DebuggerStepThrough]
        Task ILargeObjectQueueConsumerCallback.OnUnpackingErrorAsync(
            string messageId, 
            DataTransferPackage package, 
            Exception exception,
            CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return OnUnpackingErrorAsync(messageId, package, exception, cancellation);
        }

        [DebuggerStepThrough]
        Task ILargeObjectQueueConsumerCallback.OnFailedDeletingMessageAsync(
            string messageId, 
            DataTransferPackage package, 
            object dto,
            CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return OnFailedDeletingMessageAsync(messageId, package, dto, cancellation);
        }

        [DebuggerStepThrough]
        bool ILargeObjectQueueConsumerCallback.CanDeleteFailedMessage(Exception exception)
        {
            return CanDeleteFailedMessage(exception);
        }
    }
}
