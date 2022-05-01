using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DatabaseRpcClient : IDatabaseRpcClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="content"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<string> InvokeAsync(DatabaseApiCommandInfo command, string content, CancellationToken cancellation);


        [DebuggerStepThrough]
        Task<string> IDatabaseRpcClient.InvokeAsync(DatabaseApiCommandInfo command, string content, CancellationToken cancellation)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            if (content == null) throw new ArgumentNullException(nameof(content));
            cancellation.ThrowIfCancellationRequested();
            return InvokeAsync(command, content, cancellation);
        }
    }
}
