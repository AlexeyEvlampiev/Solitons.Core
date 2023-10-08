namespace Solitons.Diagnostics;

/// <summary>
/// Represents the event arguments for a logging event.
/// </summary>
/// <param name="Level">The severity level of the log event.</param>
/// <param name="Message">The message of the log event.</param>
/// <param name="SourceInfo">Information about the source of the log event.</param>
/// <param name="Content">The content associated with the log event.</param>
public sealed record LogEventArgs(
    LogLevel Level,
    string Message,
    CallerInfo SourceInfo,
    string Content);
