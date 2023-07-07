using System.Collections.Generic;
using System.Linq;

namespace Solitons;

/// <summary>
/// A static factory class for creating instances of <see cref="MultiCriteriaComparer{T}"/> class.
/// </summary>
public static class MultiCriteriaComparer
{
    /// <summary>
    /// Creates a new instance of the MultiCriteriaComparer class with a root comparer.
    /// </summary>
    /// <param name="rootComparer">The initial comparer.</param>
    /// <returns>A new instance of the MultiCriteriaComparer class.</returns>
    public static MultiCriteriaComparer<T> From<T>(
        IComparer<T> rootComparer)
    {
        return new MultiCriteriaComparer<T>(rootComparer);
    }

}

/// <summary>
/// A comparer that can compare objects of type T based on a sequence of <see cref="Comparer{T}"/> instances.
/// The comparison will proceed in the sequence the comparers were added and will stop at the first non-zero result.
/// </summary>
public sealed class MultiCriteriaComparer<T> : Comparer<T>
{
    private readonly List<IComparer<T>> _comparersInSequence = new();

    public MultiCriteriaComparer(IComparer<T> rootComparer)
    {
        _comparersInSequence.Add(rootComparer);
    }


    /// <summary>
    /// Adds a new comparer to the sequence of comparers. This comparer will be used if all previous comparers in the sequence return 0.
    /// </summary>
    /// <param name="next">The next comparer to add to the sequence.</param>
    /// <returns>The current instance with the new comparer added to the sequence.</returns>
    public MultiCriteriaComparer<T> WithFallback(IComparer<T> next)
    {
        _comparersInSequence.Add(next);
        return this;
    }

    /// <summary>
    /// Compares two objects of type T using the sequence of  <see cref="Comparer{T}"/> instances.
    /// The comparison will proceed in the sequence the comparers were added and will stop at the first non-zero result.
    /// </summary>
    /// <param name="x">The first object to compare.</param>
    /// <param name="y">The second object to compare.</param>
    /// <returns>A negative number if x is less than y, 0 if x equals y, or a positive number if x is greater than y.</returns>
    public override int Compare(T? x, T? y)
    {
        return _comparersInSequence
            .Select(comparer => comparer.Compare(x, y))
            .FirstOrDefault(result => result != 0);
    }
}