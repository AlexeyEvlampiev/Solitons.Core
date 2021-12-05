using Solitons.Web;

namespace Solitons.Azure
{
    public sealed class DataLakeBlobUriMetadata : BlobSecureAccessSignatureMetadata
    {
        public DataLakeBlobUriMetadata(BlobSasPermissions permissions, string ttlTimeSpan)
            : base(permissions, ttlTimeSpan)
        {
        }
    }
}
