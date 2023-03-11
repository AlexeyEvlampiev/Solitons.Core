using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

public partial interface IHttpTransactionCallback
{
    HttpRequestMessage Request { get; }
    HttpResponseMessage Response { get; }
    Task<bool> RollbackIfActiveAsync();
}

public partial interface IHttpTransactionCallback
{
    public sealed async Task RollbackAsync()
    {
        if (false == await RollbackIfActiveAsync())
        {
            throw new InvalidOperationException(
                "Rollback operation failed." +
                " The transaction is not currently active or could not be rolled back.");
        }
    }
}