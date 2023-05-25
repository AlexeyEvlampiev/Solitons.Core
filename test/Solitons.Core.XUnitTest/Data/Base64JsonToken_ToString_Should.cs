using System.Text.Json.Serialization;
using Xunit;

namespace Solitons.Data;

// ReSharper disable once InconsistentNaming
public sealed class Base64JsonToken_ToString_Should
{
    [Fact]
    public void Work()
    {
        var token = new TestToken() { Text = "test" };
        var base64 = token.ToString();
        var clone = TestToken.Parse(base64);

        Assert.Equal("test", clone.Text);
    }

    public sealed record TestToken : Base64JsonToken
    {
        [JsonPropertyName("txt")] public string Text { get; init; } = string.Empty;

        public static TestToken Parse(string base64) => Parse<TestToken>(base64);
    }
}