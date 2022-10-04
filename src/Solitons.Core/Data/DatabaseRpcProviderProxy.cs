using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    sealed class DatabaseRpcProviderProxy : IDatabaseRpcProvider
    {
        private readonly IDatabaseRpcProvider _innerProvider;
        private readonly IDatabaseRpcProviderCallback _callback;

        public DatabaseRpcProviderProxy(IDatabaseRpcProvider innerProvider, IDatabaseRpcProviderCallback callback)
        {
            _innerProvider = innerProvider;
            _callback = callback;
        }


        [DebuggerStepThrough]
        public async Task<T> InvokeAsync<T>(DatabaseRpcCommandMetadata metadata, string request, Func<string, Task<T>> parseResponse, CancellationToken cancellation)
        {
            try
            {
                await _callback
                    .OnStartingInvocationAsync(
                        metadata, 
                        request, 
                        cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>())
                    .ToTask(cancellation);

                var response = await _innerProvider.InvokeAsync(
                    metadata, 
                    request,
                    ExtendedCallback, 
                    cancellation);

                string capturedResponseContent = string.Empty;
                Task<T> ExtendedCallback(string responseContent)
                {
                    capturedResponseContent = responseContent;
                    return parseResponse.Invoke(responseContent);
                }

                await _callback
                    .OnInvocationCompletedAsync(
                        metadata, 
                        request,
                        capturedResponseContent, 
                        cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>())
                    .ToTask(cancellation);


                return response;
            }
            catch (Exception e)
            {
                await _callback
                    .OnInvocationErrorAsync(metadata, request, e, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>())
                    .ToTask(cancellation);
                throw;
            }
        }


        [DebuggerStepThrough]
        public async Task SendAsync(DatabaseRpcCommandMetadata metadata, string request, Func<Task> callback, CancellationToken cancellation)
        {
            try
            {
                await _callback
                    .OnSendingAsync(metadata, request, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>())
                    .ToTask(cancellation);

                await _innerProvider.SendAsync(metadata, request, callback, cancellation);

                await _callback
                    .OnSentAsync(metadata, request, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>())
                    .ToTask(cancellation);
            }
            catch (Exception e)
            {
                await _callback.OnSendingErrorAsync(metadata, request, e, cancellation);
                throw;
            }
        }

        [DebuggerStepThrough]
        public async Task ProcessQueueAsync(string queueName, CancellationToken cancellation)
        {
            try
            {
                await _callback
                    .OnQueueProcessingStartedAsync(queueName, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>())
                    .ToTask(cancellation);

                await _innerProvider.ProcessQueueAsync(queueName, cancellation);

                await _callback
                    .OnQueueProcessingFinishedAsync(queueName, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>())
                    .ToTask(cancellation);
            }
            catch (Exception e)
            {
                await _callback
                    .OnQueueProcessingErrorAsync(queueName, e, cancellation)
                    .ToObservable()
                    .OnErrorResumeNext(Observable.Empty<Unit>())
                    .ToTask(cancellation);
                throw;
            }

        }

        public override string ToString() => _innerProvider.ToString() ?? base.ToString() ?? nameof(DatabaseRpcProviderProxy);

        public override bool Equals(object? obj)
        {
            if (obj is IDatabaseRpcProvider other)
            {
                if (ReferenceEquals(this, other)) return true;
                if (ReferenceEquals(_innerProvider, other)) return true;
                if (other is DatabaseRpcProviderProxy proxy)
                {
                    return ReferenceEquals(_innerProvider, proxy._innerProvider);
                }
            }

            return false;
        }

        public override int GetHashCode() => _innerProvider.GetHashCode();

        public static IDatabaseRpcProvider Wrap(IDatabaseRpcProvider provider, IDatabaseRpcProviderCallback callback)
        {
            if (provider is DatabaseRpcProviderProxy proxy)
            {
                return ReferenceEquals(callback, proxy._callback)
                    ? proxy
                    : new DatabaseRpcProviderProxy(provider, callback);
            }

            return new DatabaseRpcProviderProxy(provider, callback);
        }
    }
}
