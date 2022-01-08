using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common
{
    public abstract class TransactionScriptApiProvider : ITransactionScriptApiProvider
    {
        protected abstract Task<string> InvokeAsync(
            StoredProcedureAttribute procedureMetadata,
            StoredProcedureRequestAttribute requestMetadata,
            StoredProcedureResponseAttribute responseMetadata,
            string request,
            CancellationToken cancellation);

        protected abstract string Serialize(object request, string requestContentType);

        protected abstract object Deserialize(Type responseInfoAsyncResultType, string contentType, string content);

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
        string ITransactionScriptApiProvider.Serialize(object request, string contentType) =>
            Serialize(
                request.ThrowIfNullArgument(nameof(request)), 
                contentType.ThrowIfNullOrWhiteSpaceArgument(nameof(contentType)));

        [DebuggerStepThrough]
        object ITransactionScriptApiProvider.Deserialize(Type targetType, string contentType, string content) =>
            Deserialize(
                    targetType.ThrowIfNullArgument(nameof(targetType)), 
                    contentType.ThrowIfNullOrWhiteSpaceArgument(nameof(contentType)), 
                    content.ThrowIfNullOrWhiteSpaceArgument(nameof(content)))
                .ThrowIfNull(() => new NullReferenceException());

        [DebuggerStepThrough]
        Task<object> ITransactionScriptApiProvider.OnResponseAsync(object response) => OnResponseAsync(response.ThrowIfNullArgument(nameof(response)));
    }
}
