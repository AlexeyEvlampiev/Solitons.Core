using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons;

public static partial class Extensions
{
    /// <summary>
    /// Determines whether the specified HTTP status code is a 4xx client error.
    /// </summary>
    /// <param name="self">The HTTP status code.</param>
    /// <returns>
    ///   <c>true</c> if the specified HTTP status code is a client error; otherwise, <c>false</c>.
    /// </returns>
    [DebuggerNonUserCode]
    public static bool IsClientError(this HttpStatusCode self)
    {
        return 4 == (int)self / 100;
    }

    /// <summary>
    /// Sends an HTTP request asynchronously.
    /// </summary>
    /// <param name="self">The HttpClient instance.</param>
    /// <param name="requestFactory">The request factory function.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>The HTTP response message.</returns>
    [DebuggerStepThrough]
    public static async Task<HttpResponseMessage> SendAsync(
        this HttpClient self,
        Func<HttpRequestMessage> requestFactory,
        CancellationToken cancellation = default)
    {
        using var request = requestFactory.Invoke();
        return await self.SendAsync(request, cancellation);
    }

    /// <summary>
    /// Sends an HTTP request asynchronously with retry logic.
    /// </summary>
    /// <typeparam name="T">The type of the items in the retry trigger sequence.</typeparam>
    /// <param name="self">The HttpClient instance.</param>
    /// <param name="requestFactory">The request factory function.</param>
    /// <param name="retryTriggerFactory">The factory function that creates the retry trigger sequence.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>The HTTP response message.</returns>
    /// <remarks>
    /// This method sends an HTTP request and retries it if the response is not successful.
    /// It uses a factory function to create the retry trigger sequence, allowing you to implement custom retry logic.
    /// Note that the request factory function is invoked for each retry attempt, ensuring that a new request message is used each time.
    /// </remarks>
    public static async Task<HttpResponseMessage> SendAsync<T>(
        this HttpClient self,
        Func<HttpRequestMessage> requestFactory,
        Func<IObservable<HttpResponseMessage>, IObservable<T>> retryTriggerFactory,
        CancellationToken cancellation = default)
    {
        var response = await self.SendAsync(requestFactory, cancellation);
        if (response.IsSuccessStatusCode || response.StatusCode.IsClientError())
        {
            return response;
        }

        var failures = new BehaviorSubject<HttpResponseMessage>(response);
        var retryTriggers = retryTriggerFactory.Invoke(failures);


        return await retryTriggers
            .SelectMany(_ => self.SendAsync(requestFactory, cancellation))
            .Synchronize()
            .Do(r =>
            {
                if (r.IsSuccessStatusCode ||
                    r.StatusCode.IsClientError())
                {
                    failures.OnCompleted();
                }
                else
                {
                    response.Dispose();
                    response = r;
                    failures
                        .NotifyOn(TaskPoolScheduler.Default)
                        .OnNext(r);
                }

            })
            .LastOrDefaultAsync() ?? response;
    }
}