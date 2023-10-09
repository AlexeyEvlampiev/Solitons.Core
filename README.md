
# Solitons.Core
Solitons.Core is a robust .NET class library, purpose-built to accelerate the development of cutting-edge cloud software systems. This library offers a suite of interfaces, algorithms, and utility functions for streamlined development, focusing on data processing and visualization.

This library acts as a catalyst for constructing contemporary cloud applications by simplifying complex processes. Key features include asynchronous logging, media content type negotiation, serialization, run-time code generation, and cloud database management.

The broad spectrum of features provided by Solitons.Core is aimed at reducing development time and resource consumption, enabling developers to focus on delivering high-quality, modern cloud computing solutions. With its comprehensive set of utilities and its emphasis on efficiency, Solitons.Core is a highly valuable tool for the open source community.

## Guided Serialization: Defining and Managing Unambiguous Data Contracts
In cloud-based distributed systems development, C#'s distinct advantage is its static safety and robust code integrity, achieved through transactional compilation of multi-project .NET solutions. This approach ensures system components communicate securely and predictably via channels like persisted queues and RPC protocols.

However, evolving contracts present challenges. Disruptive contract changes can destabilize systems. Solitons addresses this by assigning each data contract a unique GUID. Instead of altering an existing contract, a new one, with its own GUID, is introduced, sidestepping traditional semantic versioning. This methodology shields against inadvertent contract mismatches during system evolutions.

A pivotal advantage, unique to Solitons, is the resilience during code refactoring. Typically, renaming data classes risks breaking interoperability in distributed systems. However, with the Solitons approach, because contracts are tied to stable GUIDs rather than mutable class names, such refactoring becomes risk-free. This not only streamlines development and updates but also fortifies the system against unintentional disruptions.

This method is especially beneficial in multi-lingual systems, allowing components in diverse languages to communicate using these distinct GUIDs, eliminating ambiguity. For Solitons, the clear definition of system data contracts is foundational. Upcoming examples will highlight how to define these contracts for .NET cloud systems.

#### Examples: Defining Data Contracts
```csharp
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Newtonsoft.Json;
using ProtoBuf;
using Solitons.Data;
using Solitons.Data.Common;

namespace UsageExamples.Data;

[Example]
public sealed class ExampleDefiningDataContracts
{
    public void Example()
    {
        // Construct an IDataContractSerializer instance for efficient and versatile data serialization.
        var serializer = IDataContractSerializer.Build(builder =>
        {
            // Register 'Person' type for serialization, supporting both XML and JSON formats.
            // Serialization precedence is determined by the order: the first serializer specified is the default.
            builder.Add(typeof(Person),
                new[] { IMediaTypeSerializer.BasicJsonSerializer, IMediaTypeSerializer.BasicXmlSerializer });

            // Register 'ProductEventData' type, specifying support for both ProtoBuf and JSON formats.
            // Serialization precedence: ProtoBuf is the default format.
            builder.Add(typeof(ProductEventData),
                new[] { ProtoBufMediaTypeSerializer.Instance, NewtonsoftMediaTypeSerializer.Instance });

            // Allow the serializer to process classes that are not explicitly annotated with a GUID attribute.
            // This can be beneficial when dealing with data contracts from external libraries that may not use GUID annotations.
            builder.IgnoreMissingCustomGuidAnnotation(true);

            // Auto-discover and register types from the entry assembly for serialization.
            builder.AddAssemblyTypes(Assembly.GetEntryAssembly()!, type =>
            {
                // Set criteria to determine which types should be serialized.
                // Types with names ending in 'Data' or 'Dto' are processed with both JSON and XML serializers.
                // Here, the JSON serializer takes precedence due to its position in the list.
                if (type.Name.EndsWith("Data") || type.Name.EndsWith("Dto"))
                {
                    return new[]
                    {
                        IMediaTypeSerializer.BasicJsonSerializer,
                        IMediaTypeSerializer.BasicXmlSerializer
                    };
                }

                // Exclude types that don't meet the naming criteria.
                return Enumerable.Empty<IMediaTypeSerializer>();
            });
        });

        var data = new ProductEventData
        {
            LaunchDate = DateTime.UtcNow,
            Price = 120.5M,
            ProductId = 305428,
            ProductName = "SodaX"
        };

        // Serialize 'ProductEventData' to its default format (ProtoBuf in this setup).
        var text = serializer.Serialize(data);

        // Assert that the serialization process has indeed used the ProtoBuf format.
        Debug.Assert(text.ContentType == "application/x-protobuf", "Expected format: ProtoBuf");
        Console.WriteLine($"ProtoBuf: {text}");

        // Serialize 'ProductEventData' explicitly specifying the JSON format.
        text = serializer.Serialize(data, "application/json");

        // Assert that the serialization process has used the specified JSON format.
        Debug.Assert(text.ContentType == "application/json", "Expected format: JSON");
        Console.WriteLine($"JSON: {text}");

        // Prepare 'ProductEventData' for queue transmission by packaging it with relevant metadata.
        var package = serializer.Pack(data);
        package.To = "product-events-queue";
        package.CorrelationId = Guid.NewGuid().ToString();

        // Note: Insert logic here to dispatch the package via a queue mechanism.

        // Unpack the serialized data for verification.
        var dataClone = serializer.Unpack(package);

        // Validate the deserialized object type to ensure data integrity.
        Debug.Assert(dataClone is ProductEventData, "Deserialized data type mismatch: Expected 'ProductEventData'");
    }


    // Example 1: Person represents a POCO data contract with rules for both JSON and XML formatting.
    // It's identified by the GUID 460d3673-c98e-406d-80d2-2e65862c6d87.
    // Register this contract when building the Solitons IDataContractSerializer instance using explicit or reflective methods.
    [Guid("460d3673-c98e-406d-80d2-2e65862c6d87")]
    public sealed class Person
    {
        [JsonPropertyName("firstName")]
        [XmlAttribute("FirstName")]
        public string FirstName { get; set; } = String.Empty;

        [JsonPropertyName("lastName")]
        [XmlAttribute("LastName")]
        public string LastName { get; set; } = String.Empty;

        [JsonPropertyName("age")]
        [XmlAttribute("Age")]
        public int Age { get; set; }
    }

    // Example 2: CustomerEngagementData is an "XML-first" data contract, deriving from BasicXmlDataTransferObject 
    // and implementing the IBasicJsonDataTransferObject Solitons interface.
    // When serialized with Solitons IDataContractSerializer, XML serialization is applied by default.
    // Data contracts inheriting from foundational classes are detected automatically through .NET reflection by IDataContractSerializer builders.
    // The base class provides convenient methods for direct serialization.
    // Identify this contract by the GUID c0d692ba-ecb6-44fa-9954-b31f15e03954 for Solitons management.
    [Guid("c0d692ba-ecb6-44fa-9954-b31f15e03954")]
    public sealed class CustomerEngagementData : BasicXmlDataTransferObject, IBasicJsonDataTransferObject
    {
        [JsonPropertyName("userEmail")]
        [XmlAttribute("UserEmail")]
        public string UserEmail { get; set; } = String.Empty;

        [JsonPropertyName("lastEngagedDate")]
        [XmlAttribute("LastEngagedDate")]
        public DateTime LastEngagedDate { get; set; }

        [JsonPropertyName("engagementScore")]
        [XmlAttribute("EngagementScore")]
        public double EngagementScore { get; set; }
    }

    // Example 3: UserRegistrationEventData is a "JSON-first" data contract.
    // It inherits from BasicJsonDataTransferObject and also implements the IBasicXmlDataTransferObject Solitons interface.
    // JSON serialization is the default behavior with Solitons IDataContractSerializer.
    // The base class provides methods for direct serialization without additional serializer components.
    // This contract has a unique GUID identifier a01619cd-9c93-4f8a-9cf3-f7b198c0769a for Solitons usage.
    [Guid("a01619cd-9c93-4f8a-9cf3-f7b198c0769a")]
    public sealed class UserRegistrationEventData : BasicJsonDataTransferObject, IBasicXmlDataTransferObject
    {
        [JsonPropertyName("username")]
        [XmlAttribute("Username")]
        public string Username { get; set; } = String.Empty;

        [JsonPropertyName("registrationDate")]
        [XmlAttribute("RegistrationDate")]
        public DateTime RegistrationDate { get; set; }

        [JsonPropertyName("referralCode")]
        [XmlAttribute("ReferralCode")]
        public string ReferralCode { get; set; } = String.Empty;
    }

    // Example 4: ProductEventData represents a data contract that uses both Newtonsoft's JSON library 
    // and Protocol Buffers (Protobuf) for serialization.
    // This class doesn't inherit from any Solitons-specific classes or interfaces. 
    // When using with HTTP protocols, the media type for Protobuf is "application/x-protobuf".
    // To use this contract within Solitons, you must provide corresponding implementations of IMediaTypeSerializer 
    // for both the Newtonsoft JSON and Protobuf serialization formats.
    [Guid("f2c63f9b-1e5a-4b8a-8d45-65a2f245d7ed")]
    [ProtoContract]
    public sealed class ProductEventData
    {
        [JsonProperty(PropertyName = "productId")]// Newtonsoft JSON formatting
        [ProtoMember(1)]// Protobuf formatting
        public int ProductId { get; set; }

        [JsonProperty(PropertyName = "productName")]// Newtonsoft JSON formatting
        [ProtoMember(2)]// Protobuf formatting
        public string ProductName { get; set; } = String.Empty;

        [JsonProperty(PropertyName = "launchDate")]// Newtonsoft JSON formatting
        [ProtoMember(3)]// Protobuf formatting
        public DateTime LaunchDate { get; set; }

        [JsonProperty(PropertyName = "price")]// Newtonsoft JSON formatting
        [ProtoMember(4)]// Protobuf formatting
        public decimal Price { get; set; }
    }

    // Example 5: NewtonsoftMediaTypeSerializer
    // This class provides a concrete implementation of the MediaTypeSerializer for JSON serialization using the Newtonsoft library.
    // This serializer targets the "application/json" media type, which is a standard MIME type for JSON data.
    // Solitons' IDataContractSerializer implementations leverage this class to handle serialization of classes that rely on Newtonsoft's JSON format.
    public sealed class NewtonsoftMediaTypeSerializer : MediaTypeSerializer
    {
        // Provides a static instance of the NewtonsoftMediaTypeSerializer to be used wherever needed.
        // This follows a Singleton pattern, ensuring a single shared instance across the application.
        public static readonly IMediaTypeSerializer Instance = new NewtonsoftMediaTypeSerializer();

        // Private constructor ensures that no other instances can be created directly.
        // The base constructor is called with "application/json", setting the media type for this serializer.
        private NewtonsoftMediaTypeSerializer()
            : base("application/json")
        {
        }

        // Serializes the provided data transfer object (dto) to its JSON representation using Newtonsoft.
        // The result is formatted with indents for better readability.
        protected override string Serialize(object dto) => JsonConvert.SerializeObject(dto, Formatting.Indented);

        // Deserializes the provided JSON content to an object of the specified targetType using Newtonsoft.
        // Returns the deserialized object, or null if the content doesn't match the target type.
        protected override object? Deserialize(string content, Type targetType) => JsonConvert.DeserializeObject(content, targetType);
    }



    // Example 6: ProtoBufMediaTypeSerializer
    // This class provides a concrete implementation of the MediaTypeSerializer for serialization using the Protocol Buffers (ProtoBuf) format.
    // This serializer targets the "application/x-protobuf" media type, which is a common MIME type for Protocol Buffers data.
    // Solitons' IDataContractSerializer implementations can leverage this class to handle serialization of classes that rely on ProtoBuf's format.
    public sealed class ProtoBufMediaTypeSerializer : MediaTypeSerializer
    {
        // Provides a static instance of the ProtoBufMediaTypeSerializer to be used wherever needed.
        // This follows a Singleton pattern, ensuring a single shared instance across the application.
        public static readonly IMediaTypeSerializer Instance = new ProtoBufMediaTypeSerializer();

        // Private constructor ensures that no other instances can be created directly.
        // The base constructor is called with "application/x-protobuf", setting the media type for this serializer.
        private ProtoBufMediaTypeSerializer()
            : base("application/x-protobuf")
        {
        }

        // Serializes the provided data transfer object (dto) into its Protocol Buffers binary representation using the protobuf-net library.
        protected override string Serialize(object dto)
        {
            using var stream = new System.IO.MemoryStream();
            Serializer.Serialize(stream, dto);
            return Convert.ToBase64String(stream.ToArray()); // For simplicity, converting the binary to Base64 string. Adjust if needed.
        }

        // Deserializes the provided Protocol Buffers content to an object of the specified targetType using the protobuf-net library.
        // Returns the deserialized object, or null if the content doesn't match the target type.
        protected override object? Deserialize(string content, Type targetType)
        {
            var bytes = Convert.FromBase64String(content); // Convert Base64 string back to binary. Adjust if needed.
            using var stream = new System.IO.MemoryStream(bytes);
            return Serializer.Deserialize(targetType, stream);
        }
    }

}
```

## Abstract Secrets Repository
Managing secrets is essential for cloud applications, but the specifics can vary with cloud providers and deployment strategies. Solitons introduces abstractions for seamless interaction with diverse secrets repositories, ensuring cloud-independence in core development.

While Solitons doesn't offer out-of-the-box implementations for specific cloud providers like Azure's Key Vault, AWS Secrets Manager, or Google Cloud's Secret Manager, it does equip developers with foundational classes. These classes facilitate rapid and efficient crafting of cloud-specific implementations. Coupled with robust cache invalidation techniques for optimized network usage, Solitons presents a balanced blend of adaptability and security.
#### Example 1: Using ISecretsRepository
```csharp
using System.Data.Common;
using System.Data.SQLite;
using System.Diagnostics;
using System.Reactive.Linq;
using Solitons;
using Solitons.Security;
using Solitons.Security.Common;

namespace UsageExamples.Data;


[Example]
public sealed class ExampleUsingSecretsRepository
{
    public async Task Example()
    {
        // Instantiate a secrets repository interface with process environment variables as the source.
        ISecretsRepository envRepository = ISecretsRepository.Environment(EnvironmentVariableTarget.Process);

        // Pre-populate the DEMO_SECRET environment variable for this example.
        Environment.SetEnvironmentVariable("DEMO_SECRET", "Secret value goes here...");

        // Retrieve the DEMO_SECRET using the secrets repository interface.
        var secret = await envRepository.GetSecretAsync("DEMO_SECRET");
        Debug.Assert(secret == "Secret value goes here...", "Ensure the DEMO_SECRET value is correctly set in the environment variables.");

        // Update the DEMO_SECRET with a new value.
        await envRepository.SetSecretAsync("DEMO_SECRET", "New value goes here...");
        Debug.Assert("New value goes here..." == Environment.GetEnvironmentVariable("DEMO_SECRET"));

        // Instantiate a secrets repository interface with SQLite as the backend storage.
        // Note: Secrets are categorized by scopes formatted as [database name|scope name].
        var sqliteRepository = SQLiteSecretsScope.Create("secrets.db|demo-scope");

        // Store a unique secret named "MY-GUID" in the database.
        var secretValue = Guid.NewGuid().ToString();
        await sqliteRepository.SetSecretAsync("MY-GUID", secretValue);
        Debug.Assert(secretValue == await sqliteRepository.GetSecretAsync("MY-GUID"));

        // Implement an indefinite caching layer to reduce frequent read operations to the database.
        var indefiniteCacheRepository = sqliteRepository.ReadThroughCache(secretNameComparer: StringComparer.Ordinal);
        Debug.Assert(secretValue == await indefiniteCacheRepository.GetSecretAsync("MY-GUID"));

        // Implement a time-limited caching layer. More advanced cache expiration methods can also be employed.
        var temporalCacheRepository = sqliteRepository.ReadThroughCache(
            cacheExpiration: Observable.Timer(TimeSpan.FromSeconds(30)), // Cache expires after 30 seconds.
            secretNameComparer: StringComparer.Ordinal);
        Debug.Assert(secretValue == await temporalCacheRepository.GetSecretAsync("MY-GUID"));
    }

    /// <summary>
    /// An implementation of a SQLite-based secrets repository targeting SQLite version 3.
    /// </summary>
    public sealed class SQLiteSecretsScope : SQLiteSecretsRepository
    {
        private sealed class DotNetCoreSQLiteProvider : ISQLIteProvider
        {
            public DbConnection CreateConnection(string connectionString) => new SQLiteConnection(connectionString);

            public string CreateConnectionString(string filePath) => $"Data Source={filePath};Version=3;";
        }

        public SQLiteSecretsScope(string filePath, string scopeName)
            : base(filePath, scopeName, new DotNetCoreSQLiteProvider())
        {
        }

        /// <summary>
        /// Creates an instance of the SQLite secrets repository using a scope connection string.
        /// </summary>
        /// <param name="scopeConnectionString">The connection string indicating the database file and the scope.</param>
        /// <returns>An instance of <see cref="ISecretsRepository"/>.</returns>
        public static ISecretsRepository Create(string scopeConnectionString)
        {
            ThrowIf.ArgumentNullOrWhiteSpace(scopeConnectionString);
            if (IsScopeConnectionString(scopeConnectionString, out var filePath, out var scopeName))
            {
                return new SQLiteSecretsScope(filePath, scopeName);
            }

            throw new FormatException("The provided secret scope connection string is not valid.");
        }
    }
}
```


## Asynchronous Logging in Distributed Cloud Environments with IAsyncLogger
Introducing IAsyncLogger, an innovative asynchronous logging interface designed for .NET cloud-based distributed systems. This library dramatically enhances logging capabilities, making them more efficient, scalable, and adaptable. Unlike traditional .NET logging mechanisms, IAsyncLogger brings in the advantages of immutability and asynchronous I/O, setting a new standard for logging in cloud ecosystems.

### Advantages
#### Immutability
Immutability is a first-class citizen in IAsyncLogger. It ensures thread-safety without the hassle of manual locking mechanisms. This is crucial for distributed cloud applications where multiple threads or even different services may try to log data concurrently. By leveraging immutability, the library makes sure that once a log entry is created, it cannot be altered, leading to a stable and reliable logging behavior across services.

#### Asynchronous I/O
In a world where every millisecond counts, IAsyncLogger excels by providing asynchronous logging capabilities. Traditional logging solutions often employ blocking I/O operations that can become a bottleneck in high-throughput applications. With IAsyncLogger, logs are written in a non-blocking fashion, ensuring your application's performance remains at its peak.

#### Observer Pattern
The AsObservable() method enables reactive programming paradigms, allowing subscribers to be notified of logging events as they occur. This makes it perfect for real-time monitoring or diagnostic solutions.

#### Extensibility
By using a partial interface design, IAsyncLogger enables users to extend its functionalities. It comes with built-in methods for adding tags and properties (WithTags, WithProperty, WithProperties), making your logs more informative and easier to filter.

#### HttpRequestOptions Integration
IAsyncLogger incorporates tightly with HttpRequestOptions, providing an easy way to associate logs with specific HTTP requests. This is invaluable for tracing and debugging complex workflows within microservices architectures.

#### Pre-Configured Logging Levels
The library comes with predefined logging levels such as Error, Warning, and Info, each with its asynchronous method (ErrorAsync, WarningAsync, InfoAsync). This allows for easier categorization and subsequent analysis of logs.

#### Caller Information
IAsyncLogger automatically captures caller information, such as the name of the calling method, file path, and line number. This is incredibly useful for debugging and provides a rich context for each log entry.

### Example 1: Implementing and Using IAsyncLogger Interface
In this enhanced example, we implement the IAsyncLogger interface to craft two custom loggers: ColoredConsoleLogger and SQLiteAsyncLogger. The ColoredConsoleLogger enriches the console output with color-coding based on log levels, offering immediate visual feedback. On the other hand, the SQLiteAsyncLogger takes logging to the next level by persistently storing logs in a SQLite database, making it suitable for scenarios requiring long-term log analysis and storage.
```csharp
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
    /// Log events are stored in an SQLite database, which can be queried for later analysis.
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
```

