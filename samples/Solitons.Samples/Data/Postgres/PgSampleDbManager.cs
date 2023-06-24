using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solitons.Data.Management.Postgres;
using Solitons.Data.Management.Postgres.Common;

namespace Solitons.Data.Postgres;

public sealed class PgSampleDbManager : PgManager
{
    public PgSampleDbManager(string connectionString, PgManagerConfig config) 
        : base(connectionString, config)
    {
    }

    protected override async Task SaveSecretAsync(
        string secretKey, 
        string secretValue, 
        CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    protected override async Task<string?> GetSecretIfExistsAsync(
        string secretKey, 
        CancellationToken cancellation)
    {
        throw new NotImplementedException();
    }

    protected override DbConnection CreateDbConnection(
        string connectionString)
    {
        throw new NotImplementedException();
    }

    protected override PgConnectionInfo ExtractConnectionInfo(
        string connectionString)
    {
        throw new NotImplementedException();
    }

    protected override string ConstructConnectionString(string template, DatabaseConnectionOptions options)
    {
        throw new NotImplementedException();
    }
}