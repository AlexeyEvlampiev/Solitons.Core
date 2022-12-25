using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public static class ThrowIf
    {
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
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid NullOrEmpty(Guid? guid)
        {
            if (guid == null || guid == Guid.Empty)
            {
                throw new InvalidOperationException();
            }

            return guid.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid NullOrEmpty(Guid? guid, string message)
        {
            if (guid == null || guid == Guid.Empty)
            {
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
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid NullOrEmptyArgument(Guid? guid)
        {
            if (guid == null || guid == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException();
            }

            return guid.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid NullOrEmptyArgument(Guid? guid, string paramName)
        {
            if (guid == null || guid == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(paramName, $"{paramName} required.");
            }

            return guid.Value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="paramName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid NullOrEmptyArgument(Guid? guid, string paramName, string message)
        {
            if (guid == null || guid == Guid.Empty)
            {
                throw new ArgumentOutOfRangeException(message, paramName);
            }

            return guid.Value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrEmpty(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException();
            }

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrEmpty(string? value, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
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
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrEmptyArgument(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException();
            }

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrEmptyArgument(string? value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException(paramName);
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
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrEmptyArgument(string? value, string paramName, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException(message, paramName);
            }

            return value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrWhiteSpace(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException();
            }

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrWhiteSpace(string? value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
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
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrWhiteSpaceArgument(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException();
            }

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <returns><paramref name="value"/></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrWhiteSpaceArgument(string? value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException($"{paramName} is required.", paramName);
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
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrWhiteSpaceArgument(string? value, string paramName, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException(paramName, message);
            }

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NullReference<T>(T? value)
        {
            if (value is null)
                throw new NullReferenceException();
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NullReference<T>(T? value, string message)
        {
            if (value is null)
                throw new NullReferenceException(message);
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
        /// <param name="paramName"></param>
        /// <returns><paramref name="value"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NullArgument<T>(T? value, string paramName)
        {
            if (value is null)
                throw new ArgumentNullException(paramName);
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T NullArgument<T>(T? value, string paramName, string message)
        {
            if (value is null)
                throw new ArgumentNullException(paramName, message);
            return value;
        }
    }
}
