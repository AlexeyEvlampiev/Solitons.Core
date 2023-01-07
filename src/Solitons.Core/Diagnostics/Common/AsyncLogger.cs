using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using System.Text.Json.Serialization;
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

        /// <summary>
        /// 
        /// </summary>
        protected class LogJsonBuilder : Dictionary<string, object?>, ILogStringBuilder
        {
            private readonly HashSet<string> _tags = new(StringComparer.Ordinal);
            private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            /// <summary>
            /// 
            /// </summary>
            public LogJsonBuilder() 
            {
                Add("tags", _tags);
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="options"></param>
            public LogJsonBuilder(JsonSerializerOptions options) : this()
            {
                _options = options;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public ILogStringBuilder WithProperty(string name, object value)
            {
                base[name] = value;
                return this;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="tag"></param>
            /// <returns></returns>
            public ILogStringBuilder WithTags(string tag)
            {
                _tags.Add(tag);
                return this;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public override string ToString() => JsonSerializer.Serialize(this, _options);
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
                .WithProperty("level", level.ToString().ToLower())
                .WithProperty("message", message);

            principal ??= Thread.CurrentPrincipal;
            var identity = principal?.Identity;
            if (identity != null)
            {
                builder
                    .WithProperty("user", identity?.Name ?? "anonymous");
            }

            builder
                .WithProperty("source", sourceInfo);
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
