﻿using Xunit;

namespace Solitons.Data
{
    // ReSharper disable once InconsistentNaming
    public sealed class DataContractSerializer_Pack_Should
    {
        [Fact]
        public void Pack()
        {
            var dto = new MyDto() { Text = "This is a test" };
            var serializer = new MySerializer();
            var package = serializer.Pack(dto);
            var clone = (MyDto)serializer.Unpack(package);
            Assert.Equal(dto.Text, clone.Text);
        }

        public sealed class MyDto : BasicJsonDataTransferObject
        {
            public string Text { get; set; }
        }

        public sealed class MySerializer : DataContractSerializer
        {
            public MySerializer() : base(DataContractSerializerBehaviour.Default)
            {
                Register(typeof(MyDto), IMediaTypeSerializer.BasicJsonSerializer);
            }
        }
    }
}
