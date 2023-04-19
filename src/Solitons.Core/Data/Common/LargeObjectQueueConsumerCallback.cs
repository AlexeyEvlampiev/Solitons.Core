using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common;

/// <summary>
/// Provides default implementations for events that can occur during large object queue processing.
/// </summary>
public abstract class LargeObjectQueueConsumerCallback : ILargeObjectQueueConsumerCallback
{
    /// <summary>
    /// Called when the queue processing is starting. By default, writes a debug message to the console.
    /// </summary>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual Task OnStartingAsync(CancellationToken cancellation)
    {
        Debug.WriteLine($"Starting {GetType()}");
        return Task.CompletedTask;
    }


    /// <summary>
    /// Processes a retrieved object from the queue. This method is abstract and must be implemented by subclasses.
    /// </summary>
    /// <param name="package">The package containing the object and related metadata.</param>
    /// <param name="dto">The object retrieved from the queue.</param>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task ProcessAsync(
        DataTransferPackage package, 
        object dto, 
        CancellationToken cancellation);

    /// <summary>
    /// Called when a message in the queue is malformed and cannot be processed. By default, does nothing.
    /// </summary>
    /// <param name="messageId">The ID of the malformed message.</param>
    /// <param name="exception">The exception that occurred while processing the message.</param>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnMalformedMessageAsync(
        string messageId, 
        Exception exception, 
        CancellationToken cancellation);

    /// <summary>
    /// Called when an error occurs while unpacking the object from the queue. By default, does nothing.
    /// </summary>
    /// <param name="messageId">The ID of the message containing the object.</param>
    /// <param name="package">The package containing the object and related metadata.</param>
    /// <param name="exception">The exception that occurred while unpacking the object.</param>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnUnpackingErrorAsync(
        string messageId, 
        DataTransferPackage package, 
        Exception exception,
        CancellationToken cancellation);

    /// <summary>
    /// Called when an error occurs while deleting a failed message from the queue. By default, does nothing.
    /// </summary>
    /// <param name="messageId">The ID of the failed message.</param>
    /// <param name="package">The package containing the object and related metadata.</param>
    /// <param name="dto">The object retrieved from the failed message.</param>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected abstract Task OnFailedDeletingMessageAsync(
        string messageId, 
        DataTransferPackage package, 
        object dto,
        CancellationToken cancellation);

    /// <summary>
    /// Determines whether a failed message should be deleted from the queue. By default, returns false.
    /// </summary>
    /// <param name="exception">The exception that occurred while processing the message.</param>
    /// <returns>true if the message should be deleted; otherwise, false.</returns>
    protected virtual bool CanDeleteFailedMessage(Exception exception) => false;

    /// <summary>
    /// Called when the queue is empty. By default, writes a debug message to the console.
    /// </summary>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    protected virtual Task OnQueueIsEmptyAsync(CancellationToken cancellation)
    {
        Debug.WriteLine($"{nameof(OnQueueIsEmptyAsync)}");
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    [DebuggerStepThrough]
    Task ILargeObjectQueueConsumerCallback.OnStartingAsync(CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return OnStartingAsync(cancellation);
    }

    /// <inheritdoc />
    [DebuggerStepThrough]
    Task ILargeObjectQueueConsumerCallback.ProcessAsync(DataTransferPackage package, object dto, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return ProcessAsync(package, dto, cancellation);
    }

    /// <inheritdoc />
    [DebuggerStepThrough]
    Task ILargeObjectQueueConsumerCallback.OnQueueIsEmptyAsync(CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return OnQueueIsEmptyAsync(cancellation);
    }

    /// <inheritdoc />
    [DebuggerStepThrough]
    Task ILargeObjectQueueConsumerCallback.OnMalformedMessageAsync(string messageId, Exception exception, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return OnMalformedMessageAsync(messageId, exception, cancellation);
    }

    /// <inheritdoc />
    [DebuggerStepThrough]
    Task ILargeObjectQueueConsumerCallback.OnUnpackingErrorAsync(
        string messageId, 
        DataTransferPackage package, 
        Exception exception,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return OnUnpackingErrorAsync(messageId, package, exception, cancellation);
    }

    /// <inheritdoc />
    [DebuggerStepThrough]
    Task ILargeObjectQueueConsumerCallback.OnFailedDeletingMessageAsync(
        string messageId, 
        DataTransferPackage package, 
        object dto,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return OnFailedDeletingMessageAsync(messageId, package, dto, cancellation);
    }

    /// <inheritdoc />
    [DebuggerStepThrough]
    bool ILargeObjectQueueConsumerCallback.CanDeleteFailedMessage(Exception exception)
    {
        return CanDeleteFailedMessage(exception);
    }
}