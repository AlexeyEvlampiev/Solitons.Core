using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Solitons;

// ReSharper disable once InconsistentNaming
public sealed class Int32_Max_Should
{
    [Theory]
    [InlineData(10, 1, 1)]
    [InlineData(10, 2, 2)]
    [InlineData(1, 10, 1)]
    [InlineData(2, 10, 2)]
    public void Work(int source, int threshold, int expected)
    {
        var actual = source.Max(threshold);
        Assert.Equal(expected, actual);
    }
}