// ReSharper disable InconsistentNaming

using Solitons.Web.Common;
using Xunit;

namespace Solitons.Web
{
    public sealed class UriParser_ToResourceQueryStringPair_Should
    {
        [Theory]
        [InlineData("images/123?size=large", "images/123", "size=large")]
        [InlineData("images/123?greeting=Hello world!", "images/123", "greeting=Hello world!")]
        public void Work(string uri, string expectedResource, string expectedQuery)
        {
            var target = (IUriParser) new UriParser();
            Assert.True(target.TryParse(uri, out var actualResource, out var actualQueryString));
            Assert.Equal(expectedResource, actualResource);
            Assert.Equal(expectedQuery, actualQueryString);
        }
    }
}
