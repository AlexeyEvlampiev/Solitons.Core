using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons
{
    public static partial class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode]
        public static string Append(this string self, Action<StringBuilder> config)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (config == null) throw new ArgumentNullException(nameof(config));
            var builder = new StringBuilder(self);
            config.Invoke(builder);
            return builder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToBase64(this Guid self)
        {
            var bytes = self.ToByteArray();
            return System.Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBetween(this HttpStatusCode self, int min, int max)
        {
            var code = (int)self;
            return code >= min && code <= max;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="createException"></param>
        /// <returns></returns>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfCanNotRead<T>(this T self, Func<Exception> createException) where T : Stream
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (false == self.CanRead)
            {
                throw createException?.Invoke() ?? new InvalidOperationException($"{nameof(self)}.{nameof(self.CanRead)} is false.");
            }
            return self;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="paramName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfCanNotReadArgument<T>(this T self, string paramName, string message = null) where T : Stream
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (false == self.CanRead)
            {
                paramName = paramName.DefaultIfNullOrWhiteSpace("?");
                message = message.DefaultIfNullOrWhiteSpace($"The given stream {nameof(Stream.CanRead)} is false.");
                throw new ArgumentException(message, paramName);
            }
            return self;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="createException"></param>
        /// <returns></returns>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfCanNotWrite<T>(this T self, Func<Exception> createException) where T : Stream
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (false == self.CanWrite)
            {
                throw createException?.Invoke() ?? new InvalidOperationException($"{nameof(self)}.{nameof(self.CanWrite)} is false.");
            }
            return self;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="paramName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfCanNotWriteArgument<T>(this T self, string paramName, string message = null) where T : Stream
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (false == self.CanWrite)
            {
                paramName = paramName.DefaultIfNullOrWhiteSpace("?");
                message = message.DefaultIfNullOrWhiteSpace($"The given stream {nameof(Stream.CanWrite)} is false.");
                throw new ArgumentException(message, paramName);
            }
            return self;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MemoryStream ToMemoryStream(this byte[] bytes)
        {
            if (bytes == null) throw new ArgumentNullException(nameof(bytes));
            return new MemoryStream(bytes);
        }
    

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="callback"></param>
        [DebuggerStepThrough]
        public static void AsForegroundColor(this ConsoleColor self, Action callback)
        {
            var foregroundColor = Console.ForegroundColor;

            try
            {
                Console.ForegroundColor = self;
                callback?.Invoke();
            }
            finally
            {
                Console.ForegroundColor = foregroundColor;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetOrCreate<T>(this WeakReference<T> self, Func<T> factory) where T : class
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (self.TryGetTarget(out var result)) return result;
            result = factory.Invoke().ThrowIfNull(()=>new ArgumentException($"Factory invocation returned null.", nameof(factory)));
            self.SetTarget(result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="expectedKind"></param>
        /// <param name="createException"></param>
        /// <returns></returns>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Uri ThrowIfNotUri(this Uri self, UriKind expectedKind, Func<Exception> createException)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (createException == null) throw new ArgumentNullException(nameof(createException));
            if (Uri.IsWellFormedUriString(self.ToString(), expectedKind)) return self;
            throw createException.Invoke() ?? throw new InvalidCastException($"'{self}' is not a well formed {expectedKind} uri.");
        }

        public static T ThrowIfNotOfType<T>(this T self, Type expectedType, Func<Type,Exception> exceptionFactory) where T : class
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (self.GetType() == expectedType) return self;
            throw exceptionFactory?.Invoke(expectedType) ?? new InvalidCastException();
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSuccessStatusCode(this HttpStatusCode statusCode)
        {
            var x = (int) statusCode / 100;
            return x == 2;
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsRedirectStatusCode(this HttpStatusCode statusCode)
        {
            var value = (int)statusCode;
            return value == 302 ||
                   value == 307 ||
                   value == 303 ||
                   value == 308;
        }


        /// <summary>
        /// Throws if the target instance is null.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="factory">The factory.</param>
        /// <returns>The <paramref name="self"/> parameter reference. </returns>
        /// <exception cref="System.NullReferenceException"></exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNull]
        public static T ThrowIfNull<T>(this T self, Func<Exception> factory) where T : class
        {
            if (self is null)
            {
                throw factory?.Invoke() ?? new NullReferenceException();
            }

            return self;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNull]
        public static T ThrowIfNull<T>(this T self, string message) where T : class
        {
            if (self is null)
            {
                throw new NullReferenceException(message.DefaultIfNullOrWhiteSpace($"{typeof(T)} null reference."));
            }

            return self;
        }


        /// <summary>
        /// Throws if null argument.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNull]
        public static T ThrowIfNullArgument<T>(this T self, string parameterName, string message = null) where T : class
        {
            if (self is null)
            {
                throw message.IsNullOrWhiteSpace()
                    ? new ArgumentNullException(parameterName.DefaultIfNullOrEmpty(nameof(self)))
                    : new ArgumentNullException(parameterName.DefaultIfNullOrEmpty(nameof(self)), message);
            }

            return self;
        }

        /// <summary>
        /// Throws if exceeds.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">The self.</param>
        /// <param name="maxCount">The maximum count.</param>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// self
        /// or
        /// factory
        /// </exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNull]
        public static IEnumerable<T> ThrowIfCountExceeds<T>(this IEnumerable<T> self, int maxCount, Func<Exception> factory)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            var maxIndex = maxCount - 1;
            return self.Select((item, index) =>
            {
                if (index > maxIndex)
                {
                    throw factory.Invoke() ?? new NullReferenceException();
                }
                return item;
            });
        }

        /// <summary>
        /// Throws if.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">The self.</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="factory">The factory.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// self
        /// or
        /// predicate
        /// or
        /// factory
        /// </exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNull]
        public static IEnumerable<T> ThrowIf<T>(this IEnumerable<T> self, Func<T, bool> predicate, Func<Exception> factory)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            return self.Select((item) =>
            {
                if (predicate.Invoke(item))
                {
                    throw factory.Invoke() ?? new NullReferenceException();
                }
                return item;
            });
        }

        /// <summary>
        /// Zips the captures.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="keyGroupName">Name of the key group.</param>
        /// <param name="valueGroupName">Name of the value group.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">self</exception>
        /// <exception cref="System.ArgumentException">self</exception>
        /// <exception cref="System.InvalidOperationException">Detected unbound captures.</exception>
        public static IEnumerable<KeyValuePair<Capture, Capture>> ZipCaptures(
            this Match self, 
            string keyGroupName,
            string valueGroupName)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (self.Success == false)
            {
                throw new ArgumentException($"{nameof(self)}.{nameof(self.Success)} is false", nameof(self));
            }

            keyGroupName.ThrowIfNullOrWhiteSpace(() =>
                new ArgumentException($"Key group name is required.", nameof(keyGroupName)));
            valueGroupName.ThrowIfNullOrWhiteSpace(() =>
                new ArgumentException($"Value group name is required.", nameof(keyGroupName)));
            var keys = self.Groups[keyGroupName].Captures.ToList();
            var values = self.Groups[valueGroupName].Captures.ToList();
            if (keys.Count != values.Count)
            {
                throw new InvalidOperationException($"Detected unbound captures.");
            }

            return keys
                .Zip(values, KeyValuePair.Create);
        }

        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNull]
        public static Guid ThrowIfEmpty(this Guid self, Func<Exception> createException)
        {
            if (createException == null) throw new ArgumentNullException(nameof(createException));
            if (self == Guid.Empty)
            {
                var error = createException.Invoke();
                throw error;
            }

            return self;
        }

        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNull]
        public static Guid ThrowIfEmptyArgument(this Guid self, string parameterName, string message = null)
        {
            if (self == Guid.Empty)
            {
                throw message.IsNullOrWhiteSpace()
                    ? new ArgumentException($"{nameof(parameterName)} is required.", nameof(parameterName))
                    : new ArgumentException(message, nameof(parameterName));
            }

            return self;
        }


        /// <summary>
        /// Converts this <see cref="TimeSpan"/> object to a Task that will complete after the corresponding time delay.
        /// </summary>
        /// <param name="self">The time span to be converted.</param>
        /// <param name="cancellation">The cancellation token.</param>
        /// <param name="throwOnCancellation">if set to <c>true</c> the <see cref="OperationCanceledException"/> errors occurred during the method execution are passed to the called.</param>
        public static async Task DelayAsync(this TimeSpan self, CancellationToken cancellation, bool throwOnCancellation = false)
        {
            try
            {
                await Task.Delay(self, cancellation);
            }
            catch (OperationCanceledException)
            {
                if (throwOnCancellation) throw;
            }
        }


        public static string ToBase64String(this byte[] self) =>
            System.Convert.ToBase64String(self ?? throw new NullReferenceException());

        public static string ToUtf8String(this byte[] self)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            return self.ToString(Encoding.UTF8);
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="self">The self.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        /// <exception cref="ArgumentNullException"> self  or encoding </exception>
        [DebuggerStepThrough, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString(this byte[] self, Encoding encoding)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (encoding == null) throw new ArgumentNullException(nameof(encoding));
            return encoding.GetString(self);
        }
    }
}
