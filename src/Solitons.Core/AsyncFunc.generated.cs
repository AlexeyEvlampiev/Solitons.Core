using System;
using System.Threading.Tasks;
using System.Reactive;
using System.Diagnostics;
namespace Solitons; 

/// <summary>
/// Encapsulates an asynchronous method that has 2 parameters and returns a value of the type specified by the TResult parameter.
/// </summary> 
/// <typeparam name="T1">
/// The type of the first parameter of the method that this delegate encapsulates.
/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
///</typeparam> 
/// <typeparam name="T2">
/// The type of the second parameter of the method that this delegate encapsulates.
/// This type parameter is contravariant. That is, you can use either the type you specified or any type that is less derived.
///</typeparam> 
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, TResult>(T1 arg1, T2 arg2);
 

/// <summary>
/// Encapsulates an asynchronous method that has 3 parameters and returns a value of the type specified by the TResult parameter.
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
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, TResult>(T1 arg1, T2 arg2, T3 arg3);
 

/// <summary>
/// Encapsulates an asynchronous method that has 4 parameters and returns a value of the type specified by the TResult parameter.
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
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
 

/// <summary>
/// Encapsulates an asynchronous method that has 5 parameters and returns a value of the type specified by the TResult parameter.
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
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg5">The fifth parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
 

/// <summary>
/// Encapsulates an asynchronous method that has 6 parameters and returns a value of the type specified by the TResult parameter.
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
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg5">The fifth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg6">The sixth parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
 

/// <summary>
/// Encapsulates an asynchronous method that has 7 parameters and returns a value of the type specified by the TResult parameter.
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
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg5">The fifth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg6">The sixth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg7">The seventh parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
 

/// <summary>
/// Encapsulates an asynchronous method that has 8 parameters and returns a value of the type specified by the TResult parameter.
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
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg5">The fifth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg6">The sixth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg7">The seventh parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg8">The eighth parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);
 

/// <summary>
/// Encapsulates an asynchronous method that has 9 parameters and returns a value of the type specified by the TResult parameter.
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
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg5">The fifth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg6">The sixth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg7">The seventh parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg8">The eighth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg9">The ninth parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);
 

/// <summary>
/// Encapsulates an asynchronous method that has 10 parameters and returns a value of the type specified by the TResult parameter.
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
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg5">The fifth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg6">The sixth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg7">The seventh parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg8">The eighth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg9">The ninth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg10">The tenth parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);
 

/// <summary>
/// Encapsulates an asynchronous method that has 11 parameters and returns a value of the type specified by the TResult parameter.
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
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg5">The fifth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg6">The sixth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg7">The seventh parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg8">The eighth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg9">The ninth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg10">The tenth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg11">The eleventh parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);
 

/// <summary>
/// Encapsulates an asynchronous method that has 12 parameters and returns a value of the type specified by the TResult parameter.
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
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg5">The fifth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg6">The sixth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg7">The seventh parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg8">The eighth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg9">The ninth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg10">The tenth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg11">The eleventh parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg12">The twelfth parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);
 

/// <summary>
/// Encapsulates an asynchronous method that has 13 parameters and returns a value of the type specified by the TResult parameter.
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
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg5">The fifth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg6">The sixth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg7">The seventh parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg8">The eighth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg9">The ninth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg10">The tenth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg11">The eleventh parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg12">The twelfth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg13">The thirteenth parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);
 

/// <summary>
/// Encapsulates an asynchronous method that has 14 parameters and returns a value of the type specified by the TResult parameter.
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
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg5">The fifth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg6">The sixth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg7">The seventh parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg8">The eighth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg9">The ninth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg10">The tenth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg11">The eleventh parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg12">The twelfth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg13">The thirteenth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg14">The fourteenth parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);
 

/// <summary>
/// Encapsulates an asynchronous method that has 15 parameters and returns a value of the type specified by the TResult parameter.
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
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg5">The fifth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg6">The sixth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg7">The seventh parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg8">The eighth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg9">The ninth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg10">The tenth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg11">The eleventh parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg12">The twelfth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg13">The thirteenth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg14">The fourteenth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg15">The fifteenth parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);
 

/// <summary>
/// Encapsulates an asynchronous method that has 16 parameters and returns a value of the type specified by the TResult parameter.
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
/// <typeparam name="TResult">
/// The type of the asynchronously return value of the method that this delegate encapsulates.
/// This type parameter is covariant. That is, you can use either the type you specified or any type that is more derived.
/// </typeparam> 
/// <param name="arg1">The first parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg2">The second parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg3">The third parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg4">The fourth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg5">The fifth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg6">The sixth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg7">The seventh parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg8">The eighth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg9">The ninth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg10">The tenth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg11">The eleventh parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg12">The twelfth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg13">The thirteenth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg14">The fourteenth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg15">The fifteenth parameter of the method that this delegate encapsulates.</param> 
/// <param name="arg16">The sixteenth parameter of the method that this delegate encapsulates.</param> 
/// <returns>The return value of the method that this delegate encapsulates.</returns>
public delegate Task<TResult> AsyncFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, in T10, in T11, in T12, in T13, in T14, in T15, in T16, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);
 

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
    public static AsyncFunc<T1, T2,Unit> Cast<T1, T2>(Action<T1, T2> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2) =>
    {
        action.Invoke(arg1, arg2);
        return Task.FromResult(Unit.Default);
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
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static AsyncFunc<T1, T2, T3,Unit> Cast<T1, T2, T3>(Action<T1, T2, T3> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3) =>
    {
        action.Invoke(arg1, arg2, arg3);
        return Task.FromResult(Unit.Default);
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
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static AsyncFunc<T1, T2, T3, T4,Unit> Cast<T1, T2, T3, T4>(Action<T1, T2, T3, T4> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4);
        return Task.FromResult(Unit.Default);
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
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static AsyncFunc<T1, T2, T3, T4, T5,Unit> Cast<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5);
        return Task.FromResult(Unit.Default);
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
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static AsyncFunc<T1, T2, T3, T4, T5, T6,Unit> Cast<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
        return Task.FromResult(Unit.Default);
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
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7,Unit> Cast<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        return Task.FromResult(Unit.Default);
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
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8,Unit> Cast<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        return Task.FromResult(Unit.Default);
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
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9,Unit> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        return Task.FromResult(Unit.Default);
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
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10,Unit> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        return Task.FromResult(Unit.Default);
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
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11,Unit> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        return Task.FromResult(Unit.Default);
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
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12,Unit> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        return Task.FromResult(Unit.Default);
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
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13,Unit> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        return Task.FromResult(Unit.Default);
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
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14,Unit> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        return Task.FromResult(Unit.Default);
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
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15,Unit> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        return Task.FromResult(Unit.Default);
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
	/// <param name="action"></param>
	/// <returns></returns>
	[DebuggerStepThrough]
    public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16,Unit> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> action) => [DebuggerStepThrough] (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16) =>
    {
        action.Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        return Task.FromResult(Unit.Default);
    };

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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2 , TResult> Cast<T1, T2, TResult>(Func<T1, T2, Task<TResult>> func) => new(func);
	 
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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2, T3 , TResult> Cast<T1, T2, T3, TResult>(Func<T1, T2, T3, Task<TResult>> func) => new(func);
	 
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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2, T3, T4 , TResult> Cast<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, Task<TResult>> func) => new(func);
	 
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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2, T3, T4, T5 , TResult> Cast<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, Task<TResult>> func) => new(func);
	 
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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2, T3, T4, T5, T6 , TResult> Cast<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, Task<TResult>> func) => new(func);
	 
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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7 , TResult> Cast<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> func) => new(func);
	 
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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8 , TResult> Cast<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> func) => new(func);
	 
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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9 , TResult> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TResult>> func) => new(func);
	 
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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10 , TResult> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TResult>> func) => new(func);
	 
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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11 , TResult> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TResult>> func) => new(func);
	 
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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12 , TResult> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TResult>> func) => new(func);
	 
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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13 , TResult> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TResult>> func) => new(func);
	 
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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14 , TResult> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TResult>> func) => new(func);
	 
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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15 , TResult> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TResult>> func) => new(func);
	 
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
	/// <param name="func"></param>
	/// <returns></returns>
	public static AsyncFunc<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16 , TResult> Cast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<TResult>> func) => new(func);
	 
}