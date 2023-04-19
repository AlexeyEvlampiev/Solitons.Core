using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data;

/// <summary>
/// Defines methods to handle events that occur during large object queue processing.
/// </summary>
public interface ILargeObjectQueueConsumerCallback
{
    /// <summary>
    /// Called when the queue processing is starting.
    /// </summary>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnStartingAsync(CancellationToken cancellation);

    /// <summary>
    /// Called when an object is retrieved from the queue for processing.
    /// </summary>
    /// <param name="package">The package containing the object and related metadata.</param>
    /// <param name="dto">The object retrieved from the queue.</param>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ProcessAsync(DataTransferPackage package, object dto, CancellationToken cancellation);

    /// <summary>
    /// Called when the queue is empty and there are no more objects to process.
    /// </summary>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnQueueIsEmptyAsync(CancellationToken cancellation);

    /// <summary>
    /// Called when a message in the queue is malformed and cannot be processed.
    /// </summary>
    /// <param name="messageId">The ID of the malformed message.</param>
    /// <param name="exception">The exception that occurred while processing the message.</param>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnMalformedMessageAsync(string messageId, Exception exception, CancellationToken cancellation);


    /// <summary>
    /// Called when an error occurs while unpacking the object from the queue.
    /// </summary>
    /// <param name="messageId">The ID of the message containing the object.</param>
    /// <param name="package">The package containing the object and related metadata.</param>
    /// <param name="exception">The exception that occurred while unpacking the object.</param>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnUnpackingErrorAsync(string messageId, DataTransferPackage package, Exception exception, CancellationToken cancellation);

    /// <summary>
    /// Called when an error occurs while deleting a failed message from the queue.
    /// </summary>
    /// <param name="messageId">The ID of the failed message.</param>
    /// <param name="package">The package containing the object and related metadata.</param>
    /// <param name="dto">The object retrieved from the failed message.</param>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task OnFailedDeletingMessageAsync(string messageId, DataTransferPackage package, object dto, CancellationToken cancellation);

    /// <summary>
    /// Determines whether a failed message should be deleted from the queue.
    /// </summary>
    /// <param name="exception">The exception that occurred while processing the message.</param>
    /// <returns>true if the message should be deleted; otherwise, false.</returns>
    /// <remarks>
    /// This method is called when an error occurs while processing a message from the queue.
    /// If the method returns true, the message will be deleted from the queue. If it returns false,
    /// the message will be moved to a dead-letter queue for further inspection. The exception parameter
    /// contains information about the error that occurred while processing the message, which can be used
    /// to determine whether the message can be safely deleted or should be examined further.
    /// </remarks>
    bool CanDeleteFailedMessage(Exception exception);
}