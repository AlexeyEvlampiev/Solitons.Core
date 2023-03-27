using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace Solitons.Reactive;

/// <summary>
/// Provides a set of extension methods for working with observables.
/// </summary>
public static partial class FluentObservable
{
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