using System.Linq;
using System.Security.Cryptography;
using Xunit;

namespace Solitons.Security.Cryptography
{
    // ReSharper disable once InconsistentNaming
    public sealed class AesCryptographyService_Basic_Should
    {
        [Theory]
        [InlineData("a")]
        [InlineData("Hello world!")]
        [InlineData("To be or not to be")]
        public void CreateBasicService(string expected)
        {
            var target = AesCryptographyService.Basic();
            var encrypted = target.Encrypt(expected.ToUtf8Bytes());
            var decrypted = target.Decrypt(encrypted).ToUtf8String();
            Assert.Equal(expected, decrypted);
        }

        [Fact]
        public void CreateBasicService2()
        {
            var target = AesCryptographyService.Basic();
            var expected = RandomNumberGenerator.GetBytes(10000);
            var encrypted = target.Encrypt(expected);
            var decrypted = target.Decrypt(encrypted);
            Assert.True(expected.SequenceEqual( decrypted));
        }
    }
}
