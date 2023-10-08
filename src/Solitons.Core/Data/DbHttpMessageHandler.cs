using System;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Diagnostics;
using Solitons.Reactive;

namespace Solitons.Data;

/// <summary>
/// Provides a base implementation of an <see cref="HttpMessageHandler"/> that interacts with a database.
/// This handler is designed to manage HTTP requests and their associated database sessions, 
/// offering mechanisms for retrying operations in case of transient database errors and handling exceptions.
/// </summary>
/// <remarks>
/// Derived classes are expected to provide specific implementations for executing database commands based on HTTP requests.
/// </remarks>
public abstract class DbHttpMessageHandler : HttpMessageHandler
{
    /// <summary>
    /// Represents a delegate that handles an HTTP request and manages the associated database session.
    /// </summary>
    /// <param name="request">The HTTP request message.</param>
    /// <param name="response">The HTTP response message.</param>
    /// <param name="cancellation">The cancellation token to cancel the operation.</param>
    delegate Task CommandHandler(HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellation);

    /// <summary>
    /// The default maximum number of retry attempts.
    /// </summary>
    internal const int DefaultMaxRetryAttemptNumber = 3;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly CommandHandler _commandHandler;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IClock _clock = IClock.System;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbHttpMessageHandler"/> class with a specified database connection.
    /// </summary>
    /// <param name="connection">The database connection to be used by the handler.</param>
    /// <exception cref="ArgumentException">Thrown when the provided database connection is not in an open state.</exception>
    protected DbHttpMessageHandler(DbConnection connection)
    {
        if (connection.State != ConnectionState.Open)
        {
            throw new ArgumentException("The provided database connection must be in an open state.");
        }

        _commandHandler = HandleAsync;

        [DebuggerStepThrough]
        Task HandleAsync(
            HttpRequestMessage request,
            HttpResponseMessage response,
            CancellationToken cancellation)
        {
            return ExecuteAsync(connection, request, response, cancellation);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbHttpMessageHandler"/> class with a specified database connection factory.
    /// </summary>
    /// <param name="connectionFactory">A factory method that produces a new database connection for each HTTP request.</param>
    protected DbHttpMessageHandler(Func<DbConnection> connectionFactory)
    {
        _commandHandler = HandleAsync;

        [DebuggerStepThrough]
        async Task HandleAsync(
            HttpRequestMessage request,
            HttpResponseMessage response,
            CancellationToken cancellation)
        {
            await using var connection = connectionFactory.Invoke();
            await connection.OpenAsync(cancellation);
            await ExecuteAsync(connection, request, response, cancellation);
        }
    }

    /// <summary>
    /// Sends an HTTP request with an HTTP completion option and a cancellation token,
    /// and manages the associated database session.
    /// </summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellation">The cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The result of the task is an HttpResponseMessage that is the response message.</returns>
    protected sealed override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellation)
    {
        var response = new HttpResponseMessage();

        var logger = IAsyncLogger.Get(request.Options);

        try
        {
            cancellation.ThrowIfCancellationRequested();

            await Observable
                .FromAsync([DebuggerStepThrough] () => _commandHandler(request, response, cancellation))
                .WithRetryPolicy([DebuggerStepThrough](args) =>CanRetryAsync(args, cancellation));

        }
        catch (Exception e) when(e is TimeoutException ||
                                 e is DbException { InnerException: TimeoutException })
        {
            await logger.ErrorAsync(e);
            await OnTimeoutAsync(request, response, e, cancellation);
            if (response.IsSuccessStatusCode)
            {
                response.StatusCode = HttpStatusCode.GatewayTimeout;
            }
        }
        catch (Exception e)
        {
            await logger.ErrorAsync(e);
            await OnInternalErrorAsync(request, response, e, cancellation);
            if (response.IsSuccessStatusCode)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
        }
        return response;
    }

    /// <summary>
    /// Configures the logger with additional properties.
    /// </summary>
    /// <param name="logger">The logger instance to configure.</param>
    /// <param name="request">The HTTP request message.</param>
    /// <returns>The configured logger instance.</returns>
    protected virtual IAsyncLogger ConfigLogger(
        IAsyncLogger logger, 
        HttpRequestMessage request)
    {
        return logger
            .WithProperty("requestUrl", request.RequestUri?.ToString() ?? String.Empty);
    }

    /// <summary>
    /// Determines whether a retry should be attempted based on the exception type and the number of attempts made so far.
    /// </summary>
    /// <param name="args">Arguments related to the retry policy.</param>
    /// <param name="cancellation">The cancellation token to cancel the operation.</param>
    /// <returns>True if a retry should be attempted; otherwise, false.</returns>
    protected virtual async Task<bool> CanRetryAsync(RetryPolicyArgs args, CancellationToken cancellation)
    {
        if (args is
            {
                Exception: DbException { IsTransient: true },
                AttemptNumber: < DefaultMaxRetryAttemptNumber
            })
        {
            var sleepInterval = TimeSpan
                .FromMilliseconds(100)
                .ScaleByFactor(1.5, args.AttemptNumber);
            await _clock.DelayAsync(sleepInterval, cancellation);
            return true;
        }
        return false;
    }


    /// <summary>
    /// Asynchronously executes a database command based on the provided HTTP request using the specified database connection.
    /// </summary>
    /// <param name="connection">The database connection to be used for executing the command.</param>
    /// <param name="request">The HTTP request message containing the details for formulating the database command.</param>
    /// <param name="response">The HTTP response message that will be populated based on the result of the database command execution.</param>
    /// <param name="cancellation">A token that can be used to request the cancellation of the asynchronous operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// Derived classes must provide a concrete implementation of this method to define the specific logic for translating the HTTP request into a database command and executing it.
    /// </remarks>
    protected abstract Task ExecuteAsync(DbConnection connection, HttpRequestMessage request, HttpResponseMessage response, CancellationToken cancellation);

    /// <summary>
    /// Handles a timeout exception that occurred during the processing of an HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request during the processing of which the exception occurred.</param>
    /// <param name="response">The HTTP response message to be sent.</param>
    /// <param name="exception">The timeout exception that occurred.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>This implementation simply sets the status code of the response to GatewayTimeout. Override this method to provide custom behavior.</remarks>
    protected virtual Task OnTimeoutAsync(
        HttpRequestMessage request,
        HttpResponseMessage response,
        Exception exception,
        CancellationToken cancellation)
    {
        response.StatusCode = HttpStatusCode.GatewayTimeout;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles an internal exception that occurred during the processing of an HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request during the processing of which the exception occurred.</param>
    /// <param name="response">The HTTP response message to be sent.</param>
    /// <param name="exception">The exception that occurred.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>This implementation simply sets the status code of the response to InternalServerError. Override this method to provide custom behavior.</remarks>
    protected virtual Task OnInternalErrorAsync(
        HttpRequestMessage request,
        HttpResponseMessage response,
        Exception exception,
        CancellationToken cancellation)
    {
        response.StatusCode = HttpStatusCode.InternalServerError;
        return Task.CompletedTask;
    }
}

/// <summary>
/// Represents a specialized version of the <see cref="DbHttpMessageHandler"/> class that works with a specific type of database connection.
/// </summary>
/// <typeparam name="T">The type of the database connection.</typeparam>
public abstract class DbHttpMessageHandler<T> : DbHttpMessageHandler where T : DbConnection
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbHttpMessageHandler{T}"/> class with a specified database connection.
    /// </summary>
    /// <param name="connection">The database connection of type <typeparamref name="T"/> to be used by the handler.</param>
    /// <exception cref="ArgumentException">Thrown when the provided database connection is not in an open state.</exception>
    [DebuggerStepThrough]
    protected DbHttpMessageHandler(T connection) 
        : base(connection)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbHttpMessageHandler{T}"/> class with a specified database connection factory.
    /// </summary>
    /// <param name="connectionFactory">A factory method that produces a new database connection of type <typeparamref name="T"/> for each HTTP request.</param>
    [DebuggerStepThrough]
    protected DbHttpMessageHandler(Func<T> connectionFactory) 
        : base(connectionFactory)
    {
    }

    /// <summary>
    /// Executes a database command created from the given HTTP request using a specific type of database connection.
    /// </summary>
    /// <param name="connection">The database connection of type <see cref="DbConnection"/> to be used for executing the command.</param>
    /// <param name="request">The HTTP request message containing the details for the database command.</param>
    /// <param name="response">The HTTP response message that will be returned after executing the command.</param>
    /// <param name="cancellation">The cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected sealed override Task ExecuteAsync(
        DbConnection connection, 
        HttpRequestMessage request, 
        HttpResponseMessage response,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return ExecuteAsync((T)connection, request, response, cancellation);
    }

    /// <summary>
    /// Executes a database command created from the given HTTP request using a specific type of database connection.
    /// This method is intended to be implemented by derived classes to provide database-specific logic.
    /// </summary>
    /// <param name="connection">The database connection of type <typeparamref name="T"/> to be used for executing the command.</param>
    /// <param name="request">The HTTP request message containing the details for the database command.</param>
    /// <param name="response">The HTTP response message that will be returned after executing the command.</param>
    /// <param name="cancellation">The cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected abstract Task ExecuteAsync(
        T connection, 
        HttpRequestMessage request, 
        HttpResponseMessage response, 
        CancellationToken cancellation);
}