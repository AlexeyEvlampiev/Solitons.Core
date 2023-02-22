using System.Security.Principal;
using System.Text.Json.Nodes;

namespace Solitons.Diagnostics;

public static class UsingCustomLoggerExample
{
    public static async Task RunAsync()
    {
        var logger = CustomAsyncLogger
            .Create()
            .WithTags("example", "custom implementation")
            .WithProperty("machine", Environment.MachineName)
            .WithProperty("user", Environment.UserName)
            .WithPrincipal(new GenericPrincipal(
                new GenericIdentity("DemoUser", "DemoAuth"), new[] { "DemoAdmin" }));


        await logger.InfoAsync("Information goes here");
        await logger.WarningAsync("Warning goes here");
        await logger.ErrorAsync("Error goes here");
    }


    sealed class CustomAsyncLogger : Solitons.Diagnostics.Common.AsyncLogger
    {
        public static IAsyncLogger Create() => new CustomAsyncLogger();

        protected override Task LogAsync(LogEventArgs args)
        {
            // Your custom logging goes here
            return Task.CompletedTask;
        }
    }
}