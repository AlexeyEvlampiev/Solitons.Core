using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Solitons
{
    sealed class RelayAsyncDisposable : AsyncDisposable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly Func<Task> _callback;

        public RelayAsyncDisposable(Func<Task> callback)
        {
            _callback = callback;
        }

        [DebuggerStepThrough]
        protected override async ValueTask DisposeAsync()
        {
            await _callback.Invoke();
        }
    }
}
