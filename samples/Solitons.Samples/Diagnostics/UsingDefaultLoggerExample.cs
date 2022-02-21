namespace Solitons.Diagnostics;

public static class UsingDefaultLoggerExample
{
    public static async Task RunAsync()
    {
        // Create a console logger with default tags and properties
        var logger = IAsyncLogger.Console
            .WithTags("example", "console")
            .WithProperty("machine", Environment.MachineName)
            .WithProperty("user", Environment.UserName);

        await logger.InfoAsync($"Starting...");
        // Output: {"level":"Info","message":"Starting...","created":"2022-02-21T18:39:56.1717421+00:00","details":null,"tags":["example","console"],"properties":{"machine":"alexey-pc","user":"alexey"}}

        await ExecuteWithLoggingAsync(logger);
        // Output: {"level":"Info","message":"Some message","created":"2022-02-21T18:39:56.4674155+00:00","details":"Thread ID: 1","tags":["example","console"],"properties":{"machine":"alexey-pc","user":"alexey","method":"ExecuteWithLoggingAsync"}}

        await logger.InfoAsync("Done...");
        // Output: {"level":"Info","message":"Done...","created":"2022-02-21T18:39:56.4679397+00:00","details":null,"tags":["example","console"],"properties":{"machine":"alexey-pc","user":"alexey"}}
    }

    private static async Task ExecuteWithLoggingAsync(IAsyncLogger logger)
    {
        // Extend logger configurations
        logger = logger
            .WithProperty("method", nameof(ExecuteWithLoggingAsync));


        await logger.InfoAsync("Some message",
            details: $"Thread ID: {Thread.CurrentThread.ManagedThreadId}");
    }
}