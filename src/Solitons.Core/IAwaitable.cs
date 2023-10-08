using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons;

/// <summary>
/// Represents an awaitable operation that provides a task representing the completion of the operation.
/// This is a single-signaling variant of the Observable pattern, which can be either "hot" (already ongoing)
/// or "cold" (kicked off by the call to AsTask). The precise semantics depend on the implementation.
/// </summary>
/// <remarks>
/// Implementations should ensure that the AsTask method is idempotent. That is, if the operation is "cold"
/// and calling AsTask initiates the operation, subsequent calls should not initiate the operation again.
/// Implementations should document whether they are "hot" or "cold", and any other semantics related to 
/// how the operation is initiated and run.
/// </remarks>
public interface IAwaitable
{
    /// <summary>
    /// Returns a task representing the awaitable operation.
    /// This could potentially initiate the operation (for "cold" awaitables), or just return the task 
    /// for an already ongoing operation (for "hot" awaitables).
    /// </summary>
    /// <param name="cancellation">
    /// A CancellationToken to observe while waiting for the task to complete.
    /// </param>
    /// <returns>A task that represents the completion of the awaitable operation.</returns>
    Task AsTask(CancellationToken cancellation = default);

    /// <summary>
    /// An IAwaitable that is never completed. This can be useful in scenarios where an operation is 
    /// expected to run indefinitely.
    /// </summary>
    public static readonly IAwaitable NeverCompleted = new NeverCompletedAwaitable();

    /// <summary>
    /// An IAwaitable that is always completed. This can be useful in scenarios where an operation is 
    /// considered to be instantly completed.
    /// </summary>
    public static readonly IAwaitable Completed = new CompletedAwaitable();

    /// <summary>
    /// Returns an IAwaitable that completes when any of the provided awaitables complete.
    /// </summary>
    /// <param name="awaitables">The collection of IAwaitable instances to observe.</param>
    /// <returns>An IAwaitable that completes when any of the provided awaitables complete.</returns>
    [DebuggerNonUserCode]
    public static IAwaitable WhenAny(IEnumerable<IAwaitable> awaitables)
    {
        return new AnyOfAwaitable(awaitables.Distinct());
    }

    /// <summary>
    /// Returns an IAwaitable that completes when all of the provided awaitables complete.
    /// </summary>
    /// <param name="awaitables">The collection of IAwaitable instances to observe.</param>
    /// <returns>An IAwaitable that completes when all of the provided awaitables complete.</returns>
    [DebuggerNonUserCode]
    public static IAwaitable WhenAll(IEnumerable<IAwaitable> awaitables)
    {
        return new AllOfAwaitable(awaitables.Distinct());
    }

    sealed class NeverCompletedAwaitable : IAwaitable
    {
        Task IAwaitable.AsTask(CancellationToken cancellation) => Observable
            .Never<Unit>()
            .ToTask(cancellation);
    }

    sealed class CompletedAwaitable : IAwaitable
    {
        Task IAwaitable.AsTask(CancellationToken cancellation) => Task.CompletedTask;
    }

    sealed class AnyOfAwaitable : IAwaitable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IAwaitable[] _sessions;

        public AnyOfAwaitable(IEnumerable<IAwaitable> sessions)
        {
            var list = sessions.ToList();
            _sessions = list.ToArray();
        }

        public Task AsTask(CancellationToken cancellation = default)
        {
            if (_sessions.Length == 0)
            {
                return Task.CompletedTask;
            }
            return Task.WhenAny(_sessions.Select(_ => _.AsTask(cancellation)));
        }
    }

    sealed class AllOfAwaitable : IAwaitable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IAwaitable[] _sessions;

        public AllOfAwaitable(IEnumerable<IAwaitable> sessions)
        {
            var list = sessions.ToList();
            _sessions = list.ToArray();
        }

        public Task AsTask(CancellationToken cancellation = default)
        {
            if (_sessions.Length == 0)
            {
                return Task.CompletedTask;
            }
            return Task.WhenAll(_sessions.Select(_ => _.AsTask(cancellation)));
        }
    }
}
