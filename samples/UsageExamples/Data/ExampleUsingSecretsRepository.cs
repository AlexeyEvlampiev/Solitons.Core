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
