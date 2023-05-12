using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Reactive;

/// <summary>
/// Provides a set of extension methods for working with observables.
/// </summary>
public static partial class FluentObservable
{
    /// <summary>
    /// Creates an observable sequence that completes after a specified relative due time.
    /// </summary>
    /// <param name="milliseconds">The delay time, in milliseconds, before the observable sequence completes.</param>
    /// <param name="cancellation">The cancellation token that can be used to cancel the delay.</param>
    /// <returns>An observable sequence that completes after a delay.</returns>
    [DebuggerNonUserCode]
    public static IObservable<Unit> Delay(int milliseconds, CancellationToken cancellation) =>
        Task.Delay(milliseconds, cancellation)
            .ToObservable();



    /// <summary>
    /// Returns an observable sequence that contains the specified items.
    /// </summary>
    /// <typeparam name="T">The type of the items in the sequence.</typeparam>
    /// <param name="items">The items to include in the sequence.</param>
    /// <returns>An observable sequence that contains the specified items.</returns>
    [DebuggerNonUserCode]
    public static IObservable<T> SelectMany<T>(params T[] items)
    {
        return items.ToObservable();
    }

    /// <summary>
    /// Returns an observable sequence that contains the specified items, using the specified scheduler for notifications.
    /// </summary>
    /// <typeparam name="T">The type of the items in the sequence.</typeparam>
    /// <param name="scheduler">The scheduler to use for notifications.</param>
    /// <param name="items">The items to include in the sequence.</param>
    /// <returns>An observable sequence that contains the specified items, using the specified scheduler for notifications.</returns>
    [DebuggerNonUserCode]
    public static IObservable<T> SelectMany<T>(IScheduler scheduler, params T[] items)
    {
        return items.ToObservable(scheduler);
    }

    /// <summary>
    /// Returns an observable sequence that, when subscribed to, will execute the specified asynchronous task and return a single Unit value upon completion.
    /// </summary>
    /// <param name="handler">The asynchronous task to execute.</param>
    /// <returns>An observable sequence with a single Unit value.</returns>
    [DebuggerStepThrough]
    public static IObservable<Unit> Defer(Func<Task> handler) => Observable
        .Defer([DebuggerStepThrough] () => handler.Invoke().ToObservable());

    /// <summary>
    /// Returns an observable sequence that, when subscribed to, will execute the specified asynchronous task and return a single value of the specified type upon completion.
    /// </summary>
    /// <typeparam name="T">The type of the value to be returned.</typeparam>
    /// <param name="handler">The asynchronous task to execute.</param>
    /// <returns>An observable sequence with a single value of type T.</returns>
    [DebuggerStepThrough]
    public static IObservable<T> Defer<T>(Func<Task<T>> handler) => Observable
        .Defer([DebuggerStepThrough] () => handler.Invoke().ToObservable());

}