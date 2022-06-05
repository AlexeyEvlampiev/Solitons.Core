using System;
using System.Text;
using Moq;
using Xunit;

namespace Solitons.Data
{
    // ReSharper disable once InconsistentNaming
    public sealed class DataTransferPackage_ToString_Should
    {
        [Fact]
        public void ReturnValidJsonRepresentation()
        {
            var fixedClock = new Mock<IClock>();
            fixedClock
                .Setup(i => i.UtcNow)
                .Returns(DateTimeOffset.Parse("2022-01-01"));

            var package = new DataTransferPackage(
                Guid.Parse("148b347e572148af8b91ae84cb04b70b"),
                "Content goes here",
                "text/plain", 
                Encoding.ASCII)
            {
                TimeToLive = TimeSpan.FromMinutes(123), 
                CorrelationId = "Some correlation ID", 
                From = "Some from- address",
                To = "Some to- address", 
                ReplyTo = "Reply to- address",
                ReplyToSessionId = "Reply to session ID", 
                Signature = "Signature goes here".ToUtf8Bytes(), 
                SessionId = "Some session ID", 
                MessageId = "Some message ID", 
                TransactionTypeId = Guid.Parse("50dae5eb5b7d4f0aa71c6c445705c651")
            };


            package.Properties.Add("Some property key", "Some property value");

            var packageString = package.ToString(fixedClock.Object);
            var clone = DataTransferPackage.Parse(packageString, fixedClock.Object);
            Assert.Equal(Guid.Parse("148b347e572148af8b91ae84cb04b70b"), clone.TypeId);
            Assert.Equal(Guid.Parse("50dae5eb5b7d4f0aa71c6c445705c651"), clone.TransactionTypeId);
            Assert.Equal("Some correlation ID", clone.CorrelationId);
            Assert.Equal("Some from- address", clone.From);
            Assert.Equal("Some to- address", clone.To);
            Assert.Equal("Reply to- address", clone.ReplyTo);
            Assert.Equal("Reply to session ID", clone.ReplyToSessionId);
            Assert.Equal("Signature goes here", clone.Signature?.ToString(Encoding.ASCII));
            Assert.Equal("Some session ID", clone.SessionId);
            Assert.Equal("Some message ID", clone.MessageId);
            Assert.Equal(TimeSpan.FromMinutes(123), clone.TimeToLive);

            Assert.Equal(1, clone.Properties.Count);
            Assert.Equal(clone.Properties["Some property key"], "Some property value");
        }
    }
}
