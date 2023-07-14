using Xunit;

namespace Solitons.Data;

// ReSharper disable once InconsistentNaming
public sealed class TextMediaContentTests_ToString_Should
{
    [Fact]
    public void ReturnContent()
    {
        // Arrange
        string content = "Hello, world!";
        var textMediaContent = new TextMediaContent(content, "text/plain");

        // Act
        string result = textMediaContent.ToString();

        // Assert
        Assert.Equal(content, result);
    }
}