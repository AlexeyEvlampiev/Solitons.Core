

using System.Collections.Immutable;
using Solitons.Collections;
using Solitons.Data.Management.Postgres;
using Solitons.Examples.WebApp.Azure;

namespace Solitons.Examples.WebApp.Data;

sealed record WebAppDbManageConfig : PgManagerConfig
{
    public WebAppDbManageConfig() : base("webappdb")
    {
        RoleConnectionStringSecretKeys = FluentDictionary
            .Create<string, string>()
            .Add(DatabaseName + "_admin", SecretKeys.DatabaseAdminConnectionString)
            .Add(DatabaseName + "_app", SecretKeys.DatabaseAppConnectionString)
            .ToImmutableDictionary(StringComparer.Ordinal);
    }
}