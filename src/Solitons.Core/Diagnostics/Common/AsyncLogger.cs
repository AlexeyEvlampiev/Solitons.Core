using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Solitons.Diagnostics.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AsyncLogger : IAsyncLogger
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Subject<LogEventArgs> _logs = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        protected abstract Task LogAsync(LogEventArgs args);

        sealed class LogJsonBuilder : Dictionary<string, object?>, ILogStringBuilder
        {
            public ILogStringBuilder WithProperty(string name, object value)
            {
                base[name] = value;
                return this;
            }

            public ILogStringBuilder WithTags(string tag)
            {
                if (base.ContainsKey(tag) == false)
                {
                    base[tag] = null;
                }

                return this;
            }


            public override string ToString() => JsonSerializer.Serialize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected virtual string FormatSourceFilePath(string filePath) => Path.GetFileName(filePath);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="principal"></param>
        /// <param name="sourceInfo"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected virtual ILogStringBuilder CreateLogStringBuilder(
            LogLevel level,
            string message,
            IPrincipal? principal,
            CallerInfo sourceInfo)
        {
            ILogStringBuilder builder = new LogJsonBuilder()
                .WithProperty("level", level.ToString())
                .WithProperty("message", message);

            principal ??= Thread.CurrentPrincipal;
            var identity = principal?.Identity;
            if (identity != null)
            {
                builder
                    .WithProperty("user", identity?.Name ?? "anonymous");
            }

            builder
                .WithProperty("source", new
                {
                    name = sourceInfo.MemberName,
                    file = sourceInfo.FilePath,
                    line = sourceInfo.LineNumber
                });
            return builder;
        }



        async Task IAsyncLogger.LogAsync(
            LogLevel level,
            string message,
            LogMode mode,
            IPrincipal? principal,
            string callerMemberName,
            string callerFilePath,
            int callerLineNumber,
            Action<ILogStringBuilder>? config)
        {
            var sourceInfo = new CallerInfo
            {
                MemberName = callerMemberName,
                FilePath = FormatSourceFilePath(callerFilePath),
                LineNumber = callerLineNumber
            };
            var builder = CreateLogStringBuilder(level, message, principal, sourceInfo);
            
            config?.Invoke(builder);


            var content = builder.ToString();
            var args = new LogEventArgs(
                level,
                message,
                principal,
                sourceInfo,
                content);

            try
            {
                var task = LogAsync(args);
                if (mode == LogMode.Strict)
                {
                    await task;
                }

                if (_logs.HasObservers)
                {
                    _logs.OnNext(args);
                }
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
        /// <returns></returns>
        public IObservable<LogEventArgs> AsObservable() => _logs.AsObservable();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IAsyncLogger AsAsyncLogger() => this;

    }
}
