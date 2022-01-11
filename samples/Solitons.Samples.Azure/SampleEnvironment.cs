using Npgsql;

namespace Solitons.Samples.Azure
{
    public static class SampleEnvironment
    {
        private const string ConnectionStringEnvVariable = "SOLITONS_SAMPLE_CONNECTION_STRING";

        public static string GetPgConnectionString(Action<NpgsqlConnectionStringBuilder>? config = null, IEnvironment? env = null)
        {
            env ??= IEnvironment.System;
            var builder = new NpgsqlConnectionStringBuilder(env
                .ThrowIfNullArgument(nameof(env))
                .GetEnvironmentVariable(ConnectionStringEnvVariable)
                .ThrowIfNullOrWhiteSpace(() =>
                    new InvalidOperationException($"{ConnectionStringEnvVariable} environment variable is missing.")));
            config?.Invoke(builder);
            return builder.ConnectionString;
        }
    }
}
