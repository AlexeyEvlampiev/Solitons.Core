using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

/// <summary>
/// Represents a brokered HTTP message handler.
/// This class is responsible for managing the messaging flow over an asynchronous brokered connection (e.g., AMQP) between a client and a server.
/// </summary>
/// <remarks>
/// The handler works on a session basis where a session corresponds to an HTTP request-response exchange.
/// The session ID is provided by the <see cref="IBrokeredRequest.HttpSessionId"/> property.
/// In case of any exceptions or when the session is completed, the handler automatically clears its internal session cache.
/// </remarks>
public abstract class BrokeredHttpMessageHandler : HttpMessageHandler
{
    private const int DefaultActiveRequestsThreshold = 8000;
    private readonly IObservable<IBrokeredResponse> _responses;
    private readonly EventLoopScheduler _scheduler;
    private readonly Dictionary<Guid, IObserver<IBrokeredResponse>> _sessionListeners = new();
    private readonly IDisposable _responsesSubscription;

    /// <summary>
    /// Initializes a new instance of the <see cref="BrokeredHttpMessageHandler"/> class with a specified brokered responses stream.
    /// </summary>
    /// <param name="responses">The stream of brokered responses.</param>
    [DebuggerStepThrough]
    protected BrokeredHttpMessageHandler(IObservable<IBrokeredResponse> responses) 
        : this(responses, new EventLoopScheduler())
    {
        
    }

    /// <summary>
    /// Initializes a new instance of the BrokeredHttpMessageHandler class with specified observable sequence of brokered responses and a scheduler.
    /// </summary>
    /// <param name="responses">The observable sequence of brokered responses.</param>
    /// <param name="scheduler">Scheduler to execute the message handling operations on.</param>
    protected BrokeredHttpMessageHandler(
        IObservable<IBrokeredResponse> responses,  
        EventLoopScheduler scheduler)
    {
        IsActive = true;
        _responses = responses;
        _scheduler = scheduler;
        _responsesSubscription = responses
            .ObserveOn(scheduler)
            .Do(OnResponse, OnError, OnCompleted)
            .Subscribe();
    }

    public IObservable<Unit> AsObservable() => _responses.Select(_ => Unit.Default);

    /// <summary>
    /// Handles the response from the server.
    /// </summary>
    /// <param name="brokeredResponse">The brokered response received from the server.</param>

    private void OnResponse(IBrokeredResponse brokeredResponse)
    {
        if (_sessionListeners.TryGetValue(brokeredResponse.HttpSessionId, out var observer))
        {
            _sessionListeners.Remove(brokeredResponse.HttpSessionId);
            try
            {
                observer.OnNext(brokeredResponse);
                observer.OnCompleted();
            }
            catch (Exception e)
            {
                observer.OnError(e);
            }
        }
    }

    /// <summary>
    /// Handles any error that occurs while processing the responses.
    /// </summary>
    /// <param name="ex">The exception that occurred.</param>

    private void OnError(Exception ex)
    {
        Error ??= ex;
        OnCompleted();
    }

    /// <summary>
    /// Handles the completion of the responses processing.
    /// </summary>

    private void OnCompleted()
    {
        _responsesSubscription.Dispose();
        IsActive = false;
    }


    /// <summary>
    /// Gets the error occurred while processing the responses.
    /// </summary>
    public Exception? Error { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the response stream is active.
    /// </summary>
    /// <remarks>
    /// A response stream is considered active if it has not completed and has not encountered an error.
    /// </remarks>
    /// <value>
    /// True if the response stream is active; otherwise, false.
    /// </value>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Handles the HTTP request.
    /// </summary>
    /// <param name="httpRequest">The HTTP request message to send.</param>
    /// <param name="cancellation">A cancellation token to cancel operation.</param>
    /// <returns>Returns the HTTP response message from the server.</returns>
    protected sealed override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage httpRequest, 
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();

        var brokeredRequest = await CreateBrokeredRequestAsync(httpRequest, cancellation);

        var correlationId = ThrowIf.NullOrEmpty(brokeredRequest.HttpSessionId, "HTTP session ID is required.");
        var adjustedRequestTimeout = AdjustRequestTimeout(brokeredRequest.RequestTimeout);

        async Task<IDisposable> SubscribeAsync(IObserver<IBrokeredResponse> observer)
        {
            if (IsActive == false)
            {
                observer.OnError(new ServiceUnavailableException());
                return Disposable.Empty;
            }
            if (_sessionListeners.Count > ActiveRequestsThreshold)
            {
                observer.OnError(new ServiceOverloadedException());
                return Disposable.Empty;
            }

            var timeoutSubscription = Observable
                .Timer(adjustedRequestTimeout)
                .ObserveOn(_scheduler)
                .Do(_ => _sessionListeners.Remove(correlationId))
                .Do(_ => observer.OnError(new TimeoutException()))
                .Subscribe();

            
            _sessionListeners.Add(brokeredRequest.HttpSessionId, observer);

            await PublishAsync(brokeredRequest, cancellation);

            return Disposable.Create(() =>
            {
                timeoutSubscription.Dispose();
                _sessionListeners.Remove(brokeredRequest.HttpSessionId);
            });
        }

        return await Observable
            .Create<IBrokeredResponse>(SubscribeAsync)
            .SubscribeOn(_scheduler)
            .SelectMany(msg => CreateHttpResponseAsync(msg, cancellation))
            .Catch<HttpResponseMessage, Exception>(ex => Observable.FromAsync(() => CreateHttpErrorResponseAsync(httpRequest, ex, cancellation)))
            .FirstAsync()
            .ToTask(cancellation);
    }


    /// <summary>
    /// Gets the threshold for the number of active requests. When the number of active requests exceeds this value, the server is considered overloaded.
    /// </summary>
    /// <value>
    /// The maximum number of active requests allowed. The default value is defined by <see cref="DefaultActiveRequestsThreshold"/>.
    /// </value>
    /// <remarks>
    /// This property provides a mechanism to protect the server from being overwhelmed by too many requests at once. 
    /// Override this property to adjust the active request threshold based on the specific requirements or performance characteristics of your server.
    /// </remarks>
    protected virtual int ActiveRequestsThreshold => DefaultActiveRequestsThreshold;

    /// <summary>
    /// Creates an HTTP error response message based on the provided exception.
    /// </summary>
    /// <param name="request">The original HTTP request message.</param>
    /// <param name="error">The exception that has caused the error response.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the HTTP response message.
    /// </returns>
    /// <remarks>
    /// This method is responsible for converting a given exception to an appropriate HTTP response.
    /// Different types of exceptions are handled and mapped to different HTTP status codes. For instance, a 
    /// <see cref="ServiceOverloadedException"/> will result in a Service Unavailable (503) status code,
    /// with a Retry-After header suggesting the client to retry after 60 seconds. Similarly, both a 
    /// <see cref="ServiceUnavailableException"/> and a <see cref="TimeoutException"/> will result in a Service Unavailable status code.
    /// </remarks>
    protected virtual Task<HttpResponseMessage> CreateHttpErrorResponseAsync(
        HttpRequestMessage request,
        Exception error,
        CancellationToken cancellation)
    {
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
        if (error is ServiceOverloadedException)
        {
            response.StatusCode = HttpStatusCode.ServiceUnavailable;
            response.Content = new StringContent("Server is temporarily overloaded. Please try again later.");
            response.Headers.RetryAfter = new RetryConditionHeaderValue(TimeSpan.FromSeconds(60));
        }
        else if (error is ServiceUnavailableException)
        {
            response.StatusCode = HttpStatusCode.ServiceUnavailable;
        }
        else if (error is TimeoutException)
        {
            response.StatusCode = HttpStatusCode.ServiceUnavailable;
        }
        return Task.FromResult(response);
    }




    /// <summary>
    /// Adjusts the request timeout.
    /// </summary>
    /// <param name="requestedTimeout">The requested timeout value.</param>
    /// <returns>Returns the adjusted timeout value.</returns>
    /// <remarks>
    /// The method adjusts the requested timeout value to be at least 30 milliseconds and at most 30 seconds.
    /// </remarks>
    protected virtual TimeSpan AdjustRequestTimeout(TimeSpan requestedTimeout) => requestedTimeout.Ticks
        .Min(TimeSpan.TicksPerMillisecond * 30)
        .Max(TimeSpan.TicksPerSecond * 30)
        .Convert(TimeSpan.FromTicks);



    /// <summary>
    /// Asynchronously creates an HTTP response message from a brokered response.
    /// </summary>
    /// <param name="brokeredResponse">The brokered response.</param>
    /// <param name="cancellation">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous create operation. The value of the TResult parameter contains the created HttpResponseMessage.</returns>
    /// <remarks>
    /// This method should be implemented in a derived class to create an HTTP response specific to the service being called.
    /// This could include setting specific headers, body content, status codes, or other setup necessary for the specific service.
    /// </remarks>
    protected abstract Task<HttpResponseMessage> CreateHttpResponseAsync(IBrokeredResponse brokeredResponse, CancellationToken cancellation);

    /// <summary>
    /// Publishes the outgoing message.
    /// </summary>
    /// <param name="request">The outgoing brokered request message.</param>
    /// <param name="cancellation">A cancellation token to cancel operation.</param>
    /// <returns>Returns a <see cref="Task"/> representing the asynchronous operation.</returns>
    protected abstract Task PublishAsync(IBrokeredRequest request, CancellationToken cancellation);

    /// <summary>
    /// Asynchronously creates a brokered request from an HTTP request message.
    /// </summary>
    /// <param name="httpRequest">The HTTP request message.</param>
    /// <param name="cancellation">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous create operation. The value of the TResult parameter contains the created IBrokeredRequest.</returns>
    /// <remarks>
    /// This method should be implemented in a derived class to create a brokered request specific to the service being called.
    /// This could include setting specific headers, parameters, or other setup necessary for the specific service.
    /// </remarks>
    protected abstract Task<IBrokeredRequest> CreateBrokeredRequestAsync(
        HttpRequestMessage httpRequest,
        CancellationToken cancellation);

    /// <summary>
    /// Releases the unmanaged resources used by the BrokeredHttpMessageHandler and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">True to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    /// <remarks>
    /// If the disposing parameter is true, the method has been called directly or indirectly by a user's code and both managed and unmanaged resources can be disposed.
    /// If the disposing parameter is false, the method has been called by the runtime from inside the finalizer and only unmanaged resources can be disposed.
    /// After the Dispose method is called, the instance becomes unusable. You cannot reuse an instance after it has been disposed.
    /// </remarks>
    protected override void Dispose(bool disposing)
    {
        IsActive = false;
        if (disposing)
        {
            _responsesSubscription.Dispose();
            _sessionListeners.Clear();
        }

        base.Dispose(disposing);
    }

    /// <summary>
    /// Defines the common properties for a brokered message, which can be either a request or a response.
    /// </summary>
    /// <remarks>
    /// A brokered message represents the basic unit of communication for the HTTP request-response pipeline in a distributed system.
    /// Implementations of this interface represent the data necessary to handle HTTP requests and responses in a decoupled and resilient manner.
    /// </remarks>
    protected interface IBrokeredMessage
    {
        /// <summary>
        /// Gets the unique identifier for the HTTP session.
        /// </summary>
        /// <value>
        /// The unique identifier for the HTTP session.
        /// </value>
        /// <remarks>
        /// This identifier is used to correlate requests with their responses. 
        /// It should be unique for each HTTP request-response session.
        /// </remarks>
        Guid HttpSessionId { get; }
    }


    /// <summary>
    /// Represents a brokered request in the context of an HTTP session.
    /// </summary>
    protected interface IBrokeredRequest : IBrokeredMessage
    {
        /// <summary>
        /// Gets the requested timeout for the request.
        /// </summary>
        /// <value>
        /// The requested timeout for the request.
        /// </value>
        /// <remarks>
        /// This timeout value is used to ensure that the request does not wait indefinitely for a response. 
        /// If a response is not received within this timeout period, an exception is thrown.
        /// </remarks>
        TimeSpan RequestTimeout { get; }
    }

    /// <summary>
    /// Represents a brokered response in the context of an HTTP session.
    /// </summary>
    protected interface IBrokeredResponse : IBrokeredMessage
    {

    }


    /// <summary>
    /// Represents a brokered request in the context of an HTTP session.
    /// </summary>
    protected abstract class BrokeredRequest : IBrokeredRequest
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan _requestTimeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrokeredRequest"/> class with a unique HTTP session ID and a default request timeout of 30 seconds.
        /// </summary>
        protected BrokeredRequest()
        {
            RequestTimeout = TimeSpan.FromSeconds(30);
        }

        /// <summary>
        /// Gets the unique identifier for the HTTP session.
        /// </summary>
        /// <value>
        /// The unique identifier for the HTTP session.
        /// </value>
        public Guid HttpSessionId { get; } = Guid.NewGuid();

        /// <summary>
        /// Gets or sets the requested timeout for the request.
        /// </summary>
        /// <remarks>
        /// This timeout value is used to ensure that the request does not wait indefinitely for a response. 
        /// If a response is not received within this timeout period, an exception is thrown.
        /// If set to a non-positive value, it will be treated as infinity.
        /// </remarks>
        public TimeSpan RequestTimeout
        {
            get => _requestTimeout;
            set => _requestTimeout = value > TimeSpan.Zero ? value : TimeSpan.MaxValue;
        }
    }

    /// <summary>
    /// Represents an exception that is thrown when the server is overloaded.
    /// </summary>
    protected sealed class ServiceOverloadedException : Exception
    {
    }

    /// <summary>
    /// Represents an exception that is thrown when the server is unreachable.
    /// </summary>
    protected sealed class ServiceUnavailableException : Exception
    {
    }
}