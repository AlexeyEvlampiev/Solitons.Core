using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons;


/// <summary>
/// 
/// </summary>
/// <typeparam name="TResult"></typeparam>
/// <returns></returns>
public delegate Task<TResult> AsyncFunc<TResult>();

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TResult"></typeparam>
/// <param name="arg"></param>
/// <returns></returns>
public delegate Task<TResult> AsyncFunc<in T, TResult>(T arg);



/// <summary>
/// 
/// </summary>
public static partial class AsyncFunc
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static AsyncFunc<Unit> Cast(Action action) => [DebuggerStepThrough] () =>
    {
        action.Invoke();
        return Task.FromResult(Unit.Default);
    };

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static AsyncFunc<T,Unit> Cast<T>(Action<T> action) => [DebuggerStepThrough] (T arg) =>
    {
        action.Invoke(arg);
        return Task.FromResult(Unit.Default);
    };

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    [DebuggerNonUserCode]
    public static AsyncFunc<TResult> Cast<TResult>(Func<Task<TResult>> func) => new(func);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public static AsyncFunc<T, TResult> Cast<T, TResult>(Func<T, Task<TResult>> func) => new(func);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSignal"></typeparam>
    /// <param name="handler"></param>
    /// <param name="toRetrySignals"></param>
    /// <returns></returns>
    public static async Task<TResult> Invoke<TResult, TSignal>(
        AsyncFunc<TResult> handler,
        Func<IObservable<TResult>, IObservable<TSignal>> toRetrySignals)
    {
        var responses = new Subject<TResult>();
        var cts = new CancellationTokenSource();

        using var _ = responses
            .AsObservable()
            .Convert(toRetrySignals)
            .Subscribe(_ => { }, cts.Cancel);

        for(;;)
        {
            var result = await handler.Invoke();
            if(cts.IsCancellationRequested) return result;
            
            var shouldRetry = await Observable
                .Return(result)
                .Convert(toRetrySignals)
                .Take(1)
                .Any();
            if (!shouldRetry)
            {
                return result;
            }

            responses.OnNext(result);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSignal"></typeparam>
    /// <param name="handler"></param>
    /// <param name="toRetrySignals"></param>
    /// <returns></returns>
    public static async Task<TResult> Invoke<TResult, TSignal>(
        AsyncFunc<TResult> handler, 
        Func<IObservable<Exception>, IObservable<TSignal>> toRetrySignals)
    {
        var responses = new Subject<Exception>();
        var cts = new CancellationTokenSource();

        using var _ = responses
            .AsObservable()
            .Convert(toRetrySignals)
            .Subscribe(_ => { }, cts.Cancel);

        for (;;)
        {
            try
            {
                return await handler.Invoke();
            }
            catch (Exception e)
            {
                var shouldRetry = await Observable
                    .Return(e)
                    .Convert(toRetrySignals)
                    .Take(1)
                    .TakeWhile(_ => cts.IsCancellationRequested == false)
                    .Any();
                if (!shouldRetry)
                {
                    throw;
                }

                responses.OnNext(e);
            }
        }
    }

}



