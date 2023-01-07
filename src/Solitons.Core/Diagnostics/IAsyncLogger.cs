using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Diagnostics.Common;

namespace Solitons.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IAsyncLogger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="exception"></param>
        protected virtual void AppendException(ILogStringBuilder builder, Exception exception)
        {
            builder
                .WithProperty("exception", exception.ToString());
        }

        /// <summary>
        /// Logs the specified message asynchronously.
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The message to be logged</param>
        /// <param name="callerLineNumber"></param>
        /// <param name="config">The log entry configuration callback</param>
        /// <param name="mode">he logging mode</param>
        /// <param name="principal"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <returns></returns>
        protected internal abstract Task LogAsync(
            LogLevel level, 
            string message,
            LogMode mode,
            IPrincipal? principal,
            string callerMemberName,
            string callerFilePath,
            int callerLineNumber,
            Action<ILogStringBuilder>? config);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IObservable<LogEventArgs> AsObservable();
    }

    public partial interface IAsyncLogger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public sealed IAsyncLogger WithPrincipal(IPrincipal principal)
        {
            return new AsyncLoggerProxy(
                this, 
                builder =>{}, 
                principal);
        }

        /// <summary>
        /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified tag to every log entry.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>Extended <see cref="IAsyncLogger"/> instance</returns>
        [DebuggerStepThrough]
        public sealed IAsyncLogger WithTags(string tag) => new AsyncLoggerProxy(this, builder => builder.WithTags(tag));

        /// <summary>
        /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified tags to every log entry.
        /// </summary>
        /// <param name="tag0">The first tag.</param>
        /// <param name="tag1">The second tag.</param>
        /// <returns>Extended <see cref="IAsyncLogger"/> instance</returns>
        [DebuggerStepThrough]
        public sealed IAsyncLogger WithTags(string tag0, string tag1) => new AsyncLoggerProxy(this, builder => builder.WithTags(tag0, tag1));

        /// <summary>
        /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified tags to every log entry.
        /// </summary>
        /// <param name="tag0">The first tag.</param>
        /// <param name="tag1">The second tag.</param>
        /// <param name="tag2">The third tag.</param>
        /// <returns>Extended <see cref="IAsyncLogger"/> instance</returns>
        [DebuggerStepThrough]
        public sealed IAsyncLogger WithTags(string tag0, string tag1, string tag2) => new AsyncLoggerProxy(this, builder => builder.WithTags(tag0, tag1, tag2));

        /// <summary>
        /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified tags to every log entry.
        /// </summary>
        /// <param name="tags">The tags.</param>
        /// <returns>Extended <see cref="IAsyncLogger"/> instance</returns>
        [DebuggerStepThrough]
        public sealed IAsyncLogger WithTags(params string[] tags) => new AsyncLoggerProxy(this, builder => builder.WithTags(tags));

        /// <summary>
        /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified property to every log entry.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        /// <returns>Extended <see cref="IAsyncLogger"/> instance</returns>
        [DebuggerStepThrough]
        public sealed IAsyncLogger WithProperty(string name, object value) => new AsyncLoggerProxy(this, builder => builder.WithProperty(name, value));

        /// <summary>
        /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified property to every log entry.
        /// </summary>
        /// <param name="properties">The properties key-value collection.</param>
        /// <returns>Extended <see cref="IAsyncLogger"/> instance</returns>
        [DebuggerStepThrough]
        public sealed IAsyncLogger WithProperties(IEnumerable<KeyValuePair<string, object>> properties) => new AsyncLoggerProxy(this, builder => builder.WithProperties(properties));

    }

    public partial interface IAsyncLogger
    {
        /// <summary>
        /// Null Object implementation
        /// </summary>
        public static IAsyncLogger Null => AsyncNullObjectLogger.Instance;



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public Func<IAsyncLogger> AsFactory() => () => this;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="config"></param>
        /// <param name="mode"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public sealed Task ErrorAsync(
            string message,
            Action<ILogStringBuilder>? config = null,
            LogMode mode = LogMode.Strict,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            return this.LogAsync(
                LogLevel.Error,
                message,
                mode,
                Thread.CurrentPrincipal,
                callerMemberName,
                callerFilePath,
                callerLineNumber,
                config);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="config"></param>
        /// <param name="mode"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        /// <returns></returns>
        public sealed Task ErrorAsync(
            Exception ex,
            Action<ILogStringBuilder>? config = null,
            LogMode mode = LogMode.Strict,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            config = (config == null)
                ? ConfigExtension
                : config + ConfigExtension;

            return LogAsync(
                LogLevel.Error,
                ex.Message,
                mode,
                Thread.CurrentPrincipal,
                callerMemberName,
                callerFilePath,
                callerLineNumber,
                config);

            void ConfigExtension(ILogStringBuilder builder) => AppendException(builder, ex);
        }

        /// <summary>
        /// Logs the specified warning message asynchronously.
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="config">The log entry configuration callback</param>
        /// <param name="mode">The logging mode</param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        [DebuggerStepThrough]
        public sealed Task WarningAsync(
            string message,
            Action<ILogStringBuilder>? config = null,
            LogMode mode = LogMode.Strict,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            return this.LogAsync(
                LogLevel.Warning,
                message,
                mode,
                Thread.CurrentPrincipal,
                callerMemberName,
                callerFilePath,
                callerLineNumber,
                config);
        }


        /// <summary>
        /// Logs the specified information message asynchronously.
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="config">The log entry configuration callback</param>
        /// <param name="mode">The logging mode</param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        [DebuggerStepThrough]
        public sealed Task InfoAsync(
            string message,
            Action<ILogStringBuilder>? config = null,
            LogMode mode = LogMode.Strict,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            return this.LogAsync(
                LogLevel.Info,
                message,
                mode,
                Thread.CurrentPrincipal,
                callerMemberName,
                callerFilePath,
                callerLineNumber,
                config);
        }

    }
}
