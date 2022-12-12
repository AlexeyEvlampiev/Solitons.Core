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
        /// <exception cref="NullOrEmptyValueException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid NullOrEmpty(Guid? guid)
        {
            if (guid == null || guid == Guid.Empty)
            {
                throw new NullOrEmptyValueException();
            }

            return guid.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NullOrEmptyValueException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid NullOrEmpty(Guid? guid, string message)
        {
            if (guid == null || guid == Guid.Empty)
            {
                throw new NullOrEmptyValueException(message);
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
        /// <exception cref="NullOrEmptyArgumentException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid NullOrEmptyArgument(Guid? guid)
        {
            if (guid == null || guid == Guid.Empty)
            {
                throw new NullOrEmptyArgumentException();
            }

            return guid.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="paramName"></param>
        /// <returns></returns>
        /// <exception cref="NullOrEmptyArgumentException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid NullOrEmptyArgument(Guid? guid, string paramName)
        {
            if (guid == null || guid == Guid.Empty)
            {
                throw new NullOrEmptyArgumentException($"{paramName} required", paramName);
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
        /// <exception cref="NullOrEmptyArgumentException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid NullOrEmptyArgument(Guid? guid, string paramName, string message)
        {
            if (guid == null || guid == Guid.Empty)
            {
                throw new NullOrEmptyArgumentException(message, paramName);
            }

            return guid.Value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NullOrEmptyValueException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrEmpty(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new NullOrEmptyValueException();
            }

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NullOrEmptyValueException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrEmpty(string? value, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new NullOrEmptyValueException(message);
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
        /// <exception cref="NullOrEmptyArgumentException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrEmptyArgument(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new NullOrEmptyArgumentException();
            }

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramName</param>
        /// <returns></returns>
        /// <exception cref="NullOrEmptyArgumentException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrEmptyArgument(string? value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new NullOrEmptyArgumentException(paramName);
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
        /// <exception cref="NullOrEmptyArgumentException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrEmptyArgument(string? value, string paramName, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new NullOrEmptyArgumentException(message, paramName);
            }

            return value;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="NullOrWhiteSpaceStringException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrWhiteSpace(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new NullOrWhiteSpaceStringException();
            }

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NullOrWhiteSpaceStringException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrWhiteSpace(string? value, string message)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new NullOrWhiteSpaceStringException(message);
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
        /// <exception cref="NullOrEmptyArgumentException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrWhiteSpaceArgument(string? value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new NullOrWhiteSpaceStringArgumentException();
            }

            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="paramName"></param>
        /// <returns><paramref name="value"/></returns>
        /// <exception cref="NullOrWhiteSpaceStringException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrWhiteSpaceArgument(string? value, string paramName)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new NullOrWhiteSpaceStringArgumentException($"{paramName} is required.", paramName);
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
        /// <exception cref="NullOrWhiteSpaceStringArgumentException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string NullOrWhiteSpaceArgument(string? value, string paramName, string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new NullOrWhiteSpaceStringArgumentException(message, paramName);
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
