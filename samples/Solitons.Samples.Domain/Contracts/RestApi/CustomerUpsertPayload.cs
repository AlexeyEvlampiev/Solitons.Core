using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Solitons.Samples.Domain.Contracts.RestApi
{
    [Guid("c2d65db7-022e-4867-9201-80fefa76f1be")]
    [DataTransferObject(typeof(JsonSerializer), IsDefault = true)]
    [DataTransferObject(typeof(XmlSerializer)), XmlRoot("CustomerData")]
    public sealed class CustomerUpsertPayload 
    {
        [JsonPropertyName("name"), XmlAttribute("Name")]
        public string Name { get; set; }

    }
}
