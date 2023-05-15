using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

/// <summary>
/// A delegating handler that retries an HTTP request upon an unsuccessful HTTP response.
/// </summary>
public class HttpRetryPolicyHandler : HttpRetryOnExceptionHandler
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRetryPolicyHandler"/> class.
    /// </summary>
    /// <param name="innerHandler">The inner handler which is responsible for processing the HTTP response messages.</param>
    [DebuggerNonUserCode]
    public HttpRetryPolicyHandler(HttpMessageHandler innerHandler) 
        : base(innerHandler)
    {
    }


    /// <summary>
    /// Send an HTTP request with retries on unsuccessful response as an asynchronous operation.
    /// </summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellation">A cancellation token to cancel operation.</param>
    /// <returns>Returns <see cref="Task{TResult}"/>. The task object representing the asynchronous operation.</returns>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var response = await base.SendAsync(request, cancellation);
        var attempt = new HttpRequestAttempt(request, response);


        while (await EvaluateRetryNeedAsync(attempt, cancellation))
        {
            request = PrepareRetryRequest(attempt);
            if (ReferenceEquals(request, attempt.Request) == false)
            {
                attempt.Request.Dispose();
            }
            response.Dispose();

            response = await base.SendAsync(request, cancellation);
            attempt = attempt.Next(request, response);
        }

        return response;
    }

    /// <summary>
    /// Prepares the HTTP request for the next retry attempt.
    /// </summary>
    /// <param name="args">The details of the HTTP request attempt.</param>
    /// <returns>Returns the HTTP request message to be sent in the next retry attempt.</returns>
    protected virtual HttpRequestMessage PrepareRetryRequest(HttpRequestAttempt args) => args.Request;

    /// <summary>
    /// Evaluates if a retry is needed after receiving an HTTP response.
    /// </summary>
    /// <param name="attempt">The details of the HTTP request attempt.</param>
    /// <param name="cancellation">A cancellation token to cancel operation.</param>
    /// <returns>Returns <see cref="Task{TResult}"/>. The task object representing the asynchronous operation.</returns>
    protected virtual async Task<bool> EvaluateRetryNeedAsync(HttpRequestAttempt attempt, CancellationToken cancellation)
    {
        if (attempt.Response.IsSuccessStatusCode ||
            attempt.Response.StatusCode.IsClientError() ||
            cancellation.IsCancellationRequested)
        {
            return false;
        }

        var delayMilliseconds = attempt.Counter.Max(30) * 100;
        await Task.Delay(delayMilliseconds, cancellation);
        return true;
    }

    /// <summary>
    /// Represents an attempt to send an HTTP request.
    /// </summary>
    protected sealed record HttpRequestAttempt(HttpRequestMessage Request, HttpResponseMessage Response)
    {
        /// <summary>
        /// The number of attempts to send the HTTP request.
        /// </summary>
        public int Counter { get; init; } = 0;

        /// <summary>
        /// The time when the first attempt to send the HTTP request was made.
        /// </summary>
        public DateTime InitialRequestUtc { get; init; } = DateTime.UtcNow;

        /// <summary>
        /// Prepares for the next attempt to send the HTTP request.
        /// </summary>
        /// <param name="request">The HTTP request message to be sent in the next attempt.</param>
        /// <param name="response">The HTTP response message received in the current attempt.</param>
        /// <returns>Returns a new <see cref="HttpRequestAttempt"/> representing the next attempt.</returns>
        public HttpRequestAttempt Next(HttpRequestMessage request, HttpResponseMessage response)
        {
            return this with
            {
                Counter = Counter + 1, 
                Request = request, 
                Response = response
            };
        }
    }
}