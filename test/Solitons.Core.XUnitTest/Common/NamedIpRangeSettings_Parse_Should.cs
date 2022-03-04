// ReSharper disable InconsistentNaming

using System.Diagnostics;
using Xunit;

namespace Solitons.Common
{
    public sealed class NamedIpRangeSettings_Parse_Should
    {
        [Theory]
        [InlineData("name=test;start=204.120.0.10;end=204.120.0.15")]
        [InlineData("test;204.120.0.10;204.120.0.15")]
        [InlineData("id=test;from=204.120.0.10;to=204.120.0.15")]
        [InlineData("id=test;start=204.120.0.10;to=204.120.0.15")]
        [InlineData("id=test;from=204.120.0.10;end=204.120.0.15")]
        [InlineData("name=test;range=204.120.0.10-204.120.0.15")]
        [InlineData("name=test;range=204.120.0.10 - 204.120.0.15")]
        [InlineData("name=test;204.120.0.10-204.120.0.15")]
        [InlineData("name=test;204.120.0.10 - 204.120.0.15")]
        public void HandleValidRangeFormats(string input)
        {
            var expectedRange = new NamedIpRangeSettingsGroup("test", "204.120.0.10", "204.120.0.15");

            var actualRange = NamedIpRangeSettingsGroup.Parse(input);
            Assert.Equal(expectedRange, actualRange);

            var actualString = actualRange.ToString();
            actualRange = NamedIpRangeSettingsGroup.Parse(actualString);
            Assert.Equal(expectedRange, actualRange);
        }

        [Theory]
        [InlineData("name=test;start=204.120.0.10")]
        [InlineData("test;204.120.0.10")]
        [InlineData("test;204.120.0.10;")]
        [InlineData("name=test;from=204.120.0.10")]
        public void HandleValidAddressFormats(string input)
        {
            Debug.WriteLine(input);
            var expectedRange = new NamedIpRangeSettingsGroup("test", "204.120.0.10");

            var actualRange = NamedIpRangeSettingsGroup.Parse(input);
            Assert.Equal(expectedRange, actualRange);

            var actualString = actualRange.ToString();
            actualRange = NamedIpRangeSettingsGroup.Parse(actualString);
            Assert.Equal(expectedRange, actualRange);
        }
    }
}
