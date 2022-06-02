using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDatabaseRpcProvider
    {
        Task<string> InvokeAsync(DatabaseRpcCommandMetadata commandInfo, string request, CancellationToken cancellation);

        Task InvokeAsync(DatabaseRpcCommandMetadata commandInfo, string request, Func<string, Task> callback, CancellationToken cancellation);

        Task SendAsync(DatabaseRpcCommandMetadata commandInfo, string request, CancellationToken cancellation);

        Task SendAsync(DatabaseRpcCommandMetadata commandInfo, string request, Func<Task> callback, CancellationToken cancellation);
    }
}
