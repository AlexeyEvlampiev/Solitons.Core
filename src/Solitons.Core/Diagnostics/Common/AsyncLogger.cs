using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Solitons.Diagnostics.Common;

/// <summary>
/// An abstract class that implements the <see cref="IAsyncLogger"/> interface, providing a way to log messages asynchronously.
/// </summary>
public abstract class AsyncLogger : IAsyncLogger
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Subject<LogEventArgs> _logs = new();

    /// <summary>
    /// When implemented in a derived class, asynchronously logs the specified message.
    /// </summary>
    /// <param name="args">A <see cref="LogEventArgs"/> object that contains information about the log event.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    protected abstract Task LogAsync(LogEventArgs args);

    /// <summary>
    /// A class that represents a JSON-based log message builder and implements the <see cref="ILogStringBuilder"/> interface.
    /// </summary>
    protected class LogJsonBuilder : Dictionary<string, object?>, ILogStringBuilder
    {
        private readonly HashSet<string> _tags = new(StringComparer.Ordinal);
        private readonly JsonSerializerOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogJsonBuilder"/> class with default options.
        /// </summary>
        public LogJsonBuilder()
        {
            _options = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogJsonBuilder"/> class with the specified options.
        /// </summary>
        /// <param name="options">A <see cref="JsonSerializerOptions"/> object that represents the options to use.</param>
        public LogJsonBuilder(JsonSerializerOptions options) : this()
        {
            _options = options;
        }

        /// <summary>
        /// Adds a named property to the JSON log message.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value of the property.</param>
        /// <returns>The <see cref="ILogStringBuilder"/> instance.</returns>
        public ILogStringBuilder WithProperty(string name, object value)
        {
            base[name] = value;
            return this;
        }

        /// <summary>
        /// Adds a tag to the JSON log message.
        /// </summary>
        /// <param name="tag">The tag to add.</param>
        /// <returns>The <see cref="ILogStringBuilder"/> instance.</returns>
        public ILogStringBuilder WithTags(string tag)
        {
            _tags.Add(tag);
            return this;
        }

        /// <summary>
        /// Returns the JSON representation of the log message.
        /// </summary>
        /// <returns>A string that represents the JSON log message.</returns>
        public override string ToString()
        {
            this["tags"] = _tags;
            return JsonSerializer.Serialize(this, _options);
        }
    }

    /// <summary>
    /// Formats the specified file path for display in log messages.
    /// </summary>
    /// <param name="filePath">The file path to format.</param>
    /// <returns>A string that represents the formatted file path.</returns>
    [DebuggerNonUserCode]
    protected virtual string FormatSourceFilePath(string filePath) => Path.GetFileName(filePath);


    /// <summary>
    /// Creates an instance of the <see cref="ILogStringBuilder"/> interface to build a log message.
    /// </summary>
    /// <param name="level">The log level.</param>
    /// <param name="message">The log message.</param>
    /// <param name="sourceInfo">The <see cref="CallerInfo"/> object associated with the log event.</param>
    /// <returns>An instance of the <see cref="ILogStringBuilder"/> interface.</returns>
    [DebuggerNonUserCode]
    protected virtual ILogStringBuilder CreateLogStringBuilder(
        LogLevel level,
        string message,
        CallerInfo sourceInfo)
    {
        ILogStringBuilder builder = new LogJsonBuilder()
            .WithProperty("level", level.ToString().ToLower())
            .WithProperty("message", message)
            .WithProperty("createdUtc", DateTime.UtcNow.ToString("O"));

        builder
            .WithProperty("source", sourceInfo);
        return builder;
    }


    /// <inheritdoc cref="IAsyncLogger.LogAsync"/>
    async Task IAsyncLogger.LogAsync(
        LogLevel level,
        string message,
        LogMode mode,
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
        var builder = CreateLogStringBuilder(level, message, sourceInfo);
            
        config?.Invoke(builder);


        var content = builder.ToString();
        var args = new LogEventArgs(
            level,
            message,
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
    /// Returns an observable sequence of log messages.
    /// </summary>
    /// <returns>An <see cref="IObservable{T}"/> object that represents the observable sequence of log messages.</returns>
    public IObservable<LogEventArgs> AsObservable() => _logs.AsObservable();

    /// <summary>
    /// Returns this instance of the <see cref="AsyncLogger"/> class as an <see cref="IAsyncLogger"/> object.
    /// </summary>
    /// <returns>An <see cref="IAsyncLogger"/> object that represents this instance of the <see cref="AsyncLogger"/> class.</returns>
    public IAsyncLogger AsAsyncLogger() => this;
}