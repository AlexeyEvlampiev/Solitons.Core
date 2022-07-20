using System;
using System.Diagnostics;
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
        public async Task<string> InvokeAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation)
        {
            try
            {
                await _callback.OnStartingInvocationAsync(metadata, request, cancellation);
                var response = await _innerProvider.InvokeAsync(metadata, request, cancellation);
                await _callback.OnInvocationCompletedAsync(metadata, request, response, cancellation);
                return response;
            }
            catch (Exception e)
            {
                await _callback.OnInvocationErrorAsync(metadata, request, e, cancellation);
                throw;
            }
        }

        [DebuggerStepThrough]
        public async Task InvokeAsync(DatabaseRpcCommandMetadata metadata, string request, Func<string, Task> callback, CancellationToken cancellation)
        {
            try
            {
                string? capturedResponse = null;
                await _callback.OnStartingInvocationAsync(metadata, request, cancellation);
                await _innerProvider.InvokeAsync(metadata, request, OnCompletedAsync, cancellation);
                if (capturedResponse != null)
                {
                    await _callback.OnInvocationCompletedAsync(metadata, request, capturedResponse, cancellation);
                }

                Task OnCompletedAsync(string response)
                {
                    capturedResponse = response;
                    return callback.Invoke(response);
                }
            }
            catch (Exception e)
            {
                await _callback.OnInvocationErrorAsync(metadata, request, e, cancellation);
                throw;
            }

        }

        [DebuggerStepThrough]
        public async Task SendAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation)
        {
            try
            {
                await _callback.OnSendingAsync(metadata, request, cancellation);
                await _innerProvider.SendAsync(metadata, request, cancellation);
                await _callback.OnSentAsync(metadata, request, cancellation);
            }
            catch (Exception e)
            {
                await _callback.OnSendingErrorAsync(metadata, request, e, cancellation);
                throw;
            }
        }

        [DebuggerStepThrough]
        public async Task SendAsync(DatabaseRpcCommandMetadata metadata, string request, Func<Task> callback, CancellationToken cancellation)
        {
            try
            {
                await _callback.OnSendingAsync(metadata, request, cancellation);
                await _innerProvider.SendAsync(metadata, request, callback, cancellation);
                await _callback.OnSentAsync(metadata, request, cancellation);
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
                await _callback.OnQueueProcessingStartedAsync(queueName, cancellation);
                await _innerProvider.ProcessQueueAsync(queueName, cancellation);
                await _callback.OnQueueProcessingFinishedAsync(queueName, cancellation);
            }
            catch (Exception e)
            {
                await _callback.OnQueueProcessingErrorAsync(queueName, e, cancellation);
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
