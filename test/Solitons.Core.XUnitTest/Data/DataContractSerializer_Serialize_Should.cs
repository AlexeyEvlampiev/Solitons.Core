using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Xunit;

namespace Solitons.Data;

// ReSharper disable once InconsistentNaming
public sealed class DataContractSerializer_Serialize_Should
{
    private readonly IDataContractSerializer _serializer;

    public DataContractSerializer_Serialize_Should()
    {
        _serializer = IDataContractSerializer.Build(builder => builder
            .IgnoreMissingCustomGuidAnnotation(true)
            .Add(typeof(JsonOnlyDto))
            .Add(typeof(JsonFirstDto))
            .Add(typeof(XmlOnlyDto))
            .Add(typeof(XmlFirstDto)));
    }


    [Fact]
    public void ThrowIfDtoOutOfRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _serializer.Serialize(
            new NotRegisteredDto()));
    }

    [Fact]
    public void SerializeJsonOnlyDto()
    {
        var jsonContent = _serializer.Serialize(new JsonOnlyDto()
        {
            Text = "JSON test"
        });

        Assert.Equal("application/json", jsonContent.ContentType);
        var registeredJsonDto = _serializer.Deserialize<JsonOnlyDto>(jsonContent);
        Assert.Equal("JSON test", registeredJsonDto.Text, StringComparer.Ordinal);
        registeredJsonDto = _serializer.Deserialize<JsonOnlyDto>(jsonContent, "application/json");
        Assert.Equal("JSON test", registeredJsonDto.Text, StringComparer.Ordinal);
        Assert.Throws<ArgumentOutOfRangeException>(() => _serializer.Deserialize<JsonOnlyDto>(jsonContent, "application/xml"));
    }


    [Fact]
    public void SerializeJsonFirstDto()
    {
        const string text = "JSON-first test";
        var mediaContent = _serializer.Serialize(new JsonFirstDto()
        {
            Text = text
        });

        Assert.Equal("application/json", mediaContent.ContentType);
        var dto = _serializer.Deserialize<JsonFirstDto>(mediaContent);
        Assert.Equal(text, dto.Text, StringComparer.Ordinal);

        dto = _serializer.Deserialize<JsonFirstDto>(mediaContent, "application/json");
        Assert.Equal(text, dto.Text, StringComparer.Ordinal);

        Assert.Throws<InvalidOperationException>(() => _serializer.Deserialize<JsonFirstDto>(mediaContent, "application/xml"));

        mediaContent = _serializer.Serialize(dto, "application/xml");
        Assert.Equal("application/xml", mediaContent.ContentType);

        dto = _serializer.Deserialize<JsonFirstDto>(mediaContent);
        Assert.Equal(text, dto.Text, StringComparer.Ordinal);

        dto = _serializer.Deserialize<JsonFirstDto>(mediaContent, "application/xml");
        Assert.Equal(text, dto.Text, StringComparer.Ordinal);
    }

    [Fact]
    public void SerializeXmlOnlyDto()
    {
        var xmlContent = _serializer.Serialize(new XmlOnlyDto()
        {
            Text = "XML test"
        });
        Assert.Equal("application/xml", xmlContent.ContentType);
        var dto = _serializer.Deserialize<XmlOnlyDto>(xmlContent);
        Assert.Equal("XML test", dto.Text, StringComparer.Ordinal);
        dto = _serializer.Deserialize<XmlOnlyDto>(xmlContent, "application/xml");
        Assert.Equal("XML test", dto.Text, StringComparer.Ordinal);
        Assert.Throws<ArgumentOutOfRangeException>(() => _serializer.Deserialize<XmlOnlyDto>(xmlContent, "application/json"));

    }

    [Fact]
    public void SerializeXmlFirstDto()
    {
        const string text = "XML-first test";
        var mediaContent = _serializer.Serialize(new XmlFirstDto()
        {
            Text = text
        });

        Assert.Equal("application/xml", mediaContent.ContentType);
        var dto = _serializer.Deserialize<XmlFirstDto>(mediaContent);
        Assert.Equal(text, dto.Text, StringComparer.Ordinal);

        dto = _serializer.Deserialize<XmlFirstDto>(mediaContent, "application/xml");
        Assert.Equal(text, dto.Text, StringComparer.Ordinal);

        Assert.Throws<System.Text.Json.JsonException>(() => _serializer.Deserialize<XmlFirstDto>(mediaContent, "application/json"));
        
        mediaContent = _serializer.Serialize(dto, "application/json");
        Assert.Equal("application/json", mediaContent.ContentType);

        dto = _serializer.Deserialize<XmlFirstDto>(mediaContent);
        Assert.Equal(text, dto.Text, StringComparer.Ordinal);

        dto = _serializer.Deserialize<XmlFirstDto>(mediaContent, "application/json");
        Assert.Equal(text, dto.Text, StringComparer.Ordinal);
    }


    public class JsonOnlyDto : BasicJsonDataTransferObject
    {
        [JsonPropertyName("txt")]
        public string Text { get; set; } = String.Empty;
    }

    public class JsonFirstDto : BasicJsonDataTransferObject, IBasicXmlDataTransferObject
    {
        [JsonPropertyName("txt")]
        [XmlAttribute("Txt")]
        public string Text { get; set; } = String.Empty;
    }

    public class XmlOnlyDto : BasicXmlDataTransferObject
    {
        [XmlAttribute("Txt")]
        public string Text { get; set; } = String.Empty;
    }


    public class XmlFirstDto : BasicXmlDataTransferObject, IBasicJsonDataTransferObject
    {
        [JsonPropertyName("txt")]
        [XmlAttribute("Txt")]
        public string Text { get; set; } = String.Empty;
    }


    public class NotRegisteredDto
    {

    }
}