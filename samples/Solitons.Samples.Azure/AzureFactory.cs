using Azure.Messaging.EventHubs.Producer;
using Azure.Storage.Queues;
using Npgsql;
using Solitons.Samples.Azure.Security;
using Solitons.Samples.Domain.Security;

namespace Solitons.Samples.Azure
{
    public sealed class AzureFactory
    {
        private readonly IEnvironment _environment;
        private const string PostgresConnectionStringEnvVariable = "SOLITONS_SAMPLE_POSTGRES_CONNECTION_STRING";
        private const string EventHubsConnectionStringEnvVariable = "SOLITONS_SAMPLE_EVENTHUBS_CONNECTION_STRING";
        private const string AADB2CConnectionStringEnvVariable = "SOLITONS_SAMPLE_AADB2C_CONNECTION_STRING";
        private const string StorageConnectionStringEnvVariable = "AZ_STORAGE_CONNECTION_STRING";

        public AzureFactory() : this(IEnvironment.System)
        {
            
        }

        public AzureFactory(IEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }


        public IAsyncLogger GetLogger()
        {
            var eventHubsConnectionString = _environment.GetRequiredEnvironmentVariable(EventHubsConnectionStringEnvVariable);
            var storageConnectionString = _environment.GetRequiredEnvironmentVariable(StorageConnectionStringEnvVariable);
            var logsBufferQueue = new QueueClient(storageConnectionString, "logs");
            var logsHub = new EventHubProducerClient(eventHubsConnectionString);
            IAsyncLogger logger = new BufferedAsyncLogger(logsBufferQueue, logsHub);
            return logger;
        }

        public ReadOnlySasAccessSigner GetReadOnlySasAccessSigner()
        {
            var storageConnectionString =
                _environment.GetRequiredEnvironmentVariable(StorageConnectionStringEnvVariable);
            return new AzureReadOnlySasAccessSigner(storageConnectionString);
        }

        public AzureActiveDirectoryB2CSettings GetAzureActiveDirectoryB2CSettings()
        {
            return AzureActiveDirectoryB2CSettings
                .Parse(_environment
                    .GetRequiredEnvironmentVariable(AADB2CConnectionStringEnvVariable));
        }

        public string GetPgConnectionString(Action<NpgsqlConnectionStringBuilder>? config = null)
        {
            var builder = new NpgsqlConnectionStringBuilder(_environment
                .GetRequiredEnvironmentVariable(PostgresConnectionStringEnvVariable));
            config?.Invoke(builder);
            return builder.ConnectionString;
        }
    }
}
