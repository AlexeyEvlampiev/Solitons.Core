

namespace Solitons.Diagnostics;

public static class UsingCustomLoggerExample
{
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


        
}