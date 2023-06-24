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
}