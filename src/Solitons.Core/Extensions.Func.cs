using System;
using System.Diagnostics;
using System.Reactive;
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
    [DebuggerNonUserCode]
    public static Func<Task<TResult>> WithRetryOnResult<TResult, TSignal>(
        this Func<Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerNonUserCode]
        Task<TResult> Invoke()
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough] () => AsyncFunc.Invoke(self, signalFactory))
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
    [DebuggerNonUserCode]
    public static Func<Task<TResult>> WithRetryOnError<TResult, TSignal>(
        this Func<Task<TResult>> self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerNonUserCode]
        Task<TResult> Invoke()
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough] () => AsyncFunc.Invoke(self, signalFactory))
                .Invoke();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSignal"></typeparam>
    /// <param name="self"></param>
    /// <param name="signalFactory"></param>
    /// <returns></returns>
    [DebuggerNonUserCode]
    public static Func<Task> WithRetryOnError<TSignal>(
        this Func<Task> self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;

        [DebuggerStepThrough]
        async Task<Unit> GetUnitAsync()
        {
            await self.Invoke();
            return Unit.Default;
        }

        [DebuggerStepThrough]
        Task Invoke()
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough] () => AsyncFunc.Invoke(GetUnitAsync, signalFactory))
                .Invoke();
        }
    }



    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <typeparam name="TSignal"></typeparam>
    /// <param name="self"></param>
    /// <param name="signalFactory"></param>
    /// <returns></returns>
    public static Func<T, Task<TResult>> WithRetryOnResult<T, TResult, TSignal>(
        this Func<T, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T args)
        {
            return AsyncFunc
                .Wrap(() => self.Invoke(args))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
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
    public static Func<T, Task<TResult>> WithRetryOnResult<T, TResult, TSignal>(
        this Func<T, Task<TResult>> self,
        T arg,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T args)
        {
            return AsyncFunc
                .Wrap(() => self.Invoke(args))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
}