using System;
using System.Data.Common;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Reactive;

namespace Solitons.Data.Common;

/// <summary>
/// Provides a base for interacting with a remote procedure call (RPC) for a database.
/// </summary>
public abstract class DatabaseRpcProvider : IDatabaseRpcProvider
{
    private IDatabaseRpcProviderCallback _callback = new DatabaseRpcProviderCallback();

    /// <summary>
    /// Specifies the type of the database operation to be performed.
    /// </summary>
    protected enum OperationType
    {
        /// <summary>
        /// Specifies that an invocation operation is to be performed. 
        /// This involves executing a procedure on the database and waiting for its response.
        /// </summary>
        Invocation,

        /// <summary>
        /// Specifies that a 'WhatIf' operation is to be performed.
        /// This typically involves simulating a procedure execution on the database without affecting the data.
        /// </summary>
        WhatIf,

        /// <summary>
        /// Specifies that a send operation is to be performed.
        /// This involves sending a request to a database internal queue without waiting for its response.
        /// </summary>
        Send,
    }

    /// <summary>
    /// Asynchronously invokes a database operation.
    /// </summary>
    /// <typeparam name="T">The type of the expected return value.</typeparam>
    /// <param name="metadata">Metadata about the RPC command to be invoked.</param>
    /// <param name="request">The request to be sent to the database.</param>
    /// <param name="callback">The callback to be executed after the operation is complete.</param>
    /// <param name="cancellation">The cancellation token that can be used to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation, with the return type of the RPC command.</returns>
    protected abstract Task<T> InvokeAsync<T>(
        DatabaseRpcCommandMetadata metadata,
        string request,
        Func<string, Task<T>> callback,
        CancellationToken cancellation);

    /// <summary>
    /// Asynchronously performs a "what if" database operation, simulating the operation without affecting the data.
    /// </summary>
    /// <typeparam name="T">The type of the expected return value.</typeparam>
    /// <param name="metadata">Metadata about the RPC command to be invoked.</param>
    /// <param name="request">The request to be sent to the database.</param>
    /// <param name="callback">The callback to be executed after the operation is complete.</param>
    /// <param name="cancellation">The cancellation token that can be used to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation, with the return type of the RPC command.</returns>
    protected abstract Task<T> WhatIfAsync<T>(
        DatabaseRpcCommandMetadata metadata,
        string request,
        Func<string, Task<T>> callback,
        CancellationToken cancellation);

    /// <summary>
    /// Asynchronously sends a request to the database internal queue without waiting for a response.
    /// </summary>
    /// <param name="metadata">Metadata about the RPC command to be invoked.</param>
    /// <param name="request">The request to be sent to the database.</param>
    /// <param name="cancellation">The cancellation token that can be used to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected abstract Task SendAsync(
        DatabaseRpcCommandMetadata metadata, 
        string request, 
        CancellationToken cancellation);

    /// <summary>
    /// Asynchronously sends a request to the database internal queue without waiting for a response and executes a callback function after sending the request.
    /// </summary>
    /// <param name="metadata">Metadata about the RPC command to be invoked.</param>
    /// <param name="request">The request to be sent to the database.</param>
    /// <param name="callback">The callback function to be executed after the request is sent.</param>
    /// <param name="cancellation">The cancellation token that can be used to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected abstract Task SendAsync(
        DatabaseRpcCommandMetadata metadata, 
        string request, 
        Func<Task> callback, 
        CancellationToken cancellation);

    /// <summary>
    /// Asynchronously processes a specified queue in the database.
    /// </summary>
    /// <param name="queueName">The name of the queue to be processed.</param>
    /// <param name="cancellation">The cancellation token that can be used to cancel the operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected abstract Task ProcessQueueAsync(string queueName, CancellationToken cancellation);

    /// <summary>
    /// Returns a new instance of IDatabaseRpcProvider with the specified callback.
    /// </summary>
    /// <param name="callback">The callback to be used in the new instance of IDatabaseRpcProvider.</param>
    /// <returns>A new instance of IDatabaseRpcProvider with the specified callback.</returns>
    public IDatabaseRpcProvider With(IDatabaseRpcProviderCallback callback)
    {
        var clone = (DatabaseRpcProvider)MemberwiseClone();
        clone._callback = callback;
        return clone;
    }



    [DebuggerStepThrough]
    async Task IDatabaseRpcProvider.ProcessQueueAsync(string queueName, CancellationToken cancellation)
    {
        ThrowIf.Cancelled(cancellation);
        try
        {
            await _callback
                .OnQueueProcessingStartedAsync(queueName, cancellation)
                .ToObservable()
                .OnErrorResumeNext(Observable.Empty<Unit>());

            await ProcessQueueAsync(queueName, cancellation);

            await _callback
                .OnQueueProcessingFinishedAsync(queueName, cancellation)
                .ToObservable()
                .OnErrorResumeNext(Observable.Empty<Unit>());
        }
        catch (Exception e) when(e is not OperationCanceledException)
        {
            await _callback
                .OnQueueProcessingErrorAsync(queueName, e, cancellation)
                .ToObservable()
                .OnErrorResumeNext(Observable.Empty<Unit>());
            throw;
        }
    }

    [DebuggerStepThrough]
    async Task<T> IDatabaseRpcProvider.InvokeAsync<T>(
        DatabaseRpcCommandMetadata metadata,
        string request,
        Func<string, Task<T>> parseResponse,
        CancellationToken cancellation)
    {
        metadata = ThrowIf.ArgumentNull(metadata, nameof(metadata));
        request = ThrowIf.ArgumentNullOrWhiteSpace(request, nameof(request));
        parseResponse = ThrowIf.ArgumentNull(parseResponse, nameof(parseResponse));
        ThrowIf.Cancelled(cancellation);

        string capturedResponse = string.Empty;

        Task<T> CaptureAndParseResponseAsync(string response)
        {
            capturedResponse = response;
            return parseResponse.Invoke(response);
        }

        try
        {
            await _callback
                .OnStartingInvocationAsync(metadata, request, cancellation)
                .ToObservable()
                .OnErrorResumeNext(Observable.Empty<Unit>());

            var result = await Observable
                .Defer(() =>
                    InvokeAsync(metadata, request, CaptureAndParseResponseAsync, cancellation).ToObservable())
                .WithRetryPolicy(GetRetryPolicy(cancellation))
                .FirstAsync()
                .ToTask(cancellation);
            await _callback
                .OnInvocationCompletedAsync(metadata, request, capturedResponse, cancellation)
                .ToObservable()
                .OnErrorResumeNext(Observable.Empty<Unit>());
            return result;
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            await _callback
                .OnInvocationErrorAsync(metadata, request, e, cancellation)
                .ToObservable()
                .OnErrorResumeNext(Observable.Empty<Unit>());
            throw;
        }
    }

    /// <summary>
    /// Returns a function that determines the retry policy for database operations.
    /// </summary>
    /// <param name="cancellation">The cancellation token that can be used to cancel the operation.</param>
    /// <returns>A function that takes a <see cref="RetryPolicyArgs"/> object and returns a task representing the asynchronous operation that indicates whether the operation should be retried.</returns>
    [DebuggerNonUserCode]
    protected Func<RetryPolicyArgs, Task<bool>> GetRetryPolicy(CancellationToken cancellation = default)
    {
        return Capture;
        [DebuggerStepThrough]
        Task<bool> Capture(RetryPolicyArgs args)
        {
            cancellation.ThrowIfCancellationRequested();
            return ShouldRetryAsync(args, cancellation);
        }
    }

    /// <summary>
    /// Determines whether a failed operation should be retried based on the provided retry policy arguments.
    /// </summary>
    /// <param name="args">The arguments that encapsulate data about the retry attempt.</param>
    /// <param name="cancellation">The cancellation token that can be used to cancel the operation.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a boolean value 
    /// indicating whether the operation should be retried (true) or not (false).
    /// </returns>
    protected virtual async Task<bool> ShouldRetryAsync(RetryPolicyArgs args, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        if (args.AttemptNumber < 3 && args.Exception is DbException { IsTransient: true })
        {
            await Task.Delay(100, cancellation);
            return true;
        }
        return false;
    }

    [DebuggerStepThrough]
    async Task<T> IDatabaseRpcProvider.WhatIfAsync<T>(
        DatabaseRpcCommandMetadata metadata,
        string request,
        Func<string, Task<T>> parseResponse,
        CancellationToken cancellation)
    {
        metadata = ThrowIf.ArgumentNull(metadata, nameof(metadata));
        request = ThrowIf.ArgumentNullOrWhiteSpace(request, nameof(request));
        parseResponse = ThrowIf.ArgumentNull(parseResponse, nameof(parseResponse));
        ThrowIf.Cancelled(cancellation);

        string capturedResponse = string.Empty;

        Task<T> CaptureAndParseResponseAsync(string response)
        {
            capturedResponse = response;
            return parseResponse.Invoke(response);
        }

        try
        {
            await _callback
                .OnStartingInvocationAsync(metadata, request, cancellation)
                .ToObservable()
                .OnErrorResumeNext(Observable.Empty<Unit>());

            var result = await Observable
                .Defer(() =>
                    WhatIfAsync(metadata, request, CaptureAndParseResponseAsync, cancellation).ToObservable())
                .WithRetryPolicy(GetRetryPolicy(cancellation))
                .FirstAsync()
                .ToTask(cancellation);

            await _callback
                .OnInvocationCompletedAsync(metadata, request, capturedResponse, cancellation)
                .ToObservable()
                .OnErrorResumeNext(Observable.Empty<Unit>());
            return result;
        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            await _callback
                .OnInvocationErrorAsync(metadata, request, e, cancellation)
                .ToObservable()
                .OnErrorResumeNext(Observable.Empty<Unit>());
            throw;
        }
    }

    


    [DebuggerStepThrough]
    async Task IDatabaseRpcProvider.SendAsync(DatabaseRpcCommandMetadata metadata, string request, Func<Task> callback, CancellationToken cancellation)
    {
        metadata = ThrowIf.ArgumentNull(metadata, nameof(metadata));
        request = ThrowIf.ArgumentNullOrWhiteSpace(request, nameof(request));
        callback = ThrowIf.ArgumentNull(callback, nameof(callback));
        cancellation = ThrowIf.Cancelled(cancellation);

        try
        {
            await _callback
                .OnSendingAsync(metadata, request, cancellation)
                .ToObservable()
                .OnErrorResumeNext(Observable.Empty<Unit>());

            await SendAsync(metadata, request, callback, cancellation);

            await _callback
                .OnSentAsync(metadata, request, cancellation)
                .ToObservable()
                .OnErrorResumeNext(Observable.Empty<Unit>());

        }
        catch (Exception e) when (e is not OperationCanceledException)
        {
            await _callback
                .OnSendingErrorAsync(metadata, request, e, cancellation)
                .ToObservable()
                .OnErrorResumeNext(Observable.Empty<Unit>());
            throw;
        }
    }

}