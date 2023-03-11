// ReSharper disable InconsistentNaming

using System;
using System.Text.RegularExpressions;
using Solitons.Text;
using Xunit;

namespace Solitons 
{
    public sealed class RegexPatterns_Should
    {
        [Fact]
        public void MatchGuids()
        {
            var guid = Guid.Parse("23b91b70-808a-4375-abf7-c7c1c1b29706");
            var looseWithoutBraces = new Regex(RegexPatterns.Uuid.LooseFormatNoBraces);
            var loose = new Regex(RegexPatterns.Uuid.LooseFormatWithBraces);
            Assert.True(looseWithoutBraces.IsMatch(guid.ToString("N")));
            Assert.True(looseWithoutBraces.IsMatch(guid.ToString("D")));

            Assert.True(loose.IsMatch(guid.ToString("N")));
            Assert.True(loose.IsMatch(guid.ToString("D")));
            Assert.True(loose.IsMatch(guid.ToString("B")));
            Assert.True(loose.IsMatch(guid.ToString("P")));
        }
    }
}
