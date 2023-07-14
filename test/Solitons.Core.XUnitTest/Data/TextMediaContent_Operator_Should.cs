using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xunit;

namespace Solitons.Data;

// ReSharper disable once InconsistentNaming
public class TextMediaContent_Operator_Should
{

    [Fact]
    public void ImplicitlyConvertToHttpContentWithCorrectEncodingAndContentType()
    {
        // Arrange
        string content = "Hello, world!";
        string contentType = "text/plain";
        var textMediaContent = new TextMediaContent(content, contentType);

        // Act
        HttpContent httpContent = textMediaContent;

        // Assert
        var stringContent = Assert.IsType<StringContent>(httpContent);
        Assert.Equal(content, stringContent.ReadAsStringAsync().Result);
        Assert.Equal(Encoding.UTF8, Encoding.GetEncoding(stringContent.Headers.ContentType?.CharSet!));
        Assert.Equal(contentType, stringContent.Headers.ContentType?.MediaType);
    }

    [Fact]
    public void ImplicitlyConvertToContentString()
    {
        // Arrange
        string content = "Hello, world!";
        var textMediaContent = new TextMediaContent(content, "text/plain");

        // Act
        string result = textMediaContent;

        // Assert
        Assert.Equal(content, result);
    }


    [Fact]
    public void ImplicitlyConvertToContentMediaTypeHeaderValue()
    {
        // Arrange
        string content = "Hello, world!";
        var textMediaContent = new TextMediaContent(content, "text/plain");

        // Act
        MediaTypeHeaderValue result = textMediaContent;

        // Assert
        Assert.Equal("text/plain", result.MediaType);
    }
}