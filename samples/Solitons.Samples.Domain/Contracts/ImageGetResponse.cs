using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Solitons.Data;
using Solitons.Text.Json;

namespace Solitons.Samples.Domain.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    [Guid("56c876f8-d0d2-4c34-ae18-798619177419")]
    public sealed class ImageGetResponse : BasicJsonDataTransferObject
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("relativePath")]
        public string ImageRelativePath { get; set; } = string.Empty;

        [JsonPropertyName("allowAllIpAddresses")]
        public bool AllowAllIpAddresses { get; set; }

        [JsonPropertyName("accessTimeWindow")]
        [JsonConverter(typeof(TimeSpanToStringConverter))]
        public TimeSpan AccessTimeWindow { get; set; }
    }
}
