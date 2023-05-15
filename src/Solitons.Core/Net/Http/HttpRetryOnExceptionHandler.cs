using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

/// <summary>
/// A delegating handler that retries an HTTP request upon an <see cref="HttpRequestException"/>.
/// </summary>
public class HttpRetryOnExceptionHandler : DelegatingHandler
{
    private const int BaseDelayMilliseconds = 100;
    private const int DelayMultiplierUpperThreshold = 30;
    private const int DelayMultiplierLowerThreshold = 1;
    private const int MaxAttempts = 10;

    private readonly SendAsyncHandler _baseSendAsync;

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpRetryOnExceptionHandler"/> class.
    /// </summary>
    /// <param name="innerHandler">The inner handler which is responsible for processing the HTTP response messages.</param>
    [DebuggerNonUserCode]
    public HttpRetryOnExceptionHandler(HttpMessageHandler innerHandler)
        : base(innerHandler)
    {
        _baseSendAsync = base.SendAsync;
    }


    /// <summary>
    /// Send an HTTP request with retries on exception as an asynchronous operation.
    /// </summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellation">A cancellation token to cancel operation.</param>
    /// <returns>Returns <see cref="Task{TResult}"/>. The task object representing the asynchronous operation.</returns>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellation)
    {
        for (int attempt = 0;; attempt++)
        {
            cancellation.ThrowIfCancellationRequested();
            try
            {
                return await _baseSendAsync(request, cancellation).ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                if (await ShouldRetryAfterExceptionAsync(request, ex, attempt, cancellation))
                {
                    continue;
                }

                throw;
            }
        }
    }

    /// <summary>
    /// Evaluates if a retry is needed after an exception occurs.
    /// </summary>
    /// <param name="request">The failed HTTP request.</param>
    /// <param name="error">The exception that was thrown.</param>
    /// <param name="attempt">The number of the current attempt.</param>
    /// <param name="cancellation">A cancellation token to cancel operation.</param>
    /// <returns>Returns <see cref="Task{TResult}"/>. The task object representing the asynchronous operation.</returns>
    protected virtual async Task<bool> ShouldRetryAfterExceptionAsync(HttpRequestMessage request, Exception error, int attempt,
        CancellationToken cancellation)
    {
        if (attempt >= MaxAttempts)
        {
            return false;
        }
        var milliseconds = BaseDelayMilliseconds * attempt
            .Min(DelayMultiplierLowerThreshold)
            .Max(DelayMultiplierUpperThreshold);

        await Task.Delay(milliseconds, cancellation);
        return true;
    }
}