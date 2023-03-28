using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Solitons.Collections;

/// <summary>
/// Provides a collection of extension methods that return an <see cref="IEnumerable{T}"/>.
/// </summary>
public static class FluentEnumerable
{
    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> that contains a single item.
    /// </summary>
    /// <typeparam name="T">The type of the item to return.</typeparam>
    /// <param name="item">The item to return.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains only the specified item.</returns>
    public static IEnumerable<T> Yield<T>(T item)
    {
        yield return item;
    }

    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> that contains a single item, but only if the specified condition is true.
    /// </summary>
    /// <typeparam name="T">The type of the item to return.</typeparam>
    /// <param name="item">The item to return.</param>
    /// <param name="condition">The condition that must be true for the item to be returned.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains only the specified item, if the condition is true.</returns>
    [DebuggerNonUserCode]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> Yield<T>(T item, bool condition)
    {
        if(condition)yield return item;
    }

    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> that contains two items.
    /// </summary>
    /// <typeparam name="T">The type of the items to return.</typeparam>
    /// <param name="item1">The first item to return.</param>
    /// <param name="item2">The second item to return.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains the two specified items.</returns>
    public static IEnumerable<T> Yield<T>(T item1, T item2)
    {
        yield return item1;
        yield return item2;
    }

    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> that contains three items.
    /// </summary>
    /// <typeparam name="T">The type of the items to return.</typeparam>
    /// <param name="item1">The first item to return.</param>
    /// <param name="item2">The second item to return.</param>
    /// <param name="item3">The third item to return.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains the three specified items.</returns>
    public static IEnumerable<T> Yield<T>(T item1, T item2, T item3)
    {
        yield return item1;
        yield return item2;
        yield return item3;
    }

    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> that contains four items.
    /// </summary>
    /// <typeparam name="T">The type of the items to return.</typeparam>
    /// <param name="item1">The first item to return.</param>
    /// <param name="item2">The second item to return.</param>
    /// <param name="item3">The third item to return.</param>
    /// <param name="item4">The fourth item to return.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains the four specified items.</returns>
    public static IEnumerable<T> Yield<T>(T item1, T item2, T item3, T item4)
    {
        yield return item1;
        yield return item2;
        yield return item3;
        yield return item4;
    }

    /// <summary>
    /// Returns an <see cref="IEnumerable{T}"/> that contains the specified items.
    /// </summary>
    /// <typeparam name="T">The type of the items to return.</typeparam>
    /// <param name="items">The items to return.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains the specified items.</returns>
    public static IEnumerable<T> Yield<T>(params T[] items) => items;

}