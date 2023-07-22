using System;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Solitons.Net.Http;

// ReSharper disable once InconsistentNaming
public sealed class HttpResponseMessageCloneFactory_FromAsync_Should
{
    [Fact]
    public async Task CreateWorkingFactory()
    {
        // Arrange
        var originalMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("OK", Encoding.UTF8, "application/json")
        };

        // Act
        var cloneFactory = await HttpResponseMessageCloneFactory.FromAsync(originalMessage);
        var clonedMessage = cloneFactory.Create();

        // Assert
        Assert.Equal(originalMessage.StatusCode, clonedMessage.StatusCode);
        Assert.Equal(originalMessage.Version, clonedMessage.Version);
        Assert.Equal(await originalMessage.Content.ReadAsStringAsync(), await clonedMessage.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task CreateFactoryCloningHeaders()
    {
        // Arrange
        var originalMessage = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent("OK", Encoding.UTF8, "application/json"),
            Version = new Version(1, 1)
        };
        originalMessage.Headers.Add("X-Test-Header", "Test Value");
        originalMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");

        // Act
        var cloneFactory = await HttpResponseMessageCloneFactory.FromAsync(originalMessage);
        var clonedMessage = cloneFactory.Create();

        // Assert
        Assert.Equal(originalMessage.StatusCode, clonedMessage.StatusCode);
        Assert.Equal(originalMessage.Version, clonedMessage.Version);
        Assert.Equal(originalMessage.Headers.GetValues("X-Test-Header").FirstOrDefault(), clonedMessage.Headers.GetValues("X-Test-Header").FirstOrDefault());
        Assert.Equal(originalMessage.Content.Headers.ContentType, clonedMessage.Content.Headers.ContentType);
        Assert.Equal(await originalMessage.Content.ReadAsStringAsync(), await clonedMessage.Content.ReadAsStringAsync());
    }
}