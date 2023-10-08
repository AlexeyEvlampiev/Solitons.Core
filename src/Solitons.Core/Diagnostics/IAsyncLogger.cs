using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Solitons.Diagnostics;

/// <summary>
/// Represents an asynchronous logger interface.
/// </summary>
public partial interface IAsyncLogger
{
    /// <summary>
    /// Appends the specified exception to the log entry.
    /// </summary>
    /// <param name="builder">The log string builder.</param>
    /// <param name="exception">The exception to be appended.</param>
    protected virtual void AppendException(ILogStringBuilder builder, Exception exception)
    {
        builder.WithProperty("exception", exception.ToString());
    }

    /// <summary>
    /// Logs the specified message asynchronously.
    /// </summary>
    /// <param name="level">The log level.</param>
    /// <param name="message">The message to be logged.</param>
    /// <param name="mode">The logging mode.</param>
    /// <param name="callerMemberName">The name of the calling member.</param>
    /// <param name="callerFilePath">The path to the source file of the calling member.</param>
    /// <param name="callerLineNumber">The line number in the source file of the calling member.</param>
    /// <param name="config">The log entry configuration callback.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous logging operation.</returns>
    protected internal abstract Task LogAsync(
        LogLevel level,
        string message,
        LogMode mode,
        string callerMemberName,
        string callerFilePath,
        int callerLineNumber,
        Action<ILogStringBuilder>? config);

    /// <summary>
    /// Returns an <see cref="IObservable{T}"/> that represents an observer that provides notifications when a log event occurs.
    /// </summary>
    /// <returns>An observable sequence of <see cref="LogEventArgs"/>.</returns>
    IObservable<LogEventArgs> AsObservable();
}

public partial interface IAsyncLogger
{
    private const string AsyncLoggerHttpRequestOptionsKey = "2c2cc6ed61f84192a2f73afe1320d04f";

    /// <summary>
    /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified tag to every log entry.
    /// </summary>
    /// <param name="tag">The tag to be added.</param>
    /// <returns>A new instance of <see cref="IAsyncLogger"/> that is extended with the specified tag.</returns>
    [DebuggerStepThrough]
    public sealed IAsyncLogger WithTags(string tag) => new AsyncLoggerProxy(this, builder => builder.WithTags(tag));

    /// <summary>
    /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified tags to every log entry.
    /// </summary>
    /// <param name="tag0">The first tag to be added.</param>
    /// <param name="tag1">The second tag to be added.</param>
    /// <returns>A new instance of <see cref="IAsyncLogger"/> that is extended with the specified tags.</returns>
    [DebuggerStepThrough]
    public sealed IAsyncLogger WithTags(string tag0, string tag1) => new AsyncLoggerProxy(this, builder => builder.WithTags(tag0, tag1));

    /// <summary>
    /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified tags to every log entry.
    /// </summary>
    /// <param name="tag0">The first tag to be added.</param>
    /// <param name="tag1">The second tag to be added.</param>
    /// <param name="tag2">The third tag to be added.</param>
    /// <returns>A new instance of <see cref="IAsyncLogger"/> that is extended with the specified tags.</returns>
    [DebuggerStepThrough]
    public sealed IAsyncLogger WithTags(string tag0, string tag1, string tag2) => new AsyncLoggerProxy(this, builder => builder.WithTags(tag0, tag1, tag2));

    /// <summary>
    /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified tags to every log entry.
    /// </summary>
    /// <param name="tags">The tags to be added.</param>
    /// <returns>A new instance of <see cref="IAsyncLogger"/> that is extended with the specified tags.</returns>
    [DebuggerStepThrough]
    public sealed IAsyncLogger WithTags(params string[] tags) => new AsyncLoggerProxy(this, builder => builder.WithTags(tags));

    /// <summary>
    /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified property to every log entry.
    /// </summary>
    /// <param name="name">The name of the property to be added.</param>
    /// <param name="value">The value of the property to be added.</param>
    /// <returns>A new instance of <see cref="IAsyncLogger"/> that is extended with the specified property.</returns>
    [DebuggerStepThrough]
    public sealed IAsyncLogger WithProperty(string name, dynamic value) => new AsyncLoggerProxy(this, builder => builder.WithProperty(name, value));

    /// <summary>
    /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically adds the specified properties to every log entry.
    /// </summary>
    /// <param name="properties">The key-value collection of properties to be added.</param>
    /// <returns>A new instance of <see cref="IAsyncLogger"/> that is extended with the specified properties.</returns>
    [DebuggerStepThrough]
    public sealed IAsyncLogger WithProperties(IEnumerable<KeyValuePair<string, object>> properties) => new AsyncLoggerProxy(this, builder => builder.WithProperties(properties));

}

public partial interface IAsyncLogger
{
    /// <summary>
    /// Gets a null object implementation of <see cref="IAsyncLogger"/>.
    /// </summary>
    public static IAsyncLogger Null => AsyncNullObjectLogger.Instance;

    /// <summary>
    /// Returns a factory delegate that creates an instance of <see cref="IAsyncLogger"/>.
    /// </summary>
    /// <returns>A factory delegate that creates an instance of <see cref="IAsyncLogger"/>.</returns>
    [DebuggerNonUserCode]
    public Func<IAsyncLogger> AsFactory() => () => this;

    /// <summary>
    /// Logs the specified error message asynchronously.
    /// </summary>
    /// <param name="message">The error message text.</param>
    /// <param name="config">The log entry configuration callback.</param>
    /// <param name="mode">The logging mode.</param>
    /// <param name="callerMemberName">The caller member name.</param>
    /// <param name="callerFilePath">The caller file path.</param>
    /// <param name="callerLineNumber">The caller line number.</param>
    /// <returns>A task that represents the asynchronous operation of logging the error message.</returns>
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
            callerMemberName,
            callerFilePath,
            callerLineNumber,
            config);
    }

    /// <summary>
    /// Logs the specified error message asynchronously, including the exception information.
    /// </summary>
    /// <param name="ex">The exception object.</param>
    /// <param name="config">The log entry configuration callback.</param>
    /// <param name="mode">The logging mode.</param>
    /// <param name="callerMemberName">The caller member name.</param>
    /// <param name="callerFilePath">The caller file path.</param>
    /// <param name="callerLineNumber">The caller line number.</param>
    /// <returns>A task that represents the asynchronous operation of logging the error message and exception information.</returns>
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
            callerMemberName,
            callerFilePath,
            callerLineNumber,
            config);

        void ConfigExtension(ILogStringBuilder builder) => AppendException(builder, ex);
    }

    /// <summary>
    /// Logs the specified warning message asynchronously.
    /// </summary>
    /// <param name="message">The warning message text.</param>
    /// <param name="config">The log entry configuration callback.</param>
    /// <param name="mode">The logging mode.</param>
    /// <param name="callerMemberName">The caller member name.</param>
    /// <param name="callerFilePath">The caller file path.</param>
    /// <param name="callerLineNumber">The caller line number.</param>
    /// <returns>A task that represents the asynchronous operation of logging the warning message.</returns>
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
            callerMemberName,
            callerFilePath,
            callerLineNumber,
            config);
    }

    /// <summary>
    /// Logs the specified information message asynchronously.
    /// </summary>
    /// <param name="message">The information message text.</param>
    /// <param name="config">The log entry configuration callback.</param>
    /// <param name="mode">The logging mode.</param>
    /// <param name="callerMemberName">The caller member name.</param>
    /// <param name="callerFilePath">The caller file path.</param>
    /// <param name="callerLineNumber">The caller line number.</param>
    /// <returns>A task that represents the asynchronous operation of logging the information message.</returns>
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
            callerMemberName,
            callerFilePath,
            callerLineNumber,
            config);
    }


    /// <summary>
    /// Sets an <see cref="IAsyncLogger"/> instance in the <see cref="HttpRequestOptions"/>.
    /// </summary>
    /// <param name="options">The HttpRequestOptions instance to which the logger is to be added.</param>
    /// <param name="logger">The IAsyncLogger instance to be added.</param>
    /// <returns>The HttpRequestOptions instance with the added logger.</returns>
    [DebuggerNonUserCode]
    public static void Set(HttpRequestOptions options, IAsyncLogger logger)
    {
        var key = new HttpRequestOptionsKey<IAsyncLogger>(AsyncLoggerHttpRequestOptionsKey);
        options.Set(key, logger);
    }

    /// <summary>
    /// Retrieves the <see cref="IAsyncLogger"/> instance from the <see cref="HttpRequestOptions"/>.
    /// </summary>
    /// <param name="options">The HttpRequestOptions instance from which the logger is to be retrieved.</param>
    /// <returns>The IAsyncLogger instance if found; otherwise, IAsyncLogger.Null.</returns>
    /// <exception cref="ArgumentNullException">Thrown when options is null.</exception>
    [DebuggerNonUserCode]
    public static IAsyncLogger Get(HttpRequestOptions options)
    {
        var key = new HttpRequestOptionsKey<IAsyncLogger>(AsyncLoggerHttpRequestOptionsKey);
        options.TryGetValue(key, out var logger);
        return logger ?? IAsyncLogger.Null;
    }
}