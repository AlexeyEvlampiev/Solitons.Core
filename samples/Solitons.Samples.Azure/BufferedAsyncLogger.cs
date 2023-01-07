using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Security.Cryptography;
using System.Security.Principal;
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Queues;
using Solitons.Data;
using Solitons.Diagnostics;
using Solitons.Diagnostics.Common;

namespace Solitons.Samples.Azure
{
    public sealed class BufferedAsyncLogger : AsyncLogger
    {
        private readonly QueueClient _bufferQueue;
        private readonly EventHubProducerClient _logsHub;
        private readonly IDisposable _bufferQueueFlushingJob;
        private readonly MD5 _md5 = MD5.Create();
        private int _flushing;


        public BufferedAsyncLogger(QueueClient bufferQueue, EventHubProducerClient logsHub)
        {
            _bufferQueue = bufferQueue ?? throw new ArgumentNullException(nameof(bufferQueue));
            _logsHub = logsHub ?? throw new ArgumentNullException(nameof(logsHub));
            _bufferQueueFlushingJob = Observable
                .Interval(TimeSpan.FromSeconds(15))
                .SelectMany(FlushBufferAsync)
                .Subscribe();
        }



        protected override async Task LogAsync(LogEventArgs args)
        {
            try
            {
                var data = args
                    .Content
                    .ToUtf8Bytes();
                await _bufferQueue.SendMessageAsync(new BinaryData(data));
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
        }


        async Task<bool> FlushBufferAsync(long tick)
        {
            if (Interlocked.CompareExchange(ref _flushing, 1, 0) == 1)
                return false;
            try
            {
                for (int batch = 0; await FlushBatchAsync(); ++batch)
                {
                    Debug.WriteLine($"Flushed log entries batch {batch+1}");
                }
                Interlocked.Exchange(ref _flushing, 0);
                return true;
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                Interlocked.Exchange(ref _flushing, 0);
                return false;
            }

            async Task<bool> FlushBatchAsync()
            {
                var messages = await _bufferQueue
                    .ReceiveMessagesAsync(32)
                    .ToObservable()
                    .SelectMany(r => r.Value)
                    .ToList();
                Debug.WriteLine($"Buffer queue messages: {messages.Count}");
                if (messages.Count == 0)
                    return false;
  
                var eventBatch = await _logsHub.CreateBatchAsync();
                foreach (var message in messages)
                {
                    var messageId = _md5
                        .ComputeHash(message.Body.ToArray())
                        .ToBase64String();

                    var eventData = new EventData(message.Body)
                    {
                        ContentType = "application/json",
                        MessageId = messageId
                    };
                    if (false == eventBatch.TryAdd(eventData))
                    {
                        await _logsHub.SendAsync(eventBatch);
                        Debug.WriteLine($"Flushing batch of {eventBatch.Count} log entries.");
                        eventBatch.Dispose();
                        eventBatch = await _logsHub.CreateBatchAsync();
                        if (false == eventBatch.TryAdd(eventData))
                        {
                            Trace.TraceError($"Log entry is too large.");
                        }
                    }
                }

                if (eventBatch.Count > 0)
                {
                    await _logsHub.SendAsync(eventBatch);
                    Debug.WriteLine($"Flushing batch of {eventBatch.Count} log entries.");
                    eventBatch.Dispose();
                }

                await messages
                    .ToObservable()
                    .SelectMany(m => _bufferQueue.DeleteMessageAsync(m.MessageId, m.PopReceipt));
                eventBatch.Dispose();
                return true;
            }
        }

    }
}
