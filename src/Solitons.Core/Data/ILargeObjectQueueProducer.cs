

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data;

/// <summary>
/// Provides methods for sending data to a large object queue.
/// </summary>
public interface ILargeObjectQueueProducer
{
    /// <summary>
    /// Sends a package of data to the queue using the specified transfer method. Returns the actual method used.
    /// </summary>
    /// <param name="package">The package of data to send.</param>
    /// <param name="preferredMethod">The preferred transfer method to use.</param>
    /// <param name="cancellation">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The result is the actual transfer method used.</returns>
    Task<DataTransferMethod> SendAsync(
        DataTransferPackage package,
        DataTransferMethod preferredMethod = DataTransferMethod.ByValue,
        CancellationToken cancellation = default);

    /// <summary>
    /// Sends an object to the queue using the specified transfer method. Returns the actual method used.
    /// </summary>
    /// <param name="dto">The object to send.</param>
    /// <param name="preferredMethod">The preferred transfer method to use.</param>
    /// <param name="config">An optional configuration action to modify the package before sending.</param>
    /// <param name="cancellation">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation. The result is the actual transfer method used.</returns>
    Task<DataTransferMethod> SendAsync(
        object dto,
        DataTransferMethod preferredMethod = DataTransferMethod.ByValue,
        Action<DataTransferPackage>? config = default,
        CancellationToken cancellation = default);
}