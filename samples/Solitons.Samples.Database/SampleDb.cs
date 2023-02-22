using DbUp;
using DbUp.Engine;
using DbUp.Helpers;
using Npgsql;
using Solitons.Samples.Database.Scripts.PostDeployment;
using System.Reflection;
using System.Text.RegularExpressions;
using Solitons.Collections;
using Solitons.Data.Management.Postgres.Common;
using Solitons.Samples.Database.Models;
using Solitons.Security;


namespace Solitons.Samples.Database
{
    public sealed class SampleDb : PgDatabaseManager
    {
        private readonly ISecretsRepository _secrets;

        public SampleDb(ISecretsRepository secrets) 
            : base("sampledb", "sampledb_admin",FluentArray.Create("sampledb_app"))
        {
            _secrets = secrets;
        }

        public override void Upgrade()
        {
            var connectionString = GetRoleConnectionString("SAMPLEDB-CONNECTION-STRING");

            var builder = new NpgsqlConnectionStringBuilder(ThrowIf.ArgumentNullOrWhiteSpace(connectionString, nameof(connectionString)))
            {
                ApplicationName = Resources.ConsoleTitle,
                Timeout = 60,
                CommandTimeout = Convert.ToInt32(TimeSpan.FromMinutes(10).TotalSeconds)
            };
            connectionString = builder.ConnectionString;

            using var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            throw new NotImplementedException();
            //var steps = BuildUpgradeStepQueue(connectionString, superuserSettings, options);
            //while (steps.Count > 0)
            //{
            //    var step = steps.Dequeue();
            //    var result = step.PerformUpgrade();
            //    if (result.Successful) continue;
            //}

        }

        private Queue<UpgradeEngine> BuildUpgradeStepQueue(
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


        protected override string? GetRoleConnectionStringIfExists(string loginRole)
        {
            var secretName = GetRoleConnectionStringSecretName(loginRole);
            return _secrets.GetSecretIfExists(secretName);
        }


        protected override void SetRoleConnectionString(string loginRole, string loginConnectionString)
        {
            var secretName = GetRoleConnectionStringSecretName(loginRole);
            _secrets.SetSecret(secretName, loginConnectionString);
        }


        private string GetRoleConnectionStringSecretName(string loginRole)
        {
            return loginRole
                .Replace(@"\W+", m => string.Empty)
                .Convert(key => $"{key}_CONNECTION_STRING")!;
        }
    }
}
