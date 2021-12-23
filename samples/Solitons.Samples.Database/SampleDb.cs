﻿using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using Npgsql;
using Solitons.Samples.Database.Scripts.PostDeployment;
using System.Reflection;
using System.Text.RegularExpressions;
using Solitons.Data;
using Solitons.Samples.Database.Models;
using Solitons.Security.Postgres;

namespace Solitons.Samples.Database
{
    public static class SampleDb
    {
        public static void DeprovisionDatabase(IDbConnectionFactory connectionFactory, string databaseName)
        {
            using var provider = new PgSecurityManagementProvider(connectionFactory);
            provider.DropRolesByPrefix($"{databaseName}_");
            provider.DropDatabase(databaseName);
        }

        public static void ProvisionDatabase(IDbConnectionFactory connectionFactory,
            string databaseName,
            string? adminPassword = null)
        {
            using var manager = new PgSecurityManagementProvider(connectionFactory);
            manager.ProvisionDatabase(databaseName,
                roles => roles
                    .WithLoginRole("public_api")
                    .WithLoginRole("private_api")
                    .WithGroupRole("prospect")
                    .WithGroupRole("customer")
                    .WithMembership("public_api", "prospect")
                    .WithMembership("public_api", "customer"),
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


        public static int Upgrade(
            string connectionString,
            SuperuserSettings[] superuserSettings,
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
            SuperuserSettings[] superuserSettings,
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

            queue.Enqueue(DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .JournalToPostgresqlTable("system", "schemaversions")
                .LogTo(logger)
                .LogScriptOutput()
                .LogToNowhere()
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(), IsDeployment)
                .Build());

            queue.Enqueue(DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .JournalTo(new NullJournal())
                .LogTo(logger)
                .LogScriptOutput()
                .LogToNowhere()
                .WithScript("Adding superuser account", new RegisterSuperuserRtt(superuserSettings))
                .WithScript("Registering HTTP triggers", new RegisterHttpTriggersRtt())
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
