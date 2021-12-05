using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Queues
{
    sealed class DomainQueue : IDomainQueue
    {
        private readonly IDomainTransientStorage _transientStorage;
        private readonly IQueueServiceProvider _provider;

        public DomainQueue(Domain domain, IQueueServiceProvider provider, ITransientStorage transientStorage)
        {
            if (domain == null) throw new ArgumentNullException(nameof(domain));
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (transientStorage == null) throw new ArgumentNullException(nameof(transientStorage));
           var serializer = domain.GetSerializer();
            _provider = provider.AsQueueServiceProvider();
            _transientStorage = new DomainTransientStorage(transientStorage, serializer);
        }

        public async Task<DomainTransientStorageReceipt> SendMessageAsync(object dto, Action<DomainQueueMessageOptions> config = default, CancellationToken cancellation = default)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            cancellation.ThrowIfCancellationRequested();


            var options = new DomainQueueMessageOptions(_provider);
            config?.Invoke(options);
            var receipt = await _transientStorage.UploadAsync(
                dto, 
                _provider.MessageMaxTimeToLive, 
                _provider.MessageMaxSizeInBytes, 
                cancellation);

            await _provider.SendAsync(receipt.ToArray(), options, cancellation);
            return receipt;
        }

        public bool CanSend(Type type) => _transientStorage.CanUpload(type);


        [DebuggerStepThrough]
        public IObservable<object> ToObservable(IDomainQueueStreamConsumerCallback callback, IAsyncLogger logger)
        {
            callback.ThrowIfNullArgument(nameof(callback));
            logger.ThrowIfNullArgument(nameof(logger));

            return Observable
                .Create<object>(SubscribeAsync)
                .Publish()
                .RefCount();

            [DebuggerStepThrough]
            async Task SubscribeAsync(IObserver<object> observer, CancellationToken cancellationToken)
            {
                try
                {
                    await StreamAsync(observer, callback, logger, cancellationToken);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    if (e is OperationCanceledException)
                    {
                        observer.OnCompleted();
                    }
                    else
                    {
                        observer.OnError(e);
                    }
                }
            }
        }

        
        [DebuggerStepThrough]
        public IObservable<IReadOnlyList<object>> ToObservable(
            IDomainQueueBatchConsumerCallback callback,                                                                             
            IAsyncLogger logger)
        {
            callback.ThrowIfNullArgument(nameof(callback));
            logger.ThrowIfNullArgument(nameof(logger));

            return Observable
                .Create<IReadOnlyList<object>>(SubscribeAsync)
                .Publish()
                .RefCount();

            [DebuggerStepThrough]
            async Task SubscribeAsync(IObserver<IReadOnlyList<object>> observer, CancellationToken cancellation)
            {
                try
                {
                    await StreamAsync(observer, callback, logger, cancellation);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    if (e is OperationCanceledException)
                    {
                        observer.OnCompleted();
                    }
                    else
                    {
                        observer.OnError(e);
                    }
                }
            }
        }

        private async Task StreamAsync(
            IObserver<object> observer,
            IDomainQueueStreamConsumerCallback callback,
            IAsyncLogger logger,
            CancellationToken cancellation)
        {
            await callback.InitializeAsync(logger, cancellation);
            int receiveAttempt = 0;
            while (cancellation.IsCancellationRequested == false)
            {
                var visibilityTimeout = await callback
                    .GetRequiredMessageVisibilityTimeoutAsync(
                        _provider.MinMessageVisibilityTimeout,
                        _provider.MaxMessageVisibilityTimeout,
                        logger,
                        cancellation);
                visibilityTimeout.ThrowIfOutOfRange(
                    _provider.MinMessageVisibilityTimeout,
                    _provider.MaxMessageVisibilityTimeout, () => new InvalidOperationException(""));


                await using var message = await _provider.ReceiveAsync(visibilityTimeout, callback.RequiredBehaviour, cancellation);
                if (message is null)
                {
                    await callback.WhenReadyForNextDequeueAttemptAsync(receiveAttempt++, logger, cancellation);
                    continue;
                }

                receiveAttempt = 0;
                if (false == DomainTransientStorageReceipt.TryDeserialize(message.Body, out var receipt))
                {
                    await callback.OnMalformedMessageAsync(message, logger, cancellation);
                    continue;
                }

                var dto = await _transientStorage.DownloadAsync(receipt, cancellation);
                if (dto is not null)
                {
                    observer.OnNext(dto);
                }
            }
        }

        private async Task StreamAsync(
            IObserver<IReadOnlyList<object>> observer,
            IDomainQueueBatchConsumerCallback callback,
            IAsyncLogger logger,
            CancellationToken cancellation)
        {
            await callback.InitializeAsync(logger, cancellation);
            int emptyQueueResponseCount = 0;
            while (!cancellation.IsCancellationRequested)
            {
                var batchSize = await callback.GetRequiredBatchSizeAsync(1, 2000, logger, cancellation);
                var visibilityTimeout = await callback.GetRequiredMessageVisibilityTimeoutAsync(TimeSpan.FromSeconds(1), TimeSpan.FromDays(7), logger, cancellation);
                var behaviour = callback.RequiredBehaviour;

                if (batchSize < 1)
                    throw new InvalidOperationException($"{callback.GetType()}.{nameof(callback.GetRequiredBatchSizeAsync)} returned zero or negative value.");
                if (visibilityTimeout <= TimeSpan.Zero)
                    throw new InvalidOperationException($"{callback.GetType()}.{nameof(callback.GetRequiredMessageVisibilityTimeoutAsync)} returned a non-positive value.");

                var batch = await _provider.ReceiveBatchAsync(
                    batchSize,
                    visibilityTimeout,
                    behaviour,
                    cancellation)
                    .SelectMany(message =>
                    {
                        if (DomainTransientStorageReceipt.TryDeserialize(message.Body, out var receipt))
                        {
                            return _transientStorage
                                .DownloadAsync(receipt, cancellation)
                                .ToObservable()
                                .Select(dto=> KeyValuePair.Create(message, dto));
                        }

                        return callback
                            .OnMalformedMessageAsync(message, logger, cancellation)
                            .ToObservable()
                            .SelectMany(Observable.Empty<KeyValuePair<IQueueMessage, object>>());

                    })
                    .ToDictionary();
                if (batch.Count == 0)
                {
                    Interlocked.Increment(ref emptyQueueResponseCount);
                    await callback.WhenReadyForNextAttemptToReceiveMessagesAsync(emptyQueueResponseCount, logger, cancellation);
                    continue;
                }

                emptyQueueResponseCount = 0;
                await callback.OnBatchAsync(batch, logger, cancellation);

                if (behaviour == QueueConsumerBehaviour.AutoComplete)
                {
                    var messages = batch.Keys;
                    await Task.WhenAll(messages
                        .Select(message => message.CompleteIfActiveAsync(false, cancellation)));
                }

                observer.OnNext(batch.Values.ToList());
            }

            if (cancellation.IsCancellationRequested)
            {
                observer.OnCompleted();
            }
        }
    }
}
