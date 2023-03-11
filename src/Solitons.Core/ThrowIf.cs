using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace Solitons;

/// <summary>
/// Provides methods for validating input and throwing exceptions if necessary.
/// </summary>
public static class ThrowIf
{
    private static bool IsVariableName(string input) => Regex.IsMatch(input, @"^@?[a-z\d_]+$");

    /// <summary>
    /// Throws an <see cref="OperationCanceledException"/> if the provided <see cref="CancellationToken"/> has had cancellation requested.
    /// </summary>
    /// <param name="token">The <see cref="CancellationToken"/> to check for cancellation.</param>
    /// <returns>The provided <see cref="CancellationToken"/> if cancellation has not been requested.</returns>
    /// <exception cref="OperationCanceledException">Thrown if cancellation has been requested for the provided <see cref="CancellationToken"/>.</exception>
    /// <exception cref="ObjectDisposedException">Thrown if the provided <see cref="CancellationToken"/> has already been disposed.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CancellationToken Cancelled(CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        return token;
    }

    /// <summary>
    /// Throws an <see cref="OperationCanceledException"/> if the provided <see cref="CancellationTokenSource"/> has had cancellation requested.
    /// </summary>
    /// <param name="source">The <see cref="CancellationTokenSource"/> to check for cancellation.</param>
    /// <returns>The provided <see cref="CancellationTokenSource"/> if cancellation has not been requested.</returns>
    /// <exception cref="OperationCanceledException">Thrown if cancellation has been requested for the <see cref="CancellationToken"/> associated with the provided <see cref="CancellationTokenSource"/>.</exception>
    /// <exception cref="ObjectDisposedException">Thrown if the provided <see cref="CancellationTokenSource"/> has already been disposed.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CancellationTokenSource Cancelled(CancellationTokenSource source)
    {
        source.Token.ThrowIfCancellationRequested();
        return source;
    }


    /// <summary>
    /// Throws an <see cref="InvalidOperationException"/> if the provided <see cref="Guid"/> is <c>null</c> or <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/> to check for <c>null</c> or <see cref="Guid.Empty"/>.</param>
    /// <param name="message">The exception message to use if an exception is thrown.</param>
    /// <param name="argExpression">The argument expression to use in the exception message if <paramref name="message"/> is <c>null</c>.</param>
    /// <returns>The provided <see cref="Guid"/> if it is not <c>null</c> or <see cref="Guid.Empty"/>.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the provided <see cref="Guid"/> is <c>null</c> or <see cref="Guid.Empty"/>.</exception>
    [DebuggerNonUserCode]
    public static Guid NullOrEmpty(Guid? guid, string? message = null, [CallerArgumentExpression("guid")]string argExpression = "")
    {
        if (guid == null)
        {
            message ??= IsVariableName(argExpression)
                ? $"{argExpression} is null."
                : $"{argExpression} returned null.";
            throw new InvalidOperationException(message);
        }


        if (guid == Guid.Empty)
        {
            message ??= IsVariableName(argExpression)
                ? $"{argExpression} is an empty {typeof(Guid)} value."
                : $"{argExpression} returned an empty {typeof(Guid)} value.";
            throw new InvalidOperationException(message);
        }

        return guid.Value;
    }

    /// <summary>
    /// Throws an exception returned by <paramref name="exceptionFactory"/> if the provided <see cref="Guid"/> is <c>null</c> or <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/> to check for <c>null</c> or <see cref="Guid.Empty"/>.</param>
    /// <param name="exceptionFactory">A factory method that returns the exception to be thrown if the <see cref="Guid"/> is <c>null</c> or <see cref="Guid.Empty"/>.</param>
    /// <returns>The provided <see cref="Guid"/> if it is not <c>null</c> or <see cref="Guid.Empty"/>.</returns>
    /// <exception cref="Exception">The exception returned by <paramref name="exceptionFactory"/> if the provided <see cref="Guid"/> is <c>null</c> or <see cref="Guid.Empty"/>.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid NullOrEmpty(Guid? guid, Func<Exception> exceptionFactory)
    {
        if (guid == null || guid == Guid.Empty)
        {
            throw exceptionFactory();
        }

        return guid.Value;
    }


    /// <summary>
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if the provided <see cref="Guid"/> is <c>null</c> or <see cref="Guid.Empty"/>.
    /// </summary>
    /// <param name="guid">The <see cref="Guid"/> to check for <c>null</c> or <see cref="Guid.Empty"/>.</param>
    /// <param name="message">The exception message to use if an exception is thrown.</param>
    /// <param name="paramName">The name of the parameter being checked.</param>
    /// <returns>The provided <see cref="Guid"/> if it is not <c>null</c> or <see cref="Guid.Empty"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided <see cref="Guid"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the provided <see cref="Guid"/> is <see cref="Guid.Empty"/>.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Guid ArgumentNullOrEmpty(
        Guid? guid, 
        string? message = null, 
        [CallerArgumentExpression("guid")]string paramName = "")
    {
        if (guid == null)
        {
            message ??= IsVariableName(paramName)
                ? $"{paramName} is null."
                : $"{paramName} returned null.";
            throw new ArgumentNullException(paramName, message);
        }

        if (guid == Guid.Empty)
        {
            message ??= IsVariableName(paramName)
                ? $"{paramName} is an empty {typeof(Guid)} value."
                : $"{paramName} returned an empty {typeof(Guid)} value.";
            throw new ArgumentOutOfRangeException(paramName, message);
        }

        return guid.Value;
    }


    /// <summary>
    /// Throws a <see cref="NullReferenceException"/> if the provided string is <c>null</c>,
    /// and an <see cref="InvalidOperationException"/> if the provided string is an empty string.
    /// </summary>
    /// <param name="value">The string to check for <c>null</c> or an empty string.</param>
    /// <param name="message">The exception message to use if an exception is thrown.</param>
    /// <param name="argExpression">The argument expression to use in the exception message if <paramref name="message"/> is <c>null</c>.</param>
    /// <returns>The provided string if it is not <c>null</c> or an empty string.</returns>
    /// <exception cref="NullReferenceException">Thrown if the provided string is <c>null</c>.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the provided string is an empty string.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string NullOrEmpty(
        string? value, 
        string? message = null, 
        [CallerArgumentExpression("value")]string argExpression = "")
    {
        if (value == null)
        {
            message ??= IsVariableName(argExpression)
                ? $"{argExpression} is null."
                : $"{argExpression} returned null.";
            throw new NullReferenceException(message);
        }

        if (string.IsNullOrEmpty(value))
        {
            message ??= IsVariableName(argExpression)
                ? $"{argExpression} is an empty {typeof(string)} value."
                : $"{argExpression} returned an empty {typeof(string)} value.";
            throw new InvalidOperationException(message);
        }

        return value;
    }

    /// <summary>
    /// Throws an exception created by a factory method if the provided string is <c>null</c> or an empty string.
    /// </summary>
    /// <param name="value">The string to check for <c>null</c> or an empty string.</param>
    /// <param name="exceptionFactory">A factory method that returns the exception to be thrown if the provided string is <c>null</c> or an empty string.</param>
    /// <returns>The provided string if it is not <c>null</c> or an empty string.</returns>
    /// <exception cref="Exception">The exception returned by <paramref name="exceptionFactory"/> if the provided string is <c>null</c> or an empty string.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string NullOrEmpty(string? value, Func<Exception> exceptionFactory)
    {
        if (string.IsNullOrEmpty(value))
        {
            throw exceptionFactory();
        }

        return value;
    }


    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the provided string is <c>null</c>, and an <see cref="ArgumentOutOfRangeException"/> if the provided string is an empty string.
    /// </summary>
    /// <param name="value">The string to check for <c>null</c> or an empty string.</param>
    /// <param name="message">The exception message to use if an exception is thrown.</param>
    /// <param name="paramName">The name of the parameter being checked.</param>
    /// <returns>The provided string if it is not <c>null</c> or an empty string.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the provided string is <c>null</c>.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the provided string is an empty string.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ArgumentNullOrEmpty(
        string? value, 
        string? message = null, 
        [CallerArgumentExpression("value")]string paramName = "")
    {
        if (value == null)
        {
            message ??= IsVariableName(paramName)
                ? $"{paramName} is null."
                : $"{paramName} returned null.";
            throw new ArgumentNullException(paramName, message);
        }

        if (string.IsNullOrEmpty(value))
        {
            message ??= IsVariableName(paramName)
                ? $"{paramName} is an empty {typeof(string)} value."
                : $"{paramName} returned an empty {typeof(string)} value.";
            throw new ArgumentOutOfRangeException(paramName, message);
        }

        return value;
    }


    /// <summary>
    /// Throws a <see cref="NullReferenceException"/> if the given string is null or empty.
    /// Throws an <see cref="InvalidOperationException"/> if the given string is whitespace.
    /// </summary>
    /// <param name="value">The string to check for null or empty.</param>
    /// <param name="message">The exception message to use if an exception is thrown.</param>
    /// <param name="argExpression">The name of the argument being checked.</param>
    /// <returns>The original string if it is not null or empty.</returns>
    /// <exception cref="NullReferenceException">Thrown if the given string is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the given string is empty or whitespace.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string NullOrWhiteSpace(
        string? value, 
        string? message = null, 
        [CallerArgumentExpression("value")] string argExpression = "")
    {
        if (value == null)
        {
            message ??= IsVariableName(argExpression)
                ? $"{argExpression} is null."
                : $"{argExpression} returned null.";
            throw new NullReferenceException(message);
        }

        if (string.IsNullOrWhiteSpace(value))
        {
            message ??= IsVariableName(argExpression)
                ? $"{argExpression} is an empty or whitespace {typeof(string)} value."
                : $"{argExpression} returned an empty or whitespace {typeof(string)} value.";
            throw new InvalidOperationException(message);
        }

        return value;
    }


    /// <summary>
    /// Throws an exception created by the given factory if the given string is null or empty.
    /// </summary>
    /// <param name="value">The string to check for null or empty.</param>
    /// <param name="exceptionFactory">A function that creates an exception to be thrown if the given string is null or empty.</param>
    /// <returns>The original string if it is not null or empty.</returns>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string NullOrWhiteSpace(string? value, Func<Exception> exceptionFactory)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw exceptionFactory();
        }

        return value;
    }





    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the given string is null or empty.
    /// Throws an <see cref="ArgumentOutOfRangeException"/> if the given string is empty.
    /// </summary>
    /// <param name="value">The string to check for null or empty.</param>
    /// <param name="message">The exception message to use if an exception is thrown.</param>
    /// <param name="paramName">The name of the parameter being checked.</param>
    /// <returns>The original string if it is not null or empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the given string is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the given string is empty.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string ArgumentNullOrWhiteSpace(string? value, string? message = null, [CallerArgumentExpression("value")] string paramName = "")
    {
        if (value == null)
        {
            message ??= IsVariableName(paramName)
                ? $"{paramName} is null."
                : $"{paramName} returned null.";
            throw new ArgumentNullException(paramName, message);
        }

        if(string.IsNullOrEmpty(value))
        {
            message ??= IsVariableName(paramName)
                ? $"{paramName} is an empty or a whitespace {typeof(string)} value."
                : $"{paramName} returned an empty value or a whitespace. {typeof(string)} value.";
            throw new ArgumentOutOfRangeException(paramName, message);
        }

        return value;
    }


    /// <summary>
    /// Throws a <see cref="NullReferenceException"/> if the given value is null.
    /// </summary>
    /// <typeparam name="T">The type of the value to check for null.</typeparam>
    /// <param name="value">The value to check for null.</param>
    /// <param name="message">The exception message to use if an exception is thrown.</param>
    /// <param name="argExpression">The name of the argument being checked.</param>
    /// <returns>The original value if it is not null.</returns>
    /// <exception cref="NullReferenceException">Thrown if the given value is null.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T NullReference<T>(T? value, string? message = null, [CallerArgumentExpression("value")] string argExpression = "")
    {
        if (value is null)
        {
            message ??= IsVariableName(argExpression)
                ? $"{argExpression} is null."
                : $"{argExpression} returned null.";
            throw new NullReferenceException(message);
        }

        return value;
    }

    /// <summary>
    /// Throws an exception created by the given factory if the given value is null.
    /// </summary>
    /// <typeparam name="T">The type of the value to check for null.</typeparam>
    /// <param name="value">The value to check for null.</param>
    /// <param name="createException">A function that creates an exception to be thrown if the given value is null.</param>
    /// <returns>The original value if it is not null.</returns>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T NullReference<T>(T? value, Func<Exception> createException)
    {
        if (value is null)
            throw createException.Invoke();
        return value;
    }


    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if the given value is null.
    /// </summary>
    /// <typeparam name="T">The type of the value to check for null.</typeparam>
    /// <param name="value">The value to check for null.</param>
    /// <param name="message">The exception message to use if an exception is thrown.</param>
    /// <param name="paramName">The name of the parameter being checked.</param>
    /// <returns>The original value if it is not null.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the given value is null.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T ArgumentNull<T>(T? value, string? message = null, [CallerArgumentExpression("value")]string paramName = "")
    {
        if (value is null)
        {
            message ??= IsVariableName(paramName)
                ? $"{paramName} is null."
                : $"{paramName} returned null.";
            throw new ArgumentNullException(paramName, message);
        }
                
        return value;
    }
}