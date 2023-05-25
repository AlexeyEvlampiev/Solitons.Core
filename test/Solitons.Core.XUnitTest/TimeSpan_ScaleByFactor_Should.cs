using System;
using Xunit;

namespace Solitons;

// ReSharper disable once InconsistentNaming
public sealed class TimeSpan_ScaleByFactor_Should
{
    [Fact]
    public void ScalesCorrectly()
    {
        var original = TimeSpan.FromSeconds(10);
        var actual = original.ScaleByFactor(2, 3); 
        Assert.Equal(TimeSpan.FromSeconds(80), actual);
    }

    [Fact]
    public void ZeroExponent_ReturnsOriginal()
    {
        var original = TimeSpan.FromSeconds(10);
        var actual = original.ScaleByFactor(2, 0); 
        Assert.Equal(TimeSpan.FromSeconds(10), actual);
    }

    [Fact]
    public void ScaleByFactor_ZeroScaleFactor_ReturnsZero()
    {
        var original = TimeSpan.FromSeconds(10);
        var actual = original.ScaleByFactor(0, 2); // Expect 0 seconds
        Assert.Equal(TimeSpan.Zero, actual);
    }

    [Fact]
    public void ScaleByFactor_NegativeScaleFactor_ThrowsArgumentException()
    {
        var original = TimeSpan.FromSeconds(10);
        Assert.Throws<ArgumentException>(() => original.ScaleByFactor(-2, 2));
    }

    [Fact]
    public void ScaleByFactor_NegativeExponent_ThrowsArgumentException()
    {
        var original = TimeSpan.FromSeconds(10);
        Assert.Throws<ArgumentException>(() => original.ScaleByFactor(2, -2));
    }
}