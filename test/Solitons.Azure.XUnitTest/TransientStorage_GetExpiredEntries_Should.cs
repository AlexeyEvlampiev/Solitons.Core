using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Moq;
using Solitons.Azure.Referencelmpl;
using Xunit;

namespace Solitons.Azure
{
    // ReSharper disable once InconsistentNaming
    public sealed class TransientStorage_GetExpiredEntries_Should
    {
        [Fact]
        public async Task SucceedAsync()
        {
            var fakePast = new Mock<IClock>();
            var fakeNow = new Mock<IClock>();
            fakePast.SetupGet(i => i.UtcNow).Returns(DateTimeOffset.UtcNow.AddYears(-10));
            fakeNow.SetupGet(i => i.UtcNow).Returns(DateTimeOffset.UtcNow.AddYears(-9));

            var blobId = await TransientAzureBlobContainerStorage
                .Create(Host.StorageConnectionString, fakePast.Object)
                .UploadAsync("Test".ToUtf8Bytes(), fakePast.Object.UtcNow);

            var count = await TransientAzureBlobContainerStorage
                .Create(Host.StorageConnectionString, fakeNow.Object)
                .GetExpiredEntries()
                .SelectMany(async entry =>
                {
                    await entry.DeleteAsync();
                    return 1;
                })
                .Count();
            Assert.True(count == 1);
        }
    }
}
