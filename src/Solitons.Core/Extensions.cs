﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Data;

namespace Solitons
{
    public static partial class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="predicate"></param>
        /// <param name="errorFactory"></param>
        /// <returns></returns>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIf<T>(
            this T self, 
            Func<bool> predicate, 
            Func<Exception> errorFactory)
        {
            if (predicate.Invoke())
            {
                throw errorFactory.Invoke();
            }

            return self;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static byte[] ReadAllBytes(this BinaryReader reader)
        {
            const int bufferSize = 4096;
            using var ms = new MemoryStream();
            byte[] buffer = new byte[bufferSize];
            int count;
            while ((count = reader.Read(buffer, 0, buffer.Length)) != 0)
                ms.Write(buffer, 0, count);
            return ms.ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="callback"></param>
        public static void Do(this IDbConnection self, Action<IDbCommand> callback)
        {
            using var command = self.CreateCommand();
            callback.Invoke(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static async Task DoAsync(this DbConnection self, Func<DbCommand, Task> callback)
        {
            await using var command = self.CreateCommand();
            await callback.Invoke(command);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static T Do<T>(this IDbConnection self, Func<IDbCommand, T> callback)
        {
            using var command = self.CreateCommand();
            return callback.Invoke(command);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static async Task<T> DoAsync<T>(this DbConnection self, Func<DbCommand, Task<T>> callback)
        {
            await using var command = self.CreateCommand();
            return await callback.Invoke(command);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="commandText"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static T Do<T>(this IDbConnection self, string commandText, Func<IDbCommand, T> callback)
        {
            using var command = self.CreateCommand();
            command.CommandText = commandText;
            return callback.Invoke(command);
        }


        /// <summary>
        /// Inverts this sort order
        /// </summary>
        /// <param name="self">The sort order to invert.</param>
        /// <returns>The inverted sort order.</returns>
        public static SortOrder Invert(this SortOrder self) => (SortOrder)((byte)self ^ 1);

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
        /// <param name="self"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid DefaultIfNullOrEmpty(this Guid? self, Guid defaultValue)
        {
            return self.GetValueOrDefault(Guid.Empty) == Guid.Empty
                ? defaultValue
                : self!.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this Guid? self) => self == null || self == Guid.Empty;


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
        public static T ThrowIfCanNotReadArgument<T>(this T self, string paramName, string? message = null) where T : Stream
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
        public static T ThrowIfCanNotWriteArgument<T>(this T self, string paramName, string? message = null) where T : Stream
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
        /// Sets the console foreground color to the specified color and invokes the provided action.
        /// </summary>
        /// <param name="self">The color to set the console foreground to.</param>
        /// <param name="callback">The action to invoke.</param>
        [DebuggerStepThrough]
        public static void AsForegroundColor(this ConsoleColor self, Action callback)
        {
            try
            {
                Console.ForegroundColor = self;
                callback?.Invoke();
            }
            finally
            {
                Console.ResetColor();
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
            result = factory.Invoke().ThrowIfNull(() => new ArgumentException($"Factory invocation returned null.", nameof(factory)));
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

        public static T ThrowIfNotOfType<T>(this T self, Type expectedType, Func<Type, Exception> exceptionFactory) where T : class
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (self.GetType() == expectedType) return self;
            throw exceptionFactory?.Invoke(expectedType) ?? new InvalidCastException();
        }

        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSuccessStatusCode(this HttpStatusCode statusCode)
        {
            var x = (int)statusCode / 100;
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
        public static T ThrowIfNull<T>(this T self, Func<Exception> factory) where T : class?
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
        public static T ThrowIfNull<T>(this T? self, string message) where T : class
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
        [Obsolete(@"Use ThrowIf.NullArgument instead.", true)]
        public static T ThrowIfNullArgument<T>(this T self, string parameterName, string? message = null) where T : class
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="createException"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="createException"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid ThrowIfNullOrEmpty(this Guid? self, Func<Exception> createException)
        {
            if (self == null || self == Guid.Empty)
            {
                var error = createException.Invoke();
                throw error;
            }

            return self.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Guid ThrowIfNullOrEmpty(this Guid? self, string message)
        {
            if (self == null || self == Guid.Empty)
            {
                throw new InvalidOperationException(message);
            }

            return self.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Guid DefaultIfEmpty(this Guid self, Guid defaultValue)
        {
            return self == Guid.Empty
                ? defaultValue
                : self;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="parameterName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [return: NotNull]
        public static Guid ThrowIfEmptyArgument(this Guid self, string parameterName, string? message = null)
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
        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToBase64String(this byte[] self) =>
            System.Convert.ToBase64String(self ?? throw new NullReferenceException());

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cryptoTransform"></param>
        /// <returns></returns>
        public static byte[] Encrypt(this byte[] self, ICryptoTransform cryptoTransform)
        {
            using MemoryStream msEncrypt = new MemoryStream();
            using CryptoStream csEncrypt = new CryptoStream(msEncrypt, cryptoTransform, CryptoStreamMode.Write);
            csEncrypt.Write(self, 0, self.Length);
            csEncrypt.FlushFinalBlock();
            return msEncrypt.ToArray();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="genericTypeDefinition"></param>
        /// <returns></returns>
        public static bool IsSubclassOfGenericType(this Type self, Type genericTypeDefinition)
        {
            if (self.IsClass == false ||
                genericTypeDefinition.IsGenericTypeDefinition == false)
            {
                return false;
            }

            for (var type = self;
                 type is { IsClass: true } && type != typeof(object);
                 type = type.BaseType)
            {
                if (type.IsGenericType)
                {
                    if (type.GetGenericTypeDefinition() == genericTypeDefinition)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="createException"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        /// <exception cref="Exception">User constructed exception object</exception>
        [DebuggerNonUserCode]
        [return: NotNull]
        public static T ThrowIfNull<T>(this T? self, Func<Exception> createException) where T : struct
        {
            if (createException == null) throw new ArgumentNullException(nameof(createException));
            if (self.HasValue == false)
            {
                var error = createException.Invoke() ?? throw new NullReferenceException($"{nameof(createException)}() returned null.");
                throw error;
            }
            return self.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [DebuggerNonUserCode]
        [return: NotNull]
        public static T ThrowIfNull<T>(this T? self, string message) where T : struct
        {
            if (self.HasValue == false)
            {
                throw new InvalidOperationException(message);
            }
            return self.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        [DebuggerNonUserCode]
        [return: NotNull]
        public static T ThrowIfNull<T>(this T? self) where T : struct
        {
            if (self.HasValue == false)
            {
                throw new InvalidOperationException();
            }

            return self.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Convert<TSource, TResult>(this TSource self, Func<TSource, TResult> transform) => 
            transform.Invoke(self);


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="self"></param>
        /// <param name="transform"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Convert<TSource, TResult, TException>(
            this TSource self, 
            Func<TSource, TResult> transform, 
            Action<TException> onError)
        {
            try
            {
                return transform.Invoke(self);
            }
            catch (Exception ex) when(ex is TException targetEx)
            {
                onError.Invoke(targetEx);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="transform"></param>
        /// <param name="onError"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Convert<TSource, TResult>(
            this TSource self,
            Func<TSource, TResult> transform,
            Action<Exception> onError)
        {
            try
            {
                return transform.Invoke(self);
            }
            catch (Exception ex)
            {
                onError.Invoke(ex);
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TException"></typeparam>
        /// <param name="self"></param>
        /// <param name="transform"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Convert<TSource, TResult, TException>(
            this TSource self,
            Func<TSource, TResult> transform,
            Func<TException, TResult> fallback)
        {
            try
            {
                return transform.Invoke(self);
            }
            catch (Exception ex) when (ex is TException targetEx)
            {
                return fallback.Invoke(targetEx);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="self"></param>
        /// <param name="transform"></param>
        /// <param name="fallback"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TResult Convert<TSource, TResult>(
            this TSource self,
            Func<TSource, TResult> transform,
            Func<Exception, TResult> fallback)
        {
            try
            {
                return transform.Invoke(self);
            }
            catch (Exception ex) 
            {
                return fallback.Invoke(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="self"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSource Convert<TSource>(this TSource self, Action<TSource> transform)
        {
            transform.Invoke(self);
            return self;
        }
    }
}
