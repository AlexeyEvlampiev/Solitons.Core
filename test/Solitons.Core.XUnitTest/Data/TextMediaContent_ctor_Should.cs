using System.Text.Json;
using System.Xml.Linq;
using Xunit;

namespace Solitons.Data;

// ReSharper disable once InconsistentNaming
public sealed class TextMediaContent_ctor_Should
{
    [Fact]
    public void InitializeAllProperties()
    {
        // Arrange
        string content = "Hello, world!";
        string contentType = "text/plain";

        // Act
        var textMediaContent = new TextMediaContent(content, contentType);

        // Assert
        Assert.Equal(content, textMediaContent.Content);
        Assert.Equal(contentType, textMediaContent.ContentType);

        textMediaContent = new TextMediaContent(content);
        Assert.Equal(content, textMediaContent.Content);
        Assert.Equal("text/plain", textMediaContent.ContentType);
    }


    [Fact]
    public void InvokedByFactoryMethods()
    {
        string content = JsonSerializer.Serialize(new{ a= "b" });
        var mediaContent = TextMediaContent.CreateJson(content);
        Assert.Equal(content, mediaContent.Content);
        Assert.Equal("application/json", mediaContent.ContentType);

        content = new XElement("a", new XAttribute("b", "c")).ToString();
        mediaContent = TextMediaContent.CreateXml(content);
        Assert.Equal(content, mediaContent.Content);
        Assert.Equal("application/xml", mediaContent.ContentType);

        mediaContent = TextMediaContent.CreateText("Hello world!");
        Assert.Equal("Hello world!", mediaContent.Content);
        Assert.Equal("text/plain", mediaContent.ContentType);
    }

}