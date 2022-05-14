using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Caching.Common;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DatabaseRpc : ReadThroughETagCache<IDatabaseApiInfo>, IDatabaseRpcClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eTag"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<IDatabaseApiInfo?> GetApiInfoAsync(string? eTag, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="content"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task EnqueueAsync(IDatabaseApiCommandInfo command, string content, CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="content"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<string> InvokeAsync(IDatabaseApiCommandInfo command, string content, CancellationToken cancellation);

        [DebuggerStepThrough]
        Task IDatabaseRpcClient.EnqueueAsync(IDatabaseApiCommandInfo command, string content, CancellationToken cancellation)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (content == null) throw new ArgumentNullException(nameof(content));
            cancellation.ThrowIfCancellationRequested();
            return EnqueueAsync(command, content, cancellation);
        }


        [DebuggerStepThrough]
        async Task<IDatabaseApiInfo?> IDatabaseRpcClient.GetApiInfoAsync(string? eTag, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return await GetApiInfoAsync(eTag, cancellation);
        }

        [DebuggerStepThrough]
        Task<string> IDatabaseRpcClient.InvokeAsync(IDatabaseApiCommandInfo command, string content, CancellationToken cancellation)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (content == null) throw new ArgumentNullException(nameof(content));
            cancellation.ThrowIfCancellationRequested();
            return InvokeAsync(command, content, cancellation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eTag"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected sealed override async Task<State?> GetIfNonMatchAsync(string? eTag, CancellationToken cancellation)
        {
            var info = await GetApiInfoAsync(eTag, cancellation);
            if(info is null)return null;
            return new State(info, info.ETag);
        }
    }
}
