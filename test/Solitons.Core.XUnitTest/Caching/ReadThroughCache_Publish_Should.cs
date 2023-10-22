using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Xunit;

namespace Solitons.Caching;

// ReSharper disable once InconsistentNaming
public sealed class ReadThroughCache_Publish_Should
{
    [Fact]
    public async Task CacheLatestValue()
    {
        var subject = new ReplaySubject<int>(1);  // This subject replays the last value to new subscribers
        subject.OnNext(42);

        var cachedObservable = ReadThroughCache.Publish(subject);
        using var connection = cachedObservable.Connect();

        var testValue = await cachedObservable.FirstAsync();

        Assert.Equal(42, testValue);
    }

    [Fact]
    public async Task ReplayCachedValueToNewSubscribers()
    {
        var subject = new ReplaySubject<int>(1);
        subject.OnNext(42);

        var cachedObservable = ReadThroughCache.Publish(subject);
        using var connection = cachedObservable.Connect();

        subject.OnNext(43);  // Update the value

        var testValue = await cachedObservable.FirstAsync();  // New subscriber

        Assert.Equal(43, testValue);  // New subscriber should get the updated value

    }

    [Fact]
    public async Task HandleMultipleSubscribers()
    {
        var subject = new ReplaySubject<int>(1);
        subject.OnNext(42);

        var cachedObservable = ReadThroughCache.Publish(subject);
        using var connection = cachedObservable.Connect();

        var testValue1 = await cachedObservable.FirstAsync();
        var testValue2 = await cachedObservable.FirstAsync();
        Assert.Equal(42, testValue1);
        Assert.Equal(42, testValue2);

        subject.OnNext(100);

        var testValue3 = await cachedObservable.FirstAsync();
        Assert.Equal(100, testValue3);
    }
}