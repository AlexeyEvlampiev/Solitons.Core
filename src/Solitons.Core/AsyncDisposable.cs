using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons
{
    /// <summary>
    /// Represents an object that can be asynchronously disposed of, releasing any resources it holds.
    /// </summary>
    public abstract class AsyncDisposable : IAsyncDisposable
    {
        private int _disposed = 0;

        /// <summary>
        /// Returns an empty <see cref="AsyncDisposable"/> instance.
        /// </summary>
        public static IAsyncDisposable Empty => new AsyncDisposableNullObject();

        /// <summary>
        /// Creates a new <see cref="AsyncDisposable"/> instance with the specified dispose callback.
        /// </summary>
        /// <param name="callback">The dispose callback to execute when the object is disposed of.</param>
        /// <returns>A new <see cref="AsyncDisposable"/> instance.</returns>
        public static IAsyncDisposable Create(Func<Task> callback) => new RelayAsyncDisposable(callback);

        /// <summary>
        /// Releases any resources held by the object in an asynchronous manner.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous dispose operation.</returns>
        protected abstract ValueTask DisposeAsync();

        /// <summary>
        /// Disposes of the object asynchronously, releasing any resources it holds.
        /// </summary>
        /// <returns>A <see cref="ValueTask"/> representing the asynchronous dispose operation.</returns>
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
            /// Does nothing and completes immediately.
            /// </summary>
            /// <returns>A <see cref="ValueTask"/> that completes immediately.</returns>
            [DebuggerNonUserCode]
            public ValueTask DisposeAsync() => ValueTask.CompletedTask;
        }
    }
}
