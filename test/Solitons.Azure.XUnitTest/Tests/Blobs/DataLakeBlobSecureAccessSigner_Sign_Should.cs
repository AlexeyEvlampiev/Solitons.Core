using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Solitons.Azure.Blobs;
using Xunit;

namespace Solitons.Azure.Tests.Blobs
{
    // ReSharper disable once InconsistentNaming
    public sealed class DataLakeBlobSecureAccessSigner_Sign_Should
    {
        [Fact]
        public async Task Work()
        {
            var blobName = "28160a9e-45d3-4534-bf73-2b9d0de101b0";
            var expectedContent = "Hello world!";
            var dataLake = new BlobContainerClient(Host.StorageConnectionString, "datalake");
            await dataLake.CreateIfNotExistsAsync();
            var blob = dataLake.GetBlobClient(blobName);
            await blob.UploadAsync(expectedContent.ToMemoryStream(Encoding.UTF8),true);
            var reference = new DataLakeBlobReference()
            {
                ReadOnlyUri = new Uri(blobName, UriKind.Relative),
                WriteOnlyUri = new Uri(blobName, UriKind.Relative)
            };
            
            var target = await DataLakeBlobSecureAccessSigner.CreateAsync(Host.StorageConnectionString);
            var signed = target.Sign(reference);
            Assert.Equal(2, signed);

            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync(reference.ReadOnlyUri);
            Assert.Equal(expectedContent, response);


            /*
            string HostName = Dns.GetHostName();
            IPAddress[] ipaddress = Dns.GetHostAddresses(HostName)
                .Where(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                .ToArray();

            response = await httpClient.GetStringAsync(reference.IpRestrictedUri);
            Assert.Equal("Hello world!", response);
            */
        }
    }
}
