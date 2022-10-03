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
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<T> InvokeAsync<T>(DatabaseRpcCommandMetadata metadata,
            string request,
            Func<string, Task<T>> callback,
            CancellationToken cancellation);

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
        Task<T> IDatabaseRpcProvider.InvokeAsync<T>(
            DatabaseRpcCommandMetadata metadata, 
            string request,
            Func<string, Task<T>> parseResponse,
            CancellationToken cancellation)
        {
            metadata = metadata.ThrowIfNullArgument(nameof(metadata));
            request = request.ThrowIfNullOrWhiteSpaceArgument(nameof(request));
            parseResponse = parseResponse.ThrowIfNullArgument(nameof(parseResponse));
            cancellation.ThrowIfCancellationRequested();
            return InvokeAsync(metadata, request, parseResponse, cancellation);
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
