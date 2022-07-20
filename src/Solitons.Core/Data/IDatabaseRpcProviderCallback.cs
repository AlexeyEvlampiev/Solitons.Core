using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    public interface IDatabaseRpcProviderCallback
    {
        Task OnStartingInvocationAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation);
        Task OnInvocationCompletedAsync(DatabaseRpcCommandMetadata metadata, string request, string response, CancellationToken cancellation);
        Task OnInvocationErrorAsync(DatabaseRpcCommandMetadata metadata, string request, Exception exception, CancellationToken cancellation);
        Task OnSendingAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation);
        Task OnSentAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation);
        Task OnSendingErrorAsync(DatabaseRpcCommandMetadata metadata, string request, Exception exception, CancellationToken cancellation);
        Task OnQueueProcessingStartedAsync(string queueName, CancellationToken cancellation);
        Task OnQueueProcessingFinishedAsync(string queueName, CancellationToken cancellation);
        Task OnQueueProcessingErrorAsync(string queueName, Exception exception, CancellationToken cancellation);
    }
}
