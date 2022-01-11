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
            StoredProcedureAttribute procedureMetadata, 
            StoredProcedureRequestAttribute requestMetadata,
            StoredProcedureResponseAttribute responseMetadata,
            string request, 
            CancellationToken cancellation);

        Task<object> OnResponseAsync(object response);
    }
}
