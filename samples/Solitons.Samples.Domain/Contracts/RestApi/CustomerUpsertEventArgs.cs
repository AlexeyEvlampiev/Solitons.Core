using Solitons.Web;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace Solitons.Samples.Domain.Contracts.RestApi
{
    [Guid("dde05324-e1c5-45a3-b512-08b6c45260e1")]
    [SampleDbHttpTrigger(".*", "put|post", "/customers/(?<id>rgx:uuid)", procedure: "customer_upsert",
        DatabaseOperationTimeout = "00:00:02",
        ProcedureEventArgsContentType = "application/json",
        ProcedurePayloadContentType = "application/json",
        Authorize = "admin",
        PayloadObjectType = typeof(CustomerUpsertData),
        ResponseObjectType = typeof(CustomerData))]
    [DataTransferObject(typeof(JsonSerializer), IsDefault = true)]
    [DataTransferObject(typeof(XmlSerializer)), XmlRoot("CustomerUpsertEventArgs")]
    public sealed class CustomerUpsertEventArgs 
    {
        [UrlParameter("id"), JsonPropertyName("id"), XmlAttribute("Id")]
        public Guid Id { get; set; }
    }
}
