using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;

namespace Solitons.Caching;

/// <summary>
/// Represents a connectable observable that caches the most recent value from a source observable.
/// This class is designed to encapsulate asynchronous lookups for value updates,
/// returning the latest known value to subscribers as soon as it becomes available.
/// </summary>
/// <typeparam name="T">The type of elements in the sequence.</typeparam>
sealed class ReadThroughCacheConnectedObservable<T> : ObservableBase<T>, IConnectableObservable<T>
{
    private readonly IConnectableObservable<T> _source;
    private IObservable<T> _cache = Observable.Empty<T>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ReadThroughCacheConnectedObservable{T}"/> class with the specified source observable.
    /// </summary>
    /// <param name="source">The source observable whose values are to be cached and replayed to subscribers.</param>
    [DebuggerStepThrough]
    internal ReadThroughCacheConnectedObservable(IObservable<T> source)
    {
        _source = source
            .Do(next => _cache = Observable.Return(next))
            .Publish();
    }



    /// <summary>
    /// Subscribes the given observer to the observable sequence.
    /// </summary>
    /// <param name="observer">The observer that will receive notifications from the observable sequence.</param>
    /// <returns>A disposable object that can be used to unsubscribe the observer from the observable sequence.</returns>
    protected override IDisposable SubscribeCore(IObserver<T> observer)
    {
        return _cache
            .Concat(_source)
            .Subscribe(observer);
    }

    /// <summary>
    /// Connects the observable to its source, allowing values to be published to subscribers.
    /// </summary>
    /// <returns>A disposable object that can be used to disconnect the observable from its source, stopping value publication.</returns>
    [DebuggerStepThrough]
    public IDisposable Connect() => _source.Connect();
}