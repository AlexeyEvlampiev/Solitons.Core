using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Solitons
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
        public Task LogAsync(LogLevel level, string message, Action<ILogEntryBuilder> config = null)
        {
            config = config is null
                ? _innerConfig
                : _innerConfig + config;
            return _innerLogger.LogAsync(level, message, config);
        }
        
        [DebuggerStepThrough]
        public Task LogAsync(LogLevel level, Exception ex, Action<ILogEntryBuilder> config = null)
        {
            config = config is null
                ? _innerConfig
                : _innerConfig + config;
            return _innerLogger.LogAsync(level, ex, config);
        }

        [DebuggerStepThrough]
        public IObservable<ILogEntry> AsObservable() => _innerLogger.AsObservable();
        
        [DebuggerStepThrough]
        public IObserver<ILogEntry> AsObserver() => _innerLogger.AsObserver();

        [DebuggerStepThrough]
        public override string ToString() => _innerLogger.ToString();

        [DebuggerStepThrough]
        public override bool Equals(object? obj) => _innerLogger.Equals(obj);

        [DebuggerStepThrough]
        public override int GetHashCode() => _innerLogger.GetHashCode();
    }
}
