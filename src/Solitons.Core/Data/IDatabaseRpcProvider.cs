using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data;

/// <summary>
/// Provides an interface for communicating with a database via remote procedure call (RPC).
/// </summary>
public interface IDatabaseRpcProvider
{
    /// <summary>
    /// Sends a request to the database via RPC and returns the response.
    /// </summary>
    /// <typeparam name="T">The type of the response.</typeparam>
    /// <param name="metadata">The metadata for the RPC command.</param>
    /// <param name="request">The request string to send.</param>
    /// <param name="parseResponse">The function used to parse the response string.</param>
    /// <param name="cancellation">The optional cancellation token.</param>
    /// <returns>The response of type T.</returns>
    Task<T> InvokeAsync<T>(
        DatabaseRpcCommandMetadata metadata, 
        string request, 
        Func<string, Task<T>> parseResponse, 
        CancellationToken cancellation = default);

    /// <summary>
    /// Sends a request to the database via RPC and returns the response without committing the transaction.
    /// </summary>
    /// <typeparam name="T">The type of the response.</typeparam>
    /// <param name="metadata">The metadata for the RPC command.</param>
    /// <param name="request">The request string to send.</param>
    /// <param name="parseResponse">The function used to parse the response string.</param>
    /// <param name="cancellation">The optional cancellation token.</param>
    /// <returns>The response of type T.</returns>
    Task<T> WhatIfAsync<T>(
        DatabaseRpcCommandMetadata metadata,
        string request,
        Func<string, Task<T>> parseResponse,
        CancellationToken cancellation = default);

    /// <summary>
    /// Sends a request to the database via RPC and calls the specified callback function once the request has been sent.
    /// </summary>
    /// <param name="metadata">The metadata for the RPC command.</param>
    /// <param name="request">The request string to send.</param>
    /// <param name="callback">The function to call once the request has been sent.</param>
    /// <param name="cancellation">The optional cancellation token.</param>
    Task SendAsync(
        DatabaseRpcCommandMetadata metadata, 
        string request, 
        Func<Task> callback, 
        CancellationToken cancellation = default);


    /// <summary>
    /// Processes a queue of requests sent to the database using the <see cref="SendAsync"/> method.
    /// </summary>
    /// <param name="queueName">The name of the queue to process.</param>
    /// <param name="cancellation">The optional cancellation token.</param>
    Task ProcessQueueAsync(
        string queueName, 
        CancellationToken cancellation = default);


    /// <summary>
    /// Adds a callback to the database RPC provider.
    /// </summary>
    /// <param name="callback">The callback to add.</param>
    /// <returns>The modified IDatabaseRpcProvider object.</returns>
    IDatabaseRpcProvider With(IDatabaseRpcProviderCallback callback);
}