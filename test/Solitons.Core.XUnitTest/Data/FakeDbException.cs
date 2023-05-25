using System;
using System.Data.Common;
using Moq;

namespace Solitons.Data;

/// <summary>
/// Represents a mock database exception for testing purposes.
/// </summary>
public sealed class FakeDbException : DbException
{
    /// <summary>
    /// Initializes a new instance of the FakeDbException class, optionally specifying whether the exception is transient.
    /// </summary>
    /// <param name="isTransient">True if the exception is transient; false otherwise.</param>
    public FakeDbException(bool isTransient = false)
    {
        Mock.SetupGet(_ => _.IsTransient).Returns(isTransient);
    }

    /// <summary>
    /// Initializes a new instance of the FakeDbException class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public FakeDbException(string message, Exception innerException) 
        : base(message, innerException)
    {
        Mock.SetupGet(_ => _.IsTransient).Returns(false);
    }

    /// <summary>
    /// Gets the mocked <see cref="Callback"/> object.
    /// </summary>
    public Callback Mock { get; } = new Callback();

    /// <inheritdoc />
    public override bool IsTransient => Mock.Object.IsTransient;

    /// <inheritdoc />
    public override int ErrorCode => Mock.Object.ErrorCode;

    /// <inheritdoc />
    public override string SqlState => Mock.Object.SqlState;

    /// <inheritdoc />
    protected override DbBatchCommand DbBatchCommand => Mock.Object.DbBatchCommand;

    /// <summary>
    /// Interface defining the behaviour of a database exception.
    /// </summary>
    public interface ICallback
    {
        bool IsTransient { get; }
        int ErrorCode { get; }
        string SqlState { get; }
        DbBatchCommand DbBatchCommand { get; }
    }

    /// <summary>
    /// Mock class for ICallback interface.
    /// </summary>
    public sealed class Callback : Mock<ICallback>
    {
        
    }
}