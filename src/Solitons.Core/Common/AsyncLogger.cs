using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Solitons.Common
{
    public abstract class AsyncLogger : IAsyncLogger
    {
        public static readonly IAsyncLogger Null = new AsyncNullObjectLogger();

        private readonly Subject<ILogEntry> _logs = new();

        protected abstract Task LogAsync(ILogEntry entry);

        public async Task LogAsync(LogLevel level, string message, Action<ILogEntryBuilder> config = null)
        {
            var entry = new LogEntry()
            {
                Level = level,
                Message = message
            };
            config?.Invoke(entry);

            await LogAsync(entry);
            _logs.OnNext(entry);
        }

        public virtual async Task LogAsync(LogLevel level, Exception ex, Action<ILogEntryBuilder> config = null)
        {
            void Extend(ILogEntryBuilder builder)
            {
                builder
                    .WithTag(ex.GetType().FullName)
                    .WithDetails(ex.ToString());
            }

            config = config is null
                ? Extend
                : config + Extend;

            var entry = new LogEntry()
            {
                Level = level,
                Message = ex.Message
            };


            config?.Invoke(entry);

            await LogAsync(entry);
            _logs.OnNext(entry);
        }

        public IObservable<ILogEntry> AsObservable() => _logs.AsObservable();
    }
}
