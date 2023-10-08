using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Solitons.Diagnostics
{
    sealed class AsyncLoggerProxy : IAsyncLogger
    {
        #region Private Fields
        private readonly IAsyncLogger _innerLogger;
        private readonly Action<ILogStringBuilder> _innerConfig;
        #endregion

        [DebuggerNonUserCode]
        internal AsyncLoggerProxy(
            IAsyncLogger innerLogger, 
            Action<ILogStringBuilder> innerConfig)
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


        Task IAsyncLogger.LogAsync(
            LogLevel level, 
            string message,
            LogMode mode,
            string callerMemberName,
            string callerFilePath, 
            int callerLineNumber, 
            Action<ILogStringBuilder>? config)
        {
            config = config is null
                ? _innerConfig
                : _innerConfig + config;
            return _innerLogger.LogAsync(
                level,
                message,
                mode,
                callerMemberName, 
                callerFilePath, 
                callerLineNumber, 
                config);
        }


        [DebuggerStepThrough]
        public IObservable<LogEventArgs> AsObservable() => _innerLogger.AsObservable();
        

        [DebuggerStepThrough]
        public override string ToString() => _innerLogger.ToString() ?? _innerLogger.GetType().ToString();

        [DebuggerStepThrough]
        public override bool Equals(object? obj) => _innerLogger.Equals(obj);

        [DebuggerStepThrough]
        public override int GetHashCode() => _innerLogger.GetHashCode();
    }
}
