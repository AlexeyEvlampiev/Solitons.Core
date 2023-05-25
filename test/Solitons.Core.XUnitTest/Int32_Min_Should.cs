using Xunit;

namespace Solitons;

// ReSharper disable once InconsistentNaming
public sealed class Int32_Min_Should
{
    [Theory]
    [InlineData(10, 1, 10)]
    [InlineData(10, 2, 10)]
    [InlineData(1, 10, 10)]
    [InlineData(2, 10, 10)]
    public void Work(int source, int threshold, int expected)
    {
        var actual = source.Min(threshold);
        Assert.Equal(expected, actual);
    }
}