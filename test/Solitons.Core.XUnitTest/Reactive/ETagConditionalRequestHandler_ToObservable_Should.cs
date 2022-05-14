using System.Threading;
using System.Threading.Tasks;
using Solitons.Caching;
using Solitons.Caching.Common;
using Xunit;

namespace Solitons.Reactive
{
    // ReSharper disable once InconsistentNaming
    public sealed class ETagConditionalRequestHandler_ToObservable_Should
    {
        [Fact]
        public async Task Work()
        {
            IReadThroughCache<string> target = new TestEntityCacheClient();

            for(int i = 0; i < 10; ++i)
            {
                var value = await target.ReadAsync();
                Assert.Equal("This is a test", value);
            }
        }

        sealed class TestEntityCacheClient : ReadThroughETagCache<string>
        {
            protected override Task<State> GetIfNonMatchAsync(string? eTag, CancellationToken cancellation) => Task
                .FromResult(new State("This is a test", "Some ETag"));
        }
    }
}
