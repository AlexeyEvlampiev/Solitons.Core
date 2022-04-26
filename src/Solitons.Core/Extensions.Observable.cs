using System;
using System.Diagnostics.CodeAnalysis;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;

namespace Solitons
{
    public static partial class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IObservable<T> Skip<T>(this IObservable<T> self, Func<T, bool> predicate)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return self.Where(item => false == predicate.Invoke(item));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public static IObservable<T> Connect<T>(this IObservable<T> self, CancellationToken cancellation) where T : class
        {
            cancellation.ThrowIfCancellationRequested();

            var dispatcher = new BehaviorSubject<T?>(default);
            cancellation.Register(dispatcher.OnCompleted);

            self.Subscribe(Observer.Create<T>(dispatcher.OnNext, dispatcher.OnError), cancellation);
            IObservable<T> stream = dispatcher
                .Skip(item=> item is null)!;

            return Observable.Create<T>(o =>
            {
                var subscription = stream.Subscribe(o);
                cancellation.Register(subscription.Dispose);
                return subscription;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="initialValue"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public static IObservable<T> Connect<T>(this IObservable<T> self, [DisallowNull] T initialValue, CancellationToken cancellation)
        {
            if (initialValue == null) throw new ArgumentNullException(nameof(initialValue));
            cancellation.ThrowIfCancellationRequested();

            var dispatcher = new BehaviorSubject<T?>(initialValue);
            cancellation.Register(dispatcher.OnCompleted);

            
            self.Subscribe(Observer.Create<T>(dispatcher.OnNext, dispatcher.OnError), cancellation);
            IObservable<T> stream = dispatcher
                .Skip(item => item is null)!;

            return Observable.Create<T>(o =>
            {
                var subscription = stream.Subscribe(o);
                cancellation.Register(subscription.Dispose);
                return subscription;
            });
        }
    }
}
