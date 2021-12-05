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
            var looseWithoutBrakets = new Regex(RegexPatterns.Uuid.LooseWithoutBrakets);
            var loose = new Regex(RegexPatterns.Uuid.Loose);
            Assert.True(looseWithoutBrakets.IsMatch(guid.ToString("N")));
            Assert.True(looseWithoutBrakets.IsMatch(guid.ToString("D")));

            Assert.True(loose.IsMatch(guid.ToString("N")));
            Assert.True(loose.IsMatch(guid.ToString("D")));
            Assert.True(loose.IsMatch(guid.ToString("B")));
            Assert.True(loose.IsMatch(guid.ToString("P")));
        }
    }
}
