using System.Diagnostics;

namespace Solitons;

/// <summary>
/// Represents the result of a boolean evaluation, including a reason phrase if the result is False.
/// Implicit cast operations allow conversion between <see cref="BooleanResult"/> and bool or string types.
/// </summary>
public abstract class BooleanResult
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly bool _result;

    /// <summary>
    /// Initializes a new instance of the <see cref="BooleanResult"/> class with the specified result.
    /// </summary>
    /// <param name="result">The boolean value representing the result of the evaluation.</param>
    protected BooleanResult(bool result)
    {
        _result = result;
    }

    /// <summary>
    /// Gets a <see cref="BooleanTrueResult"/> instance representing a true boolean result.
    /// </summary>
    public static BooleanResult True => BooleanTrueResult.Value;

    /// <summary>
    /// Creates a new instance of the <see cref="BooleanFalseResult"/> class with the specified reason for failure.
    /// </summary>
    /// <param name="reason">The reason for the failure.</param>
    /// <returns>A new instance of the <see cref="BooleanFalseResult"/> class with the specified reason for failure.</returns>
    public static BooleanResult Failure(string reason) => new BooleanFalseResult(reason);

    /// <summary>
    /// Returns a string that represents the current <see cref="BooleanResult"/> object.
    /// </summary>
    /// <remarks>
    /// If the object represents False, the returned string is the associated reason phrase.
    /// Otherwise, the <code>true.ToString()</code> is returned.
    /// </remarks>
    /// <returns>A string that represents the current BooleanResult object.</returns>
    public sealed override string ToString() => 
        this is BooleanFalseResult failure 
            ? failure.ReasonPhrase.DefaultIfNullOrEmpty(false.ToString())
            : true.ToString();

    /// <summary>
    /// Implicitly converts a <see cref="BooleanResult"/> object to a boolean value.
    /// </summary>
    /// <param name="result">The <see cref="BooleanResult"/> object to convert.</param>
    /// <returns>The boolean value represented by the <see cref="BooleanResult"/> object.</returns>
    public static implicit operator bool(BooleanResult result) => result._result;

    /// <summary>
    /// Implicitly converts a failure reason string to a <see cref="BooleanResult"/> object representing a failure.
    /// </summary>
    /// <param name="failureReason">The reason for the failure.</param>
    /// <returns>A new instance of the <see cref="BooleanFalseResult"/> class with the specified reason for failure.</returns>
    public static implicit operator BooleanResult(string failureReason) => new BooleanFalseResult(failureReason);

    /// <summary>
    /// Implicitly converts a <see cref="BooleanResult"/> object to a string representation.
    /// </summary>
    /// <remarks>
    /// If the object is a <see cref="BooleanFalseResult"/> object, the returned string is the value of <see cref="BooleanFalseResult.ReasonPhrase"/>.
    /// Otherwise, an empty string is returned.
    /// </remarks>
    /// <param name="result">The <see cref="BooleanResult"/> object to convert.</param>
    /// <returns>A string representation of the <see cref="BooleanResult"/> object.</returns>
    public static implicit operator string(BooleanResult result) => result.ToString();

}


sealed class BooleanTrueResult : BooleanResult
{
    public static readonly BooleanTrueResult Value = new BooleanTrueResult();

    private BooleanTrueResult() : base(true)
    {
    }

    public override bool Equals(object? obj)
    {
        return obj is BooleanTrueResult;
    }

    public override int GetHashCode() => 1;
}

sealed class BooleanFalseResult : BooleanResult
{
    internal BooleanFalseResult(string reasonPhrase) : base(false)
    {
        ReasonPhrase = reasonPhrase;
    }

    public string ReasonPhrase { get; }
}