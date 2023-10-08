using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons;

/// <summary>
/// Represents a class that provides a mechanism for releasing unmanaged resources asynchronously.
/// </summary>
/// <remarks>
/// To release resources synchronously, implement the IDisposable interface.
/// </remarks>
public abstract class AsyncDisposable : IAsyncDisposable
{
    private int _disposed = 0;

    /// <summary>
    /// Provides an instance of an <see cref="AsyncDisposable"/> that does nothing when disposed.
    /// </summary>
    /// <value>
    /// The instance of an empty <see cref="AsyncDisposable"/>.
    /// </value>
    public static IAsyncDisposable Empty => new AsyncDisposableNullObject();

    /// <summary>
    /// Creates a new instance of an <see cref="AsyncDisposable"/> using the specified dispose callback.
    /// </summary>
    /// <param name="callback">A callback that represents the dispose operation to execute when the <see cref="AsyncDisposable"/> object is disposed of.</param>
    /// <returns>An instance of an <see cref="AsyncDisposable"/>.</returns>
    public static IAsyncDisposable Create(Func<Task> callback) => new RelayAsyncDisposable(callback);

    /// <summary>
    /// Creates a new instance of an <see cref="AsyncDisposable"/> using the specified dispose callback.
    /// </summary>
    /// <param name="callback">A callback that represents the dispose operation to execute when the <see cref="AsyncDisposable"/> object is disposed of.</param>
    /// <returns>An instance of an <see cref="AsyncDisposable"/>.</returns>
    public static IAsyncDisposable Create(Func<ValueTask> callback)
    {
        return Create(() => callback.Invoke().AsTask());
    }

    /// <summary>
    /// Creates a new instance of an <see cref="AsyncDisposable"/> using the specified dispose callback.
    /// </summary>
    /// <param name="callback">A callback that represents the dispose operation to execute when the <see cref="AsyncDisposable"/> object is disposed of.</param>
    /// <returns>An instance of an <see cref="AsyncDisposable"/>.</returns>
    public static IAsyncDisposable Create(Action callback) => new RelayAsyncDisposable(() =>
    {
        callback.Invoke();
        return Task.CompletedTask;
    });

    /// <summary>
    /// Creates a new instance of an <see cref="AsyncDisposable"/> using the dispose method of the given <see cref="IDisposable"/> instance.
    /// </summary>
    /// <param name="disposable">An object that implements <see cref="IDisposable"/> whose dispose method is used as the dispose operation.</param>
    /// <returns>An instance of an <see cref="AsyncDisposable"/>.</returns>
    public static IAsyncDisposable Create(IDisposable disposable)
    {
        return disposable is IAsyncDisposable ad ? ad : Create(disposable.Dispose);
    }


    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    protected abstract ValueTask DisposeAsync();

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="AsyncDisposable"/> and optionally releases the managed resources.
    /// </summary>
    /// <returns>A task that represents the asynchronous dispose operation.</returns>
    /// <exception cref="ObjectDisposedException">The object has already been disposed.</exception>
    [DebuggerStepThrough]
    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
        {
            await DisposeAsync().ConfigureAwait(false);
        }
        else
        {
            throw new ObjectDisposedException(GetType().FullName);
        }
    }

    private sealed class AsyncDisposableNullObject : IAsyncDisposable
    {
        /// <summary>
        /// A no-operation method that completes immediately when disposing an object.
        /// </summary>
        /// <returns>A completed task, indicating the completion of the disposal operation.</returns>
        [DebuggerNonUserCode]
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }
}