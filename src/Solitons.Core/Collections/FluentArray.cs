using System.Diagnostics;

namespace Solitons.Collections;

/// <summary>
/// Provides a set of fluent static methods to create arrays.
/// </summary>
public static class FluentArray
{
    /// <summary>
    /// Creates a new array of type <typeparamref name="T"/> with a single element.
    /// </summary>
    /// <typeparam name="T">The type of the array element.</typeparam>
    /// <param name="item">The element to include in the array.</param>
    /// <returns>A new array of type <typeparamref name="T"/> with a single element.</returns>
    public static T[] Create<T>(T item) => new T[] { item };

    /// <summary>
    /// Creates a new array of type <typeparamref name="T"/> with two elements.
    /// </summary>
    /// <typeparam name="T">The type of the array elements.</typeparam>
    /// <param name="item1">The first element to include in the array.</param>
    /// <param name="item2">The second element to include in the array.</param>
    /// <returns>A new array of type <typeparamref name="T"/> with two elements.</returns>
    public static T[] Create<T>(T item1, T item2) => new T[] { item1, item2 };

    /// <summary>
    /// Creates a new array of type <typeparamref name="T"/> with three elements.
    /// </summary>
    /// <typeparam name="T">The type of the array elements.</typeparam>
    /// <param name="item1">The first element to include in the array.</param>
    /// <param name="item2">The second element to include in the array.</param>
    /// <param name="item3">The third element to include in the array.</param>
    /// <returns>A new array of type <typeparamref name="T"/> with three elements.</returns>
    public static T[] Create<T>(T item1, T item2, T item3) => new T[] { item1, item2, item3 };

    /// <summary>
    /// Creates a new array of type <typeparamref name="T"/> with any number of elements.
    /// </summary>
    /// <typeparam name="T">The type of the array elements.</typeparam>
    /// <param name="items">The elements to include in the array.</param>
    /// <returns>A new array of type <typeparamref name="T"/> with the provided elements.</returns>
    [DebuggerNonUserCode]
    public static T[] Create<T>(params T[] items) => items;
}