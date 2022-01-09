using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TransactionScriptApiProvider : ITransactionScriptApiProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="procedureMetadata"></param>
        /// <param name="requestMetadata"></param>
        /// <param name="responseMetadata"></param>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<string> InvokeAsync(
            StoredProcedureAttribute procedureMetadata,
            StoredProcedureRequestAttribute requestMetadata,
            StoredProcedureResponseAttribute responseMetadata,
            string request,
            CancellationToken cancellation);

        [DebuggerNonUserCode]
        protected virtual Task<object> OnRequestAsync(object request) => Task.FromResult(request);

        [DebuggerNonUserCode]
        protected virtual Task<object> OnResponseAsync(object response) => Task.FromResult(response);

        [DebuggerStepThrough]
        Task<object> ITransactionScriptApiProvider.OnRequestAsync(object request) => OnRequestAsync(request.ThrowIfNullArgument(nameof(request)));

        [DebuggerStepThrough]
        Task<string> ITransactionScriptApiProvider.InvokeAsync(
            StoredProcedureAttribute procedureMetadata,
            StoredProcedureRequestAttribute requestMetadata,
            StoredProcedureResponseAttribute responseMetadata,
            string request, 
            CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return InvokeAsync(
                procedureMetadata.ThrowIfNullArgument(nameof(procedureMetadata)),
                requestMetadata.ThrowIfNullArgument(nameof(requestMetadata)),
                responseMetadata.ThrowIfNullArgument(nameof(responseMetadata)),
                request.ThrowIfNullOrWhiteSpaceArgument(nameof(request)), 
                cancellation);
        }


        [DebuggerStepThrough]
        Task<object> ITransactionScriptApiProvider.OnResponseAsync(object response) => OnResponseAsync(response.ThrowIfNullArgument(nameof(response)));
    }
}
