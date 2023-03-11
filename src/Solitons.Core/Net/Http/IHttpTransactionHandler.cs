using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

/// <summary>
/// Represents a handler for an HTTP transaction.
/// </summary>
public partial interface IHttpTransactionHandler
{
    /// <summary>
    /// Invokes the HTTP transaction with the specified request, interceptor, and cancellation token.
    /// </summary>
    /// <param name="request">The HTTP request message.</param>
    /// <param name="interceptor">The interceptor to use for the transaction.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation and contains the HTTP response message.</returns>
    Task<HttpResponseMessage> InvokeAsync(
        HttpRequestMessage request,
        HttpTransactionInterceptor interceptor,
        CancellationToken cancellation);
}

public partial interface IHttpTransactionHandler
{
    /// <summary>
    /// Creates a new HTTP message handler that wraps this transaction handler and the specified interceptor.
    /// </summary>
    /// <param name="interceptor">The interceptor to use for the new message handler.</param>
    /// <returns>A new HTTP message handler.</returns>
    [DebuggerNonUserCode]
    public virtual HttpMessageHandler AsHttpMessageHandler(HttpTransactionInterceptor interceptor)
    {
        return new TxHttpMessageHandler(this, interceptor);
    }

    /// <summary>
    /// Creates a new HTTP message handler that wraps this transaction handler and the specified interceptor function.
    /// </summary>
    /// <param name="interceptor">The interceptor function to use for the new message handler.</param>
    /// <returns>A new HTTP message handler.</returns>
    [DebuggerNonUserCode]
    public sealed HttpMessageHandler AsHttpMessageHandler(Func<IHttpTransactionCallback, Task> interceptor)
    {
        return AsHttpMessageHandler(OnCommitingAsync);
        [DebuggerStepThrough]
        Task OnCommitingAsync(IHttpTransactionCallback callback, CancellationToken _)
        {
            return interceptor.Invoke(callback);
        }
    }

    /// <summary>
    /// Creates a new HTTP message handler that wraps this transaction handler and a default interceptor that does nothing.
    /// </summary>
    /// <returns>A new HTTP message handler.</returns>
    [DebuggerNonUserCode]
    public sealed HttpMessageHandler AsHttpMessageHandler()
    {
        return AsHttpMessageHandler(OnCommitingAsync);
        [DebuggerStepThrough]
        Task OnCommitingAsync(IHttpTransactionCallback callback, CancellationToken _)
        {
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Invokes the HTTP transaction with the specified request, interceptor function, and cancellation token.
    /// </summary>
    /// <param name="request">The HTTP request message.</param>
    /// <param name="interceptor">The interceptor function to use for the transaction.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous operation and contains the HTTP response message.</returns>
    [DebuggerStepThrough]
    public sealed Task<HttpResponseMessage> InvokeAsync(
        HttpRequestMessage request,
        Func<IHttpTransactionCallback, Task> interceptor,
        CancellationToken cancellation)
    {
        return InvokeAsync(
            request, 
            OnCommitingAsync, 
            cancellation);

        [DebuggerStepThrough]
        Task OnCommitingAsync(IHttpTransactionCallback callback, CancellationToken _)
        {
            return interceptor.Invoke(callback);
        }
    }
}