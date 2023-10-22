using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Solitons.Collections.Common;

/// <summary>
/// Provides a base implementation for a proxy class that wraps an <see cref="IDictionary{TKey, TValue}"/> instance, enabling developers
/// to extend dictionary functionality as needed, such as creating a fluent dictionary API or intercepting read or write operations.
/// </summary>
/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
/// <example>
/// <code>
/// <![CDATA[
/// public class FluentDictionary<TKey, TValue> : DictionaryProxy<TKey, TValue>
/// {
///     public FluentDictionary(IDictionary<TKey, TValue> innerDictionary) : base(innerDictionary) { }
/// 
///     public new FluentDictionary<TKey, TValue> Add(TKey key, TValue value)
///     {
///         this.Add(key, value);
///         return this;
///     }
/// }
/// 
/// // Usage:
/// var dictionary = new Dictionary<string, int>();
/// var fluentDictionary = new FluentDictionary<string, int>(dictionary);
/// fluentDictionary.AddItem("One", 1).AddItem("Two", 2);
/// ]]>
/// </code>
/// </example>
public abstract class DictionaryProxy<TKey, TValue> : IDictionary<TKey, TValue>
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly IDictionary<TKey, TValue> _innerDictionary;

    /// <summary>
    /// Initializes a new instance of the <see cref="DictionaryProxy{TKey, TValue}"/> class that wraps the specified dictionary instance.
    /// </summary>
    /// <param name="innerDictionary">The inner dictionary instance to wrap.</param>
    [DebuggerStepThrough]
    protected DictionaryProxy(IDictionary<TKey, TValue> innerDictionary)
    {
        _innerDictionary = innerDictionary;
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _innerDictionary.GetEnumerator();
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_innerDictionary).GetEnumerator();
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public virtual void Add(KeyValuePair<TKey, TValue> item)
    {
        _innerDictionary.Add(item);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public virtual void Clear()
    {
        _innerDictionary.Clear();
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return _innerDictionary.Contains(item);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        _innerDictionary.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public virtual bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return _innerDictionary.Remove(item);
    }

    /// <inheritdoc/>
    public int Count => _innerDictionary.Count;

    /// <inheritdoc/>
    public bool IsReadOnly => _innerDictionary.IsReadOnly;

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public void Add(TKey key, TValue value)
    {
        this.Add(KeyValuePair.Create(key, value));
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public bool ContainsKey(TKey key)
    {
        return _innerDictionary.ContainsKey(key);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public virtual  bool Remove(TKey key)
    {
        return _innerDictionary.Remove(key);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public virtual bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        return _innerDictionary.TryGetValue(key, out value);
    }

    /// <inheritdoc/>
    public virtual TValue this[TKey key]
    {
        [DebuggerStepThrough]
        get => _innerDictionary[key];
        [DebuggerStepThrough]
        set => _innerDictionary[key] = value;
    }

    /// <inheritdoc/>
    public virtual ICollection<TKey> Keys => _innerDictionary.Keys;

    /// <inheritdoc/>
    public virtual ICollection<TValue> Values => _innerDictionary.Values;
}