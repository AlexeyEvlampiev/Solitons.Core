using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using Npgsql;
using Solitons.Samples.Database.Scripts.PostDeployment;
using System.Reflection;
using System.Text.RegularExpressions;
using Solitons.Data;
using Solitons.Data.Common.Postgres;
using Solitons.Samples.Database.Models;
using Solitons.Security.Postgres;


namespace Solitons.Samples.Database
{
    public static class SampleDb
    {
        public static void DeprovisionDatabase(Solitons.Data.IDbConnectionFactory connectionFactory, string databaseName)
        {
            var provider = new PgSecurityManagementProvider(connectionFactory);
            provider.DropRolesByPrefix($"{databaseName}_");
            provider.DropDatabase(databaseName);
        }

        public static void ProvisionDatabase(IDbConnectionFactory connectionFactory,
            string databaseName,
            string? adminPassword = null)
        {
            var manager = new PgSecurityManagementProvider(connectionFactory);
            manager.ProvisionDatabase(databaseName,
                BuildRoles,
                extensions=> extensions
                    .With("hstore")
                    .With("pgcrypto")
                    .With("postgis")
                    .With("pg_trgm"));

            if (false == adminPassword.IsNullOrWhiteSpace())
            {
                manager.ChangeRolePassword(databaseName, "admin", adminPassword);
            }
        }

        private static void BuildRoles(PgRolesBuilder roles)
        {
            var prospectRole = roles.AddGroupRole("prospect");
            var customerRole = roles.AddGroupRole("customer");

            roles.AddLoginRole("public_api", prospectRole, customerRole);
            roles.AddLoginRole("private_api");
        }


        public static int Upgrade(
            string connectionString,
            SuperuserSettingsGroup[] superuserSettings,
            SampleDbUpgradeOptions options)
        {
            connectionString.ThrowIfNullOrWhiteSpaceArgument(nameof(connectionString));

            var builder = new NpgsqlConnectionStringBuilder(connectionString.ThrowIfNullOrWhiteSpaceArgument(nameof(connectionString)))
            {
                ApplicationName = Resources.ConsoleTitle,
                Timeout = 60,
                CommandTimeout = Convert.ToInt32(TimeSpan.FromMinutes(10).TotalSeconds)
            };
            connectionString = builder.ConnectionString;

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            var steps = BuildUpgradeStepQueue(connectionString, superuserSettings, options);
            while (steps.Count > 0)
            {
                var step = steps.Dequeue();
                var result = step.PerformUpgrade();
                if (result.Successful) continue;
                return 1;
            }

            return 0;
        }

        private static Queue<UpgradeEngine> BuildUpgradeStepQueue(
            string connectionString, 
            SuperuserSettingsGroup[] superuserSettings,
            SampleDbUpgradeOptions options)
        {
            var logger = new SampleDbUpgradeLog();
            Queue<UpgradeEngine> queue = new ();

            if (options.HasFlag(SampleDbUpgradeOptions.DropAllObjects))
            {
                queue.Enqueue(DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .JournalTo(new NullJournal())
                .LogTo(logger)
                .LogScriptOutput()
                .LogToNowhere()
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), IsDropScript)
                .Build());
            }

            queue.Enqueue(DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .JournalTo(new NullJournal())
                .LogTo(logger)
                .LogScriptOutput()
                .LogToNowhere()
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), IsPreDeployment)
                .Build());


            var commonSql = new string[]
            {
                new CommonPgScriptRtt("system"),
                new LoggingPgScriptRtt(LoggingPgScriptPartitioningOptions.ByYearQuarter,"system"),
            }.Join(Environment.NewLine);
            queue.Enqueue(DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .JournalToPostgresqlTable("system", "schemaversions")
                .LogTo(logger)
                .LogScriptOutput()
                .WithScript("common.sql", commonSql, new SqlScriptOptions(){ RunGroupOrder = -1 })
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), IsDeployment)
                .Build());
            
            queue.Enqueue(DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .JournalTo(new NullJournal())
                .LogTo(logger)
                .LogScriptOutput()
                .LogToNowhere()
                .WithScript("Registering Data Transfer Object contracts", new RegisterDataContractsSqlRtt())
                .WithScript("Adding superuser account", new RegisterSuperuserRtt(superuserSettings))
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), IsPostDeployment)
                .Build());


            if (options.HasFlag(SampleDbUpgradeOptions.CreateStabRecords))
            {
                queue.Enqueue(DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .JournalTo(new NullJournal())
                .LogTo(logger)
                .LogScriptOutput()
                .LogToNowhere()
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), IsStabsScript)
                .Build());
            }

            return queue;

            bool IsDropScript(string scriptPath) => Regex.IsMatch(scriptPath, @"scripts[.]drop[.].*[.]sql", RegexOptions.IgnoreCase);
            bool IsPreDeployment(string scriptPath) => Regex.IsMatch(scriptPath, @"scripts[.]predeployment[.].*[.]sql", RegexOptions.IgnoreCase);
            bool IsDeployment(string scriptPath) => Regex.IsMatch(scriptPath, @"scripts[.]deployment[.].*[.]sql", RegexOptions.IgnoreCase);
            bool IsPostDeployment(string scriptPath) => Regex.IsMatch(scriptPath, @"scripts[.]postdeployment[.].*[.]sql", RegexOptions.IgnoreCase);
            bool IsStabsScript(string scriptPath) => Regex.IsMatch(scriptPath, @"scripts[.]stubs[.].*[.]sql", RegexOptions.IgnoreCase);
        }
    }
}
