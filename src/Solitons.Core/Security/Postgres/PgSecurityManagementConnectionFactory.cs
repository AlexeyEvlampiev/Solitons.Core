using System.Data;

namespace Solitons.Security.Postgres
{
    public delegate IDbConnection PgSecurityManagementConnectionFactory(string databaseName);
}
