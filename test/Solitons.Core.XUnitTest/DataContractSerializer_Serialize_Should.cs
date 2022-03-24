using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Solitons.Data;
using Xunit;

namespace Solitons
{
    // ReSharper disable once InconsistentNaming
    public class DataContractSerializer_Serialize_Should
    {
        [Fact]
        public void SupportJsonFirstSerialization()
        {
            var target = IDataContractSerializer
                .CreateBuilder()
                .RequireCustomGuidAnnotation(false)
                .Add(typeof(MyClass), 
                    IMediaTypeSerializer.BasicJsonSerializer, 
                    IMediaTypeSerializer.BasicXmlSerializer)
                .Build();
            var content = target.Serialize(new MyClass() { Text = "This is a test" }, out var actualContentType);
            Assert.Equal("application/json", actualContentType);
            var clone = target.Deserialize<MyClass>("application/json", content);
            Assert.Equal("This is a test", clone.Text);
            Assert.True(target.CanSerialize(typeof(MyClass), "application/xml"));
        }

        [Fact]
        public void SupportXmlFirstSerialization()
        {
            var target = IDataContractSerializer
                .CreateBuilder()
                .RequireCustomGuidAnnotation(false)
                .Add(typeof(MyClass),
                    IMediaTypeSerializer.BasicXmlSerializer,
                    IMediaTypeSerializer.BasicJsonSerializer)
                .Build();
            var content = target.Serialize(new MyClass() { Text = "This is a test" }, out var actualContentType);
            Assert.Equal("application/xml", actualContentType);
            var clone = target.Deserialize<MyClass>("application/xml", content);
            Assert.Equal("This is a test", clone.Text);
            Assert.True(target.CanSerialize(typeof(MyClass), "application/json"));
        }

        [XmlRoot("MyData")]
        public sealed class MyClass
        {
            [JsonPropertyName("txt")]
            [XmlAttribute("Text")]
            public string Text { get; set; }
        }

    }
}
