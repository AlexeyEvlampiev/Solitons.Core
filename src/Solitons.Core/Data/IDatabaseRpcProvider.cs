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
        Task<string> InvokeAsync(DatabaseRpcCommand commandInfo, string request, CancellationToken cancellation);

        Task InvokeAsync(DatabaseRpcCommand commandInfo, string request, Func<string, Task> callback, CancellationToken cancellation);

        Task SendAsync(DatabaseRpcCommand commandInfo, string request, CancellationToken cancellation);

        Task SendAsync(DatabaseRpcCommand commandInfo, string request, Func<Task> callback, CancellationToken cancellation);
    }
}
