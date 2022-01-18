using System.Threading.Tasks;

namespace Solitons.Common
{
    sealed class AsyncNullObjectLogger : AsyncLogger
    {
        public static readonly AsyncNullObjectLogger Instance = new();

        private AsyncNullObjectLogger() { }

        protected override Task LogAsync(ILogEntry entry) => Task.CompletedTask;
    }
}
