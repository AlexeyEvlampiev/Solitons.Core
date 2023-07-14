using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Solitons.Data;

// ReSharper disable once InconsistentNaming
public sealed class TextMediaContent_ToHttpContent_Should
{
    [Fact]
    public async Task ReturnStringContentWithCorrectEncodingAndContentType()
    {
        var textMediaContent = TextMediaContent.CreateText("Hello, world!");

        var httpContent = textMediaContent.ToHttpContent();
        var stringContent = Assert.IsType<StringContent>(httpContent);

        Assert.Equal(textMediaContent.Content, await stringContent.ReadAsStringAsync());
        Assert.Equal(Encoding.UTF8, Encoding.GetEncoding(stringContent.Headers.ContentType?.CharSet!));
        Assert.Equal(textMediaContent.ContentType, stringContent.Headers.ContentType?.MediaType);
    }
}