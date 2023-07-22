using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net;

/// <summary>
/// Represents a base class for an asynchronous remote procedure call (RPC) client that uses the brokered messaging pattern.
/// This class is abstract.
/// </summary>
/// <typeparam name="TRequest">The type of the request message.</typeparam>
/// <typeparam name="TResponse">The type of the response message.</typeparam>
public abstract class BrokeredRpcClient<TRequest, TResponse> : IAsyncDisposable
{
    private const int MinAllowedConcurrentCalls = 1;
    private const int MaxAllowedConcurrentCalls = 2000;

    sealed record Registration(
        IObserver<TResponse> Observer,
        DateTimeOffset ExpiredAfter);

    private readonly EventLoopScheduler _scheduler;
    private readonly IClock _clock;
    private readonly Dictionary<string, Registration> _pendingRequests = new(StringComparer.Ordinal);
    private readonly AsyncStackAutoDisposer _disposer = new();
    private Exception? _exception;

    /// <summary>
    /// Initializes a new instance of the BrokeredRpcClient class with the specified responses observable.
    /// </summary>
    /// <param name="responses">The observable sequence of response messages.</param>
    [DebuggerStepThrough]
    protected BrokeredRpcClient(IObservable<TResponse> responses) 
        : this(responses, new EventLoopScheduler(), IClock.System)
    {
        _disposer.AddResource(_scheduler);
    }

    /// <summary>
    /// Initializes a new instance of the BrokeredRpcClient class with the specified responses observable, scheduler, and clock.
    /// </summary>
    /// <param name="responses">The observable sequence of response messages.</param>
    /// <param name="scheduler">The scheduler to run observer callbacks on.</param>
    /// <param name="clock">The clock to use for managing request expirations.</param>
    protected BrokeredRpcClient(
        IObservable<TResponse> responses, 
        EventLoopScheduler scheduler,
        IClock clock)
    {
        _scheduler = scheduler;
        _clock = clock;
        _disposer.AddResource(responses
            .SubscribeOn(scheduler)
            .ObserveOn(scheduler)
            .Subscribe(OnDelivery, OnError, OnCompleted));
    }

    /// <summary>
    /// Sends a request asynchronously and returns an observable of the response.
    /// </summary>
    /// <param name="request">The request message to send.</param>
    /// <param name="cancellation">The cancellation token to cancel the operation.</param>
    /// <returns>A task that returns the response message.</returns>
    public async Task<TResponse> SendAsync(TRequest request, CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        if (_exception is not null)
        {
            throw _exception;
        }

        var correlationId = GetRequestCorrelationId(request);
        var expiredAfter = GetExpirationTime(request);
        var maxCapacity = DetermineMaxConcurrencyLimit()
            .Min(MinAllowedConcurrentCalls)
            .Max(MaxAllowedConcurrentCalls);

        return await Observable
            .Create<TResponse>(async (observer) =>
            {
                if (correlationId.IsNullOrWhiteSpace())
                {
                    observer.OnError(new InvalidOperationException(
                        "Correlation ID cannot be null or whitespace. Please provide a valid correlation ID."));
                    return Disposable.Empty;
                }
                var registration = new Registration(observer, expiredAfter);
                _pendingRequests.Add(correlationId, registration);
                var subscription = Disposable.Create(() => _pendingRequests.Remove(correlationId));
                if (_pendingRequests.Count > maxCapacity)
                {
                    var timeoutError = new TimeoutException(
                        $"The operation with correlation ID {correlationId} has timed out.");
                    var expired = _pendingRequests
                        .Where(_ => _.Value.ExpiredAfter < UtcNow)
                        .Do(item => item.Value.Observer
                            .OnError(timeoutError))
                        .Select(_ => _.Key)
                        .ToList();
                    expired.ForEach(id => _pendingRequests.Remove(id));
                }

                if (_pendingRequests.Count > maxCapacity)
                {
                    subscription.Dispose();
                    string errorMessage = $"Concurrency limit exceeded. The maximum allowed is {maxCapacity}, but there are currently {_pendingRequests.Count} active calls.";
                    observer.OnError(new CapacityLimitExceededException(errorMessage));
                    return Disposable.Empty;
                }
                else
                {
                    try
                    {
                        await SendRequestAsync(request, cancellation);
                        return subscription;
                    }
                    catch (Exception e)
                    {
                        subscription.Dispose();
                        observer.OnError(e);
                        return Disposable.Empty;
                    }
                }
                
            })
            .SubscribeOn(_scheduler)
            .ObserveOn(_scheduler);
    }

    /// <summary>
    /// When overridden in a derived class, publishes the specified request asynchronously.
    /// </summary>
    /// <param name="request">The request to publish.</param>
    /// <param name="cancellation">The cancellation token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous publish operation.</returns>
    protected abstract Task SendRequestAsync(TRequest request, CancellationToken cancellation);

    /// <summary>
    /// When overridden in a derived class, determines the maximum concurrency level.
    /// </summary>
    /// <returns>The maximum concurrency level.</returns>
    protected abstract int DetermineMaxConcurrencyLimit();

    /// <summary>
    /// When overridden in a derived class, gets the expiration time of the specified request.
    /// </summary>
    /// <param name="request">The request to get the expiration time of.</param>
    /// <returns>The expiration time of the request.</returns>
    protected abstract DateTimeOffset GetExpirationTime(TRequest request);

    /// <summary>
    /// When overridden in a derived class, gets the correlation ID of the specified request.
    /// </summary>
    /// <param name="request">The request to get the correlation ID of.</param>
    /// <returns>The correlation ID of the request.</returns>
    protected abstract string GetRequestCorrelationId(TRequest request);

    /// <summary>
    /// When overridden in a derived class, gets the correlation ID of the specified response.
    /// </summary>
    /// <param name="response">The response to get the correlation ID of.</param>
    /// <returns>The correlation ID of the response.</returns>
    protected abstract string GetResponseCorrelationId(TResponse response);

    /// <summary>
    /// When overridden in a derived class, disposes the resources used by the BrokeredRpcClient asynchronously.
    /// </summary>
    /// <returns>A value task that represents the asynchronous dispose operation.</returns>
    protected abstract ValueTask DisposeAsync();

    /// <summary>
    /// Gets the current UTC date and time based on the provided clock.
    /// </summary>
    protected DateTimeOffset UtcNow => _clock.UtcNow;

    private void OnCompleted()
    {
        _exception ??= new ObjectDisposedException(
            $"{GetType().Name} instance is already disposed. Further operations are not allowed on disposed objects.");
    }

    private void OnError(Exception exception)
    {
        _exception ??= exception;
    }

    private void OnDelivery(TResponse response)
    {
        var correlationId = GetResponseCorrelationId(response);
        if (false == _pendingRequests.TryGetValue(correlationId, out var registration))
        {
            return;
        }

        _pendingRequests.Remove(correlationId);
        var (observer, expiredAfter) = (registration.Observer, registration.ExpiredAfter);

        try
        {
            if (expiredAfter >= DateTimeOffset.UtcNow)
            {
                observer.OnNext(response);
                observer.OnCompleted();
            }
            else
            {
                observer.OnError(new TimeoutException());
            }
        }
        catch (Exception e)
        {
            observer.OnError(e);
        }
    }

    ValueTask IAsyncDisposable.DisposeAsync()
    {
        _exception ??= new ObjectDisposedException($"{GetType().Name} instance is already disposed. Further operations are not allowed on disposed objects.");
        return DisposeAsync();
    }
}

/// <summary>
/// Represents a base class for an asynchronous remote procedure call (RPC) client that uses the brokered messaging pattern, where the type of the request and response messages is the same.
/// This class is abstract.
/// </summary>
/// <typeparam name="T">The type of the request and response message.</typeparam>
public abstract class BrokeredRpcClient<T> : BrokeredRpcClient<T, T>
{
    /// <summary>
    /// Initializes a new instance of the BrokeredRpcClient class with the specified responses observable.
    /// </summary>
    /// <param name="responses">The observable sequence of response messages.</param>
    [DebuggerStepThrough]
    protected BrokeredRpcClient(IObservable<T> responses) : base(responses)
    {
    }

    /// <summary>
    /// Initializes a new instance of the BrokeredRpcClient class with the specified responses observable, scheduler, and clock.
    /// </summary>
    /// <param name="responses">The observable sequence of response messages.</param>
    /// <param name="scheduler">The scheduler to run observer callbacks on.</param>
    /// <param name="clock">The clock to use for managing request expirations.</param>
    [DebuggerStepThrough]
    protected BrokeredRpcClient(IObservable<T> responses, EventLoopScheduler scheduler, IClock clock) : base(responses, scheduler, clock)
    {
    }

    /// <summary>
    /// When overridden in a derived class, gets the correlation ID of the specified response.
    /// This implementation assumes that the request and response have the same correlation ID.
    /// </summary>
    /// <param name="response">The response to get the correlation ID of.</param>
    /// <returns>The correlation ID of the response.</returns>
    protected override string GetResponseCorrelationId(T response)
    {
        return GetRequestCorrelationId(response);
    }
}