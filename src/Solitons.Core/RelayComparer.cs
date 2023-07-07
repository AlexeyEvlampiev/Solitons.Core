using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solitons;

public static class RelayComparer
{
    [DebuggerNonUserCode]
    public static RelayComparer<T> Create<T>(Func<T?, T?, int> compare) => new RelayComparer<T>(compare);

}

public sealed class RelayComparer<T> : Comparer<T>
{
    private readonly Func<T?, T?, int> _compare;

    [DebuggerNonUserCode]
    public RelayComparer(Func<T, T, int> compare)
    {
        _compare = compare;
    }

    [DebuggerStepThrough]
    public override int Compare(T? x, T? y) => _compare(x, y);
}