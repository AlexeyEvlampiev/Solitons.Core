using System;
using System.Runtime.InteropServices;
using Moq;
using Xunit;

namespace Solitons.Data
{
    // ReSharper disable once InconsistentNaming
    public sealed class DataContractSerializer_Pack_Should
    {
        [Fact]
        public void PackPlainDto()
        {
            var clock = new Mock<IClock>();
            clock
                .Setup(i => i.UtcNow)
                .Returns(DateTimeOffset.Parse("2022-01-01"));

            var dto = new MyDto() { Text = "This is a test" };
            var serializer = IDataContractSerializer
                .CreateBuilder()
                .RequireCustomGuidAnnotation(false)
                .Add(typeof(MyDto), IMediaTypeSerializer.BasicJsonSerializer)
                .Build();

            var expectedTransactionTypeId = Guid.Parse("4b957593-43b3-4c48-be57-fd8b079699b9");
            

            var package = serializer.Pack(dto);

            package.TimeToLive = TimeSpan.FromMinutes(10);

            var clone = (MyDto)serializer.Unpack(package);
            Assert.Equal(dto.Text, clone.Text);

        }

        [Fact]
        public void PackExplicitCommandArgs()
        {
            var dto = new ExplicitTransactionArgs() { Value = 321 };
            var serializer = IDataContractSerializer
                .CreateBuilder()
                .RequireCustomGuidAnnotation(false)
                .Add(typeof(ExplicitTransactionArgs), IMediaTypeSerializer.BasicJsonSerializer)
                .Build();
            var expectedCommandId = ((IDistributedEventArgs)dto).IntentId;
            var package = serializer.Pack(dto);
            var clone = (ExplicitTransactionArgs)serializer.Unpack(package);
            Assert.Equal(dto.Value, clone.Value);
        }

        [Fact]
        public void PackImplicitCommandArgs()
        {
            var dto = new ImplicitTransactionArgs() { Value = 54321 };
            var serializer = IDataContractSerializer
                .CreateBuilder()
                .RequireCustomGuidAnnotation(false)
                .Add(typeof(ImplicitTransactionArgs), IMediaTypeSerializer.BasicJsonSerializer)
                .Build();
            var package = serializer.Pack(dto);
            var clone = (ImplicitTransactionArgs)serializer.Unpack(package);
            Assert.Equal(dto.Value, clone.Value);
        }

        public sealed class MyDto :
            BasicJsonDataTransferObject,
            IDistributedEventArgs
        {
            public string Text { get; set; }
        }

        [Guid("b6fd4e7d-140a-44e1-b692-27ba49e92f6f")]
        public sealed class ExplicitTransactionArgs : BasicJsonDataTransferObject, IDistributedEventArgs
        {
            Guid IDistributedEventArgs.IntentId => Guid.Parse("07041ff2319d48ada77e62cb4eb086b7");

            public int Value { get; set; }
        }

        public sealed class ImplicitTransactionArgs : BasicXmlDataTransferObject, IDistributedEventArgs
        {
            public int Value { get; set; }
        }

    }
}
