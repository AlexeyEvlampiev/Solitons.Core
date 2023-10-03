using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Solitons.Net.Http;

public sealed class HttpMessageBinaryFormatterTest
{
    [Fact]
    public async Task SerializeDeserializeRequestTest()
    {
        // Arrange
        var originalRequest = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
        originalRequest.Headers.Add("X-Test", "TestValue");
        originalRequest.Content = new StringContent("Test Content");
        originalRequest.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

        var stream = new MemoryStream();

        // Act
        await HttpMessageBinaryFormatter.WriteAsync(originalRequest, stream);
        stream.Position = 0;
        var deserializedRequest = await HttpMessageBinaryFormatter.ReadRequestAsync(stream);

        // Assert
        Assert.Equal(originalRequest.Method, deserializedRequest.Method);
        Assert.Equal(originalRequest.RequestUri, deserializedRequest.RequestUri);
        Assert.Equal(originalRequest.Headers.ToString(), deserializedRequest.Headers.ToString());
        Assert.Equal(await originalRequest.Content.ReadAsStringAsync(), await deserializedRequest.Content!.ReadAsStringAsync());
    }

    [Fact]
    public async Task SerializeDeserializeResponseTest()
    {
        // Arrange
        var originalResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("Test Content"),
            ReasonPhrase = "OK"
        };
        originalResponse.Headers.Add("X-Test", "TestValue");
        originalResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/plain");

        var stream = new MemoryStream();

        // Act
        await HttpMessageBinaryFormatter.WriteAsync(originalResponse, stream);
        stream.Position = 0;
        var deserializedResponse = await HttpMessageBinaryFormatter.ReadResponseAsync(stream);

        // Assert
        Assert.Equal(originalResponse.StatusCode, deserializedResponse.StatusCode);
        Assert.Equal(originalResponse.ReasonPhrase, deserializedResponse.ReasonPhrase);
        Assert.Equal(originalResponse.Headers.ToString(), deserializedResponse.Headers.ToString());
        var actualContent = await deserializedResponse.Content.ReadAsStringAsync();
        Assert.Equal("Test Content", actualContent);
    }

    [Fact]
    public async Task DeserializeEmptyRequestTest()
    {
        // Arrange
        var emptyRequest = new HttpRequestMessage();
        var stream = new MemoryStream();

        // Act
        await HttpMessageBinaryFormatter.WriteAsync(emptyRequest, stream);
        stream.Position = 0;
        var deserializedRequest = await HttpMessageBinaryFormatter.ReadRequestAsync(stream);

        // Assert
        Assert.Equal(emptyRequest.Method, deserializedRequest.Method);
        Assert.Equal(emptyRequest.RequestUri, deserializedRequest.RequestUri);
        Assert.Equal(emptyRequest.Headers.Count(), deserializedRequest.Headers.Count());
        Assert.Null(deserializedRequest.Content);
    }

    [Fact]
    public async Task DeserializeEmptyResponseTest()
    {
        // Arrange
        var emptyResponse = new HttpResponseMessage();
        var stream = new MemoryStream();

        // Act
        await HttpMessageBinaryFormatter.WriteAsync(emptyResponse, stream);
        stream.Position = 0;
        var deserializedResponse = await HttpMessageBinaryFormatter.ReadResponseAsync(stream);

        // Assert
        Assert.Equal(emptyResponse.StatusCode, deserializedResponse.StatusCode);
        Assert.Equal(emptyResponse.ReasonPhrase, deserializedResponse.ReasonPhrase);
        Assert.Equal(emptyResponse.Headers.Count(), deserializedResponse.Headers.Count());
    }

    [Fact]
    public async Task SerializeRequestWithMultipleHeadersTest()
    {
        // Arrange
        var originalRequest = new HttpRequestMessage(HttpMethod.Get, "http://localhost");
        originalRequest.Headers.Add("X-Test1", new List<string> { "Value1", "Value2" });
        originalRequest.Headers.Add("X-Test2", new List<string> { "Value3", "Value4" });
        var stream = new MemoryStream();

        // Act
        await HttpMessageBinaryFormatter.WriteAsync(originalRequest, stream);
        stream.Position = 0;
        var deserializedRequest = await HttpMessageBinaryFormatter.ReadRequestAsync(stream);

        // Assert
        Assert.Equal(originalRequest.Headers.Count(), deserializedRequest.Headers.Count());
        Assert.True(deserializedRequest.Headers.Contains("X-Test1"));
        Assert.True(deserializedRequest.Headers.Contains("X-Test2"));
    }

    [Fact]
    public async Task SerializeResponseWithMultipleHeadersTest()
    {
        // Arrange
        var originalResponse = new HttpResponseMessage(HttpStatusCode.OK);
        originalResponse.Headers.Add("X-Test1", new List<string> { "Value1", "Value2" });
        originalResponse.Headers.Add("X-Test2", new List<string> { "Value3", "Value4" });
        var stream = new MemoryStream();

        // Act
        await HttpMessageBinaryFormatter.WriteAsync(originalResponse, stream);
        stream.Position = 0;
        var deserializedResponse = await HttpMessageBinaryFormatter.ReadResponseAsync(stream);

        // Assert
        Assert.Equal(originalResponse.Headers.Count(), deserializedResponse.Headers.Count());
        Assert.True(deserializedResponse.Headers.Contains("X-Test1"));
        Assert.True(deserializedResponse.Headers.Contains("X-Test2"));
    }

    [Fact]
    public async Task DeserializeCorruptedStreamTest()
    {
        // Arrange
        var stream = new MemoryStream();
        var writer = new BinaryWriter(stream);
        writer.Write("This is not a valid serialized HttpRequestMessage");
        stream.Position = 0;

        // Act & Assert
        await Assert.ThrowsAsync<HttpMessageBinaryFormatterException>(() => HttpMessageBinaryFormatter.ReadRequestAsync(stream));
    }
}