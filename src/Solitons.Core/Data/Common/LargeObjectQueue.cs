using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common;

/// <summary>
/// Represents a large object queue that can be used to send and receive large objects
/// between different parts of a system, with support for both by-value and by-reference
/// transfer methods.
/// </summary>
public abstract class LargeObjectQueue : ILargeObjectQueue
{
    private const int EmptyQueueMinPullDelayInMilliseconds = 200;

    /// <summary>
    /// Represents a message in the large object queue, which can be used to
    /// acquire a lock on the message and delete it from the queue.
    /// </summary>
    protected interface IMessage
    {
        /// <summary>
        /// Gets the body of the message.
        /// </summary>
        string Body { get; }

        /// <summary>
        /// Gets the unique identifier of the message.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Deletes the message from the queue.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task DeleteAsync();

        /// <summary>
        /// Acquires a lock on the message, which should be released when the message
        /// has been processed or deleted.
        /// </summary>
        /// <param name="cancellation">A <see cref="CancellationToken"/> that can be used to
        /// cancel the asynchronous operation.</param>
        /// <returns>An <see cref="IAsyncDisposable"/> that represents the lock on the message.</returns>
        Task<IAsyncDisposable> AcquireLockAsync(CancellationToken cancellation);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="LargeObjectQueue"/> class with the
    /// specified serializer.
    /// </summary>
    /// <param name="serializer">The serializer to use for packing and unpacking objects.</param>
    /// <exception cref="ArgumentNullException">Thrown when the <paramref name="serializer"/> parameter is <see langword="null"/>.</exception>
    protected LargeObjectQueue(IDataContractSerializer serializer)
    {
        Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    /// <summary>
    /// Gets the serializer used by the large object queue to pack and unpack objects.
    /// </summary>
    protected IDataContractSerializer Serializer { get; }

    /// <summary>
    /// Sends a message with the specified body to the queue.
    /// </summary>
    /// <param name="body">The body of the message to send.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="body"/> is null.</exception>
    protected abstract Task SendAsync(string body, CancellationToken cancellation);

    /// <summary>
    /// Stores a large data transfer package in a transient storage system, so that it can be referenced by a smaller data transfer
    /// package later on.
    /// </summary>
    /// <param name="message">The data transfer package to store.</param>
    /// <param name="cancellation">A cancellation token to observe while waiting for the operation to complete.</param>
    /// <returns>The smaller data transfer package that references the stored large package.</returns>
    /// <exception cref="Exception">Thrown when the storage system fails to store the package.</exception>
    protected abstract Task<DataTransferPackage> StoreAsideAsync(DataTransferPackage message, CancellationToken cancellation);

    /// <summary>
    /// Dereferences a serialized object, returning its deserialized representation as a <see cref="DataTransferPackage"/>.
    /// If the object is already a reference, it retrieves it from the storage and returns its content.
    /// </summary>
    /// <param name="dto">The object to dereference.</param>
    /// <param name="cancellation">A cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation that returns a <see cref="DataTransferPackage"/>.</returns>
    /// <exception cref="Exception">Thrown when the dereferencing operation fails.</exception>
    /// <exception cref="OperationCanceledException">Thrown when the operation is canceled.</exception>
    protected abstract Task<DataTransferPackage> DereferenceAsync(object dto, CancellationToken cancellation);

    /// <summary>
    /// Determines if the provided object is a reference to a large object.
    /// </summary>
    /// <param name="dto">The object to check.</param>
    /// <returns>true if the object is a reference to a large object; otherwise, false.</returns>
    protected abstract bool IsReference(object dto);

    /// <summary>
    /// Determines if the provided exception represents an error where the message payload is too large for the queue.
    /// </summary>
    /// <param name="exception">The exception to check.</param>
    /// <returns>
    ///   <c>true</c> if the exception represents a message payload that is too large for the queue;
    ///   otherwise, <c>false</c>.
    /// </returns>
    protected abstract bool IsMessageOversizeError(Exception exception);

    /// <summary>
    /// Receives a message from the queue asynchronously.
    /// </summary>
    /// <param name="cancellation">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The result of the task contains an <see cref="IMessage"/> instance if a message is received, or null if the queue is empty.</returns>
    /// <exception cref="OperationCanceledException">The <paramref name="cancellation"/> token has been canceled.</exception>
    protected abstract Task<IMessage?> ReceiveMessageAsync(CancellationToken cancellation);

    /// <summary>
    /// This method is called just before a message is sent.
    /// Override this method to add any necessary processing on the message before it is sent.
    /// </summary>
    /// <param name="package">The <see cref="DataTransferPackage"/> that is being sent.</param>
    protected virtual void OnSending(DataTransferPackage package) {}

    /// <summary>
    /// Sends an object to the large object queue using the specified transfer method.
    /// </summary>
    /// <param name="dto">The object to send to the queue.</param>
    /// <param name="preferredMethod">The preferred transfer method to use for the object, either
    /// by-value or by-reference.</param>
    /// <param name="config">A callback that can be used to configure the data transfer package
    /// before it is sent to the queue.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> that can be used to cancel
    /// the asynchronous operation.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation that returns
    /// the actual transfer method used to send the object.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the object is too large to send by value
    /// and the preferred method is not by reference.</exception>
    async Task<DataTransferMethod> ILargeObjectQueueProducer.SendAsync(
        object dto,
        DataTransferMethod preferredMethod,
        Action<DataTransferPackage>? config,
        CancellationToken cancellation)
    {
        var actualMethod = preferredMethod;
        var package = Serializer.Pack(dto);
        config?.Invoke(package);
        if (preferredMethod == DataTransferMethod.ByReference)
        {
            var packageRef = await StoreAsideAsync(package, cancellation);
            if (packageRef.Content.Count < package.Content.Count)
            {
                package = packageRef;
                config?.Invoke(package);
                actualMethod = DataTransferMethod.ByReference;
            }
            else
            {
                actualMethod = DataTransferMethod.ByValue;
            }
        }

        try
        {
            await SendAsync(package.ToString(), cancellation);
            return DataTransferMethod.ByValue;
        }
        catch (Exception e) when (
            IsMessageOversizeError(e) && 
            actualMethod == DataTransferMethod.ByValue &&
            preferredMethod != DataTransferMethod.ByReference)
        {
            var packageRef = await StoreAsideAsync(package, cancellation);
            if (packageRef.Content.Count >= package.Content.Count)
            {
                throw new InvalidOperationException(
                    $"Transient storage reference size > size of the object stored.");
            }
            config?.Invoke(package);
            await SendAsync(packageRef.ToString(), cancellation);
            return DataTransferMethod.ByReference;
        }
    }

    /// <summary>
    /// Sends a large object package to the queue using the specified <paramref name="preferredMethod"/> for transfer,
    /// falling back to <see cref="DataTransferMethod.ByReference"/> if the object size exceeds the limit for the
    /// <paramref name="preferredMethod"/> and it is not already <see cref="DataTransferMethod.ByReference"/>.
    /// If the <see cref="DataTransferMethod.ByReference"/> is used, a transient storage is created for the object,
    /// and a reference to it is sent instead of the original object.
    /// </summary>
    /// <param name="package">The package containing the large object to send.</param>
    /// <param name="preferredMethod">The preferred method of data transfer.</param>
    /// <param name="cancellation">The token used to cancel the operation.</param>
    /// <returns>The method actually used to transfer the data.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the <paramref name="preferredMethod"/> is
    /// <see cref="DataTransferMethod.ByReference"/> and the transient storage reference size is larger than the
    /// size of the object stored. This can happen when the object contains circular references.</exception>
    async Task<DataTransferMethod> ILargeObjectQueueProducer.SendAsync(
        DataTransferPackage package,
        DataTransferMethod preferredMethod,
        CancellationToken cancellation)
    {
        var actualMethod = preferredMethod;
        if (preferredMethod == DataTransferMethod.ByReference)
        {
            var packageRef = await StoreAsideAsync(package, cancellation);
            if (packageRef.Content.Count < package.Content.Count)
            {
                package = packageRef;
                actualMethod = DataTransferMethod.ByReference;
            }
            else
            {
                actualMethod = DataTransferMethod.ByValue;
            }
        }

        try
        {
            await SendAsync(package.ToString(), cancellation);
            return DataTransferMethod.ByValue;
        }
        catch (Exception e) when (
            IsMessageOversizeError(e) &&
            actualMethod == DataTransferMethod.ByValue &&
            preferredMethod != DataTransferMethod.ByReference)
        {
            var packageRef = await StoreAsideAsync(package, cancellation);
            if (packageRef.Content.Count >= package.Content.Count)
            {
                throw new InvalidOperationException(
                    $"Transient storage reference size > size of the object stored.");
            }
                
            await SendAsync(packageRef.ToString(), cancellation);
            return DataTransferMethod.ByReference;
        }
    }

    /// <summary>
    /// Processes messages in the queue asynchronously and invokes the provided callback for each message.
    /// </summary>
    /// <param name="callback">The callback to invoke for each message.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="callback"/> is null.</exception>
    public async Task ProcessAsync(ILargeObjectQueueConsumerCallback callback, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        await callback.OnStartingAsync(cancellation);

        while (false == cancellation.IsCancellationRequested)
        {
            var message = await ReceiveMessageAsync(cancellation);
            if (message is null)
            {
                await Task.WhenAll(
                    callback.OnQueueIsEmptyAsync(cancellation),
                    Task.Delay(EmptyQueueMinPullDelayInMilliseconds, cancellation));
                continue;
            }

            await using var msgLock = await message.AcquireLockAsync(cancellation);

            DataTransferPackage? package = null;
            object? dto = null;
            bool processed = false;
            try
            {
                package = DataTransferPackage.Parse(message.Body);
                dto = Serializer.Unpack(package);
                if (IsReference(dto))
                {
                    package = await DereferenceAsync(dto, cancellation);
                    dto = Serializer.Unpack(package);
                    if (IsReference(dto))
                    {
                        await callback.OnUnpackingErrorAsync(message.Id, package,
                            new NotSupportedException($"Double referencing is not supported"), cancellation);
                    }
                }
                await callback.ProcessAsync(package, dto, cancellation);
                processed = true;
                await message.DeleteAsync();
            }
            catch (Exception exception)
            {
                if (package is null)
                {
                    await callback.OnMalformedMessageAsync(message.Id, exception, cancellation);
                    await message.DeleteAsync();
                }
                else if(dto is null)
                {
                    await callback.OnUnpackingErrorAsync(message.Id, package, exception, cancellation);
                    await message.DeleteAsync();
                }
                else if(processed)
                {
                    await callback.OnFailedDeletingMessageAsync(message.Id, package, dto, cancellation);
                }
                else if(callback.CanDeleteFailedMessage(exception))
                {
                    try
                    {
                        await message.DeleteAsync();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                        await callback.OnFailedDeletingMessageAsync(message.Id, package, dto, cancellation);
                    }
                }
            }
        }
    }
}