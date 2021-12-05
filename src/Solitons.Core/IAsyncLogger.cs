using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Solitons.Common;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAsyncLogger
    {
        /// <summary>
        /// Gets the <see cref="IAsyncLogger"/> null object.
        /// </summary>
        public static IAsyncLogger Null => AsyncLogger.Null;
        /// <summary>
        /// Logs the specified message asynchronously.
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The log message</param>
        /// <param name="config">The log entry configuration callback</param>
        /// <returns></returns>
        Task LogAsync(LogLevel level, string message, Action<ILogEntryBuilder> config = null);

        /// <summary>
        /// Logs the specified exception asynchronously.
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="ex">The exception</param>
        /// <param name="config">The log entry configuration callback</param>
        /// <returns></returns>
        Task LogAsync(LogLevel level, Exception ex, Action<ILogEntryBuilder> config = null);

        /// <summary>
        /// Ases the observable.
        /// </summary>
        /// <returns></returns>
        IObservable<ILogEntry> AsObservable();

        /// <summary>
        /// Logs the specified error message asynchronously.
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="config">The log entry configuration callback</param>
        [DebuggerStepThrough]
        public Task ErrorAsync(string message, Action<ILogEntryBuilder> config = null) => LogAsync(LogLevel.Error, message, config);

        /// <summary>
        /// Logs the specified warning message asynchronously.
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="config">The log entry configuration callback</param>
        [DebuggerStepThrough]
        public Task WarningAsync(string message, Action<ILogEntryBuilder> config = null) => LogAsync(LogLevel.Warning, message, config);


        /// <summary>
        /// Logs the specified information message asynchronously.
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="config">The log entry configuration callback</param>
        [DebuggerStepThrough]
        public Task InfoAsync(string message, Action<ILogEntryBuilder> config = null) => LogAsync(LogLevel.Info, message, config);

        /// <summary>
        /// Logs the specified error message asynchronously.
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="config">The log entry configuration callback</param>
        [DebuggerStepThrough]
        public Task ErrorAsync(StringBuilder message, Action<ILogEntryBuilder> config = null) => LogAsync(LogLevel.Error, message?.ToString(), config);

        /// <summary>
        /// Logs the specified warning message asynchronously.
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="config">The log entry configuration callback</param>
        [DebuggerStepThrough]
        public Task WarningAsync(StringBuilder message, Action<ILogEntryBuilder> config = null) => LogAsync(LogLevel.Warning, message?.ToString(), config);

        /// <summary>
        /// Logs the specified information message asynchronously.
        /// </summary>
        /// <param name="message">The message text</param>
        /// <param name="config">The log entry configuration callback</param>
        [DebuggerStepThrough]
        public Task InfoAsync(StringBuilder message, Action<ILogEntryBuilder> config = null) => LogAsync(LogLevel.Info, message?.ToString(), config);

        /// <summary>
        /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified tag to every log entry.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <returns>Extended <see cref="IAsyncLogger"/> instance</returns>
        [DebuggerStepThrough]
        public IAsyncLogger WithTag(string tag) => new AsyncLoggerProxy(this, builder => builder.WithTag(tag));

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
        public IAsyncLogger WithProperties(IEnumerable<KeyValuePair<string, string>> properties) => new AsyncLoggerProxy(this, builder => builder.WithProperties(properties));
    }
}
