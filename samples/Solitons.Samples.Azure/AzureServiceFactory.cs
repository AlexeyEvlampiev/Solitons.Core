using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Queues;
using Npgsql;
using Solitons.Diagnostics;

namespace Solitons.Samples.Azure
{
    public sealed class AzureServiceFactory
    {
        private readonly IEnvironment _environment;
        private const string PostgresConnectionStringEnvVariable = "SOLITONS_SAMPLE_POSTGRES_CONNECTION_STRING";
        private const string EventHubsConnectionStringEnvVariable = "SOLITONS_SAMPLE_EVENTHUBS_CONNECTION_STRING";
        private const string AADB2CConnectionStringEnvVariable = "SOLITONS_SAMPLE_AADB2C_CONNECTION_STRING";
        private const string StorageConnectionStringEnvVariable = "AZ_STORAGE_CONNECTION_STRING";

        public AzureServiceFactory() : this(IEnvironment.System)
        {
            
        }

        public AzureServiceFactory(IEnvironment environment)
        {
            _environment = ThrowIf
                .NullArgument(environment, nameof(environment))
                .With(options =>
                {
                    options.EnvironmentVariableNotFound += (_, args) 
                        => throw new InvalidOperationException($"{args.VariableName} environment variable is missing");
                });
        }


        public IAsyncLogger GetLogger()
        {
            var eventHubsConnectionString = _environment.GetEnvironmentVariable(EventHubsConnectionStringEnvVariable);
            var storageConnectionString = _environment.GetEnvironmentVariable(StorageConnectionStringEnvVariable);
            var logsBufferQueue = new QueueClient(storageConnectionString, "logs");
            var logsHub = new EventHubProducerClient(eventHubsConnectionString);
            IAsyncLogger logger = new BufferedAsyncLogger(logsBufferQueue, logsHub);
            return logger;
        }

        public AzureSecureBlobAccessUriBuilder GetSecureBlobAccessUriBuilder()
        {
            var storageConnectionString =
                _environment.GetEnvironmentVariable(StorageConnectionStringEnvVariable);
            return new AzureSecureBlobAccessUriBuilder(storageConnectionString);
        }

        public AzureActiveDirectoryB2CSettingsGroup GetAzureActiveDirectoryB2CSettings()
        {
            return AzureActiveDirectoryB2CSettingsGroup
                .Parse(_environment
                    .GetEnvironmentVariable(AADB2CConnectionStringEnvVariable));
        }

        public string GetPgConnectionString(Action<NpgsqlConnectionStringBuilder>? config = null)
        {
            var builder = new NpgsqlConnectionStringBuilder(_environment
                .GetEnvironmentVariable(PostgresConnectionStringEnvVariable));
            config?.Invoke(builder);
            return builder.ConnectionString;
        }
    }
}
