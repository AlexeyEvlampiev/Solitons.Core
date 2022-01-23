using System;
using System.Runtime.InteropServices;
using Solitons.Security;

namespace Solitons.Azure
{
    [Guid("25bab450-3bc7-4244-ac0b-2c5948ad99fd")]
    public sealed class DataLakeBlobReference
    {
        [BlobSasUri("DataLake", BlobSasPermissions.Read, "00:10:00")]
        public Uri WriteOnlyUri { get; set; }

        [BlobSasUri("DataLake", BlobSasPermissions.Read, "00:10:00")]
        public Uri ReadOnlyUri { get; set; }
    }
}
