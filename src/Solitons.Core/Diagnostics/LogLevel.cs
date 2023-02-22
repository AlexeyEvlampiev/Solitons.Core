namespace Solitons.Diagnostics;

/// <summary>
/// Specifies the severity level of a log entry.
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Indicates an informational log entry.
    /// </summary>
    Info = 0,

    /// <summary>
    /// Indicates a warning log entry.
    /// </summary>
    Warning,

    /// <summary>
    /// Indicates an error log entry.
    /// </summary>
    Error
}
