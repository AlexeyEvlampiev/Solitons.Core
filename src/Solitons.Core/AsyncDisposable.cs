using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AsyncDisposable : IAsyncDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public static IAsyncDisposable Empty => new AsyncDisposableNullObject();

        private int _disposed = 0;
        protected  abstract ValueTask DisposeAsync();

        [DebuggerStepThrough]
        ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                return DisposeAsync();
            }

            throw new ObjectDisposedException(GetType().FullName);
        }

        sealed class AsyncDisposableNullObject : IAsyncDisposable
        {
            [DebuggerNonUserCode]
            public ValueTask DisposeAsync() => ValueTask.CompletedTask;
        }
    }
}
