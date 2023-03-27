using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solitons.Collections.Common;

/// <summary>
/// Provides a base implementation for a proxy class that wraps an <see cref="IDictionary{TKey, TValue}"/> instance.
/// </summary>
/// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
/// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
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
    public void Add(KeyValuePair<TKey, TValue> item)
    {
        _innerDictionary.Add(item);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public void Clear()
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
    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        _innerDictionary.CopyTo(array, arrayIndex);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public bool Remove(KeyValuePair<TKey, TValue> item)
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
        _innerDictionary.Add(key, value);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public bool ContainsKey(TKey key)
    {
        return _innerDictionary.ContainsKey(key);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public bool Remove(TKey key)
    {
        return _innerDictionary.Remove(key);
    }

    /// <inheritdoc/>
    [DebuggerStepThrough]
    public bool TryGetValue(TKey key, out TValue value)
    {
        return _innerDictionary.TryGetValue(key, out value);
    }

    /// <inheritdoc/>
    public TValue this[TKey key]
    {
        [DebuggerStepThrough]
        get => _innerDictionary[key];
        [DebuggerStepThrough]
        set => _innerDictionary[key] = value;
    }

    /// <inheritdoc/>
    public ICollection<TKey> Keys => _innerDictionary.Keys;

    /// <inheritdoc/>
    public ICollection<TValue> Values => _innerDictionary.Values;
}