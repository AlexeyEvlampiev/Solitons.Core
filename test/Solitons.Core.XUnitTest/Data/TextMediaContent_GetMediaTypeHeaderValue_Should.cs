using Xunit;

namespace Solitons.Data;

// ReSharper disable once InconsistentNaming
public sealed class TextMediaContent_GetMediaTypeHeaderValue_Should
{
    [Fact]
    public void ReturnsMediaTypeHeaderValueWhenValid()
    {
        // Arrange
        var content = new TextMediaContent("Sample Content", "application/json");

        // Act
        var mediaTypeHeaderValue = content.GetMediaTypeHeaderValue();

        // Assert
        Assert.NotNull(mediaTypeHeaderValue);
        Assert.Equal("application/json", mediaTypeHeaderValue!.MediaType);
    }

    [Fact]
    public void ReturnsNullWhenNotValid()
    {
        // Arrange
        var content = new TextMediaContent("Sample Content", "invalid content type");

        // Act
        var mediaTypeHeaderValue = content.GetMediaTypeHeaderValue();

        // Assert
        Assert.Null(mediaTypeHeaderValue);
    }

    [Fact]
    public void ReturnsMediaTypeHeaderValueWithEncoding()
    {
        // Arrange
        var content = new TextMediaContent("Sample Content", "text/plain; charset=utf-8");

        // Act
        var mediaTypeHeaderValue = content.GetMediaTypeHeaderValue();

        // Assert
        Assert.NotNull(mediaTypeHeaderValue);
        Assert.Equal("text/plain", mediaTypeHeaderValue!.MediaType);
        Assert.Equal("utf-8", mediaTypeHeaderValue.CharSet);
    }
}