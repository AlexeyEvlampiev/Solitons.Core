using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DatabaseRpcProvider : IDatabaseRpcProvider
    {
        protected enum OperationType
        {
            Invocation, 
            Send
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<T> InvokeAsync<T>(
            DatabaseRpcCommandMetadata metadata,
            string request,
            Func<string, Task<T>> callback,
            CancellationToken cancellation);

        protected abstract Task SendAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation);
        protected abstract Task SendAsync(DatabaseRpcCommandMetadata metadata, string request, Func<Task> callback, CancellationToken cancellation);
        protected abstract Task ProcessQueueAsync(string queueName, CancellationToken cancellation);

        protected virtual bool IsTransientError(Exception exception) => false;

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

            return Observable
                .Defer(() => InvokeAsync(metadata, request, parseResponse, cancellation).ToObservable())
                .RetryWhen(exceptions => GetRetries(OperationType.Invocation, metadata, request, exceptions))
                .FirstAsync()
                .ToTask(cancellation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        /// <param name="exceptions"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        protected virtual IObservable<int> GetRetries(
            OperationType operationType,
            DatabaseRpcCommandMetadata metadata,
            string request,
            IObservable<Exception> exceptions)
        {
            return exceptions
                .Where(IsTransientError)
                .Take(3)
                .SelectMany((exception, attempt) => Task
                    .Delay(++attempt * 100)
                    .ToObservable()
                    .Select(_ => attempt));
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
