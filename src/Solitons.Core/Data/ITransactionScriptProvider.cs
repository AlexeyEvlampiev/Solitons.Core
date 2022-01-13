using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITransactionScriptProvider
    {
        Task<object> OnRequestAsync(object request);
        Task<string> InvokeAsync(
            string procedure,
            string content,
            string contentType,
            int timeoutInSeconds,
            IsolationLevel isolationLevel,
            Func<Task> completionCallback,
            CancellationToken cancellation);

        Task<object> OnResponseAsync(object response);
    }
}
