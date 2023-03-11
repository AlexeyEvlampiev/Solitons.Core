using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

/// <summary>
/// Represents a callback for an HTTP transaction.
/// </summary>
public partial interface IHttpTransactionCallback
{
    /// <summary>
    /// Gets the HTTP request message initiated the transaction.
    /// </summary>
    HttpRequestMessage Request { get; }

    /// <summary>
    /// Gets the HTTP response message for the transaction.
    /// </summary>
    HttpResponseMessage Response { get; }

    /// <summary>
    /// Attempts to roll back the transaction if it is currently active.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation and contains a boolean value indicating whether the rollback operation was successful.</returns>
    Task<bool> RollbackIfActiveAsync();
}

public partial interface IHttpTransactionCallback
{
    /// <summary>
    /// Rolls back the HTTP transaction if it is currently active.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the rollback operation fails because the transaction is not currently active or cannot be rolled back.</exception>
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