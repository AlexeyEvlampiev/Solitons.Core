using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Solitons.Common;
using Xunit;

namespace Solitons
{
    public class DomainSerializer_Should
    {
        const string TestDtoTypeId = "32209525-9afd-4d32-bbe8-0f277b5d86e3";




        [Theory]
        [InlineData(typeof(XmlOnlyDto), "application/xml", "application/json")]
        [InlineData(typeof(JsonOnlyDto), "application/json", "application/xml")]
        public void BasicContentSpecificDto(Type dtoType, string supportedContentType, string notSupportedContentType)
        {
            dynamic instance = Activator.CreateInstance(dtoType);
            Assert.NotNull(instance);
            instance.Text = "This is a test";

            var serializer = IDomainContractSerializer.FromType(((object)instance).GetType());

            Assert.True(serializer.CanSerialize((object)instance, out var contentType));
            Assert.Equal(supportedContentType, contentType);
            Assert.True(serializer.CanSerialize(instance, contentType));
            Assert.True(serializer.CanDeserialize(instance.GetType().GUID, contentType));

            Assert.False(serializer.CanSerialize(instance, notSupportedContentType));


            var content = serializer.Serialize(instance, contentType);
            var clone = serializer.Deserialize(instance.GetType().GUID, supportedContentType, content);
            Assert.Equal("This is a test", clone.Text);

            var package = serializer.Pack(instance, out contentType);
            Assert.Equal(supportedContentType, contentType);
            clone = serializer.Unpack(package);
            Assert.Equal("This is a test", clone.Text);

            Assert.Throws<NotSupportedException>(() => serializer.Serialize(instance, notSupportedContentType));
            Assert.Throws<NotSupportedException>(() => serializer.Pack(instance, notSupportedContentType));
        }

        [Fact]
        public void SupportBasicMultiContentDto()
        {
            var instance = new BasicDto()
            {
                Text = "This is a test"
            };

            var serializer = IDomainContractSerializer.FromType(instance.GetType());
            var supportedContentTypes = serializer.GetContentTypes(instance.GetType()).ToHashSet();

            Assert.Equal(2, supportedContentTypes.Count);
            Assert.True(supportedContentTypes.Contains("application/xml"));
            Assert.True(supportedContentTypes.Contains("application/json"));

            foreach (var contentType in supportedContentTypes)
            {
                Assert.True(serializer.CanSerialize(instance, contentType));
                Assert.True(serializer.CanDeserialize(instance.GetType().GUID, contentType));


                var content = serializer.Serialize(instance, contentType);
                var clone = (BasicDto)serializer.Deserialize(instance.GetType().GUID, contentType, content);
                Assert.Equal("This is a test", clone.Text);

                var package = serializer.Pack(instance, contentType);
                Assert.Equal(contentType, contentType);
                clone = (BasicDto)serializer.Unpack(package);
                Assert.Equal("This is a test", clone.Text);
            }
        }

        [Theory]
        [InlineData(typeof(PlainPocoDto))]
        [InlineData(typeof(MixedPocoDto))]
        public void SupportPocoDto(Type dtoType)
        {
            dynamic instance = Activator.CreateInstance(dtoType);
            Assert.NotNull(instance);
            instance.Text = "This is a test";

            var serializer = IDomainContractSerializer.FromType(dtoType);
            var supportedContentTypes = serializer.GetContentTypes(dtoType).ToHashSet();

            Assert.Equal(2, supportedContentTypes.Count);
            Assert.True(supportedContentTypes.Contains("application/xml"));
            Assert.True(supportedContentTypes.Contains("application/json"));

            foreach (var contentType in supportedContentTypes)
            {
                Assert.True(serializer.CanSerialize(instance, contentType));
                Assert.True(serializer.CanDeserialize(instance.GetType().GUID, contentType));


                var content = serializer.Serialize(instance, contentType);
                var clone = serializer.Deserialize(instance.GetType().GUID, contentType, content);
                Assert.Equal("This is a test", clone.Text);

                var package = serializer.Pack(instance, contentType);
                Assert.Equal(contentType, contentType);
                clone = serializer.Unpack(package);
                Assert.Equal("This is a test", clone.Text);
            }
        }




        [Fact]
        public void RespectXmlFirstRequirement()
        {
            var target = IDomainContractSerializer.FromType(typeof(XmlFirstDto));
            Assert.True(target.CanSerialize(typeof(XmlFirstDto), out var contentType));
            Assert.Equal("application/xml", contentType, StringComparer.OrdinalIgnoreCase);
        }

        [Fact]
        public void RespectJsonFirstRequirement()
        {
            var target = IDomainContractSerializer.FromType(typeof(JsonFirstDto));
            Assert.True(target.CanSerialize(typeof(JsonFirstDto), out var contentType));
            Assert.Equal("application/json", contentType, StringComparer.OrdinalIgnoreCase);
        }

        [Theory]
        [InlineData(typeof(ExplitXmlPreferenceDto), "application/xml")]
        [InlineData(typeof(ExplitJsonPreferenceDto), "application/json")]
        public void RespectExplicitSerializationPreference(Type dtoType, string expectedDefaultContentType)
        {
            var target = IDomainContractSerializer.FromType(dtoType);
            Assert.True(target.CanSerialize(dtoType, out var contentType));
            Assert.Equal(expectedDefaultContentType, contentType, StringComparer.OrdinalIgnoreCase);
        }

        [Guid(TestDtoTypeId)]
        [DataTransferObject(typeof(BasicJsonDataTransferObjectSerializer))]
        [DataTransferObject(typeof(BasicXmlDataTransferObjectSerializer))]
        public sealed class PlainPocoDto
        {
            [XmlAttribute("Text"), JsonPropertyName("text")]
            public string Text { get; set; }
        }

        [Guid(TestDtoTypeId)]
        [DataTransferObject(typeof(BasicJsonDataTransferObjectSerializer))]
        [DataTransferObject(typeof(BasicXmlDataTransferObjectSerializer))]
        public sealed class MixedPocoDto : IBasicXmlDataTransferObject, IBasicJsonDataTransferObject
        {
            [XmlAttribute("Text"), JsonPropertyName("text")]
            public string Text { get; set; }
        }



        [Guid(TestDtoTypeId)]
        public sealed class XmlOnlyDto : IBasicXmlDataTransferObject
        {
            [XmlAttribute("Text")]
            public string Text { get; set; }
        }


        [Guid(TestDtoTypeId)]
        public sealed class JsonOnlyDto : IBasicJsonDataTransferObject
        {
            [JsonPropertyName("text")]
            public string Text { get; set; }
        }

        [Guid(TestDtoTypeId)]
        public sealed class BasicDto : IBasicJsonDataTransferObject, IBasicXmlDataTransferObject
        {
            [XmlAttribute("Text"), JsonPropertyName("text")]
            public string Text { get; set; }
        }



        [Guid(TestDtoTypeId)]
        public sealed class XmlFirstDto : BasicXmlDataTransferObject, IBasicJsonDataTransferObject { }

        [Guid(TestDtoTypeId)]
        public sealed class JsonFirstDto : BasicJsonDataTransferObject, IBasicXmlDataTransferObject { }

        [Guid(TestDtoTypeId)]
        [DataTransferObject(typeof(BasicXmlDataTransferObjectSerializer), IsDefault = true)]
        public sealed class ExplitXmlPreferenceDto : BasicJsonDataTransferObject { }

        [Guid(TestDtoTypeId)]
        [DataTransferObject(typeof(BasicJsonDataTransferObjectSerializer), IsDefault = true)]
        public sealed class ExplitJsonPreferenceDto : BasicXmlDataTransferObject { }



    }
}
