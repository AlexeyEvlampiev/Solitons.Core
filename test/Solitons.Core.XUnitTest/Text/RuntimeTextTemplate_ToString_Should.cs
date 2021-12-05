using System;
using Xunit;
// ReSharper disable All

namespace Solitons.Text
{
    public class RuntimeTextTemplate_ToString_Should
    {
        [Fact]
        public void Transform()
        {
            var target = new SampleRuntimeTextTemplate();
            var actual = target.ToString();
            Assert.Equal("Hello world!", actual, StringComparer.Ordinal);
        }
    }
}
