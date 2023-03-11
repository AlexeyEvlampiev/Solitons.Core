using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading;
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
    /// <param name="principal">The security principal associated with the log entry.</param>
    /// <param name="callerMemberName">The name of the calling member.</param>
    /// <param name="callerFilePath">The path to the source file of the calling member.</param>
    /// <param name="callerLineNumber">The line number in the source file of the calling member.</param>
    /// <param name="config">The log entry configuration callback.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous logging operation.</returns>
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
    /// Returns an <see cref="IObservable{T}"/> that represents an observer that provides notifications when a log event occurs.
    /// </summary>
    /// <returns>An observable sequence of <see cref="LogEventArgs"/>.</returns>
    IObservable<LogEventArgs> AsObservable();
}

public partial interface IAsyncLogger
{
    /// <summary>
    /// Creates a new instance of <see cref="IAsyncLogger"/> that automatically associates a principal object to every log entry.
    /// </summary>
    /// <param name="principal">The principal object.</param>
    /// <returns>A new instance of <see cref="IAsyncLogger"/> that is associated with the specified principal object.</returns>
    [DebuggerStepThrough]
    public sealed IAsyncLogger WithPrincipal(IPrincipal principal)
    {
        return new AsyncLoggerProxy(
            this,
            builder => { },
            principal);
    }

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
            Thread.CurrentPrincipal,
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
            Thread.CurrentPrincipal,
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
            Thread.CurrentPrincipal,
            callerMemberName,
            callerFilePath,
            callerLineNumber,
            config);
    }
}