using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitons;

sealed class InvertedComparer<T> : Comparer<T>
{
    public InvertedComparer(IComparer<T> innerComparer)
    {
        InnerComparer = innerComparer;
    }

    public IComparer<T> InnerComparer { get; }

    [DebuggerStepThrough]
    public override int Compare(T? x, T? y)
    {
        return (-1) * InnerComparer.Compare(x, y);
    }
}