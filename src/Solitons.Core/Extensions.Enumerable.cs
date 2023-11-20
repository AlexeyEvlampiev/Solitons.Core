using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Solitons.Collections;

namespace Solitons;

public static partial class Extensions
{
    /// <summary>
    /// Returns a random element from the specified list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="self">The list to select a random element from.</param>
    /// <returns>A random element from the list.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the list is null.</exception>
    [DebuggerNonUserCode]
    public static T GetRandomElement<T>(this IReadOnlyList<T> self)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        var index = (int)(DateTime.UtcNow.Ticks % self.Count);
        return self[index];
    }



    /// <summary>
    /// Converts the specified collection to a fluent collection.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="self">The collection to convert.</param>
    /// <returns>A fluent collection that wraps the original collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the collection is null.</exception>
    [return: NotNull]
    public static FluentCollection<T> AsFluentCollection<T>(this ICollection<T> self)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));

        return self is FluentCollection<T> fluent
            ? fluent
            : new FluentCollection<T>(self);
    }


    /// <summary>
    /// Converts the specified collection to a fluent collection and applies the specified callback.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="self">The collection to convert.</param>
    /// <param name="callback">The callback to apply to the fluent collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when the collection or callback is null.</exception>
    public static void AsFluentCollection<T>(this ICollection<T> self, Action<FluentCollection<T>> callback)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        if (callback == null) throw new ArgumentNullException(nameof(callback));
        var fluentCollection = self.AsFluentCollection();
        callback.Invoke(fluentCollection);
    }

    /// <summary>
    /// Converts the specified list to a fluent list.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="self">The list to convert.</param>
    /// <returns>A fluent list that wraps the original list.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the list is null.</exception>
    [return: NotNull]
    public static FluentList<T> AsFluentList<T>(this IList<T> self)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        return self is FluentList<T> fluent
            ? fluent
            : new FluentList<T>(self);
    }

    /// <summary>
    /// Converts the specified list to a fluent list and applies the specified callback.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    /// <param name="self">The list to convert.</param>
    /// <param name="callback">The callback to apply to the fluent list.</param>
    /// <exception cref="ArgumentNullException">Thrown when the list or callback is null.</exception>
    public static void AsFluentList<T>(this IList<T> self, Action<FluentList<T>> callback)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        if (callback == null) throw new ArgumentNullException(nameof(callback));
        var fluentList = self.AsFluentList();
        callback.Invoke(fluentList);
    }


    /// <summary>
    /// Throws an exception if the specified array is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="self">The array to check.</param>
    /// <param name="factory">A factory function that returns the exception to throw if the array is null or empty.</param>
    /// <returns>The original array if it is not null or empty.</returns>
    /// <exception cref="NullReferenceException">Thrown when the array is null and no factory function is provided.</exception>
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
    /// Throws an exception if the specified collection is null or empty.
    /// </summary>
    /// <typeparam name="T">The type of collection.</typeparam>
    /// <param name="self">The collection to check.</param>
    /// <param name="factory">A factory function that returns the exception to throw if the collection is null or empty.</param>
    /// <returns>The original collection if it is not null or empty.</returns>
    /// <exception cref="NullReferenceException">Thrown when the collection is null and no factory function is provided.</exception>
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


    /// <summary>
    /// Adds the elements from the specified range to the set and returns the number of added elements.
    /// </summary>
    /// <typeparam name="T">The type of elements in the set.</typeparam>
    /// <param name="self">The set to add elements to.</param>
    /// <param name="range">The range of elements to add.</param>
    /// <param name="withPriorCleanup">Whether to clear the set before adding the elements.</param>
    /// <returns>The number of elements added to the set.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the set or range is null.</exception>
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
    /// Returns an empty enumerable if the specified enumerable is null, otherwise returns the original enumerable.
    /// </summary>
    /// <typeparam name="T">The type of elements in the enumerable.</typeparam>
    /// <param name="self">The enumerable to check.</param>
    /// <returns>An empty enumerable if the specified enumerable is null, otherwise returns the original enumerable.</returns>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? self) => self ?? Enumerable.Empty<T>();

    /// <summary>
    /// Returns an empty array if the specified array is null, otherwise returns the original array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the array.</typeparam>
    /// <param name="self">The array to check.</param>
    /// <returns>An empty array if the specified array is null, otherwise returns the original array.</returns>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[] EmptyIfNull<T>(this T[]? self) => self ?? Array.Empty<T>();

    /// <summary>
    /// Gets the value associated with the specified key, or adds a new value if the key does not exist in the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="self">The dictionary to retrieve or add the value to.</param>
    /// <param name="key">The key to retrieve or add the value for.</param>
    /// <param name="factory">A factory function that returns the value to add if the key does not exist in the dictionary.</param>
    /// <returns>The value associated with the specified key, or the newly added value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the dictionary, key, or factory is null.</exception>
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
    /// Gets the value associated with the specified key, or adds a new value if the key does not exist in the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="self">The dictionary to retrieve or add the value to.</param>
    /// <param name="key">The key to retrieve or add the value for.</param>
    /// <param name="defaultValue">The default value to add if the key does not exist in the dictionary.</param>
    /// <returns>The value associated with the specified key, or the newly added value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the dictionary or key is null.</exception>
    public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue defaultValue)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (self.TryGetValue(key, out var value))
        {
            return value;
        }

        self[key] = defaultValue;
        return defaultValue;
    }

    /// <summary>
    /// Gets the value associated with the specified key, or adds a new value if the key does not exist in the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="self">The dictionary to retrieve or add the value to.</param>
    /// <param name="key">The key to retrieve or add the value for.</param>
    /// <param name="factory">A factory function that returns the value to add if the key does not exist in the dictionary.</param>
    /// <returns>The value associated with the specified key, or the newly added value.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the dictionary, key, or factory is null.</exception>
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
    /// Returns an enumerable that skips over elements in the source sequence that satisfy a specified predicate function.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="self">The sequence to skip elements from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>An enumerable that contains the elements of the source sequence that do not satisfy the specified predicate.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the source sequence or predicate is null.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> Skip<T>(this IEnumerable<T> self, Func<T, bool> predicate)
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        if (predicate == null) throw new ArgumentNullException(nameof(predicate));
        return self.Where(item => !predicate.Invoke(item));
    }

    /// <summary>
    /// Returns an enumerable that skips over null elements in the source sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="self">The sequence to skip null elements from.</param>
    /// <returns>An enumerable that contains the non-null elements of the source sequence.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the source sequence is null.</exception>
    [DebuggerStepThrough]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> SkipNulls<T>(this IEnumerable<T> self) =>
        self.Skip(_ => _ is null);


    /// <summary>
    /// Performs the specified action on each element of the source sequence, and returns the source sequence as an enumerable.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="self">The sequence to perform the action on.</param>
    /// <param name="action">The action to perform on each element of the source sequence.</param>
    /// <returns>An enumerable that contains the elements of the source sequence, after the action has been performed on each element.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the source sequence or action is null.</exception>
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

    /// <summary>
    /// Performs the specified action on each element of the source sequence, and returns the source sequence as an enumerable, passing the index of each element to the action.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="self">The sequence to perform the action on.</param>
    /// <param name="action">The action to perform on each element of the source sequence, with the index of the element as the second parameter.</param>
    /// <returns>An enumerable that contains the elements of the source sequence, after the action has been performed on each element.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the source sequence or action is null.</exception>
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

    /// <summary>
    /// Performs the specified action on each element of the source sequence.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="self">The sequence to perform the action on.</param>
    /// <param name="action">The action to perform on each element of the source sequence.</param>
    /// <exception cref="ArgumentNullException">Thrown when the source sequence or action is null.</exception>
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

    /// <summary>
    /// Performs the specified action on each element of the source sequence, passing the index of each element to the action.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="self">The sequence to perform the action on.</param>
    /// <param name="action">The action to perform on each element of the source sequence, with the index of the element as the second parameter.</param>
    /// <exception cref="ArgumentNullException">Thrown when the source sequence or action is null.</exception>
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

#if !NET8_0_OR_GREATER
    /// <summary>
    /// Creates a dictionary from the sequence of key-value pairs.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the sequence.</typeparam>
    /// <typeparam name="TValue">The type of values in the sequence.</typeparam>
    /// <param name="self">The sequence of key-value pairs to create the dictionary from.</param>
    /// <param name="comparer">The equality comparer to use for comparing keys, or null to use the default equality comparer for the type of the key.</param>
    /// <returns>A new dictionary that contains the key-value pairs from the input sequence.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input sequence is null.</exception>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> self, IEqualityComparer<TKey>? comparer = null) where TKey : notnull
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        comparer ??= EqualityComparer<TKey>.Default;
        return self.ToDictionary(pair => pair.Key, pair => pair.Value, comparer);
    }
#endif

    /// <summary>
    /// Projects the observable sequence of key-value pairs into an observable dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the observable sequence.</typeparam>
    /// <typeparam name="TValue">The type of values in the observable sequence.</typeparam>
    /// <param name="self">The observable sequence of key-value pairs to create the dictionary from.</param>
    /// <param name="comparer">The equality comparer to use for comparing keys, or null to use the default equality comparer for the type of the key.</param>
    /// <returns>An observable dictionary that contains the key-value pairs from the input sequence.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input observable sequence is null.</exception>
    public static IObservable<IDictionary<TKey, TValue>> ToDictionary<TKey, TValue>(
        this IObservable<KeyValuePair<TKey, TValue>> self, IEqualityComparer<TKey>? comparer = null) where TKey : notnull
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        comparer ??= EqualityComparer<TKey>.Default;
        return self.ToDictionary(pair => pair.Key, pair => pair.Value, comparer);
    }

    /// <summary>
    /// Groups the key-value pairs in the input sequence by key and projects them into a dictionary with each key associated with a value that is either a single value or an array of values with that key.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the input sequence.</typeparam>
    /// <typeparam name="TValue">The type of values in the input sequence.</typeparam>
    /// <param name="self">The input sequence of key-value pairs to group and project into a dictionary.</param>
    /// <param name="comparer">The equality comparer to use for comparing keys, or null to use the default equality comparer for the type of the key.</param>
    /// <returns>A dictionary with each key associated with a value that is either a single value or an array of values with that key.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the input sequence is null.</exception>
    public static Dictionary<TKey, object> GroupByKey<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> self, IEqualityComparer<TKey>? comparer = null) where TKey : notnull
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
            .ToDictionary(pair => pair.Key, p => p.Value, comparer);
    }
}