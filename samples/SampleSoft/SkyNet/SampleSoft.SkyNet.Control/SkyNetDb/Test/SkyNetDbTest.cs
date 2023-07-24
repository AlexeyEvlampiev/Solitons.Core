﻿using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using SampleSoft.SkyNet.Azure;
using Solitons;
using Solitons.Security;
using SampleSoft.SkyNet.Azure.Diagnostics;
using SampleSoft.SkyNet.Azure.Postgres;
using Solitons.Data;

namespace SampleSoft.SkyNet.Control.SkyNetDb.Test;

public abstract class SkyNetDbTest : SkyNetIntegrationTest
{
    protected override async Task ConfigAsync(
        ServiceCollection services,
        MethodInfo test,
        ISecretsRepository secrets,
        CancellationToken cancellation)
    {
        cancellation.ThrowIfCancellationRequested();
        var connectionString = await test
            .GetCustomAttribute<SkyNetConnectionStringSecretAttribute>()
            .Convert(att => att?.SecretName)
            .Convert(sn => sn.DefaultIfNullOrWhiteSpace(KeyVaultSecretNames.SkyNetDbAdminConnectionString))
            .Convert(sn => secrets.GetSecretAsync(sn, cancellation));

        services.AddScoped<NpgsqlConnection>(provider => provider.GetRequiredService<NpgsqlTransaction>().Connection!);

        services.AddScoped<NpgsqlTransaction>(provider =>
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString)
            {
                CommandTimeout = 30,
                ApplicationName = "SkyNet Database test"
            };
            var connection = new NpgsqlConnection(builder.ConnectionString);
            connection.Open();
            connection.StateChange += (sender, args) =>
            {
                Debug.WriteLine($"{test.Name} connection status changed: {args.OriginalState} -> {args.CurrentState}");
            };
            return connection.BeginTransaction();
        });

        services.AddScoped<SkyNetDbHttpClient>(provider => provider
            .GetRequiredService<NpgsqlTransaction>()
            .Convert(transaction => new SkyNetDbHttpClient(transaction)));
    }

    protected override void OnTestStarting(MethodInfo test)
    {
        ConsoleColor.Green.AsForegroundColor(() => 
            Console.WriteLine($"{test.DeclaringType?.Name}.{test.Name}"));
    }
}