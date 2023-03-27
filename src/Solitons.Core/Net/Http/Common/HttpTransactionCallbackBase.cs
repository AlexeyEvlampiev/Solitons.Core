using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Solitons.Net.Http.Common;

/// <summary>
/// Provides a base class for HTTP transaction callbacks that implement the <see cref="IHttpTransactionCallback"/> and <see cref="IAsyncDisposable"/> interfaces.
/// </summary>
public abstract class HttpTransactionCallbackBase :
    IHttpTransactionCallback,
    IAsyncDisposable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpTransactionCallbackBase"/> class with the specified HTTP request and response messages.
    /// </summary>
    /// <param name="request">The HTTP request message.</param>
    /// <param name="response">The HTTP response message.</param>
    protected HttpTransactionCallbackBase(
        HttpRequestMessage request,
        HttpResponseMessage response)
    {
        Request = request;
        Response = response;
    }

    /// <inheritdoc/>
    public HttpRequestMessage Request { get; }

    /// <inheritdoc/>
    public HttpResponseMessage Response { get; }

    /// <summary>
    /// Attempts to roll back the HTTP transaction if it is currently active.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation and contains a boolean value indicating whether the rollback operation was successful.</returns>
    public abstract Task<bool> RollbackIfActiveAsync();

    /// <summary>
    /// Commits the HTTP transaction if it is currently active.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation and contains a boolean value indicating whether the commit operation was successful.</returns>
    protected abstract Task<bool> CommitIfActiveAsync();

    /// <summary>
    /// Commits the HTTP transaction asynchronously.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous operation.</returns>
    /// <remarks>
    /// This method is called when the object is being disposed.
    /// It commits the transaction if it is currently active.
    /// </remarks>
    [DebuggerStepThrough]
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await CommitIfActiveAsync();
    }
}