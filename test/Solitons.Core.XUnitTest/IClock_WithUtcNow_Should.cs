using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Solitons
{
    // ReSharper disable once InconsistentNaming
    public sealed class IClock_WithUtcNow_Should
    {
        [Fact]
        public void Work()
        {
            var utcNow = DateTime.Parse("1943-03-23");
            IClock target = new StoppedClock(DateTime.Parse("1975-03-15"), TimeSpan.FromHours(2));
            
            var adjusted = target.WithUtcNow(utcNow);
            Assert.Equal(new DateTimeOffset(utcNow, TimeSpan.Zero), adjusted.UtcNow);
            Assert.Equal(new DateTimeOffset(utcNow, TimeSpan.FromHours(2)), adjusted.Now);
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
