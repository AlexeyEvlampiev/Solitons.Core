using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Solitons.Samples.Domain.Contracts
{
    [Guid("83a45c34-7569-4a18-bfd2-71ac94bec684"), XmlRoot("Customer")]
    public sealed class CustomerUpsertResponse : BasicJsonDataTransferObject, IBasicXmlDataTransferObject
    {
        [JsonPropertyName("oid"), XmlAttribute("Guid")]
        public Guid Guid { get; set; }

        [JsonPropertyName("id"), XmlAttribute("CustomerId")]
        public string Id { get; set; }

        [JsonPropertyName("email"), XmlAttribute("Email")]
        public string Email { get; set; }
    }
}
