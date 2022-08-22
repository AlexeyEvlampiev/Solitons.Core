using System;
using System.Linq;
using System.Text;
using Xunit;

namespace Solitons
{
    // ReSharper disable once InconsistentNaming
    public class Extensions_String_IsBase64_Should
    {
        [Fact]
        public void DetectValidBase64()
        {
            var input = "This is a test";
            var samples =
                Enumerable.Range(0, input.Length)
                    .Select(len => input.Substring(0, len))
                    .Select(s => Encoding.UTF8.GetBytes(s))
                    .Select(Convert.ToBase64String);
            foreach (var sample in samples)
            {
                Assert.True(sample.IsBase64String());
            }
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("123")]
        [InlineData("12345")]
        public void DetectInvalidBase64(string sample)
        {
            Assert.False(sample.IsBase64String());
        }
    }
}
