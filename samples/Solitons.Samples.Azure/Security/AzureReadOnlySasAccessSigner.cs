using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using Azure.Storage.Sas;
using Solitons.Samples.Domain.Security;

namespace Solitons.Samples.Azure.Security
{
    public sealed class AzureReadOnlySasAccessSigner : ReadOnlySasAccessSigner
    {
        private readonly BlobContainerClient _digitalAssetsContainer;
        public AzureReadOnlySasAccessSigner(string storageConnectionString)
        {
            _digitalAssetsContainer = new BlobContainerClient(storageConnectionString, "assets");
        }

        protected override string Sign(string url, ReadOnlySasAccessAttribute attribute)
        {
            var blobClient = _digitalAssetsContainer.GetBlobClient(url);
            BlobSasBuilder sasBuilder = new BlobSasBuilder()
            {
                BlobContainerName = blobClient.GetParentBlobContainerClient().Name,
                BlobName = blobClient.Name,
                Resource = "b"
            };

            sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.Add(attribute.ExpiresAfter);
            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            Uri sasUri = blobClient.GenerateSasUri(sasBuilder);
            return sasUri.ToString();
        }
    }
}
