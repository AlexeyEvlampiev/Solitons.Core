

namespace Solitons.Diagnostics
{
    public static class UsingDefaultLoggerExample
    {
        public static async Task RunAsync()
        {
            // Create a configure a console logger
            var logger = IAsyncLogger.Console
                .WithTags("example", "console")
                .WithProperty("machine", Environment.MachineName)
                .WithProperty("user", Environment.UserName);

            await logger.InfoAsync($"Starting...");

            await ExecuteWithLoggingAsync(logger);

            await logger.InfoAsync("Done...");
        }

        private static async Task ExecuteWithLoggingAsync(IAsyncLogger logger)
        {
            // Extend logger configurations
            logger = logger
                .WithProperty("method", nameof(ExecuteWithLoggingAsync));

            await logger.InfoAsync("Some message",
                log => log.WithDetails($"Thread ID: {Thread.CurrentThread.ManagedThreadId}"));
        }
    }
}
