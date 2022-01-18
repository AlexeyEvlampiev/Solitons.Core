using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Solitons.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AsyncLogger : IAsyncLogger
    {
        private readonly Subject<ILogEntry> _logs = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected abstract Task LogAsync(ILogEntry entry);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public async Task LogAsync(LogLevel level, string message, Action<ILogEntryBuilder> config = null)
        {
            var entry = new LogEntry()
            {
                Level = level,
                Message = message
            };
            config?.Invoke(entry);

            try
            {
                await LogAsync(entry);
                _logs.OnNext(entry);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
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
            try
            {
                await LogAsync(entry);
                _logs.OnNext(entry);
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IObservable<ILogEntry> AsObservable() => _logs.AsObservable();
    }
}
