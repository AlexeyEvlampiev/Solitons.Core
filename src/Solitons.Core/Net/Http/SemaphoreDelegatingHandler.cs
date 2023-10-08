using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

/// <summary>
/// Provides a handler that throttles the number of simultaneous requests based on a specified count.
/// This is implemented as a decorator for an <see cref="HttpMessageHandler"/> where the handler's <see cref="SendAsync"/> method is invoked
/// only when the number of currently executing requests is below the threshold.
/// If the threshold is exceeded, the handler immediately returns a 'Service Unavailable' response.
/// </summary>
/// <remarks>
/// This class is designed to be used in a 'DelegatingHandler' pipeline and is typically used to enforce a limit on the maximum
/// number of concurrent requests that can be processed. This can be useful in scenarios where resource usage needs to be controlled.
/// </remarks>
public sealed class SemaphoreDelegatingHandler : DelegatingHandler
{
    private readonly TimeSpan _timeout;
    private readonly SemaphoreSlim _semaphore;

    /// <summary>
    /// Initializes a new instance of the <see cref="SemaphoreDelegatingHandler"/> class with a specified initial count and timeout.
    /// </summary>
    /// <param name="initialCount">The initial number of requests that can be processed concurrently.</param>
    /// <param name="timeout">The time span to wait for the semaphore before timing out.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the <paramref name="initialCount"/> is less than or equal to zero.</exception>
    public SemaphoreDelegatingHandler(int initialCount, TimeSpan timeout)
    {
        if (initialCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(initialCount),
                "The initial count for the semaphore must be greater than zero.");
        }
        _timeout = timeout;
        _semaphore = new SemaphoreSlim(initialCount);
    }

    /// <summary>
    /// Sends an HTTP request to the inner handler to send to the server as an asynchronous operation.
    /// </summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellation">A cancellation token to cancel operation.</param>
    /// <returns>Returns <see cref="Task{TResult}"/>. The task object representing the asynchronous operation.</returns>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellation)
    {
        if (false == await _semaphore.WaitAsync(_timeout, cancellation))
        {
            var response = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable);
            response.ReasonPhrase = "Server too busy. Maximum concurrent requests limit reached.";
            return response;
        }

        try
        {
            return await base.SendAsync(request, cancellation);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="SemaphoreDelegatingHandler"/>, and optionally disposes of the managed resources.
    /// </summary>
    /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _semaphore.Dispose();
        }
        
        base.Dispose(disposing);
    }
}