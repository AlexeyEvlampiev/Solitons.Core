# Solitons.Core
Solitons.Core is a .NET base class library providing interfaces and implementations for types, algorithms, and utility functions commonly required for cloud software systems. 
## Diagnostics
Solitons Diagnostics namespace provides types and interfaces for application logging and tracing purposes.
### IAsyncLogger interface
Represents an immutable asynchronous event logger.
#### Example 1: Using default IAsyncLogger implementations
```csharp
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
```
#### Example 2: Using custom IAsyncLogger implementations
```csharp
    public static async Task RunAsync()
    {
        var logger = CustomAsyncLogger
            .Create()
            .WithTags("example", "custom implementation")
            .WithProperty("machine", Environment.MachineName)
            .WithProperty("user", Environment.UserName);

        await logger.InfoAsync("Information goes here", log => log.WithDetails("Should be in green"));
        await logger.WarningAsync("Warning goes here", log => log.WithDetails("Should be in yellow"));
        await logger.ErrorAsync("Error goes here", log => log.WithDetails("Should be in red"));
    }


    sealed class CustomAsyncLogger : Solitons.Diagnostics.Common.AsyncLogger
    {
        public static IAsyncLogger Create() => new CustomAsyncLogger();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        protected override Task LogAsync(ILogEntry entry)
        {
            var json = entry.ToJsonString(indented: true);

            var color = Console.ForegroundColor;
            try
            {
                Console.WriteLine(new string('-', 50));
                Console.ForegroundColor = entry.Level switch 
                {
                    LogLevel.Error => ConsoleColor.Red, 
                    LogLevel.Warning => ConsoleColor.DarkYellow, 
                    _=> ConsoleColor.Green
                };
                Console.WriteLine(json);
                return Task.CompletedTask;
            }
            finally
            {
                Console.ForegroundColor = color;
            }
        }
    }
```
