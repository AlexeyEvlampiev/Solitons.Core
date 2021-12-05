using System;
using System.Threading.Tasks;
using Solitons.Common;
using Solitons.Queues;

namespace Solitons.Azure.Queues
{
    sealed class StorageQueueAsyncLogger : AsyncLogger
    {
        private readonly IDomainQueue _queue;

        public StorageQueueAsyncLogger(IDomainQueue queue)
        {
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
            var type = typeof(LogEntryData);
            if (queue.CanSend(type) == false)
                throw new ArgumentException($"{typeof(LogEntryData)} dto type is not supported.", nameof(queue));
        }

        protected override Task LogAsync(ILogEntry entry)
        {
            var dto = new LogEntryData(entry);
            return _queue.SendMessageAsync(dto);

        }
    }
}
