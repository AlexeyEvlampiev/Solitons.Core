using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Solitons;
using Solitons.Diagnostics;
using Solitons.Security;

namespace SampleSoft.SkyNet.Azure.Diagnostics;

/// <summary>
/// Provides a base class for integration tests using SkyNet services.
/// </summary>
public abstract class SkyNetIntegrationTest : IntegrationTest
{
    /// <summary>
    /// Configures the services for the test. Override this method to add services to the 
    /// <see cref="IServiceCollection"/> or to replace existing service registrations.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    /// <param name="test">The test method to execute.</param>
    /// <param name="secrets">The repository to access secrets.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    protected abstract Task ConfigAsync(
        ServiceCollection services, 
        MethodInfo test,
        ISecretsRepository secrets, 
        CancellationToken cancellation);

    /// <summary>
    /// Builds the service provider for the test.
    /// </summary>
    /// <param name="testMethod">The test method to execute.</param>
    /// <param name="secrets">The repository to access secrets.</param>
    /// <param name="cancellation">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation, containing the service provider.</returns>
    protected sealed override async Task<ScopedServiceProvider> BuildScopedServiceProviderAsync(
        MethodInfo testMethod, 
        ISecretsRepository secrets, 
        CancellationToken cancellation)
    {
        var services = new ServiceCollection();
        services.AddSingleton(testMethod.GetType(), testMethod);
        services.AddSingleton<ISecretsRepository>(secrets);
        services.AddSingleton(cancellation.GetType(), cancellation);

        await ConfigAsync(services, testMethod, secrets, cancellation);
        var provider = services.BuildServiceProvider(validateScopes: true);
        return new DotNetCoreScopedServiceProvider(provider);
    }

    
}