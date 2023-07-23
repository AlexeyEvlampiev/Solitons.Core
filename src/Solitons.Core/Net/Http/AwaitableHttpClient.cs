using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

/// <summary>
/// Represents an awaitable HttpClient that depends on its inner services' availability. 
/// It encapsulates the native HttpClient while providing additional functionality for 
/// asynchronous operations based on the state of the inner services it relies upon.
/// </summary>
/// <remarks>
/// The <see cref="AwaitableHttpClient"/> implements <see cref="IAwaitable"/> and <see cref="IAsyncDisposable"/>, 
/// meaning it provides a task for clients to wait upon until all its dependencies are ready and 
/// ensures proper asynchronous cleanup of resources.
/// </remarks>
public abstract class AwaitableHttpClient : HttpClient, IAwaitable, IAsyncDisposable
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly IAwaitable _awaitable;

    /// <summary>
    /// Initializes a new instance of the <see cref="AwaitableHttpClient"/> with the specified <see cref="HttpMessageHandler"/>.
    /// </summary>
    /// <param name="handler">The <see cref="HttpMessageHandler"/> responsible for processing the HTTP response messages.</param>
    /// <remarks>
    /// The HttpClient will be ready to execute requests as soon as the handler is ready.
    /// If the handler or any of its inner handlers implement <see cref="IAwaitable"/>, they will be incorporated into the client's IAwaitable implementation.
    /// </remarks>
    [DebuggerStepThrough]
    protected AwaitableHttpClient(
        HttpMessageHandler handler) 
        : this(handler, Enumerable.Empty<IAwaitable>())
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AwaitableHttpClient"/> with the specified <see cref="HttpMessageHandler"/> and inner awaitables.
    /// </summary>
    /// <param name="handler">The <see cref="HttpMessageHandler"/> responsible for processing the HTTP response messages.</param>
    /// <param name="innerAwaitables">The inner services this client depends on.</param>
    /// <remarks>
    /// The HttpClient will be ready to execute requests as soon as the handler and all inner awaitables are ready.
    /// This constructor unrolls the handler chain to incorporate any inner handlers that implement <see cref="IAwaitable"/>, in addition to any explicitly specified inner awaitables.
    /// </remarks>
    protected AwaitableHttpClient(
        HttpMessageHandler handler, 
        IEnumerable<IAwaitable> innerAwaitables) : base(handler)
    {
        _awaitable = handler
            .UnrollHandlerChain()
            // ReSharper disable once SuspiciousTypeConversion.Global
            .OfType<IAwaitable>()
            .Union(innerAwaitables)
            .Distinct()
            .Convert(IAwaitable.WhenAny);
    }

    /// <summary>
    /// Asynchronously releases the unmanaged resources used by the <see cref="AwaitableHttpClient"/>.
    /// Must be implemented by any concrete subclasses.
    /// </summary>
    /// <returns>A ValueTask representing the asynchronous dispose operation.</returns>
    /// <remarks>
    /// This method is called by the <see cref="IAsyncDisposable.DisposeAsync"/> method to release unmanaged resources 
    /// and perform other cleanup operations before the <see cref="AwaitableHttpClient"/> is reclaimed by garbage collection.
    /// </remarks>
    protected abstract ValueTask DisposeAsync();

    /// <summary>
    /// Initiates the awaitable process and returns a task representing the operation.
    /// </summary>
    /// <param name="cancellation">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the operation.</returns>
    /// <remarks>
    /// The returned task will be completed when the HttpClient is ready to execute requests.
    /// </remarks>
    public Task RunAsync(CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return _awaitable.AsTask(cancellation);
    }

    /// <summary>
    /// This is a concrete implementation of the <see cref="IAwaitable.AsTask"/> method.
    /// </summary>
    [DebuggerStepThrough]
    Task IAwaitable.AsTask(CancellationToken cancellation) => RunAsync(cancellation);

    /// <summary>
    /// Asynchronously releases the unmanaged resources and disposes of the managed resources used by the <see cref="AwaitableHttpClient"/>.
    /// </summary>
    /// <returns>A ValueTask representing the asynchronous dispose operation.</returns>
    /// <remarks>
    /// This method is automatically invoked when the object is disposed or finalized. It should not be called manually.
    /// </remarks>
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await DisposeAsync();
        this.Dispose(true);
    }
}   