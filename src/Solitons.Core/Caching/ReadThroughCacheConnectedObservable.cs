using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;

namespace Solitons.Caching;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
sealed class ReadThroughCacheConnectedObservable<T> : ObservableBase<T>, IConnectableObservable<T>
{
    private readonly IConnectableObservable<T> _source;
    private IObservable<T> _cache = Observable.Empty<T>();


    [DebuggerStepThrough]
    internal ReadThroughCacheConnectedObservable(IObservable<T> source)
    {
        _source = source
            .Do(next => _cache = Observable.Return(next))
            .Publish();
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="observer"></param>
    /// <returns></returns>
    protected override IDisposable SubscribeCore(IObserver<T> observer)
    {
        return _cache
            .Concat(_source)
            .Subscribe(observer);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public IDisposable Connect()
    {
        return _source.Connect();
    }
}