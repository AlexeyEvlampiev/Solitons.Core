# Solitons.Core
Solitons.Core is a .NET base class library providing interfaces and implementations for types, algorithms, and utility functions commonly required for cloud software systems. 
## Configuration
Contains types that provide a programming model for handling configuration data.
### SettingsGroup class
A common base for configuration settings classes requiring plain text serialization in the form of semicolon delimited key-value pairs.
#### Example 1: Using custom settings group
```csharp
    public static void Run()
    {
        var login = new UserLoginSettings()
        {
            User = "admin",
            Password = "bb307dafcbe7"
        };

        Console.WriteLine(login);
        //Output: user=admin;password=bb307dafcbe7

        login = UserLoginSettings.Parse("u=superuser;pwd=c82584fa160f");
        Console.WriteLine(login);
        //Output: user=superuser;password=c82584fa160f
    }

    public sealed class UserLoginSettings : SettingsGroup
    {
        [Setting("user", IsRequired = true, Pattern = "(?i)(username|use?r|u)")]
        public string User { get; set; } = String.Empty;

        [Setting("password", IsRequired = true, Pattern = "(?i)(password|pass|pwd|p)")]
        public string Password { get; set; } = String.Empty;

        public static UserLoginSettings Parse(string text) => Parse<UserLoginSettings>(text);
    }
```
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

        await logger.InfoAsync("Information goes here", details: "Should be in green");
        await logger.WarningAsync("Warning goes here", details: "Should be in yellow");
        await logger.ErrorAsync("Error goes here", details: "Should be in red");
    }


    sealed class CustomAsyncLogger : Solitons.Diagnostics.Common.AsyncLogger
    {
        public static IAsyncLogger Create() => new CustomAsyncLogger();

        protected override Task LogAsync(ILogEntry entry)
        {
            var json = entry.ToJsonString(indented: true);

            Console.WriteLine(new string('=', 50));

            var color = Console.ForegroundColor;
            try
            {
                Console.ForegroundColor = entry.Level switch
                {
                    LogLevel.Error => ConsoleColor.Red,
                    LogLevel.Warning => ConsoleColor.DarkYellow,
                    _ => ConsoleColor.Green
                };
                Console.WriteLine(json);
                return Task.CompletedTask;
            }
            finally
            {
                // Restore the original text color
                Console.ForegroundColor = color;
            }
        }
    }
```
