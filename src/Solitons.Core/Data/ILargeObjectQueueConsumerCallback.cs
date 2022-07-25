using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILargeObjectQueueConsumerCallback
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task OnStartingAsync(CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <param name="dto"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task ProcessAsync(DataTransferPackage package, object dto, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task OnQueueIsEmptyAsync(CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="exception"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task OnMalformedMessageAsync(string messageId, Exception exception, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="package"></param>
        /// <param name="exception"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task OnUnpackingErrorAsync(string messageId, DataTransferPackage package, Exception exception, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageId"></param>
        /// <param name="package"></param>
        /// <param name="dto"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task OnFailedDeletingMessageAsync(string messageId, DataTransferPackage package, object dto, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        bool CanDeleteFailedMessage(Exception exception);
    }
}
