using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace Solitons.Reactive;

/// <summary>
/// Arguments supplied to the Retry Policy Handler.
/// </summary>
public sealed class RetryPolicyArgs 
{
    /// <summary>
    /// Initializes a new instance of the RetryPolicyArgs class.
    /// </summary>
    /// <param name="exception">The exception that caused the retry.</param>
    /// <param name="attemptNumber">The number of the current attempt.</param>
    /// <param name="firstAttemptTime">The time of the first attempt.</param>
    internal RetryPolicyArgs(Exception exception, int attemptNumber, DateTimeOffset firstAttemptTime)
    {
        Exception = exception;
        AttemptNumber = attemptNumber;
        FirstAttemptTime = firstAttemptTime;
        ElapsedTimeSinceFirstException = (DateTimeOffset.UtcNow - firstAttemptTime);
    }

    /// <summary>
    /// Gets the exception that caused the retry.
    /// </summary>
    public Exception Exception { get; }

    /// <summary>
    /// Gets the number of the current attempt.
    /// </summary>
    public int AttemptNumber { get; }

    /// <summary>
    /// Gets the time of the first attempt.
    /// </summary>
    public DateTimeOffset FirstAttemptTime { get; }

    /// <summary>
    /// Gets the elapsed time since the first exception.
    /// </summary>
    public TimeSpan ElapsedTimeSinceFirstException { get; }

    /// <summary>
    /// Determines whether to signal the next retry attempt based on the provided condition.
    /// </summary>
    /// <param name="shouldSignal">
    /// A boolean value indicating whether the next retry attempt should be signaled. 
    /// If true, a new RetryPolicyArgs instance is returned, signifying a new retry attempt.
    /// If false, an empty observable sequence is returned, indicating no further retry attempts.
    /// </param>
    /// <returns>
    /// An IObservable of RetryPolicyArgs. If 'shouldSignal' is true, returns an observable sequence 
    /// containing the current RetryPolicyArgs instance. If 'shouldSignal' is false, returns an empty 
    /// observable sequence.
    /// </returns>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IObservable<RetryPolicyArgs> SignalNextAttempt(bool shouldSignal) =>
        shouldSignal
            ? Observable.Return(this)
            : Observable.Empty<RetryPolicyArgs>();
}
