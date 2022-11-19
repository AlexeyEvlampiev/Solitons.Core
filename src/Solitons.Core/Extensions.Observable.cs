using Solitons.Reactive;
using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;


namespace Solitons;

public static partial class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    public static IConnectableObservable<T> Publish<T>(this IObservable<T> source, Action<PublicationOptions<T>> config)
    {
        var options = new PublicationOptions<T>();
        config.Invoke(options);
        return new ReadThroughCacheConnectedObservable<T>(source, options);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="self"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [DebuggerNonUserCode]
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
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IObservable<T> SkipNulls<T>(this IObservable<T?> self) where T : class
    {
        if (self == null) throw new ArgumentNullException(nameof(self));
        return self.Skip(_ => _ is null)!;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TSignal"></typeparam>
    /// <param name="source"></param>
    /// <param name="toSignal"></param>
    /// <returns></returns>
    //[DebuggerNonUserCode]
    public static IObservable<TSource> RetryWhenSignaled<TSource, TSignal>(
        this IObservable<TSource> source, 
        Func<IObservable<TSource>, IObservable<TSignal>> toSignal)
    {
        var monitor = new Subject<TSource>();
        var signals = monitor
            .Convert(toSignal);

        int attempt = 0;
        return source
            .SelectMany(item => item
                .Convert(Observable.Return)
                .Convert(toSignal)
                .Any()
                .Select(faulted => new
                {
                    Item = item,
                    Faulted = faulted
                }))
            .Select(i=> i.Item);
    }
}