using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Solitons
{
    // ReSharper disable once InconsistentNaming
    public sealed class IClock_WithOffset_Should
    {
        [Fact]
        public void Work()
        {
            var now = DateTime.Parse("1972-06-27");
            var offset = TimeSpan.FromMilliseconds(1234567);

            IClock target = new StoppedClock(now, TimeSpan.FromHours(2));

            var withOffset = target.WithOffset(offset);

            Assert.Equal(target.UtcNow + offset, withOffset.UtcNow);
            Assert.Equal(target.Now + offset, withOffset.Now);
        }

        sealed class StoppedClock : IClock
        {
            public StoppedClock(DateTime utcNow, TimeSpan offset)
            {
                Now = new DateTimeOffset(utcNow, offset);
                UtcNow = new DateTimeOffset(utcNow, TimeSpan.Zero);
            }

            public DateTimeOffset Now { get; init; }
            public DateTimeOffset UtcNow { get; init; }
            public Task DelayAsync(TimeSpan duration, CancellationToken cancellation)
            {
                throw new NotImplementedException();
            }
        }
    }
}
