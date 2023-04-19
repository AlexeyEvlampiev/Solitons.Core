using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data;

/// <summary>
/// Represents an object that consumes large objects from a queue.
/// </summary>
public interface ILargeObjectQueueConsumer
{
    /// <summary>
    /// Processes objects retrieved from the queue asynchronously.
    /// </summary>
    /// <param name="callback">The callback object to handle retrieved objects.</param>
    /// <param name="cancellation">The token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method represents the core logic of the consumer object, and is responsible
    /// for retrieving objects from the queue and passing them to the callback object
    /// for processing. The cancellation token allows the caller to cancel the operation
    /// if needed.
    /// </remarks>
    Task ProcessAsync(
        ILargeObjectQueueConsumerCallback callback,
        CancellationToken cancellation);
}