using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Solitons.Diagnostics
{
    sealed class AsyncLoggerProxy : IAsyncLogger
    {
        #region Private Fields
        private readonly IAsyncLogger _innerLogger;
        private readonly Action<ILogEntryBuilder> _innerConfig;
        #endregion


        [DebuggerNonUserCode]
        internal AsyncLoggerProxy(IAsyncLogger innerLogger, Action<ILogEntryBuilder> innerConfig)
        {
            if (innerLogger is AsyncLoggerProxy other)
            {
                _innerLogger = other._innerLogger;
                _innerConfig = other._innerConfig + innerConfig;
            }
            else
            {
                _innerLogger = innerLogger;
                _innerConfig = innerConfig;
            }
        }



        [DebuggerStepThrough]
        public Task LogAsync(
            string callerMemberName,
            string callerFilePath,
            int callerLineNumber, 
            LogLevel level, 
            string message, 
            string? details,
            Action<ILogEntryBuilder>? config = null, 
            LogMode mode = LogMode.Strict)
        {
            config = config is null
                ? _innerConfig
                : _innerConfig + config;

            return _innerLogger.LogAsync(
                callerMemberName, 
                callerFilePath, 
                callerLineNumber,
                level, 
                message,
                details,
                config, 
                mode);
        }


        [DebuggerStepThrough]
        public IObservable<ILogEntry> AsObservable() => _innerLogger.AsObservable();
        
        [DebuggerStepThrough]
        public IObserver<ILogEntry> AsObserver() => _innerLogger.AsObserver();

        [DebuggerStepThrough]
        public override string ToString() => _innerLogger.ToString() ?? _innerLogger.GetType().ToString();

        [DebuggerStepThrough]
        public override bool Equals(object? obj) => _innerLogger.Equals(obj);

        [DebuggerStepThrough]
        public override int GetHashCode() => _innerLogger.GetHashCode();
    }
}
