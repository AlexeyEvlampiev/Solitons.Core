using System;
using System.Diagnostics;
using System.Reactive.Subjects;

namespace Solitons.Caching;


/// <summary>
/// Provides a mechanism to cache and share the most recent value of an observable sequence.
/// This class encapsulates the logic of creating a connectable observable that retains the latest value from a source observable,
/// allowing subscribers to receive the latest known value as soon as they subscribe.
/// </summary>
/// <remarks>
/// The <see cref="ReadThroughCache"/> class promotes a separation of concerns between data retrieval and data consumption.
/// By encapsulating the pulling logic, it allows developers to take full control over the data retrieval process, including the application
/// of retry mechanisms and error handling strategies. This separation ensures that connectivity errors can be handled gracefully,
/// without propagating them to the consumers, thereby enhancing the robustness and resilience of the software.
///
/// Moreover, this approach aligns with the principles of the CAP theorem, enabling developers to make informed trade-offs between
/// consistency, availability, and partition tolerance, based on the specific requirements of their applications. For instance,
/// in scenarios where availability is prioritized, the <see cref="ReadThroughCache"/> can continue delivering the last known value
/// from the cache, even in the face of network partitions or other connectivity issues.
/// </remarks>
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
///         var cachedExchangeRateObservable = CacheWithExpiration.Publish(exchangeRateObservable);
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
    /// </summary>
    /// <remarks>
    /// This method encapsulates the creation of a <see cref="IConnectableObservable{T}"/>, allowing developers to focus on
    /// the pulling logic and error handling strategies for the source observable. By controlling the pulling logic, developers can
    /// implement desired retry mechanisms, handle connectivity errors gracefully, and decide whether or not to propagate errors,
    /// based on the specific needs and resilience requirements of their applications.
    ///
    /// Moreover, the <see cref="ReadThroughCache"/> class's ability to cache and replay the latest known value promotes a level of
    /// availability and resilience, aligning with the CAP theorem's considerations. It enables applications to maintain a level of
    /// service, even under adverse conditions such as network partitions or source observable failures, by continuing to provide
    /// the last known value to subscribers.
    /// </remarks>
    /// <typeparam name="T">The type of elements in the sequence.</typeparam>
    /// <param name="source">The source observable whose values are to be cached and replayed to subscribers.</param>
    /// <returns>
    /// A connectable observable that retains the most recent value from the source observable.
    /// This enables subscribers to receive the latest known value as soon as they subscribe.
    /// The returned observable also allows the calling code to control when the source observable is connected,
    /// thus providing control over the pulling logic, error handling, and data retrieval process.
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