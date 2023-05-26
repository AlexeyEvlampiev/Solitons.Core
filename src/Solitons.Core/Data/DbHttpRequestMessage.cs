using System;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Reactive;

namespace Solitons.Data;

/// <summary>
/// Represents an HTTP request message to be handled by a transactional database.
/// This class extends <see cref="HttpRequestMessage"/> with database-specific functionality.
/// </summary>
public sealed class DbHttpRequestMessage : HttpRequestMessage
{
    /// <summary>
    /// The default command timeout duration in seconds.
    /// </summary>
    public const int DefaultCommandTimeout = 3;

    /// <summary>
    /// The default maximum number of retry attempts.
    /// </summary>
    public const int DefaultMaxRetryAttemptNumber = 3;

    /// <summary>
    /// Represents a method that will handle committing or rolling back database transactions.
    /// </summary>
    /// <param name="response">The HTTP response message related to the transaction.</param>
    /// <param name="cancellation">The token that allows for the operation to be cancelled.</param>
    /// <returns>A Task representing the asynchronous operation with a result indicating whether the transaction was successfully committed or not.</returns>
    public delegate Task<bool> CommitApprovalHandler(HttpResponseMessage response, CancellationToken cancellation);

    private CommitApprovalHandler? _commitApprovalHandler;
    private Func<RetryPolicyArgs, CancellationToken, Task<bool>> _retryPolicyCallback;

    /// <summary>
    /// Initializes a new instance of the <see cref="DbHttpRequestMessage"/> class.
    /// </summary>
    /// <param name="method">The HTTP method to be used by the request message.</param>
    /// <param name="requestUrl">The request URI used by the request message.</param>
    public DbHttpRequestMessage(HttpMethod method, Uri? requestUrl) : base(method, requestUrl)
    {
        _retryPolicyCallback = DefaultRetryPolicy;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DbHttpRequestMessage"/> class.
    /// </summary>
    /// <param name="method">The HTTP method to be used by the request message.</param>
    /// <param name="requestUrl">A string representing the request URI to be used by the request message.</param>
    public DbHttpRequestMessage(HttpMethod method, string? requestUrl) : base(method, requestUrl)
    {
        _retryPolicyCallback = DefaultRetryPolicy;
    }

    /// <summary>
    /// Default retry policy callback function that determines if a retry attempt should be made.
    /// </summary>
    /// <param name="args">The retry policy arguments containing information about the retry attempt.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the retry attempt.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains a boolean indicating whether a retry attempt should be made.</returns>
    [DebuggerStepThrough]
    public static Task<bool> DefaultRetryPolicy(
        RetryPolicyArgs args,
        CancellationToken cancellation)
    {
        return DefaultRetryPolicy(args, IClock.System, cancellation);
    }

    /// <summary>
    /// Default retry policy callback function that determines if a retry attempt should be made.
    /// </summary>
    /// <param name="args">The retry policy arguments containing information about the retry attempt.</param>
    /// <param name="clock">The clock provider used for timing-related operations.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the retry attempt.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains a boolean indicating whether a retry attempt should be made.</returns>
    internal static async Task<bool> DefaultRetryPolicy(
        RetryPolicyArgs args, 
        IClock clock,
        CancellationToken cancellation)
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
            await clock.DelayAsync(sleepInterval, cancellation);
            return true;
        }
        return false;
    }



    /// <summary>
    /// Asynchronously determines whether the database transaction triggered by this HTTP request can be committed, based on the received HTTP response.
    /// </summary>
    /// <param name="response">The received <see cref="HttpResponseMessage"/>.</param>
    /// <param name="cancellation">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous operation. The task result contains a boolean indicating the commit eligibility of the transaction associated with this HTTP request.</returns>
    internal async Task<bool> CanCommitAsync(HttpResponseMessage response, CancellationToken cancellation)
    {
        if (_commitApprovalHandler is null)
        {
            return true;
        }

        var tasks = _commitApprovalHandler
            .GetInvocationList()
            .Cast<CommitApprovalHandler>()
            .Select(handler => handler.Invoke(response, cancellation))
            .ToList();
        foreach (var task in tasks)
        {
            if (false == await task)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Determines whether the operation should be retried given the specified retry policy arguments.
    /// </summary>
    /// <param name="args">The retry policy arguments.</param>
    /// <param name="cancellation">A CancellationToken to observe while waiting for the task to complete.</param>
    /// <returns>A task that represents the asynchronous operation. 
    /// The task result contains a boolean indicating whether the operation should be retried.</returns>
    /// <exception cref="OperationCanceledException">Thrown when the operation is cancelled.</exception>
    [DebuggerStepThrough]
    internal Task<bool> ShouldRetryAsync(RetryPolicyArgs args, CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        return _retryPolicyCallback.Invoke(args, cancellation);
    }

    /// <summary>
    /// Sets the completion callback function to determine if the HTTP request can be committed.
    /// </summary>
    /// <param name="callback">A function to be called to determine if the HTTP request can be committed.</param>
    /// <returns>The current <see cref="DbHttpRequestMessage"/> instance.</returns>
    [DebuggerNonUserCode]
    public DbHttpRequestMessage WithCommitApproval(
        CommitApprovalHandler callback)
    {
        _commitApprovalHandler += callback;
        return this;
    }

    /// <summary>
    /// Sets the retry policy callback function to determine if a retry attempt should be made.
    /// </summary>
    /// <param name="handler">A function to be called to determine if a retry attempt should be made.</param>
    /// <returns>The current <see cref="DbHttpRequestMessage"/> instance.</returns>
    [DebuggerNonUserCode]
    public DbHttpRequestMessage WithRetryPolicy(Func<RetryPolicyArgs, CancellationToken, Task<bool>> handler)
    {
        _retryPolicyCallback = handler;
        return this;
    }
}