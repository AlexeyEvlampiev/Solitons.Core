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
        protected abstract Task<string> InvokeAsync(DatabaseRpcCommandMetadata commandInfo, string request, CancellationToken cancellation);
        protected abstract Task InvokeAsync(DatabaseRpcCommandMetadata commandInfo, string request, Func<string, Task> callback, CancellationToken cancellation);
        protected abstract Task SendAsync(DatabaseRpcCommandMetadata commandInfo, string request, CancellationToken cancellation);
        protected abstract Task SendAsync(DatabaseRpcCommandMetadata commandInfo, string request, Func<Task> callback, CancellationToken cancellation);

        [DebuggerStepThrough]
        Task<string> IDatabaseRpcProvider.InvokeAsync(DatabaseRpcCommandMetadata commandInfo, string request, CancellationToken cancellation)
        {
            commandInfo = commandInfo.ThrowIfNullArgument(nameof(commandInfo));
            request = request.ThrowIfNullOrWhiteSpaceArgument(nameof(request));
            cancellation.ThrowIfCancellationRequested();
            return InvokeAsync(commandInfo, request, cancellation);
        }

        [DebuggerStepThrough]
        Task IDatabaseRpcProvider.InvokeAsync(DatabaseRpcCommandMetadata commandInfo, string request, Func<string, Task> callback, CancellationToken cancellation)
        {
            commandInfo = commandInfo.ThrowIfNullArgument(nameof(commandInfo));
            request = request.ThrowIfNullOrWhiteSpaceArgument(nameof(request));
            callback = callback.ThrowIfNullArgument(nameof(callback));
            cancellation.ThrowIfCancellationRequested();
            return InvokeAsync(commandInfo, request, callback, cancellation);
        }

        [DebuggerStepThrough]
        Task IDatabaseRpcProvider.SendAsync(DatabaseRpcCommandMetadata commandInfo, string request, CancellationToken cancellation)
        {
            commandInfo = commandInfo.ThrowIfNullArgument(nameof(commandInfo));
            request = request.ThrowIfNullOrWhiteSpaceArgument(nameof(request));
            cancellation.ThrowIfCancellationRequested();
            return SendAsync(commandInfo, request, cancellation);
        }

        [DebuggerStepThrough]
        Task IDatabaseRpcProvider.SendAsync(DatabaseRpcCommandMetadata commandInfo, string request, Func<Task> callback, CancellationToken cancellation)
        {
            commandInfo = commandInfo.ThrowIfNullArgument(nameof(commandInfo));
            request = request.ThrowIfNullOrWhiteSpaceArgument(nameof(request));
            callback = callback.ThrowIfNullArgument(nameof(callback));
            cancellation.ThrowIfCancellationRequested();
            return SendAsync(commandInfo, request, callback, cancellation);
        }
    }
}
