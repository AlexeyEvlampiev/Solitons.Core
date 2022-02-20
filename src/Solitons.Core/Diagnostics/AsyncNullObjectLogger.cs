using System.Threading.Tasks;
using Solitons.Diagnostics.Common;

namespace Solitons.Diagnostics
{
    sealed class AsyncNullObjectLogger : AsyncLogger
    {
        public static readonly AsyncNullObjectLogger Instance = new();

        private AsyncNullObjectLogger() { }

        protected override Task LogAsync(ILogEntry entry) => Task.CompletedTask;
    }
}
