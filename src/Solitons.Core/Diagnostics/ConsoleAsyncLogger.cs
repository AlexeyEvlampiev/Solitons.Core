using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Solitons.Diagnostics.Common;

namespace Solitons.Diagnostics
{
    sealed class ConsoleAsyncLogger : AsyncLogger
    {
        public static readonly ConsoleAsyncLogger Instance = new ConsoleAsyncLogger();

        private ConsoleAsyncLogger() {}

        [DebuggerNonUserCode]
        protected override Task LogAsync(ILogEntry entry)
        {
            Console.WriteLine(entry.AsDataTransferObject().ToJsonString());
            return Task.CompletedTask;
        }
    }
}
