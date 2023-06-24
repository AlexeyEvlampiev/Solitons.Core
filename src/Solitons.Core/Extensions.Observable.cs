using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
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
    [DebuggerStepThrough]
    public static IObservable<T> WithRetryPolicy<T, TSignal>(
        this IObservable<T> source,
        Func<RetryPolicyArgs, IObservable<TSignal>> trigger)
    {
        return new RetryPolicyObservable<T>(source, Handler);
        [DebuggerStepThrough]
        Task<bool> Handler(RetryPolicyArgs args) => 
            trigger(args)
                .Any()
                .ToTask();
    }

    /// <summary>
    /// Creates a new Observable that applies a retry policy to the source Observable using a specified handler function.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">The source Observable to apply the retry policy to.</param>
    /// <param name="handler">The function to use as a retry policy handler.</param>
    /// <returns>A new Observable that applies the retry policy to the source Observable.</returns>
    [DebuggerNonUserCode]
    public static IObservable<T> WithRetryPolicy<T>(
        this IObservable<T> source,
        Func<RetryPolicyArgs, Task<bool>> handler)
    {
        return new RetryPolicyObservable<T>(source, handler);
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
    /// Delays the emission of items in an <see cref="IObservable{T}"/> sequence by the specified duration.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">The source <see cref="IObservable{T}"/> sequence.</param>
    /// <param name="span">The duration to delay each item.</param>
    /// <param name="cancellation">The <see cref="CancellationToken"/> that can be used to cancel the delay.</param>
    /// <returns>An <see cref="IObservable{T}"/> sequence that delays the emission of items.</returns>
    public static IObservable<T> Delay<T>(this IObservable<T> source, TimeSpan span, CancellationToken cancellation = default)
    {
        return source
            .SelectMany(async item =>
            {
                await Task
                    .Delay(span, cancellation)
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
    [DebuggerStepThrough]
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
    /// Delays the emission of items in an <see cref="IObservable{T}"/> sequence based on a delay selector function.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source sequence.</typeparam>
    /// <param name="source">The source <see cref="IObservable{T}"/> sequence.</param>
    /// <param name="delaySelector">A function that provides the delay duration based on the sequence number of each item.</param>
    /// <param name="cancellation">The <see cref="CancellationToken"/> that can be used to cancel the delay.</param>
    /// <returns>An <see cref="IObservable{T}"/> sequence that delays the emission of items.</returns>
    [DebuggerStepThrough]
    public static IObservable<T> Delay<T>(this IObservable<T> source, Func<int, TimeSpan> delaySelector, CancellationToken cancellation = default)
    {
        return source
            .SelectMany(async (item, sequenceNumber) =>
            {
                var delayDuration = delaySelector(sequenceNumber);
                await Task
                    .Delay(delayDuration, cancellation)
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

    /// <summary>
    /// Converts an <see cref="IAsyncEnumerable{T}"/> to an <see cref="IObservable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to observe.</typeparam>
    /// <param name="source">The <see cref="IAsyncEnumerable{T}"/> source.</param>
    /// <returns>An <see cref="IObservable{T}"/> that observers can subscribe to.</returns>
    /// <remarks>
    /// If an exception occurs during the iteration over the IAsyncEnumerable, the exception will be passed to the observer.
    /// </remarks>
    [DebuggerStepThrough]
    public static IObservable<T> ToObservable<T>(this IAsyncEnumerable<T> source)
    {
        return Observable.Create<T>(async (observer, cancellation) =>
        {
            try
            {
                await foreach (var item in source.WithCancellation(cancellation))
                {
                    observer.OnNext(item);
                }
                observer.OnCompleted();
            }
            catch (Exception e)
            {
                observer.OnError(e);
            }
        });
    }

    /// <summary>
    /// Converts an <see cref="IAsyncEnumerable{T}"/> to an <see cref="IObservable{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of objects to observe.</typeparam>
    /// <param name="source">The <see cref="IAsyncEnumerable{T}"/> source.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> used to signal cancellation of the iteration over the source.</param>
    /// <returns>An <see cref="IObservable{T}"/> that observers can subscribe to.</returns>
    /// <remarks>
    /// If an exception occurs during the iteration over the <see cref="IAsyncEnumerable{T}"/>, the exception will be passed to the observer.
    /// The iteration can be cancelled by the provided <see cref="CancellationToken"/> or by the observer's cancellation token.
    /// </remarks>
    [DebuggerStepThrough]
    public static IObservable<T> ToObservable<T>(
        this IAsyncEnumerable<T> source, 
        CancellationToken cancellation)
    {
        
        return Observable.Create<T>(async (observer, token) =>
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(token, cancellation);
            try
            {
                await foreach (var item in source.WithCancellation(cts.Token))
                {
                    observer.OnNext(item);
                }
                observer.OnCompleted();
            }
            catch (Exception e)
            {
                observer.OnError(e);
            }
        });
    }

    /// <summary>
    /// Returns an observable sequence that produces a single <see cref="Unit"/> value 
    /// when the source sequence either emits any items or does not emit any items 
    /// based on the provided value parameter.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The source observable sequence.</param>
    /// <param name="value">
    /// If true, the resulting sequence emits a <see cref="Unit"/> value when the source sequence emits any items.
    /// If false, the resulting sequence emits a <see cref="Unit"/> value when the source sequence does not emit any items.
    /// The default value is true.
    /// </param>
    /// <returns>
    /// An observable sequence that produces a single <see cref="Unit"/> value when the source sequence's emissions 
    /// match the provided value parameter.
    /// </returns>
    [DebuggerStepThrough]
    public static IObservable<Unit> WhenAnyIs<T>(this IObservable<T> source, bool value = true) => source
        .Any()
        .Where(_ => _ == value)
        .Select(_ => Unit.Default);
}