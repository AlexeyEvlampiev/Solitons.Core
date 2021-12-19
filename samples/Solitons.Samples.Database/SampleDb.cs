using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using Npgsql;
using Solitons.Samples.Database.Scripts.PostDeployment;
using Solitons.Security.Postgres;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Solitons.Samples.Database
{
    public class SampleDb
    {        
        public static void CreateRoleWithLogin(
            string connectionString, 
            string roleName, 
            string password, 
            CreateRoleOptions options, 
            int connectionLimit)
        {
            string sql = new CreateRoleCommandRtt(roleName, password,options,connectionLimit);
            using var connection = new NpgsqlConnection(connectionString);
            using var command = new NpgsqlCommand(sql.ToString(),connection);
            connection.Open();
            command.ExecuteNonQuery();
        }


        public static int Upgrade(
            string connectionString,
            string[] superuserEmails,
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
            var steps = BuildUpgradeStepQueue(connectionString, superuserEmails, options);
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
            string[] superuserEmails,
            SampleDbUpgradeOptions options)
        {
            var logger = new SampleUpgradeLog();
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
                .WithScript("Adding superuser account", new RegisterSuperuserRtt(superuserEmails))
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
