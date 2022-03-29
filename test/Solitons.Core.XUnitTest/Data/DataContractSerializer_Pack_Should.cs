using System;
using System.Runtime.InteropServices;
using Xunit;

namespace Solitons.Data
{
    // ReSharper disable once InconsistentNaming
    public sealed class DataContractSerializer_Pack_Should
    {
        [Fact]
        public void PackPlainDto()
        {
            var dto = new MyDto() { Text = "This is a test" };
            var serializer = IDataContractSerializer
                .CreateBuilder()
                .RequireCustomGuidAnnotation(false)
                .Add(typeof(MyDto), IMediaTypeSerializer.BasicJsonSerializer)
                .Build();
            var expectedCommandId = Guid.Parse("4b957593-43b3-4c48-be57-fd8b079699b9");
            var package = serializer.Pack(dto, expectedCommandId);
            var clone = (MyDto)serializer.Unpack(package, out var actualCommandId);
            Assert.Equal(dto.Text, clone.Text);
            Assert.Equal(expectedCommandId, actualCommandId);
        }

        [Fact]
        public void PackExplicitCommandArgs()
        {
            var dto = new ExplicitCommandArgs() { Value = 321 };
            var serializer = IDataContractSerializer
                .CreateBuilder()
                .RequireCustomGuidAnnotation(false)
                .Add(typeof(ExplicitCommandArgs), IMediaTypeSerializer.BasicJsonSerializer)
                .Build();
            var expectedCommandId = ((ICommandArgs)dto).CommandId;
            var package = serializer.Pack(dto);
            var clone = (ExplicitCommandArgs)serializer.Unpack(package, out var actualCommandId);
            Assert.Equal(dto.Value, clone.Value);
            Assert.Equal(expectedCommandId, actualCommandId);
        }

        [Fact]
        public void PackImplicitCommandArgs()
        {
            var dto = new ImplicitCommandArgs() { Value = 54321 };
            var serializer = IDataContractSerializer
                .CreateBuilder()
                .RequireCustomGuidAnnotation(false)
                .Add(typeof(ImplicitCommandArgs), IMediaTypeSerializer.BasicJsonSerializer)
                .Build();
            var package = serializer.Pack(dto);
            var clone = (ImplicitCommandArgs)serializer.Unpack(package, out var actualCommandId);
            Assert.Equal(dto.Value, clone.Value);
            Assert.Equal(dto.GetType().GUID, actualCommandId);
        }

        public sealed class MyDto : BasicJsonDataTransferObject
        {
            public string Text { get; set; }
        }

        [Guid("b6fd4e7d-140a-44e1-b692-27ba49e92f6f")]
        public sealed class ExplicitCommandArgs : BasicJsonDataTransferObject, ICommandArgs
        {
            Guid ICommandArgs.CommandId => Guid.Parse("07041ff2319d48ada77e62cb4eb086b7");

            public int Value { get; set; }
        }

        public sealed class ImplicitCommandArgs : BasicXmlDataTransferObject, ICommandArgs
        {
            public int Value { get; set; }
        }

    }
}
