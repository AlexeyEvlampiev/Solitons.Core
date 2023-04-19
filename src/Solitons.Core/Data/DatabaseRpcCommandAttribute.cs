

using System;
using System.Data;

namespace Solitons.Data;

/// <summary>
/// An attribute used to mark a class as a Database RPC command and to provide additional metadata for the command.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class DatabaseRpcCommandAttribute : Attribute
{
    /// <summary>
    /// The default content type for the request and response.
    /// </summary>
    public const string DefaultContentType = "application/json";

    /// <summary>
    /// The default operation timeout value in case one is not specified in the attribute.
    /// </summary>
    public const string DefaultOperationTimeout = "00:00:30";

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseRpcCommandAttribute"/> class with the specified procedure name.
    /// </summary>
    /// <param name="procedure">The name of the procedure that the command maps to.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="procedure"/> argument is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the <paramref name="procedure"/> argument is empty or whitespace.</exception>
    public DatabaseRpcCommandAttribute(string procedure)
    {
        Procedure = ThrowIf.ArgumentNullOrWhiteSpace(procedure, nameof(procedure));
        RequestContentType = DefaultContentType;
        ResponseContentType = DefaultContentType;
        IsolationLevel = IsolationLevel.ReadCommitted;
        OperationTimeoutTimeSpan = TimeSpan.Parse(DefaultOperationTimeout);
    }

    /// <summary>
    /// Gets the name of the procedure that the command maps to.
    /// </summary>
    public string Procedure { get; }

    /// <summary>
    /// Gets or sets the content type of the request payload.
    /// </summary>
    public string RequestContentType { get; init; }

    /// <summary>
    /// Gets or sets the content type of the response payload.
    /// </summary>
    public string ResponseContentType { get; init; }

    /// <summary>
    /// Gets or sets the isolation level used for the command.
    /// </summary>
    public IsolationLevel IsolationLevel { get; init; }

    /// <summary>
    /// Gets or sets the string representation of the operation timeout.
    /// </summary>
    /// <remarks>
    /// This value will be parsed into a <see cref="TimeSpan"/> and stored in the <see cref="OperationTimeoutTimeSpan"/> property.
    /// If this property is not set, the <see cref="DefaultOperationTimeout"/> value will be used.
    /// </remarks>
    public string OperationTimeout
    {
        get => OperationTimeoutTimeSpan.ToString();
        init => OperationTimeoutTimeSpan = TimeSpan.Parse(value ?? DefaultOperationTimeout);
    }

    /// <summary>
    /// Gets or sets the operation timeout as a <see cref="TimeSpan"/> value.
    /// </summary>
    /// <remarks>
    /// This property is used internally to store the parsed value of the <see cref="OperationTimeout"/> property.
    /// </remarks>
    internal TimeSpan OperationTimeoutTimeSpan { get; private set; }
}