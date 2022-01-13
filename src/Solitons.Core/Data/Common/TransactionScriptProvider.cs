using System.Data;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TransactionScriptProvider : ITransactionScriptProvider
    {
        protected abstract Task<string> InvokeAsync(
            string procedure,
            string content,
            string contentType,
            int timeoutInSeconds,
            IsolationLevel isolationLevel,
            CancellationToken cancellation);

        [DebuggerNonUserCode]
        protected virtual Task<object> OnRequestAsync(object request) => Task.FromResult(request);


        [DebuggerNonUserCode]
        protected virtual Task<object> OnResponseAsync(object response) => Task.FromResult(response);

        [DebuggerStepThrough]
        Task<string> ITransactionScriptProvider.InvokeAsync(
            string procedure, 
            string content,
            string contentType,
            int timeoutInSeconds, 
            IsolationLevel isolationLevel,
            CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return InvokeAsync(
                procedure.ThrowIfNullOrWhiteSpaceArgument(nameof(procedure)),
                content.ThrowIfNullOrWhiteSpaceArgument(nameof(content)),
                contentType.ThrowIfNullOrWhiteSpaceArgument(nameof(contentType)),
                timeoutInSeconds,
                isolationLevel,
                cancellation);
        }
        

        [DebuggerStepThrough]
        Task<object> ITransactionScriptProvider.OnRequestAsync(object request) => OnRequestAsync(request.ThrowIfNullArgument(nameof(request)));



        [DebuggerStepThrough]
        Task<object> ITransactionScriptProvider.OnResponseAsync(object response) => OnResponseAsync(response.ThrowIfNullArgument(nameof(response)));
    }
}
