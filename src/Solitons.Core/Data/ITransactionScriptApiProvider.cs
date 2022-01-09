using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITransactionScriptApiProvider
    {
        Task<object> OnRequestAsync(object request);
        Task<string> InvokeAsync(
            StoredProcedureAttribute procedureMetadata, 
            StoredProcedureRequestAttribute requestMetadata,
            StoredProcedureResponseAttribute responseMetadata,
            string request, 
            CancellationToken cancellation);

        Task<object> OnResponseAsync(object response);
    }
}
