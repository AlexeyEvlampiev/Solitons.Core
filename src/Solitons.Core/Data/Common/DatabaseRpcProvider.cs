using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DatabaseRpcProvider : IDatabaseRpcProvider
    {
        protected abstract Task<string> InvokeAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation);
        protected abstract Task InvokeAsync(DatabaseRpcCommandMetadata metadata, string request, Func<string, Task> callback, CancellationToken cancellation);
        protected abstract Task SendAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation);
        protected abstract Task SendAsync(DatabaseRpcCommandMetadata metadata, string request, Func<Task> callback, CancellationToken cancellation);
        protected abstract Task ProcessQueueAsync(string queueName, CancellationToken cancellation);


        [DebuggerNonUserCode]
        public IDatabaseRpcProvider WithCallback(IDatabaseRpcProviderCallback callback) => DatabaseRpcProviderProxy.Wrap(this, callback);

        [DebuggerStepThrough]
        Task IDatabaseRpcProvider.ProcessQueueAsync(string queueName, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return ProcessQueueAsync(queueName, cancellation);
        }

        [DebuggerStepThrough]
        Task<string> IDatabaseRpcProvider.InvokeAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation)
        {
            metadata = metadata.ThrowIfNullArgument(nameof(metadata));
            request = request.ThrowIfNullOrWhiteSpaceArgument(nameof(request));
            cancellation.ThrowIfCancellationRequested();
            return InvokeAsync(metadata, request, cancellation);
        }

        [DebuggerStepThrough]
        Task IDatabaseRpcProvider.InvokeAsync(DatabaseRpcCommandMetadata metadata, string request, Func<string, Task> callback, CancellationToken cancellation)
        {
            metadata = metadata.ThrowIfNullArgument(nameof(metadata));
            request = request.ThrowIfNullOrWhiteSpaceArgument(nameof(request));
            callback = callback.ThrowIfNullArgument(nameof(callback));
            cancellation.ThrowIfCancellationRequested();
            return InvokeAsync(metadata, request, callback, cancellation);
        }

        [DebuggerStepThrough]
        Task IDatabaseRpcProvider.SendAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation)
        {
            metadata = metadata.ThrowIfNullArgument(nameof(metadata));
            request = request.ThrowIfNullOrWhiteSpaceArgument(nameof(request));
            cancellation.ThrowIfCancellationRequested();
            return SendAsync(metadata, request, cancellation);
        }

        [DebuggerStepThrough]
        Task IDatabaseRpcProvider.SendAsync(DatabaseRpcCommandMetadata metadata, string request, Func<Task> callback, CancellationToken cancellation)
        {
            metadata = metadata.ThrowIfNullArgument(nameof(metadata));
            request = request.ThrowIfNullOrWhiteSpaceArgument(nameof(request));
            callback = callback.ThrowIfNullArgument(nameof(callback));
            cancellation.ThrowIfCancellationRequested();
            return SendAsync(metadata, request, callback, cancellation);
        }

    }
}
