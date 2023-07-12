using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data;

/// <summary>
/// Defines a set of methods that can be used to interact with a database through remote procedure calls (RPCs).
/// </summary>
public interface IDatabaseRpcCommand
{
    /// <summary>
    /// Determines whether this command can accept the specified request.
    /// </summary>
    /// <param name="request">The <see cref="TextMediaContent"/> object to be evaluated.</param>
    /// <returns><c>true</c> if the command can accept the request; otherwise, <c>false</c>.</returns>
    bool CanAccept(TextMediaContent request);

    /// <summary>
    /// Executes this command asynchronously with the specified <paramref name="request"/> and optional <paramref name="cancellation"/> token.
    /// </summary>
    /// <param name="request">The <see cref="TextMediaContent"/> object to be processed.</param>
    /// <param name="cancellation">The optional <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task{TResult}"/> object that represents the asynchronous operation. The task result contains the response <see cref="TextMediaContent"/> object.</returns>
    /// <exception cref="TaskCanceledException">Thrown if the cancellation token is triggered.</exception>
    Task<TextMediaContent> InvokeAsync(
        TextMediaContent request, 
        CancellationToken cancellation = default);

    /// <summary>
    /// Sends the specified <paramref name="request"/> asynchronously and optionally observes a <paramref name="cancellation"/> token.
    /// </summary>
    /// <param name="request">The <see cref="TextMediaContent"/> object to be sent.</param>
    /// <param name="cancellation">The optional <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task"/> object that represents the asynchronous operation.</returns>
    /// <exception cref="TaskCanceledException">Thrown if the cancellation token is triggered.</exception>
    Task SendAsync(
        TextMediaContent request, 
        CancellationToken cancellation = default);

    /// <summary>
    /// Evaluates the specified <paramref name="request"/> and returns the <see cref="TextMediaContent"/> object that represents what would happen if the request was executed.
    /// </summary>
    /// <param name="request">The <see cref="TextMediaContent"/> object to be evaluated.</param>
    /// <param name="cancellation">The optional <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task{TResult}"/> object that represents the asynchronous operation. The task result contains the response <see cref="TextMediaContent"/> object.</returns>
    Task<TextMediaContent> WhatIfAsync(
        TextMediaContent request, 
        CancellationToken cancellation = default);

    /// <summary>
    /// Sends the specified <paramref name="dto"/> object to the specified <paramref name="queue"/> via a <see cref="ILargeObjectQueueProducer"/> and applies configuration options via the <paramref name="config"/> action. 
    /// </summary>
    /// <param name="queue">The <see cref="ILargeObjectQueueProducer"/> object to use for the send operation.</param>
    /// <param name="dto">The object to be sent.</param>
    /// <param name="config">The action to be applied to the <see cref="DataTransferPackage"/> object.</param>
    /// <param name="cancellation">The optional <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task"/> object that represents the asynchronous operation.</returns>
    /// <remarks>
    /// The <paramref name="config"/> action can be used to configure the data transfer package before it is sent. For example, the action could be used to set the package's priority, expiration time, or routing key.
    /// </remarks>
    Task SendViaAsync(
        ILargeObjectQueueProducer queue,
        object dto,
        Action<DataTransferPackage> config,
        CancellationToken cancellation = default);


    /// <summary>
    /// Sends the specified <paramref name="dto"/> object to the specified <paramref name="queue"/> via a <see cref="ILargeObjectQueueProducer"/> without applying any configuration options.
    /// </summary>
    /// <param name="queue">The <see cref="ILargeObjectQueueProducer"/> object to use for the send operation.</param>
    /// <param name="dto">The object to be sent.</param>
    /// <param name="cancellation">The optional <see cref="CancellationToken"/> to observe.</param>
    /// <returns>A <see cref="Task"/> object that represents the asynchronous operation.</returns>
    Task SendViaAsync(
        ILargeObjectQueueProducer queue,
        object dto,
        CancellationToken cancellation = default);
}