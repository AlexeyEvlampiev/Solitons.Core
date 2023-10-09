using Solitons.Diagnostics;
using Solitons.Diagnostics.Common;
using System.Data.SQLite;

namespace UsageExamples.Diagnostics;

/// <summary>
/// Demonstrates the usage of different asynchronous logging mechanisms.
/// </summary>
[Example]
public sealed class ExampleUsingAsyncLogger
{
    /// <summary>
    /// Showcases logging operations with metadata and console output using <see cref="ColoredConsoleLogger"/>.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task ConsoleExample()
    {
        // Initialize the logger with global properties and tags
        var logger = ColoredConsoleLogger.Singleton
            .WithProperty("component", "demo-component")
            .WithProperty("user", Environment.UserName)
            .WithTags(Environment.MachineName, Environment.OSVersion.ToString());

        // Log an informational message about the first operation
        await logger.InfoAsync("Successfully completed the first operation.", log => log
            .WithProperty("operation", "Operation 1"), LogMode.FireAndForget);

        // Update the logger with additional metadata
        logger = logger.WithProperty("completion", "Step 1 of 3");

        // Log a warning message about the second operation
        await logger.WarningAsync("Completed the second operation with minor issues.", log => log
            .WithProperty("operation", "Operation 2"));

        // Update the logger with additional metadata
        logger = logger.WithProperty("completion", "Step 2 of 3");

        // Log an error message about the third operation
        await logger.ErrorAsync("The third operation encountered a failure.", log => log
            .WithProperty("operation", "Operation 3"));
    }

    /// <summary>
    /// Illustrates how to perform logging operations with SQLite-based storage using <see cref="SQLiteAsyncLogger"/>.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task SQLiteExample()
    {
        // Initialize the SQLiteAsyncLogger
        var logger = SQLiteAsyncLogger.Singleton
            .WithProperty("component", "ECommerceEngine")
            .WithProperty("user", Environment.UserName)
            .WithTags("E-Commerce", "OrderProcessing");

        // Simulating: Order has been placed
        await logger.InfoAsync("Order has been placed successfully.", log => log
                .WithProperty("orderId", Guid.NewGuid())
                .WithProperty("operation", "Order Placement"),
            LogMode.FireAndForget);

        logger = logger.WithProperty("completion", "1 of 3");

        // Simulating: Payment Processing
        await logger.WarningAsync("Payment processed with a minor issue.", log => log
                .WithProperty("paymentId", Guid.NewGuid())
                .WithProperty("operation", "Payment Processing"),
            LogMode.FireAndForget);

        logger = logger.WithProperty("completion", "2 of 3");

        // Simulating: Shipment
        await logger.ErrorAsync("Failed to generate shipment label.", log => log
                .WithProperty("shipmentId", Guid.NewGuid())
                .WithProperty("operation", "Shipment Processing"),
            LogMode.FireAndForget);
    }

    /// <summary>
    /// Provides an asynchronous logging capability with colored console output.
    /// </summary>
    sealed class ColoredConsoleLogger : ConsoleAsyncLogger
    {
        // Private constructor to prevent external instantiation
        private ColoredConsoleLogger() { }

        // Singleton instance
        public static readonly IAsyncLogger Singleton = new ColoredConsoleLogger();

        /// <summary>
        /// Handles pre-logging activities, such as setting the console color based on log level.
        /// </summary>
        /// <param name="args">Event arguments containing log details.</param>
        protected override void OnLogging(Solitons.Diagnostics.LogEventArgs args)
        {
            Console.ForegroundColor = args.Level switch
            {
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Warning => ConsoleColor.Yellow,
                _ => ConsoleColor.Green
            };
        }

        /// <summary>
        /// Handles post-logging activities, such as resetting the console color.
        /// </summary>
        /// <param name="args">Event arguments containing log details.</param>
        protected override void OnLogged(Solitons.Diagnostics.LogEventArgs args) => Console.ResetColor();
    }

    /// <summary>
    /// Provides an asynchronous logging capability with SQLite-based storage.
    /// </summary>
    sealed class SQLiteAsyncLogger : AsyncLogger
    {
        private readonly string _connectionString;

        private SQLiteAsyncLogger()
        {
            _connectionString = "Data Source=logging.db;Version=3;";
            InitializeDatabase();
        }

        public static readonly IAsyncLogger Singleton = new SQLiteAsyncLogger();

        private void InitializeDatabase()
        {
            using var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            string sql = @"
            CREATE TABLE IF NOT EXISTS source (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT,
                file TEXT,
                line INTEGER,
                UNIQUE(file, line)
            );
            
            CREATE TABLE IF NOT EXISTS event (
                source_id INTEGER REFERENCES source(id),
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                content TEXT,
                level TEXT,
                createdUtc DATETIME DEFAULT CURRENT_TIMESTAMP
            );";

            using var cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Performs the actual logging operation, inserting log events into the SQLite database.
        /// </summary>
        /// <param name="args">Event arguments containing log details.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task LogAsync(Solitons.Diagnostics.LogEventArgs args)
        {
            await using var conn = new SQLiteConnection(_connectionString);
            await conn.OpenAsync();

            var cmdText = @"
            INSERT OR IGNORE INTO source (name, file, line) VALUES (@name, @file, @line);

            INSERT INTO event (source_id, content, level)
            SELECT src.id, @content, @level
            FROM source AS src 
            WHERE 
                file = @file 
            AND line = @line;";

            await using var cmd = new SQLiteCommand(cmdText, conn);
            cmd.Parameters.AddWithValue("@name", args.SourceInfo.MemberName);
            cmd.Parameters.AddWithValue("@file", args.SourceInfo.FilePath);
            cmd.Parameters.AddWithValue("@line", args.SourceInfo.LineNumber);
            cmd.Parameters.AddWithValue("@content", args.Content);
            cmd.Parameters.AddWithValue("@level", args.Level.ToString());

            await cmd.ExecuteNonQueryAsync();
        }

    }
}


