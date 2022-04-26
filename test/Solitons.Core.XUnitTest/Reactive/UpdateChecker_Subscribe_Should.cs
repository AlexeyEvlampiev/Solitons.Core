using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Solitons.Reactive
{
    // ReSharper disable once InconsistentNaming
    public sealed class UpdateChecker_Subscribe_Should
    {
        [Fact]
        public async Task Work()
        {
            var updates = new TestUpdateChecker()
                .Connect(CancellationToken.None);

            for(int i = 0; i < 10; ++i)
            {
                var value = await updates.FirstAsync();
                Assert.Equal("This is a test", value);
            }
        }

        sealed class TestUpdateChecker : UpdateChecker<string>
        {
            public TestUpdateChecker() : base(Observable
                .Never(Unit.Default))
            {
            }

            protected override Task<State> LoadLatestStateAsync(State? currentState, CancellationToken cancellation)
            {
                return Task.FromResult(new State("This is a test", "Dummy-ETag"));
            }
        }
    }
}
