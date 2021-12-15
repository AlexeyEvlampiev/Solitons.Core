using Solitons.Cloud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Solitons.Web
{
    public class HttpEventArgsTest
    {
        [Guid("bbd464a1-ce5a-444e-9a7a-947d932913b0")]
        [BasicHttpEventArgs(@".*","put|post","/products/(?<id>rgx:guid)/orders")]
        [TargetQueue("orders")]
        public sealed class CustomerOrderRequestData : 
            BasicJsonDataTransferObject, 
            IBasicXmlDataTransferObject
        {
            [UrlParameter("id"), JsonPropertyName("productId"), XmlAttribute("ProductId")]
            public Guid ProductId { get; set; }

            [QueryParameter("count","(?:items?-?)?count"), JsonPropertyName("itemsCount"), XmlAttribute("ItemsCount")]
            public int ItemsCount { get; set; }

            [Claim(ClaimTypes.NameIdentifier, IsRequired = true), JsonPropertyName("userId"), XmlAttribute("UserId")]
            public Guid UserId { get; set; }

            protected override void OnSerializing(object sender)
            {
                if (UserId == Guid.Empty)
                    throw new InvalidOperationException($"{nameof(UserId)} is required.");
                if (ProductId == Guid.Empty)
                    throw new InvalidOperationException($"{nameof(ProductId)} is required.");
                if (ItemsCount < 1)
                    throw new InvalidOperationException($"{nameof(ItemsCount)} is required.");
                base.OnSerializing(sender);
            }
        }
    }
}
