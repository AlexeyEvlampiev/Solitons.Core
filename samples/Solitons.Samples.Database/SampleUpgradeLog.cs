using System;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using DbUp.Engine.Output;
using Solitons;

namespace Solitons.Samples.Database
{
    class SampleUpgradeLog : IUpgradeLog
    {
        public sealed record LogEventArgs(TraceLevel Level, string Message);
        private readonly Subject<LogEventArgs> _logs = new();


        public SampleUpgradeLog()
        {
            _logs
                .DistinctUntilChanged()
                .Subscribe(args =>
                {
                    void WriteLine() => Console.WriteLine(args.Message);
                    switch (args.Level)
                    {
                        case TraceLevel.Error:
                            ConsoleColor.Red.AsForegroundColor(WriteLine);
                            break;
                        case TraceLevel.Warning:
                            ConsoleColor.Yellow.AsForegroundColor(WriteLine);
                            break;
                        default:
                            ConsoleColor.Gray.AsForegroundColor(WriteLine);
                            break;
                    }
                });

        }


        public void WriteInformation(string format, params object[] args)
        {
            var message = string.Format(format, args);
            _logs.OnNext(new LogEventArgs(TraceLevel.Info, message));
        }

        public void WriteError(string format, params object[] args)
        {
            if ("{0}".Equals(format)) return;
            var message = string
                .Format(format, args)
                .Replace(@"(?xs-m)\s*Npgsql[.]PostgresException.+", m => string.Empty);
            _logs.OnNext(new LogEventArgs(TraceLevel.Error, message));
        }

        public void WriteWarning(string format, params object[] args)
        {
            var message = string.Format(format, args);
            _logs.OnNext(new LogEventArgs(TraceLevel.Warning, message));
        }
    }
}
