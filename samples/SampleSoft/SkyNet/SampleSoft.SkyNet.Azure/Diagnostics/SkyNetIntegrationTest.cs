using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Solitons;
using Solitons.Diagnostics;
using Solitons.Security;

namespace SampleSoft.SkyNet.Azure.Diagnostics;

/// <summary>
/// Serves as the abstract base class for integration tests within the SkyNet system.
/// </summary>
/// <remarks>
/// This class extends the <see cref="IntegrationTest"/> class and adds additional setup for tests 
/// specific to the SkyNet system. It is designed to be subclassed for individual test suites,
/// where each subclass will provide specific configurations for the service provider through the <see cref="ConfigAsync"/> method.
/// </remarks>
public abstract class SkyNetIntegrationTest : IntegrationTest
{
    /// <summary>
    /// Asynchronously configures the services needed for the test.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    /// <param name="test">The test method to be executed.</param>
    /// <param name="secrets">The secrets repository to be used in the test.</param>
    /// <param name="cancellation">The cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
    /// <remarks>
    /// This method is called during the setup phase of each test method, allowing test classes to configure the services 
    /// required for the test. Override this method to add or replace services in the service collection.
    /// </remarks>
    protected abstract Task ConfigAsync(
        ServiceCollection services, 
        MethodInfo test,
        ISecretsRepository secrets, 
        CancellationToken cancellation);

    /// <summary>
    /// Asynchronously builds the service provider for the test.
    /// </summary>
    /// <param name="testMethod">The test method for which the service provider is being created.</param>
    /// <param name="secrets">The secrets repository to be used in the test.</param>
    /// <param name="cancellation">The cancellation token that can be used to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains the created <see cref="ScopedServiceProvider"/>.</returns>
    /// <remarks>
    /// This method creates a new service collection, adds basic services to it, and then calls <see cref="ConfigAsync"/> to allow further configuration. 
    /// Finally, it builds the service provider and wraps it in a <see cref="DotNetCoreScopedServiceProvider"/>.
    /// </remarks>
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