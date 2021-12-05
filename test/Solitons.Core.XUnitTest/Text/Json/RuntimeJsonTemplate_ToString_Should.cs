using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;
// ReSharper disable All

namespace Solitons.Text.Json
{
    public class RuntimeJsonTemplate_ToString_Should
    {
        [Fact]
        public void Transform()
        {
            var target = new SampleRuntimeJsonTemplate();
            var json = target.ToString();
            var dto = JsonSerializer.Deserialize<Dto>(json);
            Assert.True(dto.Boolean);
        }

        [Fact]
        public void AutoCorrect()
        {
            var target = new AutoCorrectedJsonTemplate();
            var json = target.ToString();
            var dto = JsonSerializer.Deserialize<object>(json);
        }

        public sealed class Dto
        {
            [JsonPropertyName("boolean")]
            public bool Boolean { get; set; }
        }
    }
}
