using Npgsql;

namespace Solitons.Samples.Azure
{
    public static class EnvironmentVariables
    {
        private const string PostgresConnectionStringEnvVariable = "SOLITONS_SAMPLE_POSTGRES_CONNECTION_STRING";

        private const string AADB2CConnectionStringEnvVariable = "SOLITONS_SAMPLE_AADB2C_CONNECTION_STRING";

        public static AzureActiveDirectoryB2CSettingsGroup GetAzureActiveDirectoryB2CSettings(IEnvironment? env = null)
        {
            env ??= IEnvironment.System;
            return AzureActiveDirectoryB2CSettingsGroup
                .Parse(env
                    .GetEnvironmentVariable(AADB2CConnectionStringEnvVariable)
                    .ThrowIfNullOrWhiteSpace(()=> new InvalidOperationException($"{AADB2CConnectionStringEnvVariable} environment variable is missing.")));
        }

        public static string GetPgConnectionString(Action<NpgsqlConnectionStringBuilder>? config = null, IEnvironment? env = null)
        {
            env ??= IEnvironment.System;
            var builder = new NpgsqlConnectionStringBuilder(
                ThrowIf.NullArgument(env, nameof(env))
                .GetEnvironmentVariable(PostgresConnectionStringEnvVariable)
                .ThrowIfNullOrWhiteSpace(() =>
                    new InvalidOperationException($"{PostgresConnectionStringEnvVariable} environment variable is missing.")));
            config?.Invoke(builder);
            return builder.ConnectionString;
        }
    }
}
