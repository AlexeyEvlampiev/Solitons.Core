namespace Solitons.Diagnostics;

/// <summary>
/// Specifies the logging mode for the logger.
/// </summary>
public enum LogMode
{
    /// <summary>
    /// Indicates strict logging mode, where each log entry is immediately written to the log.
    /// </summary>
    Strict = 0,

    /// <summary>
    /// Indicates fire and forget logging mode, where log entries are queued and written to the log in the background.
    /// </summary>
    FireAndForget = 64
}
