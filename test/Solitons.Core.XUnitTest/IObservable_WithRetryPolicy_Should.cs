using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Solitons;

// ReSharper disable once InconsistentNaming
public class IObservable_WithRetryPolicy_Should
{
    [Theory]
    [InlineData(0, "test0")]
    [InlineData(1, "test1")]
    [InlineData(3, "test3")]
    public async Task ReturnCorrectValue_AfterSpecifiedFailuresAsync(int timesToThrow, string expected)
    {
        var actual = await FaultyObservable
            .Create(timesToThrow, expected)
            .WithRetryPolicy(args => args
                .SignalNextAttempt(args.AttemptNumber < timesToThrow));

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ThrowException_WhenRetryLimitExceededAsync()
    {
        // Task based version
        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await FaultyObservable
                .Create(3, "test")
                .WithRetryPolicy(args => args
                    .SignalNextAttempt(args.AttemptNumber < 2));
        });
    }


    [Fact]
    public async Task Retry_Until_Successful()
    {
        var actual = await FaultyObservable
            .Create(3, "test")
            .WithRetryPolicy(args => args
                .SignalNextAttempt(args.AttemptNumber < 3));
        Assert.Equal("test", actual);
    }

    sealed class FaultyObservable : ObservableBase<string>
    {
        private int _counter = 0;
        private readonly int _failuresMaxCount;
        private readonly string _result;

        [DebuggerNonUserCode]
        private FaultyObservable(int timesToThrow, string result)
        {
            _failuresMaxCount = timesToThrow;
            _result = result;
        }

        [DebuggerNonUserCode]
        public static IObservable<string> Create(int timesToThrow, string result) =>
            new FaultyObservable(timesToThrow, result);

        protected override IDisposable SubscribeCore(IObserver<string> observer)
        {
            _counter++;
            observer = observer.NotifyOn(TaskPoolScheduler.Default);
            if (_counter <= _failuresMaxCount)
            {
                observer.OnError(new Exception());
                return Disposable.Empty;
            }

            observer.OnNext(_result);
            observer.OnCompleted();
            return Disposable.Empty;
        }
    }
}
