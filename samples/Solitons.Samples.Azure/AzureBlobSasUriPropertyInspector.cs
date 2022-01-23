using System.Diagnostics;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Solitons.Common;
using Solitons.Security;
using BlobSasPermissions = Azure.Storage.Sas.BlobSasPermissions;

namespace Solitons.Samples.Azure
{
    public sealed class AzureBlobSasUriPropertyInspector : BlobSasUriPropertyInspector
    {
        private readonly BlobContainerClient _digitalAssetsContainer;


        public AzureBlobSasUriPropertyInspector(string storageConnectionString)
        {
            _digitalAssetsContainer = new BlobContainerClient(storageConnectionString, "assets");
        }

        private AzureBlobSasUriPropertyInspector(AzureBlobSasUriPropertyInspector other)
        {
            _digitalAssetsContainer = other._digitalAssetsContainer;
        }

        [DebuggerStepThrough]
        protected override BlobSasUriPropertyInspector Clone() => new AzureBlobSasUriPropertyInspector(this);

        protected override string GenerateSasUri(string blobPath, BlobSasUriAttribute attribute)
        {
            var blobClient = _digitalAssetsContainer.GetBlobClient(blobPath);
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                BlobName = blobClient.Name,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.Add(attribute.ExpiresAfter)
            };

            sasBuilder.SetPermissions(Convert(attribute.Permissions));

            if (attribute.IpRangeRestriction != IpRangeRestriction.None)
            {
                sasBuilder.IPRange = new SasIPRange(this.IpRange.Start, this.IpRange.End);
            }


            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
            return sasUri.ToString();
        }

        private BlobSasPermissions Convert(Solitons.Security.BlobSasPermissions permissions)
        {
            var sdkPermissions = (BlobSasPermissions)0;
            return sdkPermissions
                   | (permissions.HasFlag(Solitons.Security.BlobSasPermissions.Create) ? BlobSasPermissions.Create : 0)
                   | (permissions.HasFlag(Solitons.Security.BlobSasPermissions.Delete) ? BlobSasPermissions.Delete : 0)
                   | (permissions.HasFlag(Solitons.Security.BlobSasPermissions.Read) ? BlobSasPermissions.Read : 0)
                   | (permissions.HasFlag(Solitons.Security.BlobSasPermissions.Write) ? BlobSasPermissions.Write : 0);
        }
    }
}
