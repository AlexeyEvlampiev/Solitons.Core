using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Caching.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ReadThroughETagCache<T> : ReadThroughCache<T> where T : class
    {
        #region Private Fields

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private State? _volatileState;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly object _syncRoot = new();
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eTag"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<State?> GetIfNonMatchAsync(string? eTag, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxEntityAge"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        protected sealed override async Task<T?> GetAsync(TimeSpan maxEntityAge, CancellationToken cancellation = default)
        {
            State? stateCopy = null;
            lock (_syncRoot)
            {
                stateCopy = _volatileState;
            }

            if (stateCopy == null)
            {
                stateCopy = await GetIfNonMatchAsync(null, cancellation);
                if (stateCopy is null) return null;
                lock (_syncRoot)
                {
                    _volatileState = stateCopy;
                    return stateCopy.Value;
                }
            }
            
            var creationTimeThreshold = DateTimeOffset.UtcNow - maxEntityAge;
            if (stateCopy.CreatedOn >= creationTimeThreshold)
            {
                return stateCopy.Value;
            }

            stateCopy = await GetIfNonMatchAsync(stateCopy.ETag, cancellation);

            lock (_syncRoot)
            {
                Debug.Assert(_volatileState is not null);
                stateCopy ??= _volatileState!.Clone();
                _volatileState = stateCopy;
            }

            return stateCopy.Value;
        }


        /// <summary>
        /// 
        /// </summary>
        protected sealed class State
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="value"></param>
            /// <param name="eTag"></param>
            [DebuggerNonUserCode]
            public State(T? value, string? eTag)
            {
                Value = value;
                ETag = eTag;
                CreatedOn = DateTimeOffset.UtcNow;
            }

            [DebuggerNonUserCode]
            private State(State other)
            {
                Value = other.Value;
                ETag = other.ETag;
                CreatedOn = DateTimeOffset.UtcNow;
            }

            internal State Clone() => new(this);

            /// <summary>
            /// 
            /// </summary>
            public T? Value { get; }

            /// <summary>
            /// 
            /// </summary>
            public string? ETag { get; }

            /// <summary>
            /// 
            /// </summary>
            public DateTimeOffset CreatedOn { get; }
        }
    }
}
