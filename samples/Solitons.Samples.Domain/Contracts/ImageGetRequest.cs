using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Solitons.Samples.Domain.Contracts
{
    [Guid("3988f64a-89e7-4619-b925-ebbcb68b040e")]
    public sealed class ImageGetRequest : BasicJsonDataTransferObject
    {
        public ImageGetRequest()
        {
        }

        public ImageGetRequest(Guid imageGuid)
        {
            ImageGuid = imageGuid;
        }

        [JsonPropertyName("oid")]
        public Guid ImageGuid { get; }
    }
}
