using System;
using System.Threading.Tasks;
using System.Reactive;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Runtime.CompilerServices;
namespace Solitons;

public static partial class AsyncFunc
{ 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2,Task> Wrap<T1, T2>(Action<T1, T2> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2) =>
    {
        action.Invoke(arg1, arg2);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2,Task<TResult>> Wrap<T1, T2, TResult>(Func<T1, T2,Task<TResult>> func) => func;

 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3,Task> Wrap<T1, T2, T3>(Action<T1, T2, T3> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3) =>
    {
        action.Invoke(arg1, arg2, arg3);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3,Task<TResult>> Wrap<T1, T2, T3, TResult>(Func<T1, T2, T3,Task<TResult>> func) => func;

 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4,Task> Wrap<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, T4,Task<TResult>> Wrap<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4,Task<TResult>> func) => func;

 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5,Task> Wrap<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, T4, T5,Task<TResult>> Wrap<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5,Task<TResult>> func) => func;

 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6,Task> Wrap<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, T4, T5, T6,Task<TResult>> Wrap<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6,Task<TResult>> func) => func;

 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7,Task> Wrap<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, T4, T5, T6, T7,Task<TResult>> Wrap<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7,Task<TResult>> func) => func;

 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8,Task> Wrap<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8,Task<TResult>> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8,Task<TResult>> func) => func;

 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,Task> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,Task<TResult>> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9,Task<TResult>> func) => func;

 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,Task> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,Task<TResult>> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,Task<TResult>> func) => func;

 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,Task> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,Task<TResult>> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,Task<TResult>> func) => func;

 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,Task> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,Task<TResult>> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,Task<TResult>> func) => func;

 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,Task> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,Task<TResult>> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,Task<TResult>> func) => func;

 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,Task> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,Task<TResult>> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,Task<TResult>> func) => func;

 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T15">
	/// The type of the fifteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,Task> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T15">
	/// The type of the fifteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,Task<TResult>> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,Task<TResult>> func) => func;

 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T15">
	/// The type of the fifteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T16">
	/// The type of the sixteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,Task> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        return Task.CompletedTask;
    };

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T15">
	/// The type of the fifteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T16">
	/// The type of the sixteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="func"></param>
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,Task<TResult>> Wrap<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,Task<TResult>> func) => func;

}

public static partial class Extensions
{ 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, Task<TResult>> WithRetryOnResult<T1, T2, TResult, TSignal>(
        this Func<T1, T2, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, Task<TResult>> WithRetryOnError<T1, T2, TResult, TSignal>(
        this Func<T1, T2, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, Task> WithRetryOnError<T1, T2, TSignal>(
        this Func<T1, T2, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, TResult>(
		this Func<T1, T2, Task<TResult>>  self,
		T1 arg1, T2 arg2) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2>(
		this Func<T1, T2, Task> self,
		T1 arg1, T2 arg2) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2).ToObservable());
	
	
	 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, T3, Task<TResult>> WithRetryOnResult<T1, T2, T3, TResult, TSignal>(
        this Func<T1, T2, T3, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, Task<TResult>> WithRetryOnError<T1, T2, T3, TResult, TSignal>(
        this Func<T1, T2, T3, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, T3, Task> WithRetryOnError<T1, T2, T3, TSignal>(
        this Func<T1, T2, T3, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2, T3 arg3)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, T3, TResult>(
		this Func<T1, T2, T3, Task<TResult>>  self,
		T1 arg1, T2 arg2, T3 arg3) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2, T3>(
		this Func<T1, T2, T3, Task> self,
		T1 arg1, T2 arg2, T3 arg3) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3).ToObservable());
	
	
	 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, T3, T4, Task<TResult>> WithRetryOnResult<T1, T2, T3, T4, TResult, TSignal>(
        this Func<T1, T2, T3, T4, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, Task<TResult>> WithRetryOnError<T1, T2, T3, T4, TResult, TSignal>(
        this Func<T1, T2, T3, T4, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, Task> WithRetryOnError<T1, T2, T3, T4, TSignal>(
        this Func<T1, T2, T3, T4, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, T3, T4, TResult>(
		this Func<T1, T2, T3, T4, Task<TResult>>  self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2, T3, T4>(
		this Func<T1, T2, T3, T4, Task> self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4).ToObservable());
	
	
	 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, T3, T4, T5, Task<TResult>> WithRetryOnResult<T1, T2, T3, T4, T5, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, Task<TResult>> WithRetryOnError<T1, T2, T3, T4, T5, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, Task> WithRetryOnError<T1, T2, T3, T4, T5, TSignal>(
        this Func<T1, T2, T3, T4, T5, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, T3, T4, T5, TResult>(
		this Func<T1, T2, T3, T4, T5, Task<TResult>>  self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2, T3, T4, T5>(
		this Func<T1, T2, T3, T4, T5, Task> self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5).ToObservable());
	
	
	 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, T3, T4, T5, T6, Task<TResult>> WithRetryOnResult<T1, T2, T3, T4, T5, T6, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, Task<TResult>> WithRetryOnError<T1, T2, T3, T4, T5, T6, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, Task> WithRetryOnError<T1, T2, T3, T4, T5, T6, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, T3, T4, T5, T6, TResult>(
		this Func<T1, T2, T3, T4, T5, T6, Task<TResult>>  self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2, T3, T4, T5, T6>(
		this Func<T1, T2, T3, T4, T5, T6, Task> self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6).ToObservable());
	
	
	 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> WithRetryOnResult<T1, T2, T3, T4, T5, T6, T7, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, Task> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, T3, T4, T5, T6, T7, TResult>(
		this Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>>  self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2, T3, T4, T5, T6, T7>(
		this Func<T1, T2, T3, T4, T5, T6, T7, Task> self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7).ToObservable());
	
	
	 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> WithRetryOnResult<T1, T2, T3, T4, T5, T6, T7, T8, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>>  self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, Task> self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8).ToObservable());
	
	
	 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TResult>> WithRetryOnResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TResult>> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TResult>>  self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task> self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9).ToObservable());
	
	
	 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TResult>> WithRetryOnResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TResult>> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <param name="arg10"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TResult>>  self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <param name="arg10"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task> self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10).ToObservable());
	
	
	 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TResult>> WithRetryOnResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TResult>> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <param name="arg10"></param> 
	/// <param name="arg11"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TResult>>  self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <param name="arg10"></param> 
	/// <param name="arg11"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task> self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11).ToObservable());
	
	
	 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TResult>> WithRetryOnResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TResult>> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <param name="arg10"></param> 
	/// <param name="arg11"></param> 
	/// <param name="arg12"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TResult>>  self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <param name="arg10"></param> 
	/// <param name="arg11"></param> 
	/// <param name="arg12"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task> self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12).ToObservable());
	
	
	 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TResult>> WithRetryOnResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TResult>> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <param name="arg10"></param> 
	/// <param name="arg11"></param> 
	/// <param name="arg12"></param> 
	/// <param name="arg13"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TResult>>  self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <param name="arg10"></param> 
	/// <param name="arg11"></param> 
	/// <param name="arg12"></param> 
	/// <param name="arg13"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task> self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13).ToObservable());
	
	
	 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TResult>> WithRetryOnResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TResult>> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <param name="arg10"></param> 
	/// <param name="arg11"></param> 
	/// <param name="arg12"></param> 
	/// <param name="arg13"></param> 
	/// <param name="arg14"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TResult>>  self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <param name="arg10"></param> 
	/// <param name="arg11"></param> 
	/// <param name="arg12"></param> 
	/// <param name="arg13"></param> 
	/// <param name="arg14"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task> self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14).ToObservable());
	
	
	 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T15">
	/// The type of the fifteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TResult>> WithRetryOnResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T15">
	/// The type of the fifteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TResult>> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T15">
	/// The type of the fifteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <param name="arg10"></param> 
	/// <param name="arg11"></param> 
	/// <param name="arg12"></param> 
	/// <param name="arg13"></param> 
	/// <param name="arg14"></param> 
	/// <param name="arg15"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TResult>>  self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T15">
	/// The type of the fifteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <param name="arg10"></param> 
	/// <param name="arg11"></param> 
	/// <param name="arg12"></param> 
	/// <param name="arg13"></param> 
	/// <param name="arg14"></param> 
	/// <param name="arg15"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task> self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15).ToObservable());
	
	
	 

	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T15">
	/// The type of the fifteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T16">
	/// The type of the sixteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult">The return value of the method that the casted delegate encapsulates.</typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerNonUserCode]
	public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<TResult>> WithRetryOnResult<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<TResult>> self,
        Func<IObservable<TResult>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16))
                .WithRetryOnResult(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T15">
	/// The type of the fifteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T16">
	/// The type of the sixteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <typeparam name="TSignal"></typeparam>
	/// <param name="self"></param>
	/// <param name="signalFactory"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<TResult>> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<TResult>>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task<TResult> Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }


	[DebuggerStepThrough]
    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task> WithRetryOnError<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TSignal>(
        this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task>  self,
        Func<IObservable<Exception>, IObservable<TSignal>> signalFactory)
    {
        return Invoke;
        [DebuggerStepThrough]
        Task Invoke(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16)
        {
            return AsyncFunc
                .Wrap([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16))
                .WithRetryOnError(signalFactory)
                .Invoke();
        }
    }
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T15">
	/// The type of the fifteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T16">
	/// The type of the sixteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="TResult"></typeparam>
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <param name="arg10"></param> 
	/// <param name="arg11"></param> 
	/// <param name="arg12"></param> 
	/// <param name="arg13"></param> 
	/// <param name="arg14"></param> 
	/// <param name="arg15"></param> 
	/// <param name="arg16"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<TResult> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<TResult>>  self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16).ToObservable());
	
	
	/// <summary>
	/// 
	/// </summary> 
	/// <typeparam name="T1">
	/// The type of the first parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T2">
	/// The type of the second parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T3">
	/// The type of the third parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T4">
	/// The type of the fourth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T5">
	/// The type of the fifth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T6">
	/// The type of the sixth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T7">
	/// The type of the seventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T8">
	/// The type of the eighth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T9">
	/// The type of the ninth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T10">
	/// The type of the tenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T11">
	/// The type of the eleventh parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T12">
	/// The type of the twelfth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T13">
	/// The type of the thirteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T14">
	/// The type of the fourteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T15">
	/// The type of the fifteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <typeparam name="T16">
	/// The type of the sixteenth parameter of the method that this delegate encapsulates.
	/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
	///</typeparam> 
	/// <param name="self"></param> 
	/// <param name="arg1"></param> 
	/// <param name="arg2"></param> 
	/// <param name="arg3"></param> 
	/// <param name="arg4"></param> 
	/// <param name="arg5"></param> 
	/// <param name="arg6"></param> 
	/// <param name="arg7"></param> 
	/// <param name="arg8"></param> 
	/// <param name="arg9"></param> 
	/// <param name="arg10"></param> 
	/// <param name="arg11"></param> 
	/// <param name="arg12"></param> 
	/// <param name="arg13"></param> 
	/// <param name="arg14"></param> 
	/// <param name="arg15"></param> 
	/// <param name="arg16"></param> 
	/// <returns></returns>
	[DebuggerNonUserCode, MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IObservable<Unit> ToObservable<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(
		this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task> self,
		T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) =>
		Observable.Defer([DebuggerStepThrough]() => self.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16).ToObservable());
	
	
	 
}