using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons;



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
    [DebuggerNonUserCode]
    public static Func<Task> Wrap(Action action) => [DebuggerStepThrough] () =>
    {
        action.Invoke();
        return Task.CompletedTask;
    };

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="action"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static Func<T,Task> Wrap<T>(Action<T> action) => [DebuggerStepThrough] (T arg) =>
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
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<Task<TResult>> Wrap<TResult>(Func<Task<TResult>> func) => func;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    [DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T, Task<TResult>> Wrap<T, TResult>(Func<T, Task<TResult>> func) => func;

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSignal"></typeparam>
    /// <param name="handler"></param>
    /// <param name="toRetrySignals"></param>
    /// <returns></returns>
    internal static async Task<TResult> Invoke<TResult, TSignal>(
        Func<Task<TResult>> handler,
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
                .Select(_ => true)
                .FirstOrDefaultAsync();
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
    internal static async Task<TResult> Invoke<TResult, TSignal>(
        Func<Task<TResult>> handler, 
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
                    .Select(_ => true)
                    .TakeWhile(_ => cts.IsCancellationRequested == false)
                    .FirstOrDefaultAsync();
                if (!shouldRetry)
                {
                    throw;
                }

                responses.OnNext(e);
            }
        }
    }

}
