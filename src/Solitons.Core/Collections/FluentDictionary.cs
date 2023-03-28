using System.Collections.Generic;
using System.Diagnostics;
using Solitons.Collections.Common;

namespace Solitons.Collections;

/// <summary>
/// Represents a dictionary that provides a fluent interface for adding key-value pairs.
/// </summary>
/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
public sealed class FluentDictionary<TKey, TValue> : DictionaryProxy<TKey, TValue>
{
    internal FluentDictionary(IDictionary<TKey, TValue> innerDictionary) : base(innerDictionary)
    {
    }

    /// <summary>
    /// Adds a key-value pair to the dictionary and returns the current instance of the FluentDictionary class.
    /// </summary>
    /// <param name="key">The key to add to the dictionary.</param>
    /// <param name="value">The value to add to the dictionary.</param>
    /// <returns>The current instance of the FluentDictionary class.</returns>
    [DebuggerStepThrough]
    public new FluentDictionary<TKey, TValue> Add(TKey key, TValue value)
    {
        ((IDictionary<TKey, TValue>)this).Add(key, value);
        return this;
    }
}

/// <summary>
/// Provides methods to create instances of the FluentDictionary class.
/// </summary>
public static class FluentDictionary
{
    /// <summary>
    /// Creates a new instance of the FluentDictionary class with a default comparer.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <returns>A new instance of the FluentDictionary class.</returns>
    public static FluentDictionary<TKey, TValue> Create<TKey, TValue>() where TKey : notnull => 
        new (new Dictionary<TKey, TValue>());


    /// <summary>
    /// Creates a new instance of the FluentDictionary class with a specified comparer.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="comparer">The comparer to use for comparing keys.</param>
    /// <returns>A new instance of the FluentDictionary class.</returns>
    public static FluentDictionary<TKey, TValue> Create<TKey, TValue>(IEqualityComparer<TKey> comparer) where TKey : notnull =>
        new(new Dictionary<TKey, TValue>(comparer));

    /// <summary>
    /// Creates a new instance of the FluentDictionary class from an existing dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="innerDictionary">The dictionary to use as the inner dictionary for the new instance.</param>
    /// <returns>A new instance of the FluentDictionary class.</returns>
    public static FluentDictionary<TKey, TValue> Create<TKey, TValue>(IDictionary<TKey, TValue> innerDictionary) where TKey : notnull =>
        new(innerDictionary);
}