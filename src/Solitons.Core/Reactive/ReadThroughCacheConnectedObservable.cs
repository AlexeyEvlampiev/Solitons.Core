using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;

namespace Solitons.Reactive;

public static class ReadThroughCacheConnectedObservable
{
    public static ReadThroughCacheConnectedObservable<T> Create<T>(IObservable<T> source)
    {
        return new ReadThroughCacheConnectedObservable<T>(source);
    }
}
public sealed class ReadThroughCacheConnectedObservable<T> : ObservableBase<T>, IConnectableObservable<T>
{
    private readonly IConnectableObservable<T> _source;
    private IObservable<T> _cache = Observable.Empty<T>();


    [DebuggerStepThrough]
    internal ReadThroughCacheConnectedObservable(IObservable<T> source)
    {
        _source = source
            .Do(next =>
            {
                _cache = Observable.Return(next);
                CacheUpdatedUtc = DateTime.UtcNow;
            })
            .Publish();
    }

    public DateTime? CacheUpdatedUtc { get; private set; }

    protected override IDisposable SubscribeCore(IObserver<T> observer)
    {
        return _cache
            .Concat(_source)
            .Subscribe(observer);
    }

    public IDisposable Connect()
    {
        return _source.Connect();
    }
}