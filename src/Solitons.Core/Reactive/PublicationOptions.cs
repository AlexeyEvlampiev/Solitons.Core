using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Solitons.Reactive
{
    /// <summary>
    /// 
    /// </summary>
    public static class PublicationOptions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        public static void ReadThroughCache<T>(PublicationOptions<T> options)
        {
            options.ReadThroughCache();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        /// <param name="cacheExpirationInterval"></param>
        public static void ReadThroughCache<T>(PublicationOptions<T> options, TimeSpan cacheExpirationInterval)
        {
            options.ReadThroughCache(value => Observable.Interval(cacheExpirationInterval));
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PublicationOptions<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Func<T, IObservable<Unit>> _expirationSignalFactory = (T _) => Observable.Never<Unit>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PublicationOptions<T> ReadThroughCache()
        {
            _expirationSignalFactory = (T _) => Observable.Never<Unit>();
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="cacheExpirationPolicy"></param>
        /// <returns></returns>
        public PublicationOptions<T> ReadThroughCache<TResult>(Func<T, IObservable<TResult>> cacheExpirationPolicy)
        {
            _expirationSignalFactory = (value) => cacheExpirationPolicy
                .Invoke(value)
                .Select(_ => Unit.Default);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheExpirationInterval"></param>
        /// <returns></returns>
        public PublicationOptions<T> ReadThroughCache(TimeSpan cacheExpirationInterval)
        {
            _expirationSignalFactory = (_) => Task
                .Delay(cacheExpirationInterval)
                .ToObservable()
                .Select(_ => Unit.Default);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <returns></returns>
        internal IObservable<Unit> GetCacheExpirationSignal(T next)
        {
            return _expirationSignalFactory
                .Invoke(next)
                .FirstOrDefaultAsync()
                .ToTask()
                .ToObservable();
        }
    }
}
