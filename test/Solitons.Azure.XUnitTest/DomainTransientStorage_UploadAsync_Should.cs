using Solitons.Azure.Referencelmpl;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Solitons.Azure
{
    // ReSharper disable once InconsistentNaming
    public sealed class DomainTransientStorage_UploadAsync_Should
    {
        [DataTransferObject, Guid("671ccf85-260c-4045-bcb1-45a5c1d86011")]
        sealed record Dto(string Text);

        public sealed record NotDto(string Text);

        private readonly ITransientStorage _transientStorage;
        private readonly IDomainTransientStorage _domainTransientStorage;
             

        public DomainTransientStorage_UploadAsync_Should()
        {
            _transientStorage = new TransientAzureBlobContainerStorage(Host.StorageConnectionString);
            _domainTransientStorage = TestDomain.Instance.CreateDomainTransientStorage(_transientStorage);
        }

        [Fact]
        public async Task UploadStreamAsync()
        {
            var receipt = await _domainTransientStorage.UploadAsync("Hello world!".ToMemoryStream(Encoding.UTF8), TimeSpan.FromHours(10));
            await using var stream = (Stream)await _domainTransientStorage.DownloadAsync(receipt);
            using var reader = new StreamReader(stream);
            var actual = await reader.ReadToEndAsync();
            Assert.Equal("Hello world!", actual);
        }


        [Fact]
        public async Task UploadBytesAsync()
        {
            var receipt = await _domainTransientStorage.UploadAsync("Hello world!".ToBytes(Encoding.UTF8), TimeSpan.FromHours(10));
            await using var stream = (Stream)await _domainTransientStorage.DownloadAsync(receipt);
            using var reader = new StreamReader(stream);
            var actual = await reader.ReadToEndAsync();
            Assert.Equal("Hello world!", actual);
        }


        [Theory]
        [InlineData(1)]
        [InlineData(1000)]
        [InlineData(1000000)]
        public async Task UploadDtoAsync(int length)
        {
            var expected = new string('x', length);
            var dto = new Dto(expected);
            var receipt = await _domainTransientStorage.UploadAsync(dto, TimeSpan.FromHours(1), 1000);
            dto = (Dto)await _domainTransientStorage.DownloadAsync(receipt);
            Assert.Equal(expected, dto.Text);
        }

        [Fact]
        public async Task ThrowIfNotDto()
        {
            var notDto = new NotDto(":-)");
            await Assert.ThrowsAsync<ArgumentException>( ()=>  _domainTransientStorage
                .UploadAsync(notDto, TimeSpan.FromHours(1), 1000));
            
        }
    }
}
