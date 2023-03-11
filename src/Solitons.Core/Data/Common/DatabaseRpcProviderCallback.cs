using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseRpcProviderCallback : IDatabaseRpcProviderCallback
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected virtual Task OnStartingInvocationAsync(
            DatabaseRpcCommandMetadata metadata, 
            string request,
            CancellationToken cancellation)
        {
            Debug.WriteLine($"Starting invocation: {metadata.Procedure}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        /// <param name="response"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected virtual Task OnInvocationCompletedAsync(
            DatabaseRpcCommandMetadata metadata,
            string request,
            string response,
            CancellationToken cancellation)
        {
            Debug.WriteLine($"Invocation completed: {metadata.Procedure}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        /// <param name="exception"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected virtual Task OnInvocationErrorAsync(
            DatabaseRpcCommandMetadata metadata,
            string request,
            Exception exception,
            CancellationToken cancellation)
        {
            Trace.TraceError($"Invocation failed: {metadata.Procedure}; Error: {exception}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected virtual Task OnSendingAsync(
            DatabaseRpcCommandMetadata metadata, 
            string request,
            CancellationToken cancellation)
        {
            Debug.WriteLine($"Sending context: {metadata.Procedure}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected virtual Task OnSentAsync(
            DatabaseRpcCommandMetadata metadata, 
            string request,
            CancellationToken cancellation)
        {
            Debug.WriteLine($"Command sent: {metadata.Procedure}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="request"></param>
        /// <param name="exception"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected virtual Task OnSendingErrorAsync(
            DatabaseRpcCommandMetadata metadata,
            string request,
            Exception exception,
            CancellationToken cancellation)
        {
            Trace.TraceError($"Sending context failed: {metadata.Procedure}; Error: {exception}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected virtual Task OnQueueProcessingStartedAsync(
            string queueName, 
            CancellationToken cancellation)
        {
            Debug.WriteLine($"Starting database queue processing: {queueName}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected virtual Task OnQueueProcessingFinishedAsync(string queueName, CancellationToken cancellation)
        {
            Debug.WriteLine($"Database queue processing finished: {queueName}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="exception"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected virtual Task OnQueueProcessingErrorAsync(
            string queueName,
            Exception exception,
            CancellationToken cancellation)
        {
            Trace.TraceError($"Database queue processing failed: {queueName}; Error: {exception}");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected virtual bool HandleCallbackException(Exception exception)
        {
            try
            {
                Trace.TraceError(exception.ToString());
            }
            catch(Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
            
            return true;
        }

        [DebuggerStepThrough]
        async Task IDatabaseRpcProviderCallback.OnStartingInvocationAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            try
            {
                await OnStartingInvocationAsync(metadata, request, cancellation);
            }
            catch (Exception e)
            {
                if (HandleCallbackException(e))
                {
                    return;
                }
                throw;
            }
        }

        [DebuggerStepThrough]
        async Task IDatabaseRpcProviderCallback.OnInvocationCompletedAsync(DatabaseRpcCommandMetadata metadata, string request, string response,
            CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            try
            {
                await OnInvocationCompletedAsync(metadata, request, response, cancellation);
            }
            catch (Exception e)
            {
                if (HandleCallbackException(e))
                {
                    return;
                }
                throw;
            }
        }

        [DebuggerStepThrough]
        async Task IDatabaseRpcProviderCallback.OnInvocationErrorAsync(DatabaseRpcCommandMetadata metadata, string request, Exception exception,
            CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            try
            {
                await OnInvocationErrorAsync(metadata, request, exception, cancellation);
            }
            catch (Exception e)
            {
                if (HandleCallbackException(e))
                {
                    return;
                }
                throw;
            }
        }

        [DebuggerStepThrough]
        async Task IDatabaseRpcProviderCallback.OnSendingAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            try
            {
                await OnSendingAsync(metadata, request, cancellation);
            }
            catch (Exception e)
            {
                if (HandleCallbackException(e))
                {
                    return;
                }
                throw;
            }
        }

        [DebuggerStepThrough]
        async Task IDatabaseRpcProviderCallback.OnSentAsync(DatabaseRpcCommandMetadata metadata, string request, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            try
            {
                await OnSentAsync(metadata, request, cancellation);
            }
            catch (Exception e)
            {
                if (HandleCallbackException(e))
                {
                    return;
                }
                throw;
            }
        }

        [DebuggerStepThrough]
        async Task IDatabaseRpcProviderCallback.OnSendingErrorAsync(DatabaseRpcCommandMetadata metadata, string request, Exception exception,
            CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            try
            {
                await OnSendingErrorAsync(metadata, request, exception, cancellation);
            }
            catch (Exception e)
            {
                if (HandleCallbackException(e))
                {
                    return;
                }
                throw;
            }
        }

        [DebuggerStepThrough]
        async Task IDatabaseRpcProviderCallback.OnQueueProcessingStartedAsync(string queueName, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            try
            {
                await OnQueueProcessingStartedAsync(queueName, cancellation);
            }
            catch (Exception e)
            {
                if (HandleCallbackException(e))
                {
                    return;
                }
                throw;
            }
        }

        [DebuggerStepThrough]
        async Task IDatabaseRpcProviderCallback.OnQueueProcessingFinishedAsync(string queueName, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            try
            {
                await OnQueueProcessingFinishedAsync(queueName, cancellation);
            }
            catch (Exception e)
            {
                if (HandleCallbackException(e))
                {
                    return;
                }
                throw;
            }
        }

        [DebuggerStepThrough]
        async Task IDatabaseRpcProviderCallback.OnQueueProcessingErrorAsync(string queueName, Exception exception, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            try
            {
                await OnQueueProcessingErrorAsync(queueName, exception, cancellation);
            }
            catch (Exception e)
            {
                if (HandleCallbackException(e))
                {
                    return;
                }
                throw;
            }
        }
    }
}
