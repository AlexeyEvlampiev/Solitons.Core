using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;
using System.Reactive.Threading.Tasks;

namespace Solitons.Reactive;

sealed class ReadThroughCacheConnectedObservable<T> : ObservableBase<T>, IConnectableObservable<T>
{

    private readonly IConnectableObservable<T> _innerObservable;
    private IObservable<T> _cache = Observable.Empty<T>();

    [DebuggerStepThrough]
    internal ReadThroughCacheConnectedObservable(IObservable<T> source, PublicationOptions<T> options)
    {
        _innerObservable = source
            .Do(next =>
            {
                _cache = Observable
                    .Return(next)
                    .TakeUntil(options
                        .GetExpirationSignal(next));
            })
            .Publish();
    }

    protected override IDisposable SubscribeCore(IObserver<T> observer)
    {
        return _cache
            .Concat(_innerObservable)
            .Subscribe(observer);
    }

    public IDisposable Connect()
    {
        return _innerObservable.Connect();
    }
}