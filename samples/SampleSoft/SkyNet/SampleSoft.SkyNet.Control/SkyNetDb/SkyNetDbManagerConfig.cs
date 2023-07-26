using System.Collections.Immutable;
using System.Diagnostics;
using SampleSoft.SkyNet.Azure;
using Solitons.Collections;
using Solitons.Data.Management.Postgres.Common;

namespace SampleSoft.SkyNet.Control.SkyNetDb;

public sealed record SkyNetDbManagerConfig : PgManagerConfig
{
    public new const string DatabaseName = "skynetdb";

    [DebuggerStepThrough]
    public SkyNetDbManagerConfig() 
        : this(DatabaseName)
    {
        
    }
    public SkyNetDbManagerConfig(string databaseName) 
        : base(databaseName)
    {
        RoleConnectionSecretKeys = FluentDictionary
            .Create<string, string>()
            .Add("skynetdb_admin", KeyVaultSecretNames.SkyNetDbAdminConnectionString)
            .Add("skynetdb_api", KeyVaultSecretNames.SkyNetDbApiConnectionString)
            .ToImmutableDictionary();
    }
}