using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Solitons;

// ReSharper disable once InconsistentNaming
public class IObservable_CatchAndRetry_Should
{
    [Theory]
    [InlineData(0, "test0")]
    [InlineData(1, "test1")]
    [InlineData(3, "test3")]
    public async Task ReturnCorrectValue_AfterSpecifiedFailuresAsync(int timesToThrow, string expected)
    {
        var faultyService = new FaultyObservable(timesToThrow, expected);
        var actual = await faultyService
            .CatchAndRetry(async args =>
            {
                Debug.WriteLine(args.AttemptNumber);
                return args.AttemptNumber < timesToThrow;
            });

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async Task ThrowException_WhenRetryLimitExceededAsync()
    {
        var faultyService = new FaultyObservable(3, "test");
        await Assert.ThrowsAsync<Exception>(async () =>
        {
            await faultyService
                .CatchAndRetry(async args =>
                {
                    Debug.WriteLine(args.AttemptNumber);
                    return args.AttemptNumber < 2;
                });
        });
    }

    sealed class FaultyObservable : ObservableBase<string>
    {
        private int _counter = 0;
        private readonly int _failuresMaxCount;
        private readonly string _result;

        public FaultyObservable(int timesToThrow, string result)
        {
            _failuresMaxCount = timesToThrow;
            _result = result;
        }

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
