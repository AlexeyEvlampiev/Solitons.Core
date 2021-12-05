using Solitons.Azure.Referencelmpl;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Solitons.Azure.Blobs
{
    // ReSharper disable once InconsistentNaming
    public sealed class TransientAzureBlobContainerStorage_UploadAsync_Should
    {
        [Fact]
        public async Task UploadBytesAsync()
        {
            var target = TransientAzureBlobContainerStorage.Create(Host.StorageConnectionString);
            var receipt = await target.UploadAsync("Hello world!".ToUtf8Bytes(), TimeSpan.FromMinutes(10));
            await using var stream = await target.DownloadAsync(receipt);
            using var reader = new StreamReader(stream);
            var actual = await reader.ReadToEndAsync();
            Assert.Equal("Hello world!", actual);
        }

        [Fact]
        public async Task UploadStreamAsync()
        {
            var target = TransientAzureBlobContainerStorage.Create(Host.StorageConnectionString);
            var receipt = await target.UploadAsync("Hello world!".ToMemoryStream(Encoding.UTF8), TimeSpan.FromMinutes(10));
            await using var stream = await target.DownloadAsync(receipt);
            using var reader = new StreamReader(stream);
            var actual = await reader.ReadToEndAsync();
            Assert.Equal("Hello world!", actual);
        }
    }
}
