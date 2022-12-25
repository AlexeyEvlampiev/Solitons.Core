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
        /// <param name="callerLineNumber"></param>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="details"></param>
        /// <param name="config"></param>
        /// <param name="mode"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <returns></returns>
        public async Task LogAsync(
            string callerMemberName,
            string callerFilePath,
            int callerLineNumber,
            LogLevel level,
            string message,
            string? details,
            Action<ILogEntryBuilder>? config,
            LogMode mode = LogMode.Strict)
        {
            var entry = new LogEntry(level, message, details);
            config?.Invoke(entry);
            AddCallerInfo(entry, callerMemberName, callerFilePath, callerLineNumber);
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
        /// <param name="entry"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        protected virtual void AddCallerInfo(
            ILogEntryBuilder entry,
            string callerMemberName,
            string callerFilePath,
            int callerLineNumber)
        {
            entry
                .WithProperty("callerMemberName", callerMemberName)
                .WithProperty("callerFilePath", callerFilePath)
                .WithProperty("callerLineNumber", callerLineNumber.ToString());
        }
        
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
