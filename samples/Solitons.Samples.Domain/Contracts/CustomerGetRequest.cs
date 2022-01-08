
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Solitons.Samples.Domain.Contracts
{
    [Guid("3390af6e-fb91-42fe-bde3-c9b188d0b705"), XmlRoot("Customer")]
    public class CustomerGetRequest : BasicJsonDataTransferObject, IBasicXmlDataTransferObject
    {
        public CustomerGetRequest()
        {
            
        }

        [DebuggerNonUserCode]
        public CustomerGetRequest(Guid guid)
        {
            Guid = guid.ThrowIfEmptyArgument(nameof(guid));
        }

        [JsonPropertyName("oid"), XmlAttribute("Guid")]
        public Guid Guid { get; set; }
    }
}
