using System.Diagnostics;
using System.Threading.Tasks;
using Solitons.Diagnostics.Common;

namespace Solitons.Diagnostics
{
    sealed class AsyncNullObjectLogger : AsyncLogger
    {
        public static readonly AsyncNullObjectLogger Instance = new();

        private AsyncNullObjectLogger() { }

        [DebuggerNonUserCode]
        protected override Task LogAsync(LogEventArgs args) => Task.CompletedTask;

    }
}
