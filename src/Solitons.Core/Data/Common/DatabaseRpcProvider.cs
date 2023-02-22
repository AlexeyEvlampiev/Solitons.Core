using System;
using System.Diagnostics;
using System.Reactive;
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
        private IDatabaseRpcProviderCallback _callback = new DatabaseRpcProviderCallback();

        /// <summary>
        /// 
        /// </summary>
        protected enum OperationType
        {
            /// <summary>
            /// 
            /// </summary>
            Invocation,

            /// <summary>
            /// 
            /// </summary>
            WhatIf,

            /// <summary>
            /// 
            /// </summary>
            Send,
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

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<T> WhatIfAsync<T>(
            DatabaseRpcCommandMetadata metadata,
            string request,
            Func<string, Task<T>> callback,
            CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task SendAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        /// <param name="callback"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task SendAsync(DatabaseRpcCommandMetadata metadata, string request, Func<Task> callback, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task ProcessQueueAsync(string queueName, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        public IDatabaseRpcProvider With(IDatabaseRpcProviderCallback callback)
        {
            var clone = (DatabaseRpcProvider)MemberwiseClone();
            clone._callback = callback;
            return clone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected virtual bool IsTransientError(Exception exception) => false;


        [DebuggerStepThrough]
        async Task IDatabaseRpcProvider.ProcessQueueAsync(string queueName, CancellationToken cancellation)
        {
            ThrowIf.Cancelled(cancellation);
            try
            {
                await _callback
                    .OnQueueProcessingStartedAsync(queueName, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>());

                await ProcessQueueAsync(queueName, cancellation);

                await _callback
                    .OnQueueProcessingFinishedAsync(queueName, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>());
            }
            catch (Exception e) when(e is not OperationCanceledException)
            {
                await _callback
                    .OnQueueProcessingErrorAsync(queueName, e, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>());
                throw;
            }
        }

        [DebuggerStepThrough]
        async Task<T> IDatabaseRpcProvider.InvokeAsync<T>(
            DatabaseRpcCommandMetadata metadata,
            string request,
            Func<string, Task<T>> parseResponse,
            CancellationToken cancellation)
        {
            metadata = ThrowIf.ArgumentNull(metadata, nameof(metadata));
            request = ThrowIf.ArgumentNullOrWhiteSpace(request, nameof(request));
            parseResponse = ThrowIf.ArgumentNull(parseResponse, nameof(parseResponse));
            ThrowIf.Cancelled(cancellation);

            string capturedResponse = string.Empty;

            Task<T> CaptureAndParseResponseAsync(string response)
            {
                capturedResponse = response;
                return parseResponse.Invoke(response);
            }

            try
            {
                await _callback
                    .OnStartingInvocationAsync(metadata, request, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>());

                var result = await Observable
                    .Defer(() =>
                        InvokeAsync(metadata, request, CaptureAndParseResponseAsync, cancellation).ToObservable())
                    .RetryWhen(exceptions => this.GetRetries(OperationType.Invocation, metadata, request, exceptions))
                    .FirstAsync()
                    .ToTask(cancellation);
                await _callback
                    .OnInvocationCompletedAsync(metadata, request, capturedResponse, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>());
                return result;
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                await _callback
                    .OnInvocationErrorAsync(metadata, request, e, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>());
                throw;
            }
        }

        [DebuggerStepThrough]
        async Task<T> IDatabaseRpcProvider.WhatIfAsync<T>(
            DatabaseRpcCommandMetadata metadata,
            string request,
            Func<string, Task<T>> parseResponse,
            CancellationToken cancellation)
        {
            metadata = ThrowIf.ArgumentNull(metadata, nameof(metadata));
            request = ThrowIf.ArgumentNullOrWhiteSpace(request, nameof(request));
            parseResponse = ThrowIf.ArgumentNull(parseResponse, nameof(parseResponse));
            ThrowIf.Cancelled(cancellation);

            string capturedResponse = string.Empty;

            Task<T> CaptureAndParseResponseAsync(string response)
            {
                capturedResponse = response;
                return parseResponse.Invoke(response);
            }

            try
            {
                await _callback
                    .OnStartingInvocationAsync(metadata, request, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>());

                var result = await Observable
                    .Defer(() =>
                        WhatIfAsync(metadata, request, CaptureAndParseResponseAsync, cancellation).ToObservable())
                    .RetryWhen(exceptions => this.GetRetries(OperationType.WhatIf, metadata, request, exceptions))
                    .FirstAsync()
                    .ToTask(cancellation);

                await _callback
                    .OnInvocationCompletedAsync(metadata, request, capturedResponse, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>());
                return result;
            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                await _callback
                    .OnInvocationErrorAsync(metadata, request, e, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>());
                throw;
            }
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
                    .Delay((++attempt * 100).Min(100).Max(3000))
                    .ToObservable()
                    .Select(_ => attempt));
        }


        [DebuggerStepThrough]
        async Task IDatabaseRpcProvider.SendAsync(DatabaseRpcCommandMetadata metadata, string request, Func<Task> callback, CancellationToken cancellation)
        {
            metadata = ThrowIf.ArgumentNull(metadata, nameof(metadata));
            request = ThrowIf.ArgumentNullOrWhiteSpace(request, nameof(request));
            callback = ThrowIf.ArgumentNull(callback, nameof(callback));
            cancellation = ThrowIf.Cancelled(cancellation);

            try
            {
                await _callback
                    .OnSendingAsync(metadata, request, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>());

                await SendAsync(metadata, request, callback, cancellation);

                await _callback
                    .OnSentAsync(metadata, request, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>());

            }
            catch (Exception e) when (e is not OperationCanceledException)
            {
                await _callback
                    .OnSendingErrorAsync(metadata, request, e, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>());
                throw;
            }
        }

    }
}
