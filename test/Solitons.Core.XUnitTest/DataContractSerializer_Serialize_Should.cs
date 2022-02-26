using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Xunit;

namespace Solitons
{
    // ReSharper disable once InconsistentNaming
    public class DataContractSerializer_Serialize_Should
    {
        [Theory]
        [InlineData(typeof(JsonFirstTestSerialize), "application/json")]
        [InlineData(typeof(XmlFirstTestSerialize), "application/xml")]
        public void SupportHandleDefaultContentType(Type serializerType, string expectedDefaultContentType)
        {
            var target = (DataContractSerializer)Activator.CreateInstance(serializerType);
            var content = target!.Serialize(new MyClass() { Text = "This is a test" }, out var actualContentType);
            Assert.Equal(expectedDefaultContentType, actualContentType);
            var clone = target.Deserialize<MyClass>(expectedDefaultContentType, content);
            Assert.Equal("This is a test", clone.Text);
        }

        [XmlRoot("MyData")]
        public sealed class MyClass
        {
            [JsonPropertyName("txt")]
            [XmlAttribute("Text")]
            public string Text { get; set; }
        }

        public sealed class JsonFirstTestSerialize : DataContractSerializer
        {
            public JsonFirstTestSerialize()
            {
                Register(typeof(MyClass), IMediaTypeSerializer.BasicJsonSerializer);
                Register(typeof(MyClass), IMediaTypeSerializer.BasicXmlSerializer);
            }
        }

        public sealed class XmlFirstTestSerialize : DataContractSerializer
        {
            public XmlFirstTestSerialize()
            {
                Register(typeof(MyClass), IMediaTypeSerializer.BasicXmlSerializer);
                Register(typeof(MyClass), IMediaTypeSerializer.BasicJsonSerializer);
            }
        }
    }
}
