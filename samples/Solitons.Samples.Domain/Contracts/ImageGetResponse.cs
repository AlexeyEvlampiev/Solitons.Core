using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Solitons.Security;

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
        [JsonPropertyName("uri")]
        [BlobSasUri("DigitalAssets", BlobSasPermissions.Read, "00:05:00", IpRangeRestriction = IpRangeRestriction.Required)]
        public string ImageSource { get; set; } = string.Empty;

        protected override void OnSerializing(object sender)
        {
            base.OnSerializing(sender);
            Debug.Assert(Uri.IsWellFormedUriString(ImageSource, UriKind.Absolute));
        }
    }
}
