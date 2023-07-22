using System;

namespace Solitons;

/// <summary>
/// The exception that is thrown when the maximum allowed level of concurrency is exceeded.
/// </summary>
/// <remarks>
/// ConcurrencyLimitExceededException is thrown when an operation attempts to exceed the predefined limit for concurrent operations. The most common cause of this exception is a high level of concurrent requests exceeding the capacity of the system.
/// </remarks>
public class CapacityLimitExceededException : Exception
{
    /// <summary>
    /// Initializes a new instance of the ConcurrencyLimitExceededException class.
    /// </summary>
    public CapacityLimitExceededException()
        : base("The maximum allowed level of concurrency has been exceeded.")
    {
    }

    /// <summary>
    /// Initializes a new instance of the ConcurrencyLimitExceededException class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public CapacityLimitExceededException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the ConcurrencyLimitExceededException class with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</param>
    public CapacityLimitExceededException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
