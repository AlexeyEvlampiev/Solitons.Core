using System;
using System.Net;
using Moq;
using Xunit;

namespace Solitons.Web
{
    public sealed class RSASecureAccessSignatureService_SignQueryString_Should
    {
        [Theory]
        [InlineData("mid=3cdc0a6e-50bc-4b96-8297-37ee753bf0a8", "2022-01-01", null, null, null)]
        [InlineData("mid=3cdc0a6e-50bc-4b96-8297-37ee753bf0a8", "2022-01-01", "2022-01-02", null, null)]
        [InlineData("mid=3cdc0a6e-50bc-4b96-8297-37ee753bf0a8", "2022-01-01", "2022-01-03", "192.0.2.1", null)]
        [InlineData("mid=3cdc0a6e-50bc-4b96-8297-37ee753bf0a8", "2022-01-01", "2022-01-19", "192.0.2.1", "192.0.3.1")]
        public void Sing(
            string queryString, 
            string startTimeString, 
            string expiryTimeString, 
            string startAddressString, 
            string endAddressString)
        {
            var startTime = startTimeString.Convert(DateTime.Parse);
            var expiryTime = expiryTimeString?.Convert(DateTime.Parse);
            var startAddress = startAddressString?.Convert(IPAddress.Parse);
            var endAddress = endAddressString?.Convert(IPAddress.Parse);

            var clock = new Mock<IClock>();
            clock.SetupGet(i => i.UtcNow).Returns(DateTimeOffset.Parse("2022-01-01"));

            ISecureAccessSignatureService target = new RSASecureAccessSignatureService(
                new System.Security.Cryptography.RSACryptoServiceProvider(), 
                clock.Object);
            var signedQueryString = target.SignQueryString(queryString, startTime, expiryTime, startAddress, endAddress);
            Assert.True(target.IsGenuineQueryString(
                signedQueryString, 
                out var actualStartTime,
                out var actualExpiry, 
                out var actualStartAddress, 
                out var actualEndAddress));
            Assert.Equal(actualStartTime, startTime);
        }
    }
}
