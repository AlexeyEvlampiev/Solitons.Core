using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Solitons.Data;

// ReSharper disable once InconsistentNaming
public sealed class TextMediaContent_FromHttpContent_Should
{
    [Fact]
    public async Task CreateTextMediaContentWithCorrectContentAndContentType()
    {
        // Arrange
        string content = "Hello, world!";
        string contentType = "text/plain";
        var stringContent = new StringContent(content, Encoding.UTF8, contentType);

        // Act
        var textMediaContent = await TextMediaContent.FromHttpContent(stringContent);

        // Assert
        Assert.Equal(content, textMediaContent.Content);
        Assert.Equal(contentType, textMediaContent.ContentType);
    }

    [Fact]
    public async Task CreateTextMediaContentWithBase64EncodedContentAndCorrectContentType()
    {
        // Arrange
        byte[] bytes = { 0x12, 0x34, 0x56, 0x78 };
        string contentType = "application/octet-stream";
        var binaryContent = new ByteArrayContent(bytes);
        binaryContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

        // Act
        var textMediaContent = await TextMediaContent.FromHttpContent(binaryContent);

        // Assert
        string expectedBase64String = Convert.ToBase64String(bytes);
        Assert.Equal(expectedBase64String, textMediaContent.Content);
        Assert.Equal(contentType, textMediaContent.ContentType);
    }

}