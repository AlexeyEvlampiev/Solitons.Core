using Solitons.Web;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Solitons.Data;

namespace Solitons.Samples.Domain.Contracts.RestApi
{
    [Guid("8db0d392-3fcd-4d14-8f3d-d404b229fd50")]
    [DatabaseHttpTriggerEventArgs("get", "/customers/(?<id>rgx:uuid)", procedure: "customer_get",
        DatabaseOperationTimeout = "00:00:02",
        Authorize = "admin",
        ResponseObjectType = typeof(CustomerData))]
    [XmlRoot("CustomerInfoRequest")]
    public sealed class CustomerRequestEventArgs : BasicJsonDataTransferObject, IBasicXmlDataTransferObject
    {
        [UrlParameter("id"), JsonPropertyName("id"), XmlAttribute("Id")]
        public Guid Id { get; set; }
    }
}
