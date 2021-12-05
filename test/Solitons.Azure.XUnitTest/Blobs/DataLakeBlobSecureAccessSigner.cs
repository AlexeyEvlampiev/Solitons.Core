using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Solitons.Web;
using Solitons.Web.Common;
using BlobSasPermissions = Solitons.Web.BlobSasPermissions;

namespace Solitons.Azure.Blobs
{
    class DataLakeBlobSecureAccessSigner : SecureAccessSigner<DataLakeBlobUriMetadata>
    {
        private readonly BlobContainerClient _container;

        private DataLakeBlobSecureAccessSigner(BlobContainerClient container) : base(MyDomain.Instance)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public static async Task<ISecureAccessSigner> CreateAsync(string connectionString = "UseDevelopmentStorage=true")
        {
            var container = new BlobContainerClient(connectionString, "datalake");
            await container.CreateIfNotExistsAsync();
            return new DataLakeBlobSecureAccessSigner(container);
        }

        protected override Uri SpecializedSign(string blobName, IPAddress startAddress, IPAddress endAddress, DataLakeBlobUriMetadata metadata)
        {
            var blobClient = _container.GetBlobClient(blobName);
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _container.Name,
                BlobName = blobClient.Name,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.Add(metadata.TimeToLive)
            };

            var azPermissions = (global::Azure.Storage.Sas.BlobSasPermissions)0;
            Enum
                .GetValues<BlobSasPermissions>()
                .Where(permission => metadata.Permissions.HasFlag(permission))
                .ForEach(permission =>
                {
                    azPermissions |= permission switch
                    {
                        BlobSasPermissions.Read => global::Azure.Storage.Sas.BlobSasPermissions.Read,
                        BlobSasPermissions.Create => global::Azure.Storage.Sas.BlobSasPermissions.Create,
                        BlobSasPermissions.Delete => global::Azure.Storage.Sas.BlobSasPermissions.Delete,
                        BlobSasPermissions.Write => global::Azure.Storage.Sas.BlobSasPermissions.Write,
                        _ => throw new ArgumentOutOfRangeException(nameof(permission), permission, null)
                    };
                });
            if (startAddress != null)
            {
                sasBuilder.IPRange = new SasIPRange(startAddress, endAddress ?? startAddress);
            }
            sasBuilder.SetPermissions(azPermissions);
            return blobClient.GenerateSasUri(sasBuilder);
        }
    }
}
