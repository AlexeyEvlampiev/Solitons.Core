using System;
using System.Diagnostics;
using System.IO;
using System.Security.Principal;
using System.Threading.Tasks;
using Solitons.Data;

namespace Solitons.Diagnostics.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsoleAsyncLogger : AsyncLogger
    {
        private static readonly object SyncObject = new object();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnLogging(LogEventArgs args)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnLogged(LogEventArgs args)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="writer"></param>
        protected virtual void Log(LogEventArgs args, TextWriter writer)
        {
            writer.WriteLine(args.Content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        protected virtual ConsoleColor ToForegroundColor(LogLevel level) => Console.ForegroundColor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        protected sealed override Task LogAsync(LogEventArgs args)
        {
            OnLogging(args);
            lock (SyncObject)
            {
                var foregroundColor = Console.ForegroundColor;
                try
                {
                    Console.ForegroundColor = ToForegroundColor(args.Level);
                    Log(args, args.Level == LogLevel.Error ? Console.Error : Console.Out);
                }
                finally
                {
                    Console.ForegroundColor = foregroundColor;
                }
            }
            
            OnLogged(args);
            return Task.CompletedTask;
        }
    }
}
