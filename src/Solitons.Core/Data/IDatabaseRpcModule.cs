using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data;

/// <summary>
/// Provides a collection of methods for managing and invoking database RPC commands.
/// </summary>
public partial interface IDatabaseRpcModule
{
    /// <summary>
    /// Determines whether the specified command identifier exists in the module.
    /// </summary>
    /// <param name="commandId">The unique identifier of the command to locate.</param>
    /// <returns><see langword="true"/> if the command identifier is found; otherwise, <see langword="false"/>.</returns>
    bool Contains(Guid commandId);

    /// <summary>
    /// Returns the database RPC command with the specified command identifier.
    /// </summary>
    /// <param name="commandId">The unique identifier of the command to retrieve.</param>
    /// <returns>The <see cref="IDatabaseRpcCommand"/> object associated with the specified command identifier.</returns>
    /// <exception cref="KeyNotFoundException">The command identifier is not found in the module.</exception>
    IDatabaseRpcCommand GetCommand(Guid commandId);

    /// <summary>
    /// Invokes the specified database RPC command with the provided request content and returns the response content.
    /// </summary>
    /// <param name="commandId">The unique identifier of the command to invoke.</param>
    /// <param name="request">The request content to send to the command.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains the response content returned by the command.</returns>
    /// <exception cref="KeyNotFoundException">The command identifier is not found in the module.</exception>
    Task<MediaContent> InvokeAsync(Guid commandId, MediaContent request, CancellationToken cancellation = default);

    /// <summary>
    /// Sends the specified request content to the database RPC command with the provided command identifier.
    /// </summary>
    /// <param name="commandId">The unique identifier of the command to send the request to.</param>
    /// <param name="content">The request content to send to the command.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">The command identifier is not found in the module.</exception>
    Task SendAsync(Guid commandId, MediaContent content, CancellationToken cancellation = default);
}

public partial interface IDatabaseRpcModule
{
    /// <summary>
    /// Determines whether the module contains a command of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the command.</typeparam>
    /// <returns><c>true</c> if the module contains a command of type <typeparamref name="T"/>; otherwise, <c>false</c>.</returns>
    [DebuggerStepThrough]
    public bool Contains<T>() where T : IDatabaseRpcCommand => Contains(typeof(T).GUID);

    /// <summary>
    /// Gets the command of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the command.</typeparam>
    /// <returns>The command of type <typeparamref name="T"/>.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the module does not contain a command of type <typeparamref name="T"/>.</exception>
    [DebuggerStepThrough]
    public T GetCommand<T>() where T : IDatabaseRpcCommand
    {
        return (T)GetCommand(typeof(T).GUID);
    }
}