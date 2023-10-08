using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons;

/// <summary>
/// Represents an abstract base class for an asynchronous disposer with an ability to dispose multiple resources at once.
/// </summary>
public abstract class AsyncAutoDisposer : IAsyncDisposable
{
    private int _isDisposed = 0;

    /// <summary>
    /// Gets the total number of disposable resources managed by the disposer.
    /// </summary>
    public abstract int ResourceCount { get; }

    /// <summary>
    /// Adds a disposable resource to the collection of resources managed by the disposer.
    /// </summary>
    /// <remarks>
    /// Derived classes must implement this method to determine how to store and manage 
    /// disposable resources. For example, a derived class might decide to store all resources 
    /// in a  collection.
    /// </remarks>
    /// <param name="resource">The resource to be managed by the disposer.</param>
    protected abstract void Add(DisposableResource resource);

    /// <summary>
    /// An abstract method to dispose all resources.
    /// </summary>
    /// <returns>The task object representing the asynchronous operation.</returns>
    protected abstract Task DisposeAllAsync();

    /// <summary>
    /// Asynchronously disposes all disposable resources managed by this disposer.
    /// </summary>
    /// <returns>A ValueTask that represents the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        if (Interlocked.CompareExchange(ref _isDisposed, 1, 0) != 0)
        {
            return;
        }

        await DisposeAllAsync();
    }

    /// <summary>
    /// Checks whether the disposer is already disposed, if so throws an ObjectDisposedException.
    /// </summary>
    /// <exception cref="ObjectDisposedException">Thrown when the method is called after the disposer has been disposed.</exception>
    protected void ThrowIfDisposed()
    {
        if (_isDisposed != 0)
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }


    /// <summary>
    /// Adds an IAsyncDisposable resource to the inner collection.
    /// </summary>
    /// <param name="resource">The IAsyncDisposable resource to be added to the collection.</param>
    /// <param name="description">Optional description for the resource. Defaults to the name of the calling member.</param>
    /// <param name="submitterMemberName">The calling member's name.</param>
    /// <param name="submitterFilePath">The source file path of the calling member.</param>
    /// <param name="submitterLineNumber">The source file line number of the calling member.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the method is called after the object has been disposed.</exception>
    public void AddResource(
        IAsyncDisposable resource,
        string? description = default,
        [CallerMemberName] string submitterMemberName = "",
        [CallerFilePath] string submitterFilePath = "",
        [CallerLineNumber] int submitterLineNumber = -1)
    {
        ThrowIfDisposed();
        Add(new DisposableResource(
            resource,
            description.DefaultIfNullOrWhiteSpace(submitterMemberName),
            submitterMemberName,
            submitterFilePath,
            submitterLineNumber));
    }

    /// <summary>
    /// Adds an IDisposable resource to the inner collection.
    /// </summary>
    /// <param name="resource">The IDisposable resource to be added to the collection.</param>
    /// <param name="description">Optional description for the resource. Defaults to the name of the calling member.</param>
    /// <param name="submitterMemberName">The calling member's name.</param>
    /// <param name="submitterFilePath">The source file path of the calling member.</param>
    /// <param name="submitterLineNumber">The source file line number of the calling member.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the method is called after the object has been disposed.</exception>
    [DebuggerNonUserCode]
    public void AddResource(
        IDisposable resource,
        string? description = default,
        [CallerMemberName] string submitterMemberName = "",
        [CallerFilePath] string submitterFilePath = "",
        [CallerLineNumber] int submitterLineNumber = -1)
    {
        ThrowIfDisposed();
        Add(new DisposableResource(
            AsyncDisposable.Create(resource),
            description.DefaultIfNullOrWhiteSpace(submitterMemberName),
            submitterMemberName,
            submitterFilePath,
            submitterLineNumber));
    }

    /// <summary>
    /// Adds a <see cref="Func{Task}"/> callback to the inner collection.
    /// </summary>
    /// <param name="handler">The <see cref="Func{Task}"/>  callback to be added to the collection.</param>
    /// <param name="description">Optional description for the resource. Defaults to the name of the calling member.</param>
    /// <param name="submitterMemberName">The calling member's name.</param>
    /// <param name="submitterFilePath">The source file path of the calling member.</param>
    /// <param name="submitterLineNumber">The source file line number of the calling member.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the method is called after the object has been disposed.</exception>
    [DebuggerNonUserCode]
    public void AddResource(
        Func<Task> handler,
        string? description = default,
        [CallerMemberName] string submitterMemberName = "",
        [CallerFilePath] string submitterFilePath = "",
        [CallerLineNumber] int submitterLineNumber = -1)
    {
        ThrowIfDisposed();
        Add(new DisposableResource(
            AsyncDisposable.Create(handler),
            description.DefaultIfNullOrWhiteSpace(submitterMemberName),
            submitterMemberName,
            submitterFilePath,
            submitterLineNumber));
    }

    /// <summary>
    /// Adds a <see cref="Func{ValueTask}"/> callback to the inner collection.
    /// </summary>
    /// <param name="handler">The <see cref="Func{ValueTask}"/> callback to be added to the collection.</param>
    /// <param name="description">Optional description for the resource. Defaults to the name of the calling member.</param>
    /// <param name="submitterMemberName">The calling member's name.</param>
    /// <param name="submitterFilePath">The source file path of the calling member.</param>
    /// <param name="submitterLineNumber">The source file line number of the calling member.</param>
    /// <exception cref="ObjectDisposedException">Thrown when the method is called after the object has been disposed.</exception>
    [DebuggerNonUserCode]
    public void AddResource(
        Func<ValueTask> handler,
        string? description = default,
        [CallerMemberName] string submitterMemberName = "",
        [CallerFilePath] string submitterFilePath = "",
        [CallerLineNumber] int submitterLineNumber = -1)
    {
        ThrowIfDisposed();
        Add(new DisposableResource(
            AsyncDisposable.Create(handler),
            description.DefaultIfNullOrWhiteSpace(submitterMemberName),
            submitterMemberName,
            submitterFilePath,
            submitterLineNumber));
    }

    /// <summary>
    /// Represents a resource managed by the AsyncStackAutoDisposer.
    /// </summary>
    public sealed record DisposableResource
    {
        /// <summary>
        /// Initializes a new instance of the Resource record with the specified values.
        /// </summary>
        /// <param name="asyncDisposable">The asynchronous disposable resource.</param>
        /// <param name="description">The description or identifier of the resource.</param>
        /// <param name="submitterMemberName">The name of the member that submitted the resource.</param>
        /// <param name="submitterFilePath">The file path of the member that submitted the resource.</param>
        /// <param name="submitterFileLineNumber">The line number in the file of the member that submitted the resource.</param>
        public DisposableResource(
            IAsyncDisposable asyncDisposable,
            string description,
            string submitterMemberName,
            string submitterFilePath,
            int submitterFileLineNumber)
        {
            this.AsyncDisposable = asyncDisposable;
            this.Description = description;
            this.SubmitterMemberName = submitterMemberName;
            this.SubmitterFilePath = submitterFilePath;
            this.SubmitterFileLineNumber = submitterFileLineNumber;
        }

        /// <summary>
        /// Gets the asynchronous disposable resource.
        /// </summary>
        public IAsyncDisposable AsyncDisposable { get; init; }

        /// <summary>
        /// Gets the description or identifier of the resource.
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        /// Gets the name of the member that submitted the resource.
        /// </summary>
        public string SubmitterMemberName { get; init; }

        /// <summary>
        /// Gets the file path of the member that submitted the resource.
        /// </summary>
        public string SubmitterFilePath { get; init; }

        /// <summary>
        /// Gets the line number in the file of the member that submitted the resource.
        /// </summary>
        public int SubmitterFileLineNumber { get; init; }
    }
}