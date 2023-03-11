using System.Security.Principal;

namespace Solitons.Diagnostics;

/// <summary>
/// 
/// </summary>
/// <param name="Level">Gets the severity level of the log event.</param>
/// <param name="Message">Gets the message of the log event.</param>
/// <param name="Principal">Gets the principal associated with the log event, if any.</param>
/// <param name="SourceInfo">Gets information about the source of the log event.</param>
/// <param name="Content">Gets the content associated with the log event.</param>
public sealed record LogEventArgs(
    LogLevel Level,
    string Message,
    IPrincipal? Principal,
    CallerInfo SourceInfo,
    string Content);
