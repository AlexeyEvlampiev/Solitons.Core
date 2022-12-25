using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Solitons.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IAsyncLogger
    {
        /// <summary>
        /// Logs the specified message asynchronously.
        /// </summary>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        /// <param name="level">The log level</param>
        /// <param name="message">The message to be logged</param>
        /// <param name="details"></param>
        /// <param name="config">The log entry configuration callback</param>
        /// <param name="mode">he logging mode</param>
        /// <returns></returns>
        Task LogAsync(
            string callerMemberName,
            string callerFilePath,
            int callerLineNumber,
            LogLevel level, 
            string message,
            string? details,
            Action<ILogEntryBuilder>? config, 
            LogMode mode = LogMode.Strict);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IObservable<ILogEntry> AsObservable();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IObserver<ILogEntry> AsObserver();
    }

    public partial interface IAsyncLogger
    {
        /// <summary>
        /// Null Object implementation
        /// </summary>
        public static IAsyncLogger Null => AsyncNullObjectLogger.Instance;


        /// <summary>
        /// Console output implementation
        /// </summary>
        public static IAsyncLogger Console => ConsoleAsyncLogger.Instance;


        /// <summary>
        /// Logs the specified error message asynchronously.
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="config">The log entry configuration callback</param>
        /// <param name="mode">The logging mode</param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        [DebuggerStepThrough]
        public Task ErrorAsync(
            string message,
            Action<ILogEntryBuilder>? config = null,
            LogMode mode = LogMode.Strict,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            return LogAsync(
                callerMemberName,
                callerFilePath,
                callerLineNumber,
                LogLevel.Error,
                message,
                null,
                config,
                mode);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="details"></param>
        /// <param name="config"></param>
        /// <param name="mode"></param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public Task ErrorAsync(
            string message,
            string details,
            Action<ILogEntryBuilder>? config = null,
            LogMode mode = LogMode.Strict,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            return LogAsync(
                callerMemberName,
                callerFilePath,
                callerLineNumber,
                LogLevel.Error,
                message,
                details,
                config,
                mode);
        }


        /// <summary>
        /// Logs the specified error asynchronously.
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="config"></param>
        /// <param name="mode">The logging mode</param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public Task ErrorAsync(
            Exception ex, 
            Action<ILogEntryBuilder>? config = null, 
            LogMode mode = LogMode.Strict,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            return LogAsync(
                callerMemberName,
                callerFilePath,
                callerLineNumber,
                LogLevel.Error,
                ex.Message,
                ex.ToString(),
                config,
                mode);
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
        public Task WarningAsync(
            string message, 
            Action<ILogEntryBuilder>? config = null, 
            LogMode mode = LogMode.Strict,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            return LogAsync(
                callerMemberName,
                callerFilePath,
                callerLineNumber,
                LogLevel.Warning,
                message,
                null,
                config,
                mode);
        }

        /// <summary>
        /// Logs the specified warning message asynchronously.
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="details">The details text</param>
        /// <param name="config">The log entry configuration callback</param>
        /// <param name="mode">The logging mode</param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        [DebuggerStepThrough]
        public Task WarningAsync(
            string message, 
            string details, 
            Action<ILogEntryBuilder>? config = null, 
            LogMode mode = LogMode.Strict,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            return LogAsync(
                callerMemberName,
                callerFilePath,
                callerLineNumber,
                LogLevel.Warning,
                message,
                details,
                config,
                mode);
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
        public Task InfoAsync(
            string message, 
            Action<ILogEntryBuilder>? config = null, 
            LogMode mode = LogMode.Strict,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            return LogAsync(
                callerMemberName,
                callerFilePath,
                callerLineNumber,
                LogLevel.Info,
                message,
                null,
                config,
                mode);
        }

        /// <summary>
        /// Logs the specified information message asynchronously.
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="details">The details text</param>
        /// <param name="config">The log entry configuration callback</param>
        /// <param name="mode">The logging mode</param>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        [DebuggerStepThrough]
        public Task InfoAsync(
            string message, 
            string details, 
            Action<ILogEntryBuilder>? config = null, 
            LogMode mode = LogMode.Strict,
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = -1)
        {
            return LogAsync(
                callerMemberName,
                callerFilePath,
                callerLineNumber,
                LogLevel.Info,
                message,
                details,
                config,
                mode);
        }

        /// <summary>
        /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified tag to every log entry.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>Extended <see cref="IAsyncLogger"/> instance</returns>
        [DebuggerStepThrough]
        public IAsyncLogger WithTags(string tag) => new AsyncLoggerProxy(this, builder => builder.WithTags(tag));

        /// <summary>
        /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified tags to every log entry.
        /// </summary>
        /// <param name="tag0">The first tag.</param>
        /// <param name="tag1">The second tag.</param>
        /// <returns>Extended <see cref="IAsyncLogger"/> instance</returns>
        [DebuggerStepThrough]
        public IAsyncLogger WithTags(string tag0, string tag1) => new AsyncLoggerProxy(this, builder => builder.WithTags(tag0, tag1));

        /// <summary>
        /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified tags to every log entry.
        /// </summary>
        /// <param name="tag0">The first tag.</param>
        /// <param name="tag1">The second tag.</param>
        /// <param name="tag2">The third tag.</param>
        /// <returns>Extended <see cref="IAsyncLogger"/> instance</returns>
        [DebuggerStepThrough]
        public IAsyncLogger WithTags(string tag0, string tag1, string tag2) => new AsyncLoggerProxy(this, builder => builder.WithTags(tag0, tag1, tag2));

        /// <summary>
        /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified tags to every log entry.
        /// </summary>
        /// <param name="tags">The tags.</param>
        /// <returns>Extended <see cref="IAsyncLogger"/> instance</returns>
        [DebuggerStepThrough]
        public IAsyncLogger WithTags(params string[] tags) => new AsyncLoggerProxy(this, builder => builder.WithTags(tags));

        /// <summary>
        /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified property to every log entry.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <param name="value">The property value.</param>
        /// <returns>Extended <see cref="IAsyncLogger"/> instance</returns>
        [DebuggerStepThrough]
        public IAsyncLogger WithProperty(string name, string value) => new AsyncLoggerProxy(this, builder => builder.WithProperty(name, value));

        /// <summary>
        /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified property to every log entry.
        /// </summary>
        /// <param name="properties">The properties key-value collection.</param>
        /// <returns>Extended <see cref="IAsyncLogger"/> instance</returns>
        [DebuggerStepThrough]
        public IAsyncLogger WithProperties(IEnumerable<KeyValuePair<string, string>>? properties) => new AsyncLoggerProxy(this, builder => builder.WithProperties(properties));


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public Func<IAsyncLogger> AsFactory()
        {
            return () => this;
        }
    }
}
