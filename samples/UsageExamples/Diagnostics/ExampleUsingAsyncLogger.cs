using Solitons.Diagnostics;
using Solitons.Diagnostics.Common;
using System.Data.SQLite;
using LogEventArgs = Solitons.Diagnostics.LogEventArgs;

namespace UsageExamples.Diagnostics;

/// <summary>
/// This class demonstrates the usage of asynchronous logging with ColoredConsoleLogger.
/// </summary>
[Example]
public sealed class ExampleUsingAsyncLogger
{
    /// <summary>
    /// Demonstrates different logging operations with metadata and colored console output.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ConsoleExample()
    {
        // Initialize the logger with global properties and tags
        var logger = ColoredConsoleLogger.Object
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

    public async Task SQLiteExample()
    {
        // Initialize the SQLiteAsyncLogger
        var logger = SQLiteAsyncLogger.Instance
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
    /// ColoredConsoleLogger provides an asynchronous, colored console logging capability.
    /// </summary>
    sealed class ColoredConsoleLogger : ConsoleAsyncLogger
    {
        // Private constructor to prevent external instantiation
        private ColoredConsoleLogger() { }

        // Singleton instance
        public static readonly IAsyncLogger Object = new ColoredConsoleLogger();

        /// <summary>
        /// Changes the console color based on the log level before logging the message.
        /// </summary>
        /// <param name="args">Arguments encapsulating the log event details.</param>
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
        /// Resets the console color back to its original state after logging.
        /// </summary>
        /// <param name="args">Arguments encapsulating the log event details.</param>
        protected override void OnLogged(LogEventArgs args) => Console.ResetColor();
    }

    /// <summary>
    /// SQLiteAsyncLogger provides an asynchronous SQLite-based logging capability.
    /// </summary>
    sealed class SQLiteAsyncLogger : AsyncLogger
    {
        private readonly string _connectionString;

        private SQLiteAsyncLogger()
        {
            _connectionString = "Data Source=logging.db;Version=3;";
            InitializeDatabase();
        }

        public static readonly IAsyncLogger Instance = new SQLiteAsyncLogger();

        private void InitializeDatabase()
        {
            using var conn = new SQLiteConnection(_connectionString);
            conn.Open();

            // Create tables if they do not exist
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

        protected override async Task LogAsync(LogEventArgs args)
        {
            var utc = DateTime.UtcNow;
            await using var conn = new SQLiteConnection(_connectionString);
            await conn.OpenAsync();

            await using var transaction = conn.BeginTransaction();
            // Use INSERT OR IGNORE followed by SELECT to get the id in a single SQL execution
            string upsertAndFetchIdSql = @"
            INSERT OR IGNORE INTO source (name, file, line) VALUES (@name, @file, @line);
            SELECT id FROM source WHERE file = @file AND line = @line;";

            await using var upsertAndFetchIdCmd = new SQLiteCommand(upsertAndFetchIdSql, conn);
            upsertAndFetchIdCmd.Parameters.AddWithValue("@name", args.SourceInfo.MemberName);
            upsertAndFetchIdCmd.Parameters.AddWithValue("@file", args.SourceInfo.FilePath);
            upsertAndFetchIdCmd.Parameters.AddWithValue("@line", args.SourceInfo.LineNumber);

            // Execute the command and fetch the source ID
            var sourceId = (long)(await upsertAndFetchIdCmd.ExecuteScalarAsync() ?? throw new InvalidOperationException());

            // Insert the log event with a single SQL execution
            string insertEventSql = @"
            INSERT INTO event (source_id, content, level, createdUtc)
            VALUES (@source_id, @content, @level, @createdUtc);";

            await using var insertEventCmd = new SQLiteCommand(insertEventSql, conn, transaction);
            insertEventCmd.Parameters.AddWithValue("@source_id", sourceId);
            insertEventCmd.Parameters.AddWithValue("@content", args.Content);
            insertEventCmd.Parameters.AddWithValue("@level", args.Level.ToString());
            insertEventCmd.Parameters.AddWithValue("@createdUtc", utc);
            await insertEventCmd.ExecuteNonQueryAsync();

            transaction.Commit();
        }

    }
}


