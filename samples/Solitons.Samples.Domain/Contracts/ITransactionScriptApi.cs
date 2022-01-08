using System.Data;
using Solitons.Data;

namespace Solitons.Samples.Domain.Contracts
{
    public interface ITransactionScriptApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [StoredProcedure("customer_get", IsolationLevel = IsolationLevel.ReadCommitted, OperationTimeoutInSeconds = 2)]
        [return: StoredProcedureResponse(ContentType = "application/json")]
        Task<CustomerGetResponse> InvokeAsync(
            [StoredProcedureRequest(ContentType = "application/json")] CustomerGetRequest request,
            CancellationToken cancellation = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [StoredProcedure("customer_upsert", IsolationLevel = IsolationLevel.ReadCommitted, OperationTimeoutInSeconds = 5)]
        [return: StoredProcedureResponse(ContentType = "application/json")]
        Task<CustomerUpsertResponse> UpsertCustomerRecordAsync(
            [StoredProcedureRequest(ContentType = "application/json")] CustomerUpsertRequest request, 
            CancellationToken cancellation = default);
    }
}
