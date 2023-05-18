using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Reactive;


namespace Solitons;

public static partial class Extensions
{
    /// <summary>
    /// Creates a new Observable that applies a retry policy to the source Observable.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <typeparam name="TSignal">The type of elements in the signal sequence.</typeparam>
    /// <param name="source">The source Observable to apply the retry policy to.</param>
    /// <param name="trigger">A function that defines the retry policy in terms of an observable sequence.</param>
    /// <returns>A new Observable that applies the retry policy to the source Observable.</returns>
    public static IObservable<T> WithRetryPolicy<T, TSignal>(
        this IObservable<T> source,
        Func<IObservable<RetryPolicyArgs>, IObservable<TSignal>> trigger)
    {
        return new RetryPolicyObservable<T>(source, Handler);
        [DebuggerStepThrough]
        Task<bool> Handler(RetryPolicyArgs args) => 
            trigger(Observable.Return(args))
                .Any()
                .ToTask();
    }

    /// <summary>
    /// Delays the emission of items in an <see cref="IObservable{T}"/> sequence by the specified duration.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">The source <see cref="IObservable{T}"/> sequence.</param>
    /// <param name="milliseconds">The duration to delay each item in milliseconds.</param>
    /// <param name="cancellation">The <see cref="CancellationToken"/> that can be used to cancel the delay.</param>
    /// <returns>An <see cref="IObservable{T}"/> sequence that delays the emission of items.</returns>
    public static IObservable<T> Delay<T>(this IObservable<T> source, int milliseconds, CancellationToken cancellation = default)
    {
        return source
            .SelectMany(async item =>
            {
                await Task
                    .Delay(milliseconds, cancellation)
                    .ConfigureAwait(false);
                return item;
            });
    }

    /// <summary>
    /// Delays the emission of items in an <see cref="IObservable{T}"/> sequence based on a delay selector function.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">The source <see cref="IObservable{T}"/> sequence.</param>
    /// <param name="delaySelector">A function that provides the delay duration based on the sequence number of each item.</param>
    /// <param name="cancellation">The <see cref="CancellationToken"/> that can be used to cancel the delay.</param>
    /// <returns>An <see cref="IObservable{T}"/> sequence that delays the emission of items.</returns>
    public static IObservable<T> Delay<T>(this IObservable<T> source, Func<int, int> delaySelector, CancellationToken cancellation = default)
    {
        return source
            .SelectMany(async (item, sequenceNumber) =>
            {
                var delayDuration = delaySelector(sequenceNumber);
                await Task
                    .Delay(delayDuration , cancellation)
                    .ConfigureAwait(false);
                return item;
            });
    }

    /// <summary>
    /// Filters the elements of an observable sequence based on a predicate.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The observable sequence to filter.</param>
    /// <param name="predicate">A function to test each element for a condition. If the condition returns true, the element is excluded from the observable sequence.</param>
    /// <returns>
    /// An observable sequence that contains elements from the input sequence that do not satisfy the condition specified by <paramref name="predicate"/>.
    /// </returns>
    /// <remarks>
    /// This method is similar to the Where extension method, but it inverts the predicate, effectively implementing an "Except" operation.
    /// </remarks>
    [DebuggerNonUserCode]
    public static IObservable<T> Except<T>(this IObservable<T> source, Func<T, bool> predicate)
    {
        return source.Where(_ => predicate.Invoke(_) == false);
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