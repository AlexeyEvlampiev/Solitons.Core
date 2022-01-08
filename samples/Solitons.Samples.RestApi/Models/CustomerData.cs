using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Solitons.Samples.Domain.Contracts;

namespace Solitons.Samples.RestApi.Models
{
    public class CustomerData : BasicJsonDataTransferObject, IBasicXmlDataTransferObject
    {

        /// <summary>
        /// 
        /// </summary>
        public CustomerData()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        public CustomerData(CustomerGetResponse response)
        {
            response.ThrowIfNullArgument(nameof(response));
            Guid = response.Guid;
            Id = response.Id;
            Email = response.Email;
        }

        [JsonPropertyName("oid"), XmlAttribute("Guid")]
        public Guid Guid { get; set; }

        [JsonPropertyName("id"), XmlAttribute("CustomerId")]
        public string Id { get; set; }

        [JsonPropertyName("email"), XmlAttribute("Email")]
        public string Email { get; set; }
    }
}
