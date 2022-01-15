using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Queues;
using Npgsql;

namespace Solitons.Samples.Azure
{
    public static class AzureFactory
    {
        private const string PostgresConnectionStringEnvVariable = "SOLITONS_SAMPLE_POSTGRES_CONNECTION_STRING";
        private const string EventHubsConnectionStringEnvVariable = "SOLITONS_SAMPLE_EVENTHUBS_CONNECTION_STRING";
        private const string AADB2CConnectionStringEnvVariable = "SOLITONS_SAMPLE_AADB2C_CONNECTION_STRING";
        private const string StorageConnectionStringEnvVariable = "AZ_STORAGE_CONNECTION_STRING";

        public static BlobStorageAsyncLogger GetLogger(IEnvironment? env = null)
        {
            env ??= IEnvironment.System;
            var logsBufferQueue = new QueueClient(
                env.GetEnvironmentVariable(StorageConnectionStringEnvVariable)
                    .ThrowIfNullOrWhiteSpace(()=> new InvalidOperationException($"{StorageConnectionStringEnvVariable} environment variable is missing.")), 
                "logs");
            var logsHub = new EventHubProducerClient(env.GetEnvironmentVariable(EventHubsConnectionStringEnvVariable)
                .ThrowIfNullOrWhiteSpace(() => new InvalidOperationException($"{EventHubsConnectionStringEnvVariable} environment variable is missing.")), "logs");
            return new BlobStorageAsyncLogger(logsBufferQueue, logsHub);
        }

        public static AzureActiveDirectoryB2CSettings GetAzureActiveDirectoryB2CSettings(IEnvironment? env = null)
        {
            env ??= IEnvironment.System;
            return AzureActiveDirectoryB2CSettings
                .Parse(env
                    .GetEnvironmentVariable(AADB2CConnectionStringEnvVariable)
                    .ThrowIfNullOrWhiteSpace(()=> new InvalidOperationException($"{AADB2CConnectionStringEnvVariable} environment variable is missing.")));
        }

        public static string GetPgConnectionString(Action<NpgsqlConnectionStringBuilder>? config = null, IEnvironment? env = null)
        {
            env ??= IEnvironment.System;
            var builder = new NpgsqlConnectionStringBuilder(env
                .ThrowIfNullArgument(nameof(env))
                .GetEnvironmentVariable(PostgresConnectionStringEnvVariable)
                .ThrowIfNullOrWhiteSpace(() =>
                    new InvalidOperationException($"{PostgresConnectionStringEnvVariable} environment variable is missing.")));
            config?.Invoke(builder);
            return builder.ConnectionString;
        }
    }
}
