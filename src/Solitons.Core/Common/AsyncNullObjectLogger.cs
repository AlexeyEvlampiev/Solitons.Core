using System.Threading.Tasks;

namespace Solitons.Common
{
    sealed class AsyncNullObjectLogger : AsyncLogger
    {
        protected override Task LogAsync(ILogEntry entry) => Task.CompletedTask;
    }
}
