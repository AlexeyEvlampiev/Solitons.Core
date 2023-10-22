using System;
using System.Diagnostics;
using System.Reactive.Subjects;

namespace Solitons.Caching;

/// <summary>
/// Provides a mechanism to cache and share the most recent value of an observable sequence.
/// This class encapsulates the logic of creating a connectable observable that retains the latest value from a source observable,
/// allowing subscribers to receive the latest known value as soon as they subscribe.
/// </summary>
/// <example>
/// This example demonstrates how to use the <see cref="ReadThroughCache"/> to cache and share the latest USD to INR exchange rate.
/// <code>
/// using System;
/// using System.Net.Http;
/// using System.Reactive.Linq;
/// using System.Reactive.Threading.Tasks;
/// using Solitons.Caching;
/// using Newtonsoft.Json.Linq;
///
/// public class Program
/// {
///     public static async void Main(string[] args)
///     {
///         // Assume there's a free API endpoint that provides the latest USD to INR exchange rate
///         var exchangeRateObservable = Observable
///             .Interval(TimeSpan.FromHours(1))  // Refresh the rate every hour
///             .SelectMany(_ => new HttpClient().GetStringAsync("https://free-api.example.com/exchange-rate/usd/inr").ToObservable())
///             .Select(json => JObject.Parse(json)["rate"].Value&lt;decimal&gt;());
///
///         var cachedExchangeRateObservable = ReadThroughCache.Publish(exchangeRateObservable);
///         using (var connection = cachedExchangeRateObservable.Connect())
///         {
///             // Subscribers receive the latest known exchange rate as soon as they subscribe
///             cachedExchangeRateObservable.Subscribe(rate => Console.WriteLine("Subscriber 1 received rate: " + rate));
///             await Task.Delay(TimeSpan.FromMinutes(30));  // Simulate some delay
///             cachedExchangeRateObservable.Subscribe(rate => Console.WriteLine("Subscriber 2 received rate: " + rate));
///         }
///     }
/// }
/// </code>
/// </example>
public static class ReadThroughCache
{
    /// <summary>
    /// Creates a connectable observable from the specified source observable that caches the most recent value.
    /// If the source is already a <see cref="ReadThroughCacheConnectedObservable{T}"/>, it is returned directly.
    /// Otherwise, a new <see cref="ReadThroughCacheConnectedObservable{T}"/> is created and returned.
    /// </summary>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="source">The source observable whose values are to be cached and replayed to subscribers.</param>
    /// <returns>
    /// A connectable observable that caches the most recent value from the source observable.
    /// Subscribers receive the latest known value as soon as they subscribe.
    /// </returns>
    [DebuggerStepThrough]
    public static IConnectableObservable<T> Publish<T>(IObservable<T> source)
    {
        if (source is ReadThroughCacheConnectedObservable<T> connected)
        {
            return connected;
        }

        return new ReadThroughCacheConnectedObservable<T>(source);
    }
}