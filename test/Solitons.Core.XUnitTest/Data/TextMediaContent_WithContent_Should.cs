using Xunit;

namespace Solitons.Data;

// ReSharper disable once InconsistentNaming
public sealed class TextMediaContent_WithContent_Should
{
    [Fact]
    public void CreateNewTextMediaContentWithUpdatedContent()
    {
        // Arrange
        var originalTextMediaContent = new TextMediaContent("Original Content", "text/plain");
        string updatedContent = "Updated Content";

        // Act
        var newTextMediaContent = originalTextMediaContent.WithContent(updatedContent);

        // Assert
        Assert.Equal(updatedContent, newTextMediaContent.Content);
        Assert.Equal(originalTextMediaContent.ContentType, newTextMediaContent.ContentType);
    }
}