using Xunit;

namespace Solitons.Data.Spatial;

// ReSharper disable once InconsistentNaming
public class BoundingBox_TryParseCsv_Should
{
    [Theory]
    [InlineData("149488.29419188126,410950.33041645837,149537.4305324534,411007.5940121479", 
        149488.29419188126, 410950.33041645837, 149537.4305324534, 411007.5940121479)]
    public void HandleValidCases(string csv, float xmin, float ymin, float xmax, float ymax)
    {
        Assert.True(BoundingBox.TryParseCsv(csv, out var boundingBox));
        Assert.Equal(xmin, boundingBox.Xmin);
        Assert.Equal(ymin, boundingBox.Ymin);
        Assert.Equal(xmax, boundingBox.Xmax);
        Assert.Equal(ymax, boundingBox.Ymax);

        var cloneCsv = boundingBox.ToString();
        Assert.True(BoundingBox.TryParseCsv(cloneCsv, out boundingBox));
        Assert.Equal(xmin, boundingBox.Xmin);
        Assert.Equal(ymin, boundingBox.Ymin);
        Assert.Equal(xmax, boundingBox.Xmax);
        Assert.Equal(ymax, boundingBox.Ymax);
    }
}