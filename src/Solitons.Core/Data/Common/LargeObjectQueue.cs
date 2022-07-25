using System;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class LargeObjectQueue : ILargeObjectQueue
    {
        private const int EmptyQueueMinPullDelayInMilliseconds = 200;
        
        /// <summary>
        /// 
        /// </summary>
        protected interface IMessage
        {
            /// <summary>
            /// 
            /// </summary>
            string Body { get; }

            /// <summary>
            /// 
            /// </summary>
            string Id { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            Task DeleteAsync();

            /// <summary>
            /// 
            /// </summary>
            /// <param name="cancellation"></param>
            /// <returns></returns>
            Task<IAsyncDisposable> AcquireLockAsync(CancellationToken cancellation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected LargeObjectQueue(IDataContractSerializer serializer)
        {
            Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        /// <summary>
        /// 
        /// </summary>
        protected IDataContractSerializer Serializer { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task SendAsync(string message, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<DataTransferPackage> StoreAsideAsync(DataTransferPackage message, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<DataTransferPackage> DereferenceAsync(object dto, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        protected abstract bool IsReference(object dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        protected abstract bool IsMessageOversizeError(Exception exception);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<IMessage?> ReceiveMessageAsync(CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="preferredMethod"></param>
        /// <param name="config"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<DataTransferMethod> SendAsync(
            object dto,
            DataTransferMethod preferredMethod = DataTransferMethod.ByValue,
            Action<DataTransferPackage>? config = default,
            CancellationToken cancellation = default)
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
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
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
                        await TryCatch.Invoke(
                            message.DeleteAsync,
                            ex => callback.OnFailedDeletingMessageAsync(message.Id, package, dto, cancellation));
                    }
                }
            }
        }
    }
}
