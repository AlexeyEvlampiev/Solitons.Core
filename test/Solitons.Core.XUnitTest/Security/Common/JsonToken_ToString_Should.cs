using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace Solitons.Security.Common
{

    // ReSharper disable once InconsistentNaming
    public sealed class JsonToken_ToString_Should
    {
        [Fact]
        public void Work()
        {
            var token = new TestToken { Id = Guid.Parse("500b0833-94ba-4670-9adb-45277cc25e3e") };

            var tokenString = token.ToString();
            var clone = TestToken.Parse(tokenString);
            Assert.Equal(token.Id, clone.Id);
            Assert.Equal(token, clone);
        }

        public sealed class TestToken : JsonToken
        {
            [JsonPropertyName("uuid")]
            public Guid Id { get; set; }

            public static TestToken Parse(string input) => Parse<TestToken>(input);
        }
    }
}
