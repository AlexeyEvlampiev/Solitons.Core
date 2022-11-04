using McMaster.Extensions.CommandLineUtils;
using Npgsql;
using Sample.DbUp;
using Solitons;
using Solitons.Samples.Database;
using Solitons.Samples.Database.Models;
using Solitons.Samples.Database.Validators;
using Solitons.Security;

Console.Title = Resources.ConsoleTitle;
Console.WriteLine(Resources.AsciiArtHeader);


var cli = new CommandLineApplication
{
    Name = "sampledb",
    Description = "Sample DB management utility"
};

cli.HelpOption(inherited: true);
cli.OnExecute(() =>
{
    Console.WriteLine(@"Specify a sub-command");
    cli.ShowHelp();
    return 1;
});

cli.Command("provision", provisionDb =>
{
    provisionDb.Description = "Provisions an empty sampledb with required roles and extensions.";
    var options = new ProvisionOptions(provisionDb);
    options.Username.IsRequired(false, "Username is required.");
    options.Password.IsRequired(false, "Password is required.");
    provisionDb.OnExecute(() =>
    {
        var databaseName = (options.Database.HasValue() ? options.Database.Value() : null)
            .DefaultIfNullOrWhiteSpace("sampledb");
        var host = options.Host.HasValue() ? options.Host.Value() : "localhost";
        var adminPassword = options.DatabaseAdminPassword.HasValue() ? options.DatabaseAdminPassword.Value() : null;
        var csBuilder = new NpgsqlConnectionStringBuilder("Server=localhost;Database=postgres;Port=5432;User Id=postgres;Password=password;")
        {
            Host = host,
            Database = "postgres",
            Username = options.Username.Value(),
            Password = options.Password.Value(), 
            Timeout = 300
        };

        if (false == PgConnectionStringOptionValidator.IsValidConnectionString(csBuilder.ConnectionString, out var comment))
        {
            ConsoleColor.Red.AsForegroundColor(()=> Console.WriteLine($@"Postgres connection failed. {comment}"));
            return 1;
        }

        var connectionBuilder = new NpgsqlConnectionFactory(csBuilder.ConnectionString);
        var secrets = ISecretsRepository.InMemory();
        var sampleDb = new SampleDb(secrets);
        sampleDb.Deprovision(connectionBuilder);
        return 0;
    });
});

cli.Command("deprovision", deprovisionDb =>
{
    deprovisionDb.Description = "Drops the database and all its associated roles.";
    var options = new DeprovisionOptions(deprovisionDb);
    options.Username.IsRequired(false, "Username is required.");
    options.Password.IsRequired(false, "Password is required.");
    deprovisionDb.OnExecute(() =>
    {
        var databaseName = options.Database.HasValue() ? options.Database.Value() : "sampledb";
        var host = options.Host.HasValue() ? options.Host.Value() : "localhost";
        var csBuilder = new NpgsqlConnectionStringBuilder("Server=localhost;Database=postgres;Port=5432;User Id=postgres;Password=password;")
        {
            Host = host,
            Database = "postgres",
            Username = options.Username.Value(),
            Password = options.Password.Value()
        };

        if (false == PgConnectionStringOptionValidator.IsValidConnectionString(
                csBuilder.ConnectionString,
                out var comment))
        {
            ConsoleColor.Red.AsForegroundColor(() => Console.WriteLine($@"Postgres connection failed. {comment}"));
            return 1;
        }
        var connectionBuilder = new NpgsqlConnectionFactory(csBuilder.ConnectionString);

        var proceed = Prompt.GetYesNo($"Are you sure you want to deprovision the {databaseName} database?",
            defaultAnswer: false,
            promptColor: ConsoleColor.Yellow);
        if (proceed == false) return 0;

        var secrets = ISecretsRepository.InMemory();
        var sampleDb = new SampleDb(secrets);
        sampleDb.Deprovision(connectionBuilder);
        return 0;
    });
});

cli.Command("upgrade", upgrade =>
{
    upgrade.Description = "Upgrades database schema to the latest version.";
    var options = new UpgradeOptions(upgrade);

    options.ConnectionString.IsRequired(false, "ADO.NET PostgreSQL connection string is required");
    options.ConnectionString.Validators.Add(new PgConnectionStringOptionValidator());
    options.Superuser.IsRequired(true);
    options.Superuser.Validators.Add(new SuperuserSettingsValidator());
    options.Superuser.Description += $" ({SuperuserSettingsGroup.GetSynopsis()})";
    upgrade.OnExecute(() =>
    {
        var connection = new NpgsqlConnectionStringBuilder(options.ConnectionString.Value());
        Console.WriteLine();
        void WriteLine(string key, object value) => Console.WriteLine(@"{0,-12}{1}", key, value);
        WriteLine(@"Server:", connection.Host);
        WriteLine(@"Database:", connection.Database);
        WriteLine(@"Username:", connection.Username);

        var superuserSettings = options.Superuser
            .Values
            .Select(SuperuserSettingsGroup.Parse)
            .ToArray();
        Console.WriteLine();
        if (options.Recreate.HasValue())
        {
            var proceed = Prompt.GetYesNo("Are you sure you want to drop and recreate all the database objects?",
                defaultAnswer: false,
                promptColor: ConsoleColor.Yellow);
            if (proceed == false) return 0;
        }

        try
        {
            var upgradeOptions = SampleDbUpgradeOptions.Default;
            if (options.Recreate.HasValue())
                upgradeOptions |= SampleDbUpgradeOptions.DropAllObjects;
            if (options.Stubs.HasValue())
                upgradeOptions |= SampleDbUpgradeOptions.CreateStabRecords;

            var factory = new NpgsqlConnectionFactory(connection.ConnectionString);
            var secrets = ISecretsRepository.InMemory(data => data
                .Add("SAMPLEDB-CONNECTION-STRING", connection.ConnectionString));
            var sampleDb = new SampleDb(secrets);
            sampleDb.Upgrade();
            return 0;
        }
        catch (Exception e)
        {
            upgrade.Error.WriteLine(e.ToString());
            return 1;
        }
    });
});


try
{
    return cli.Execute(args);
}
catch (Exception ex)
{
    ConsoleColor.Red.AsForegroundColor(()=>
        Console.WriteLine(ex.Message));
    return 1; 
}
