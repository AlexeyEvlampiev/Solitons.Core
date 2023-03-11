using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Solitons.Diagnostics.Common;

/// <summary>
/// Represents a logger that writes log messages to the console asynchronously.
/// </summary>
public class ConsoleAsyncLogger : AsyncLogger
{
    private static readonly object SyncObject = new object();

    /// <summary>
    /// Creates an instance of the ConsoleAsyncLogger class.
    /// </summary>
    /// <returns>An instance of the ConsoleAsyncLogger class.</returns>
    public static IAsyncLogger Create() => new ConsoleAsyncLogger();

    /// <summary>
    /// Called before a log message is written.
    /// </summary>
    /// <param name="args">The log event arguments.</param>
    protected virtual void OnLogging(LogEventArgs args)
    {
    }

    /// <summary>
    /// Called after a log message is written.
    /// </summary>
    /// <param name="args">The log event arguments.</param>
    protected virtual void OnLogged(LogEventArgs args)
    {
    }

    /// <summary>
    /// Writes the log message to the console.
    /// </summary>
    /// <param name="args">The log event arguments.</param>
    /// <param name="writer">The text writer to write the log message to.</param>
    protected virtual void Log(LogEventArgs args, TextWriter writer)
    {
        writer.WriteLine(args.Content);
    }

    /// <summary>
    /// Gets the foreground color to use for a log message based on its log level.
    /// </summary>
    /// <param name="level">The log level.</param>
    /// <returns>The foreground color to use for the log message.</returns>
    protected virtual ConsoleColor ToForegroundColor(LogLevel level) => Console.ForegroundColor;

    /// <summary>
    /// Asynchronously writes the log message to the console.
    /// </summary>
    /// <param name="args">The log event arguments.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    [DebuggerNonUserCode]
    protected sealed override Task LogAsync(LogEventArgs args)
    {
        OnLogging(args);
        lock (SyncObject)
        {
            var foregroundColor = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = ToForegroundColor(args.Level);
                Log(args, args.Level == LogLevel.Error ? Console.Error : Console.Out);
            }
            finally
            {
                Console.ForegroundColor = foregroundColor;
            }
        }

        OnLogged(args);
        return Task.CompletedTask;
    }
}
