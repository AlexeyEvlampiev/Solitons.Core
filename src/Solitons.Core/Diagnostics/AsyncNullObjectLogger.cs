using System.Diagnostics;
using System.Security.Principal;
using System.Threading.Tasks;
using Solitons.Diagnostics.Common;
using static System.Reflection.Metadata.BlobBuilder;

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
