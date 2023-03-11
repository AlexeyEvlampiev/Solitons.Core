﻿using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Solitons.Diagnostics
{
    sealed class AsyncLoggerProxy : IAsyncLogger
    {
        #region Private Fields
        private readonly IAsyncLogger _innerLogger;
        private readonly Action<ILogStringBuilder> _innerConfig;
        private readonly IPrincipal? _principal;
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
                _principal = other._principal;
            }
            else
            {
                _innerLogger = innerLogger;
                _innerConfig = innerConfig;
            }
        }

        internal AsyncLoggerProxy(
            IAsyncLogger innerLogger,
            Action<ILogStringBuilder> innerConfig,
            IPrincipal principal) 
            : this(innerLogger, innerConfig)
        {
            _principal = principal;
        }

        Task IAsyncLogger.LogAsync(
            LogLevel level, 
            string message,
            LogMode mode, 
            IPrincipal? principal,
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
                _principal,
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
