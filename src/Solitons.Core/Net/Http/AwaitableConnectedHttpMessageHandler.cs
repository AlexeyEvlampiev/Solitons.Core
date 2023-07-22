using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

/// <summary>
/// Represents an abstract HTTP message handler that has active connections to other services 
/// and supports awaitable operations. These connections could be to databases, cache servers, 
/// authorization servers, or other types of remote services.
/// This handler is an IAwaitable, which means it exposes a method to return a task that 
/// completes when the handler has finished processing its current set of messages. 
/// </summary>
public abstract class AwaitableConnectedHttpMessageHandler : HttpMessageHandler, IAwaitable
{
    /// <summary>
    /// Runs the main logic of the HTTP message handler asynchronously. This method should 
    /// contain the core logic for interacting with the connected services and processing 
    /// the messages.
    /// </summary>
    /// <param name="cancellation">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A Task representing the ongoing operation.</returns>
    protected abstract Task RunAsync(CancellationToken cancellation);

    /// <summary>
    /// Converts the message handler to a Task for use with async/await. 
    /// This method calls the RunAsync method and can be overridden by subclasses if needed.
    /// </summary>
    /// <param name="cancellation">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A Task that represents the operation of the handler.</returns>
    Task IAwaitable.AsTask(CancellationToken cancellation)
    {
        return RunAsync(cancellation);
    }
}