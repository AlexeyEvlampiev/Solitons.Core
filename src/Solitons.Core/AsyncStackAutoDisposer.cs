using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Solitons;


/// <summary>
/// Manages a stack of IAsyncDisposable resources. 
/// Disposes the items in a last-in-first-out (LIFO) order when this instance is disposed asynchronously.
/// </summary>
/// <remarks>
/// Methods adding disposable resources are not thread-safe. Users should ensure thread-safety when using this class.
/// </remarks>
public class AsyncStackAutoDisposer : AsyncAutoDisposer
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly Stack<DisposableResource> _stack = new();
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Action<ErrorArgs> _onError;

    /// <summary>
    /// Contains information related to an error that occurred during resource disposal.
    /// </summary>
    public sealed class ErrorArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorArgs"/> class.
        /// </summary>
        /// <param name="resource">The resource that caused the error during disposal.</param>
        /// <param name="exception">The exception that occurred during disposal.</param>
        internal ErrorArgs(DisposableResource resource, Exception exception)
        {
            Resource = resource;
            Exception = exception;
            CanProceed = true;
        }

        /// <summary>
        /// Gets the resource that caused the error during disposal.
        /// </summary>
        public DisposableResource Resource { get; }

        /// <summary>
        /// Gets the exception that occurred during disposal.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to continue disposing of other resources after this error.
        /// </summary>
        public bool CanProceed { get; set; }
    }

    /// <summary>
    /// Creates a new instance of the <see cref="AsyncStackAutoDisposer"/>.
    /// </summary>
    public AsyncStackAutoDisposer() 
        : this((args) =>{})
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="AsyncStackAutoDisposer"/>.
    /// </summary>
    /// <param name="onError">An optional delegate for handling any exceptions thrown during asynchronous disposal.</param>
    public AsyncStackAutoDisposer(Action<ErrorArgs> onError)
    {
        _onError = onError;
    }

    /// <summary>
    /// Gets the number of elements contained in the stack.
    /// </summary>
    public override int ResourceCount => _stack.Count;

    /// <summary>
    /// Adds a disposable resource to the top of the internal stack of resources.
    /// </summary>
    /// <param name="resource">The resource to be managed by the disposer.</param>
    /// <remarks>
    /// This method is not thread-safe and should be invoked in a thread-safe manner if used in a multi-threaded context.
    /// </remarks>
    protected override void Add(DisposableResource resource)
    {
        _stack.Push(resource);
    }


    /// <summary>
    /// Asynchronously disposes all the disposable resources that are present in the stack. 
    /// If any exception occurs during disposal, the <see cref="_onError"/> delegate is invoked.
    /// </summary>
    /// <exception cref="AggregateException">Thrown when one or more exceptions occur during disposal.</exception>
    protected sealed override async Task DisposeAllAsync()
    {
        var exceptions = new List<Exception>();
        while (_stack.TryPop(out var item))
        {
            try
            {
                await item.AsyncDisposable.DisposeAsync();
            }
            catch (Exception e)
            {
                exceptions.Add(e);
                var errorArgs = new ErrorArgs(item, e);
                _onError.Invoke(errorArgs);
                if (errorArgs.CanProceed)
                {
                    exceptions.Add(e);
                    continue;
                }

                throw new AggregateException(exceptions);
            }
        }

        if (exceptions.Count > 0)
        {
            throw new AggregateException(exceptions);
        }
    }
}