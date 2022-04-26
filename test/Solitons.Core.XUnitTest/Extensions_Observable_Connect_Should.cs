using System;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Collections;
using Xunit;

namespace Solitons
{
    // ReSharper disable once InconsistentNaming
    public sealed class Extensions_Observable_Connect_Should
    {
        [Fact]
        public async Task MulticastUnderlyingStreamErrors()
        {
            var hotStream = Observable
                .Throw<int>(new Exception("This is a test"))
                .Connect(-1, CancellationToken.None);

            await Observable
                .Range(0, 3)
                .SelectMany(_ => Assert.ThrowsAsync<Exception>(async () => await hotStream.FirstOrDefaultAsync()));
        }

        [Fact]
        public async Task CacheColdObservableLastItem()
        {
            var hotStream = FluentArray
                .Create(9, 8, 7, 6, 5)
                .ToObservable()
                .Connect(-1, CancellationToken.None);

            await Observable
                .Range(0, 3)
                .SelectMany(_ => hotStream.FirstAsync())
                .Do(item=> Assert.Equal(5, item));
        }

        [Fact]
        public async Task BroadcastLastEvent()
        {
            var subject = new Subject<string>();
            var dispatcher = subject
                .Connect(CancellationToken.None)
                .Take(TimeSpan.FromMilliseconds(10));


            Assert.Equal(0, await dispatcher.Count());
            await dispatcher
                .FirstOrDefaultAsync()
                .Do(Assert.Null);
            

            subject.OnNext("Message 1");
            Assert.Equal(1, await dispatcher.Count());
            await dispatcher
                .FirstAsync()
                .Do(actual=> Assert.Equal("Message 1", actual));

            subject.OnNext("Message 2");
            Assert.Equal(1, await dispatcher.Count());
            await dispatcher
                .FirstAsync()
                .Do(actual => Assert.Equal("Message 2", actual));

            subject.OnNext("Message 3");
            subject.OnNext("Message 4");
            Assert.Equal(1, await dispatcher.Count());
            await dispatcher
                .FirstAsync()
                .Do(actual => Assert.Equal("Message 4", actual));

            subject.OnNext("Message 5");
            subject.OnNext("Message 6");
            subject.OnNext("Message 7");
            Assert.Equal(1, await dispatcher.Count());
            await dispatcher
                .FirstAsync()
                .Do(actual => Assert.Equal("Message 7", actual));
        }

        [Fact]
        public async Task CompleteOnCancellation()
        {
            var subject = new Subject<string>();
            var cts = new CancellationTokenSource();

            var dispatcher = subject
                .Connect(cts.Token)
                .Take(TimeSpan.FromMilliseconds(10));

            subject.OnNext("Message 1");
            Assert.Equal(1, await dispatcher.Count());
            await dispatcher
                .FirstAsync()
                .Do(actual => Assert.Equal("Message 1", actual));

            cts.Cancel();
            Assert.Equal(0, await dispatcher.Count());
            dispatcher.Subscribe(
                _ => throw new InvalidOperationException("The stream should be cancelled. Actual: active"),
                ex => throw new InvalidOperationException("The stream should be cancelled. Actual: errored"), 
                () => Debug.WriteLine("Ok"));
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await dispatcher.FirstAsync());
        }
    }
}
