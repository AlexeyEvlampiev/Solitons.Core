using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EntryExpiredEventArgs : EventArgs
    {
        private readonly Func<CancellationToken, Task> _deletionCallback;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deletionCallback"></param>
        public EntryExpiredEventArgs(string id, Func<CancellationToken, Task> deletionCallback)
        {
            Id = id.ThrowIfNullOrWhiteSpaceArgument(nameof(id));
            _deletionCallback = deletionCallback ?? throw new ArgumentNullException(nameof(deletionCallback));
        }

        /// <summary>
        /// 
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public Task DeleteAsync(CancellationToken cancellation = default)
            => _deletionCallback.Invoke(cancellation);
    }
}
