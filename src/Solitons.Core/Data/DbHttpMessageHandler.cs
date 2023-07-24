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
using Solitons.Net.Http;
using Solitons.Reactive;

namespace Solitons.Data;


/// <summary>
/// Abstract base class for a handler that handles HTTP request messages and manages database transactions associated with them.
/// This class extends <see cref="HttpMessageHandler"/>.
/// </summary>
public abstract class DbHttpMessageHandler : HttpMessageHandler
{
    private const string TransactionApprovalOptionsKey = "2c2cc6ed61f84192a2f73afe1320d04f";

    /// <summary>
    /// The default maximum number of retry attempts.
    /// </summary>
    internal const int DefaultMaxRetryAttemptNumber = 3;

    private readonly Func<HttpRequestMessage, CancellationToken, Task<DbTransaction>> _beginTransactionAsync;
    private readonly IClock _clock = IClock.System;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbHttpMessageHandler"/> class with the provided database transaction.
    /// </summary>
    /// <param name="transaction">The externally managed database transaction.</param>
    /// <exception cref="ArgumentException">Thrown if the connection of the provided transaction is not in an open state.</exception>
    protected DbHttpMessageHandler(DbTransaction transaction)
    {
        var connection = ThrowIf.NullReference(transaction.Connection);
        if (connection.State != ConnectionState.Open)
        {
            throw new ArgumentException("The connection must be in an open state.", nameof(transaction));
        }

        var managedTransaction =
            transaction as ManagedDbTransaction
            ?? new ManagedDbTransaction(transaction);
        _beginTransactionAsync = [DebuggerNonUserCode](request, cancellation) => Task.FromResult((DbTransaction)managedTransaction);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbHttpMessageHandler"/> class with the provided connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <exception cref="ArgumentException">Thrown if the provided connection string is null or whitespace.</exception>
    protected DbHttpMessageHandler(string connectionString)
    {
        connectionString = ThrowIf.ArgumentNullOrWhiteSpace(connectionString);
        _beginTransactionAsync = BeginAsync;

        [DebuggerStepThrough]
        async Task<DbTransaction> BeginAsync(HttpRequestMessage request, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            DbConnection? connection = null;
            try
            {
                connection = CreateConnection(connectionString);
                await connection.OpenAsync(cancellation);
                return await BeginTransactionAsync(request, connection, cancellation);
            }
            catch (Exception e)
            {
                connection?.Dispose();
                throw;
            }
        }
    }



    /// <summary>
    /// Begins a database transaction asynchronously, associated with the provided HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request associated with the transaction.</param>
    /// <param name="connection">The database connection to use for the transaction.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the begun database transaction.</returns>
    protected virtual ValueTask<DbTransaction> BeginTransactionAsync(
        HttpRequestMessage request,
        DbConnection connection,
        CancellationToken cancellation) => connection.BeginTransactionAsync(cancellation);

    /// <summary>
    /// Creates a new connection to the database.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <returns>The created database connection.</returns>
    protected abstract DbConnection CreateConnection(string connectionString);


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


            await using var transaction = await _beginTransactionAsync(request, cancellation);
            await using var _ = transaction is ManagedDbTransaction
                ? AsyncDisposable.Empty :
                transaction.Connection;

            var connection = ThrowIf.NullReference(transaction.Connection);


            await Observable
                .FromAsync([DebuggerStepThrough] () => ExecuteAsync(connection, request, response, cancellation))
                .WithRetryPolicy([DebuggerStepThrough](args) =>CanRetryAsync(args, cancellation));


            if (transaction is not ManagedDbTransaction)
            {
                if (await CanCommitAsync(response, cancellation))
                {
                    await transaction.CommitAsync(cancellation);
                }

                await connection.CloseAsync();
            }
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

    protected virtual IAsyncLogger ConfigLogger(
        IAsyncLogger logger, 
        HttpRequestMessage request)
    {
        return logger
            .WithProperty("requestUrl", request.RequestUri?.ToString() ?? String.Empty);
    }


    protected virtual Task<bool> CanCommitAsync(HttpResponseMessage response, CancellationToken cancellation) =>
        Task.FromResult(true);

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
    /// Executes a database command created from the given HTTP request.
    /// </summary>
    /// <param name="connection">The database connection.</param>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="response">The HTTP response message.</param>
    /// <param name="cancellation">The cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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


    /// <summary>
    /// Sets the <see cref="HttpTransactionCallback"/> for the request.
    /// </summary>
    /// <param name="options">The options for the HTTP request.</param>
    /// <param name="callback">The callback to be invoked when the HTTP request completes.</param>
    /// <returns>The modified request options.</returns>
    /// <remarks>
    /// This extension method allows developers to include custom commit/rollback logic in the HTTP request pipeline.
    /// The callback is invoked when the HTTP request completes and is used to decide whether to commit or rollback the transaction based on the HTTP response.
    /// </remarks>
    [DebuggerNonUserCode]
    public static HttpRequestOptions SetTransactionCallback(
        HttpRequestOptions options,
        HttpTransactionCallback callback)
    {
        var key = new HttpRequestOptionsKey<HttpTransactionCallback>(TransactionApprovalOptionsKey);
        options.Set(key, callback);
        return options;
    }

    /// <summary>
    /// Sets the transaction callback for the request.
    /// </summary>
    /// <param name="options">The options for the HTTP request.</param>
    /// <param name="callback">A function that returns a task that completes when the HTTP request completes.</param>
    /// <returns>The modified request options.</returns>
    /// <remarks>
    /// This version of the SetTransactionCallback extension method accepts a function that returns a <see cref="Task"/> representing the transaction commit/rollback decision.
    /// This allows developers to perform asynchronous operations in the callback.
    /// </remarks>
    [DebuggerNonUserCode]
    public static HttpRequestOptions SetTransactionCallback(
        HttpRequestOptions options,
        Func<HttpResponseMessage, Task<bool>> callback)
    {
        var key = new HttpRequestOptionsKey<HttpTransactionCallback>(TransactionApprovalOptionsKey);
        options.Set(key, CanCommitAsync);
        return options;
        [DebuggerNonUserCode]
        Task<bool> CanCommitAsync(HttpResponseMessage response, CancellationToken cancellation) => callback(response);
    }

    /// <summary>
    /// Sets the transaction callback for the request.
    /// </summary>
    /// <param name="options">The options for the HTTP request.</param>
    /// <param name="callback">A function that returns a boolean indicating whether to commit or rollback the transaction.</param>
    /// <returns>The modified request options.</returns>
    /// <remarks>
    /// This version of the SetTransactionCallback extension method accepts a function that synchronously returns the transaction commit/rollback decision.
    /// This can be useful in scenarios where the decision does not rely on asynchronous operations.
    /// </remarks>
    [DebuggerNonUserCode]
    public static HttpRequestOptions SetTransactionCallback(
        HttpRequestOptions options,
        Func<HttpResponseMessage, bool> callback)
    {
        var key = new HttpRequestOptionsKey<HttpTransactionCallback>(TransactionApprovalOptionsKey);
        options.Set(key, CanCommitAsync);
        return options;
        [DebuggerNonUserCode]
        Task<bool> CanCommitAsync(HttpResponseMessage response, CancellationToken cancellation) => Task.FromResult(callback(response));
    }

    /// <summary>
    /// Retrieves the transaction callback from the request options.
    /// </summary>
    /// <param name="options">The options for the HTTP request.</param>
    /// <returns>The transaction callback for the request.</returns>
    /// <remarks>
    /// If no callback has been set, a default callback that always returns true is provided, indicating that the transaction should always be committed.
    /// </remarks>
    [DebuggerNonUserCode]
    public static HttpTransactionCallback GetTransactionCallback(HttpRequestOptions options)
    {
        var key = new HttpRequestOptionsKey<HttpTransactionCallback>(TransactionApprovalOptionsKey);
        options.TryGetValue(key, out var callback);
        return callback ?? new HttpTransactionCallback(CanCommitAsync);
        [DebuggerNonUserCode]
        Task<bool> CanCommitAsync(HttpResponseMessage response, CancellationToken cancellation) => Task.FromResult(true);
    }
}

/// <summary>
/// Abstract base class for a handler that handles HTTP request messages and manages database transactions associated with them.
/// This class extends <see cref="DbHttpMessageHandler"/>, and is intended for use with a specific type of database connection.
/// </summary>
/// <typeparam name="T">The type of the database connection to be used. This must be a subtype of <see cref="DbConnection"/>.</typeparam>
public abstract class DbHttpMessageHandler<T> : DbHttpMessageHandler where T : DbConnection
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbHttpMessageHandler{T}"/> class using the provided database transaction.
    /// </summary>
    /// <param name="transaction">The externally managed database transaction to be used for the HTTP request handling process.</param>
    /// <remarks>As this constructor directly accepts a <see cref="DbTransaction"/>, it expects the transaction's connection to be in an open state.</remarks>
    [DebuggerStepThrough]
    protected DbHttpMessageHandler(DbTransaction transaction) : base(transaction)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbHttpMessageHandler{T}"/> class using the provided connection string.
    /// </summary>
    /// <param name="connectionString">The connection string to be used for establishing a database connection for the HTTP request handling process.</param>
    /// <remarks>The connection string should be valid according to the specific database provider's requirements.</remarks>
    [DebuggerStepThrough]
    protected DbHttpMessageHandler(string connectionString) : base(connectionString)
    {
    }

    /// <summary>
    /// Executes a database transaction for processing the HTTP request asynchronously.
    /// </summary>
    /// <param name="connection">The database connection to be used in the request handling process.</param>
    /// <param name="request">The HTTP request to be processed.</param>
    /// <param name="response">The HTTP response to be sent.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <remarks>This method should be overridden in a derived class with the specific logic to handle the HTTP request.</remarks>
    protected abstract Task ExecuteAsync(
        T connection,
        HttpRequestMessage request,
        HttpResponseMessage response,
        CancellationToken cancellation);

    /// <summary>
    /// Begins a database transaction asynchronously in the context of an HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request to be processed.</param>
    /// <param name="connection">The database connection to be used in the request handling process.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the started database transaction.</returns>
    /// <remarks>This method should be overridden in a derived class with the specific logic to begin a database transaction.</remarks>
    protected abstract ValueTask<DbTransaction> BeginTransactionAsync(
        HttpRequestMessage request,
        T connection,
        CancellationToken cancellation);

    /// <summary>
    /// Creates a new provider-specific database connection.
    /// </summary>
    /// <param name="connectionString">The connection string to the database.</param>
    /// <returns>The created database connection.</returns>
    protected abstract T CreateProviderConnection(string connectionString);

    /// <summary>
    /// Begins a database transaction asynchronously in the context of an HTTP request. This is a sealed override that casts the provided <see cref="DbConnection"/> to the type-specific connection before invoking the transaction.
    /// </summary>
    /// <param name="request">The HTTP request to be processed.</param>
    /// <param name="connection">The database connection to be used in the request handling process. This connection will be cast to the type-specific connection.</param>
    /// <param name="cancellation">A cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result is the started database transaction.</returns>
    /// <remarks>
    /// This method cannot be overridden in a derived class.
    /// It ensures that the type-specific <see cref="BeginTransactionAsync(HttpRequestMessage, T, CancellationToken)"/> is called with the correct type of connection.
    /// </remarks>
    [DebuggerStepThrough]
    protected sealed override ValueTask<DbTransaction> BeginTransactionAsync(
        HttpRequestMessage request,
        DbConnection connection,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return BeginTransactionAsync(request, (T)connection, cancellation);
    }

    /// <summary>
    /// Creates a new instance of the type-specific <see cref="DbConnection"/> using the provided connection string. 
    /// </summary>
    /// <param name="connectionString">The connection string used to establish the database connection.</param>
    /// <returns>The newly created type-specific database connection.</returns>
    /// <remarks>
    /// This method cannot be overridden in a derived class.
    /// It ensures that the type-specific <see cref="CreateProviderConnection(string)"/> is used to create the database connection.
    /// </remarks>
    [DebuggerStepThrough]
    protected sealed override DbConnection CreateConnection(string connectionString) => CreateProviderConnection(connectionString);

    /// <summary>
    /// Executes a database command created for the given HTTP request.
    /// </summary>
    /// <param name="connection">The provider-specific database connection.</param>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="response">The HTTP response message.</param>
    /// <param name="cancellation">The cancellation token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    [DebuggerStepThrough]
    protected override Task ExecuteAsync(
        DbConnection connection,
        HttpRequestMessage request,
        HttpResponseMessage response,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return ExecuteAsync((T)connection, request, response, cancellation);
    }
}