namespace Solitons.Security.Postgres.Scripts
{
    public partial class CreatePgDatabaseScriptRtt
    {
        private readonly PgNamingRules _namingRules;

        internal CreatePgDatabaseScriptRtt(string databaseName, PgNamingRules namingRules)
        {
            DatabaseName = databaseName;
            _namingRules = namingRules;
        }

        internal string DatabaseName { get; }

        internal string DatabaseAdmin => _namingRules.BuildRoleFullName(DatabaseName, "admin");



    }
}
