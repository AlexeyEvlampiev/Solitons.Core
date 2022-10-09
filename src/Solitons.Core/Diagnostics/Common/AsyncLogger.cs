using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Solitons.Diagnostics.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AsyncLogger : IAsyncLogger
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
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
        /// <param name="mode"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public Task LogAsync(LogLevel level, string message, Action<ILogEntryBuilder>? config = null, LogMode mode = LogMode.Strict) =>
            LogAsync(level, message, null, config, mode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="details"></param>
        /// <param name="config"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public async Task LogAsync(LogLevel level, string message, string? details, Action<ILogEntryBuilder>? config = null, LogMode mode = LogMode.Strict)
        {
            var entry = new LogEntry(level, message, details);
            config?.Invoke(entry);
            try
            {
                var task = LogAsync(entry);
                if (mode == LogMode.Strict)
                {
                    await task;
                }
                _logs.OnNext(entry);
            }
            catch (Exception e)
            {
                Debug.Fail(e.Message);
                Trace.TraceError(e.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="ex"></param>
        /// <param name="config"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual Task LogAsync(LogLevel level, Exception ex, Action<ILogEntryBuilder>? config = null, LogMode mode = LogMode.Strict) =>
            LogAsync(level, ex.Message, ex.ToString(), config, mode);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IObservable<ILogEntry> AsObservable() => _logs
            .ObserveOn(TaskPoolScheduler.Default);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IObserver<ILogEntry> AsObserver() => Observer
            .Create<ILogEntry>(log => LogAsync(log));

    }
}
