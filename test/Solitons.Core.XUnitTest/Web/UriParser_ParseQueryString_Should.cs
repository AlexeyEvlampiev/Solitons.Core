using System.Linq;
using Solitons.Web.Common;
using Xunit;

namespace Solitons.Web
{
    public sealed class UriParser_ParseQueryString_Should
    {
        [Theory]
        [InlineData("?a=1&b=2&c=3")]
        [InlineData("a=1&b=2&c=3")]
        [InlineData("   ?a=1&b=2&c=3   ")]
        public void Work(string variant)
        {
            Assert.True(UriParser.TryParseQueryString(variant, out var parameters));
            var parts = parameters
                .ToDictionary(p=>p.Key, p=> int.Parse(p.Value));
            Assert.Equal(3, parts.Count);
            Assert.Equal(1, parts["a"]);
            Assert.Equal(2, parts["b"]);
            Assert.Equal(3, parts["c"]);
        }
    }
}
