using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

/// <summary>
/// A DelegatingHandler that is responsible for rate limiting on a per-client basis.
/// </summary>
/// <remarks>
/// This class should be used in a scenario where each client's request should be rate limited based on certain conditions. 
/// To use this class, it needs to be sub-classed and the abstract methods need to be implemented.
/// </remarks>
public abstract class ClientRateLimitDelegatingHandler : DelegatingHandler
{
    private readonly IClock _clock;

    /// <summary>
    /// Construct a new <see cref="ClientRateLimitDelegatingHandler"/>.
    /// </summary>
    [DebuggerStepThrough]
    protected ClientRateLimitDelegatingHandler() : this(IClock.System)
    {
    }

    /// <summary>
    /// Construct a new <see cref="ClientRateLimitDelegatingHandler"/>.
    /// </summary>
    /// <param name="clock">An object of type <see cref="IClock"/> which provides the current time.</param>
    protected ClientRateLimitDelegatingHandler(IClock clock)
    {
        _clock = clock;
    }

    /// <summary>
    /// Sends an HTTP request to the inner handler to send to the server.
    /// </summary>
    /// <param name="request">The HTTP request message to send to the server.</param>
    /// <param name="cancellation">A cancellation token to cancel operation.</param>
    /// <returns>A Task that represents the HttpResponseMessage from server.</returns>
    protected sealed override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellation)
    {
        var clientId = ExtractClientId(request);
        if (TryGetRateLimitData(
                clientId, 
                out var expiredAfter, 
                out var cloneFactory) && 
            _clock.UtcNow < expiredAfter)
        {
            return cloneFactory.Create();
        }

        var response = await base.SendAsync(request, cancellation);
        if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            ClientRateLimitData? rateLimitData = ExtractClientRateLimitIfFound(request);
            if (rateLimitData != null)
            {
                var factory = await HttpResponseMessageCloneFactory.FromAsync(response); ;
                await AddToCacheAsync(rateLimitData, factory, cancellation);
            }
        }

        return response;
    }

    /// <summary>
    /// Try to get the rate limit data and clone factory for the given client ID.
    /// </summary>
    /// <param name="clientId">The ID of the client.</param>
    /// <param name="expiredAfter">The date and time when the rate limit expires.</param>
    /// <param name="factory">The factory to create HttpResponseMessage.</param>
    /// <returns>True if data and factory found, otherwise false.</returns>
    protected abstract bool TryGetRateLimitData(
        object? clientId, 
        out DateTimeOffset expiredAfter, 
        out HttpResponseMessageCloneFactory factory);

    /// <summary>
    /// Extract the client ID from the given HttpRequestMessage.
    /// </summary>
    /// <param name="request">The HttpRequestMessage.</param>
    /// <returns>The client ID if found, otherwise null.</returns>
    protected abstract object? ExtractClientId(HttpRequestMessage request);

    /// <summary>
    /// Adds the given rate limit data and clone factory to cache.
    /// </summary>
    /// <param name="data">The rate limit data.</param>
    /// <param name="factory">The factory to create HttpResponseMessage.</param>
    /// <param name="cancellation">A cancellation token to cancel operation.</param>
    /// <returns>A task that represents the completion of adding data to cache.</returns>
    protected abstract Task AddToCacheAsync(
        ClientRateLimitData data, 
        HttpResponseMessageCloneFactory factory, 
        CancellationToken cancellation);

    /// <summary>
    /// Extract the rate limit data from the given HttpRequestMessage.
    /// </summary>
    /// <param name="request">The HttpRequestMessage.</param>
    /// <returns>The rate limit data if found, otherwise null.</returns>
    protected abstract ClientRateLimitData? ExtractClientRateLimitIfFound(HttpRequestMessage request);

    /// <summary>
    /// Represents the rate limit data for a client.
    /// </summary>
    protected sealed record ClientRateLimitData(object ClientId, DateTimeOffset ExpiredAfter);
}