using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Solitons.Samples.Domain.Contracts
{
    [Guid("e4867f9c-2dd3-488f-9efd-f0fa2e73da8e"), XmlRoot("Customer")]
    public class CustomerUpsertRequest : BasicJsonDataTransferObject, IBasicXmlDataTransferObject
    {
        [JsonPropertyName("oid"), XmlAttribute("Guid")]
        public Guid Guid { get; set; }

        [JsonPropertyName("id"), XmlAttribute("CustomerId")]
        public string Id { get; set; }

        [JsonPropertyName("email"), XmlAttribute("Email")]
        public string Email { get; set; }
    }
}
