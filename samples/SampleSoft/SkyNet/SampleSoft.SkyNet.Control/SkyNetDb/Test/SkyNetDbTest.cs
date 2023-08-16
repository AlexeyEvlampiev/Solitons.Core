using System.Data;
using System.Diagnostics;
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

/// <summary>
/// Serves as the abstract base class for SkyNet database integration tests.
/// </summary>
/// <remarks>
/// This class extends the <see cref="SkyNetIntegrationTest"/> class and provides additional setup specifically for tests 
/// that interact with the SkyNet database. It provides a standard <see cref="ConfigAsync"/> implementation that sets up a connection 
/// and transaction with the SkyNet database. It also provides a <see cref="SkyNetDbHttpClient"/> for making HTTP requests within the database transaction context.
/// </remarks>
public abstract class SkyNetDbTest : SkyNetIntegrationTest
{
    /// <summary>
    /// Asynchronously configures the services needed for the database test.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    /// <param name="test">The test method to be executed.</param>
    /// <param name="secrets">The secrets repository to be used in the test.</param>
    /// <param name="cancellation">The cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <remarks>
    /// This method is called during the setup phase of each test method, allowing the test to configure the services 
    /// required for the test. It sets up a database connection and transaction using the connection string retrieved from secrets repository.
    /// </remarks>
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

        services.AddScoped<NpgsqlConnection>(provider =>
        {
            var connection = new NpgsqlConnection(connectionString);
            connection.Open();
            connection.BeginTransaction(IsolationLevel.RepeatableRead);
            return connection;
        });


        services.AddScoped<SkyNetDbHttpClient>(provider => provider
            .GetRequiredService<NpgsqlConnection>()
            .Convert(connection => new SkyNetDbHttpClient(connection)));

        services.AddScoped<HttpClient>(provider => provider
            .GetRequiredService<SkyNetDbHttpClient>());
    }

    /// <summary>
    /// Called just before the execution of each test method.
    /// </summary>
    /// <param name="test">The test method about to be executed.</param>
    /// <remarks>
    /// This method is called just before each test method is run. It can be overridden to perform any necessary setup or logging.
    /// </remarks>
    protected override void OnTestStarting(MethodInfo test)
    {
        ConsoleColor.Green.AsForegroundColor(() => 
            Console.WriteLine($"{test.DeclaringType?.Name}.{test.Name}"));
    }
}