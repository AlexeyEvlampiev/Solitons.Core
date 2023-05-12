using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

/// <summary>
/// Represents a fluent HTTP client for making HTTP requests.
/// </summary>
public readonly struct FluentHttpClient
{
    private readonly HttpClient _client;
    private readonly Func<HttpRequestMessage> _requestFactory;
    private readonly Func<IObservable<HttpResponseMessage>, IObservable<Unit>> _retryTriggerFactory;

    internal FluentHttpClient(
        HttpClient client, 
        Func<HttpRequestMessage> requestFactory)
    {
        _client = client;
        _requestFactory = requestFactory;
        _retryTriggerFactory = DefaultRetryTriggerFactory;

        [DebuggerStepThrough]
        IObservable<Unit> DefaultRetryTriggerFactory(IObservable<HttpResponseMessage> responses)
        {
            return responses
                .Select(_ => Unit.Default)
                .Take(3)
                .Delay(100, CancellationToken.None);
        }
    }

    

    /// <summary>
    /// Configures the HTTP client to include retries based on the provided retry trigger factory.
    /// </summary>
    /// <typeparam name="T">The type of the retry trigger.</typeparam>
    /// <param name="retryTriggerFactory">A function that provides the retry trigger based on the HTTP response.</param>
    /// <returns>A new instance of <see cref="FluentHttpClient"/> with the configured retry behavior.</returns>
    public FluentHttpClient WithRetryTrigger<T>(Func<IObservable<HttpResponseMessage>, IObservable<T>> retryTriggerFactory)
    {
        return new FluentHttpClient(_client, _requestFactory, Generalize);

        [DebuggerStepThrough]
        IObservable<Unit> Generalize(IObservable<HttpResponseMessage> responses)
        {
            return retryTriggerFactory.Invoke(responses).Select(_ => Unit.Default);
        }
    }

    private FluentHttpClient(
        HttpClient client,
        Func<HttpRequestMessage> requestFactory,
        Func<IObservable<HttpResponseMessage>, IObservable<Unit>> retryTriggerFactory)
    {
        _client = client;
        _requestFactory = requestFactory;
        _retryTriggerFactory = retryTriggerFactory;
    }

    /// <summary>
    /// Sends an HTTP request asynchronously.
    /// </summary>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A task that represents the asynchronous HTTP request operation.</returns>
    [DebuggerStepThrough]
    public Task<HttpResponseMessage> SendAsync(CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        return _retryTriggerFactory is null
            ? _client.SendAsync(_requestFactory, cancellation)
            : _client.SendAsync(_requestFactory, _retryTriggerFactory, cancellation);
    }
}