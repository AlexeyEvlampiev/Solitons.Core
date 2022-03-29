using System;
using Xunit;

namespace Solitons.Data
{
    // ReSharper disable once InconsistentNaming
    public sealed class DataContractSerializer_Pack_Should
    {
        [Fact]
        public void Pack()
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

        public sealed class MyDto : BasicJsonDataTransferObject
        {
            public string Text { get; set; }
        }

    }
}
