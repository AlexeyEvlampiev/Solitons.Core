using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Solitons.Diagnostics.Common;

namespace Solitons.Diagnostics;

public static class UsingCustomConsoleLoggerExample
{
    public static async Task RunAsync()
    {
        var logger = new CustomConsoleLogger()
            .AsAsyncLogger()
            .WithTags("example", "custom implementation")
            .WithProperty("machine", Environment.MachineName)
            .WithProperty("user", Environment.UserName)
            .WithPrincipal(new GenericPrincipal(
                new GenericIdentity("DemoUser", "DemoAuth"), new[] { "DemoAdmin" }));


        await logger.InfoAsync("Information goes here");
        await logger.WarningAsync("Warning goes here");
        await logger.ErrorAsync("Error goes here");
    }

    sealed class CustomConsoleLogger : ConsoleAsyncLogger
    {
        protected override void Log(LogEventArgs args, TextWriter writer)
        {
            writer.WriteLine(JsonNode.Parse(args.Content));
        }

        protected override void OnLogging(LogEventArgs args)
        {
            Console.WriteLine(new string('=', 50));
        }




        protected override ConsoleColor ToForegroundColor(LogLevel level)
        {
            return level switch
            {
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Info => ConsoleColor.DarkGray,
                _ => Console.ForegroundColor
            };
        }
    }
}