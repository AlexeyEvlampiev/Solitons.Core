using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Solitons.Samples.Domain.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    [Guid("9187b986-07f4-494a-a5c6-85ee2bdffcbd"), XmlRoot("Customer")]
    public sealed  class CustomerGetResponse : BasicJsonDataTransferObject, IBasicXmlDataTransferObject
    {
        [JsonPropertyName("oid"), XmlAttribute("Guid")]
        public Guid Guid { get; set; }

        [JsonPropertyName("id"), XmlAttribute("CustomerId")]
        public string Id { get; set; }

        [JsonPropertyName("email"), XmlAttribute("Email")]
        public string Email { get; set; }
    }
}
