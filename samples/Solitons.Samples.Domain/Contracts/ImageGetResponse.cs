using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Solitons.Samples.Domain.Security;

namespace Solitons.Samples.Domain.Contracts
{
    [Guid("56c876f8-d0d2-4c34-ae18-798619177419")]
    public sealed class ImageGetResponse : BasicJsonDataTransferObject
    {
        [JsonPropertyName("uri")]
        [ReadOnlySasAccess("00:03:00")]
        public string? ImageSource { get; set; }
    }
}
