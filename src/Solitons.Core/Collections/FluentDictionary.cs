using System;
using System.Collections.Generic;
using System.Diagnostics;
using Solitons.Collections.Common;

namespace Solitons.Collections;

/// <summary>
/// Represents a dictionary that provides a fluent interface for adding key-value pairs.
/// </summary>
/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
/// <example>
/// <code>
/// <![CDATA[
/// var fluentDict = FluentDictionary.Create<string, int>()
///                                   .Add("One", 1)
///                                   .Add("Two", 2);
/// ]]>
/// </code>
/// </example>
public sealed class FluentDictionary<TKey, TValue> : DictionaryProxy<TKey, TValue>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="FluentDictionary{TKey, TValue}"/> class that wraps the specified dictionary instance.
    /// </summary>
    /// <param name="innerDictionary">The inner dictionary instance to wrap.</param>
    [DebuggerStepThrough]
    internal FluentDictionary(IDictionary<TKey, TValue> innerDictionary) : base(innerDictionary)
    {
    }

    /// <summary>
    /// Adds a key-value pair to the dictionary and returns the current instance of the FluentDictionary class, allowing for fluent chaining of add operations.
    /// </summary>
    /// <param name="key">The key to add to the dictionary.</param>
    /// <param name="value">The value to add to the dictionary.</param>
    /// <returns>The current instance of the FluentDictionary class.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the key is null.</exception>
    /// <exception cref="ArgumentException">Thrown when an element with the same key already exists in the dictionary.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentDict = FluentDictionary.Create<string, int>()
    ///                                   .Add("One", 1)
    ///                                   .Add("Two", 2);
    /// ]]>
    /// </code>
    /// </example>
    [DebuggerStepThrough]
    public new FluentDictionary<TKey, TValue> Add(TKey key, TValue value)
    {
        ((IDictionary<TKey, TValue>)this).Add(key, value);
        return this;
    }
}

/// <summary>
/// Provides static methods to create instances of the <see cref="FluentDictionary{TKey, TValue}"/> class.
/// </summary>
public static class FluentDictionary
{
    /// <summary>
    /// Creates a new instance of the <see cref="FluentDictionary{TKey, TValue}"/> class with a default comparer.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <returns>A new instance of the <see cref="FluentDictionary{TKey, TValue}"/> class.</returns>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var fluentDict = FluentDictionary.Create<string, int>();
    /// ]]>
    /// </code>
    /// </example>
    public static FluentDictionary<TKey, TValue> Create<TKey, TValue>() where TKey : notnull => 
        new (new Dictionary<TKey, TValue>());


    /// <summary>
    /// Creates a new instance of the <see cref="FluentDictionary{TKey, TValue}"/> class with a specified comparer.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="comparer">The comparer to use for comparing keys.</param>
    /// <returns>A new instance of the <see cref="FluentDictionary{TKey, TValue}"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the comparer is null.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var comparer = StringComparer.OrdinalIgnoreCase;
    /// var fluentDict = FluentDictionary.Create<string, int>(comparer);
    /// ]]>
    /// </code>
    /// </example>
    public static FluentDictionary<TKey, TValue> Create<TKey, TValue>(IEqualityComparer<TKey> comparer) where TKey : notnull =>
        new(new Dictionary<TKey, TValue>(comparer));

    /// <summary>
    /// Creates a new instance of the <see cref="FluentDictionary{TKey, TValue}"/> class from an existing dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    /// <param name="innerDictionary">The dictionary to use as the inner dictionary for the new instance.</param>
    /// <returns>A new instance of the <see cref="FluentDictionary{TKey, TValue}"/> class.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the inner dictionary is null.</exception>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// var dictionary = new Dictionary<string, int> { { "One", 1 }, { "Two", 2 } };
    /// var fluentDict = FluentDictionary.Create(dictionary);
    /// ]]>
    /// </code>
    /// </example>
    public static FluentDictionary<TKey, TValue> Create<TKey, TValue>(IDictionary<TKey, TValue> innerDictionary) where TKey : notnull =>
        new(innerDictionary);
}