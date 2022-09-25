using System;
using Xunit;

namespace Solitons.Text
{
    // ReSharper disable once InconsistentNaming
    public sealed class ContainerImageRefFormatter_Format_Should
    {
        [Theory]
        [InlineData(null, "solitons.webapp", null, "solitons.webapp")]
        [InlineData(null, "solitons.backend", null, "solitons.backend")]
        [InlineData("", "solitons.backend", "", "solitons.backend")]
        [InlineData("docker.io", "solitons.backend", "", "docker.io/solitons.backend")]
        [InlineData("docker.io/", "solitons.backend", "", "docker.io/solitons.backend")]
        [InlineData(" docker.io / ", "solitons.backend", "", "docker.io/solitons.backend")]
        [InlineData(null, "solitons.backend", "latest", "solitons.backend:latest")]
        [InlineData(null, "solitons.backend", ":latest", "solitons.backend:latest")]
        [InlineData(null, "solitons.backend", " : latest ", "solitons.backend:latest")]
        [InlineData("docker.io", "solitons.backend", ":latest", "docker.io/solitons.backend:latest")]
        public void Work(string registry, string image, string tag, string expected)
        {
            var target = new ContainerImageRefFormatter()
            {
                Registry = registry,
                Tag = tag
            };

            var actual = target.Format(image);
            Assert.Equal(expected, actual, StringComparer.Ordinal);
        }
    }
}
