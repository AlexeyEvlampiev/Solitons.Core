using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Solitons;

public static partial class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSignal"></typeparam>
    /// <param name="self"></param>
    /// <param name="signalFactory"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static AsyncFunc<TResult> WithRetry<TResult, TSignal>(
        this AsyncFunc<TResult> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return [DebuggerStepThrough] () => AsyncFunc.Invoke(self, signalFactory);
    }


    public static AsyncFunc<T, TResult> WithRetry<T, TResult, TSignal>(
        this AsyncFunc<T, TResult> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T args)
        {
            return AsyncFunc
                .Cast(() => self.Invoke(args))
                .WithRetry(signalFactory)
                .Invoke();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSignal"></typeparam>
    /// <param name="self"></param>
    /// <param name="signalFactory"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static AsyncFunc<TResult> WithRetryOnError<TResult, TSignal>(
        this AsyncFunc<TResult> self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return [DebuggerStepThrough] () => AsyncFunc.Invoke(self, signalFactory);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSignal"></typeparam>
    /// <param name="self"></param>
    /// <param name="arg"></param>
    /// <param name="signalFactory"></param>
    /// <returns></returns>
    [DebuggerStepThrough]
    public static AsyncFunc<TResult> WithRetry<T, TResult, TSignal>(
        this AsyncFunc<T, TResult> self, 
        T arg,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return AsyncFunc
            .Cast([DebuggerStepThrough] () => self.Invoke(arg))
            .WithRetry(signalFactory);
    }
}