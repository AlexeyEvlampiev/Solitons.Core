using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Solitons.Caching
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReadThroughCacheObservable<T> : ObservableBase<T> where T : class
    {
        private T? _snapshot;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IObservable<T> _updates;

        protected ReadThroughCacheObservable(IObservable<Unit> autoRefreshTrigger)
        {
            _updates = autoRefreshTrigger
                .SelectMany(_ => GetAsync())
                .Do(update => _snapshot = update, OnFetchError)
                .OnErrorResumeNext(Observable.Empty<T>());
        }

        protected abstract Task<T> GetAsync();

        protected virtual void OnFetchError(Exception error)
        {
            Trace.TraceError(error.ToString());
        }

        protected sealed override IDisposable SubscribeCore(IObserver<T> observer)
        {
            if (_snapshot != null)
            {
                observer.OnNext(_snapshot);
                return _updates.Subscribe(observer);
            }

            return GetAsync()
                .ToObservable()
                .Do(update => _snapshot = update, OnFetchError)
                .OnErrorResumeNext(Observable.Empty<T>())
                .Merge(_updates)
                .Subscribe(observer);
        }
    }
}
