using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public static class ThrowIf
    {
        private static bool IsVariableName(string input) => Regex.IsMatch(input, @"^@?[a-z\d_]+$");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        public static CancellationToken Cancelled(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            return token;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="OperationCanceledException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CancellationTokenSource Cancelled(CancellationTokenSource source)
        {
            source.Token.ThrowIfCancellationRequested();
            return source;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="message"></param>
        /// <param name="argExpression"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
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
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="exceptionFactory"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="message"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid NullOrEmptyArgument(
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
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <param name="argExpression"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
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
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="exceptionFactory"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrEmptyArgument(
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
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <param name="argExpression"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
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
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="exceptionFactory"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrWhiteSpaceArgument(string? value, string? message = null, [CallerArgumentExpression("value")] string paramName = "")
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <param name="argExpression"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NullReference<T>(T? value, string message, [CallerArgumentExpression("value")] string argExpression = "")
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
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="createException"></param>
        /// <returns></returns>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NullReference<T>(T? value, Func<Exception> createException)
        {
            if (value is null)
                throw createException.Invoke();
            return value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NullArgument<T>(T? value, string? message = null, [CallerArgumentExpression("value")]string paramName = "")
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
}
