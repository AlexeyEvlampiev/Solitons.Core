using System.Runtime.InteropServices;
using System.Xml.Serialization;
using Xunit;

namespace Solitons
{
    public class DomainSerializer_Should
    {
        [Guid("32209525-9afd-4d32-bbe8-0f277b5d86e3")]
        public sealed class BasicXmlDataTransferTestObject : IBasicXmlDataTransferObject
        {
            [XmlAttribute("Text")]
            public string Text { get; set; }
        }


        [Guid("32209525-9afd-4d32-bbe8-0f277b5d86e3")]
        public sealed class BasicJsonDataTransferTestObject : IBasicJsonDataTransferObject
        {
            [XmlAttribute("Text")]
            public string Text { get; set; }
        }

        [Fact]
        public void SupportBasicXmlDataTransferObjects()
        {
            var xmlOnlyObj = new BasicXmlDataTransferTestObject()
            {
                Text = "This is a test"
            };

            var serializer = IDomainSerializer.FromTypes(xmlOnlyObj.GetType());

            Assert.True(serializer.CanSerialize(xmlOnlyObj, out var contentType));
            Assert.Equal("application/xml", contentType);
            Assert.True(serializer.CanSerialize(xmlOnlyObj, contentType));
            Assert.True(serializer.CanDeserialize(xmlOnlyObj.GetType().GUID, contentType));

            Assert.False(serializer.CanSerialize(xmlOnlyObj, "application/json"));


            var xml = serializer.Serialize(xmlOnlyObj, contentType);
            var clone = (BasicXmlDataTransferTestObject)serializer.Deserialize(xmlOnlyObj.GetType().GUID, "application/xml", xml);
            Assert.Equal("This is a test", clone.Text);

            var package = serializer.Pack(xmlOnlyObj, out contentType);
            Assert.Equal("application/xml", contentType);
            clone = (BasicXmlDataTransferTestObject)serializer.Unpack(package);
            Assert.Equal("This is a test", clone.Text);


        }


    }
}
