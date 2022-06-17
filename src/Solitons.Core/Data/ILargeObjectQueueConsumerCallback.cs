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
        Task ProcessAsync(DataTransferPackage package, object dto, CancellationToken cancellation);
        Task OnQueueIsEmptyAsync(CancellationToken cancellation);
        Task OnMalformedMessageAsync(string messageId, Exception exception, CancellationToken cancellation);
        Task OnUnpackingErrorAsync(string messageId, DataTransferPackage package, Exception exception, CancellationToken cancellation);
        Task OnFailedDeletingMessageAsync(string messageId, DataTransferPackage package, object dto, CancellationToken cancellation);
        bool CanDeleteFailedMessage(Exception exception);
    }
}
