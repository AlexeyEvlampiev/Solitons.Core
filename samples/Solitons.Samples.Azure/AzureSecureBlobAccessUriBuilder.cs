
using System.Net;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Solitons.Security.Common;

namespace Solitons.Samples.Azure
{
    public sealed class AzureSecureBlobAccessUriBuilder : SecureBlobAccessUriBuilder
    {
        private readonly BlobContainerClient _digitalAssetsContainer;

        public AzureSecureBlobAccessUriBuilder(string storageConnectionString)
        {
            _digitalAssetsContainer = new BlobContainerClient(storageConnectionString, "assets");
        }


        protected override Uri BuildDownloadUri(string relativePath, TimeSpan expiredAfter, IPAddress? remoteIpAddress)
        {
            var blobClient = _digitalAssetsContainer.GetBlobClient(relativePath);
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                BlobName = blobClient.Name,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.Add(expiredAfter)
            };

            sasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

            if (remoteIpAddress is not null)
            {
                sasBuilder.IPRange = new SasIPRange(remoteIpAddress);
            }


            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
            return sasUri;
        }
    }
}
