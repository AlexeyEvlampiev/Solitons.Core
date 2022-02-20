using System.Diagnostics;
using System.Threading.Tasks;
using Solitons.Diagnostics.Common;

namespace Solitons.Diagnostics
{
    sealed class TraceAsyncLogger : AsyncLogger
    {
        public static readonly TraceAsyncLogger Instance = new();

        private TraceAsyncLogger() { }

        protected override Task LogAsync(ILogEntry entry)
        {
            var json = entry
                .AsDataTransferObject()
                .ToJsonString();

            switch (entry.Level)
            {
                case (LogLevel.Error):
                    Trace.TraceError(json);
                    break;
                case (LogLevel.Warning):
                    Trace.TraceWarning(json);
                    break;
                default:
                    Trace.TraceInformation(json);
                    break;
            }
            return Task.CompletedTask;
        }
    }
}
