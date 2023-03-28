

namespace Solitons.Collections;

/// <summary>
/// Represents a method that can be used to look up a value in a dictionary-like data structure using a specified key.
/// </summary>
/// <typeparam name="TKey">The type of the key to look up in the data structure.</typeparam>
/// <typeparam name="TValue">The type of the value associated with the specified key.</typeparam>
/// <param name="key">The key to look up in the data structure.</param>
/// <param name="value">When this method returns, contains the value associated with the specified key if the key was found in the data structure; otherwise, the default value for the type of the value parameter.</param>
/// <returns>true if the key was found in the data structure and the associated value was returned in the value parameter; otherwise, false.</returns>
public delegate bool KeyValueLookup<in TKey, TValue>(TKey key, out TValue value);