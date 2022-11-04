using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Solitons.Collections;

namespace Solitons
{
    public static partial class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode]
        public static T GetRandomElement<T>(this IReadOnlyList<T> self)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            var index = (int)(DateTime.UtcNow.Ticks % self.Count);
            return self[index];
        }



        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [return: NotNull]
        public static FluentCollection<T> AsFluentCollection<T>(this ICollection<T> self)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));

            return self is FluentCollection<T> fluent
                ? fluent
                : new FluentCollection<T>(self);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="callback"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AsFluentCollection<T>(this ICollection<T> self, Action<FluentCollection<T>> callback)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            var fluentCollection = self.AsFluentCollection();
            callback.Invoke(fluentCollection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [return: NotNull]
        public static FluentList<T> AsFluentList<T>(this IList<T> self)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            return self is FluentList<T> fluent
                ? fluent
                : new FluentList<T>(self);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="callback"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AsFluentList<T>(this IList<T> self, Action<FluentList<T>> callback)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (callback == null) throw new ArgumentNullException(nameof(callback));
            var fluentList = self.AsFluentList();
            callback.Invoke(fluentList);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ThrowIfNullOrEmpty<T>(this T[] self, Func<Exception> factory) where T : class
        {
            if (self is null || self.Length == 0)
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
        /// <param name="argName"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ThrowIfNullOrEmptyArgument<T>(this T[] self, string argName, string? message = null) where T : class
        {
            if (self is null || self.Length == 0)
            {
                argName = argName.DefaultIfNullOrWhiteSpace("?");
                message = message.DefaultIfNullOrWhiteSpace($"{argName} array is null or empty.");
                throw new ArgumentException(message, argName);
            }

            return self;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ThrowIfNullOrEmpty<T>(this T self, Func<Exception> factory) where T : ICollection
        {
            if (self is null || self.Count == 0)
            {
                throw factory?.Invoke() ?? new NullReferenceException();
            }

            return self;
        }



        public static int AddRange<T>(this ISet<T> self, IEnumerable<T> range, bool withPriorCleanup = false)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (range == null) throw new ArgumentNullException(nameof(range));
            if (withPriorCleanup)
            {
                self.Clear();
            }
            return range.Count(self.Add);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T> self) => self ?? Enumerable.Empty<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] EmptyIfNull<T>(this T[] self) => self ?? Array.Empty<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, Func<TValue> factory)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (self.TryGetValue(key, out var value))
            {
                return value;
            }

            self[key] = value = factory();
            return value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, object> self, TKey key, Func<TValue> factory)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            if (self.TryGetValue(key, out var value) && value is TValue result)
            {
                return result;
            }

            self[key] = result = factory();
            return result;
        }

        /// <summary>
        /// Filters out values from this sequence based on a predicate.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="self"/>.</typeparam>
        /// <param name="self">The sequence to filter.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that contains all the elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException">self or predicate </exception>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> Skip<T>(this IEnumerable<T> self, Func<T, bool> predicate)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return self.Where(item => !predicate.Invoke(item));
        }

        /// <summary>
        /// Skips the nulls.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self">The self.</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SkipNulls<T>(this IEnumerable<T> self) =>
            self.Skip(_ => _ is null);


        /// <summary>
        /// Invokes the specified action for each element of this sequence.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="self"/>.</typeparam>
        /// <param name="self">The sequence of elements to invoke action upon.</param>
        /// <param name="action">The action to invoke for each element.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> of the original sequence elements observed or modified by the <paramref name="action"/>.</returns>
        /// <exception cref="ArgumentNullException">self or action</exception>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> self, Action<T> action)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (action == null) throw new ArgumentNullException(nameof(action));
            foreach (var item in self)
            {
                action.Invoke(item);
                yield return item;
            }
        }

        public static IEnumerable<T> Do<T>(this IEnumerable<T> self, Action<T, int> action)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (action == null) throw new ArgumentNullException(nameof(action));

            int index = 0;
            foreach (var item in self)
            {
                action.Invoke(item, index++);
                yield return item;
            }
        }

        [DebuggerStepThrough]
        public static void ForEach<T>(this IEnumerable<T> self, Action<T> action)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (action == null) throw new ArgumentNullException(nameof(action));

            foreach (var item in self)
            {
                action.Invoke(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> self, Action<T, int> action)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (action == null) throw new ArgumentNullException(nameof(action));

            int count = 0;
            foreach (var item in self)
            {
                action.Invoke(item, count++);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="self"></param>
        /// <param name="comparer"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> self, IEqualityComparer<TKey>? comparer = null)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            comparer ??= EqualityComparer<TKey>.Default;
            return self.ToDictionary(pair => pair.Key, pair => pair.Value, comparer);
        }

        public static IObservable<IDictionary<TKey, TValue>> ToDictionary<TKey, TValue>(
            this IObservable<KeyValuePair<TKey, TValue>> self, IEqualityComparer<TKey>? comparer = null)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            comparer ??= EqualityComparer<TKey>.Default;
            return self.ToDictionary(pair => pair.Key, pair => pair.Value, comparer);
        }

        public static Dictionary<TKey, object> GroupByKey<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> self, IEqualityComparer<TKey>? comparer = null)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            comparer ??= EqualityComparer<TKey>.Default;
            var list = new List<TValue>();
            return self
                .GroupBy(p => p.Key, p => p.Value, comparer)
                .Select(item =>
                {
                    list.Clear();
                    list.AddRange(item);
                    object value = list.Count == 1 ? list[0] : list.ToArray();
                    return KeyValuePair.Create(item.Key, value);
                })
                .ToDictionary(pair=> pair.Key, p=>p.Value, comparer);
        }
    }
}
