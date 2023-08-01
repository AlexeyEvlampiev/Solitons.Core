using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Collections;
using Solitons.Security;

namespace Solitons.Diagnostics;

/// <summary>
/// Serves as the abstract base class for all integration tests.
/// </summary>
/// <remarks>
/// Integration tests are typically used to test interactions between multiple parts of the system,
/// such as service calls, database operations, or network requests.
/// </remarks>
public abstract class IntegrationTest
{
    /// <summary>
    /// Asynchronously builds a new service provider for the given test method.
    /// </summary>
    /// <param name="testMethod">The test method for which the service provider is being created.</param>
    /// <param name="secrets">The secrets repository to be used in the test.</param>
    /// <param name="cancellation">The cancellation token that can be used to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the created <see cref="ScopedServiceProvider"/>.</returns>
    /// <remarks>
    /// This method is called once for each test method, before the test is run. 
    /// The returned service provider is used to resolve any services required by the test method.
    /// </remarks>
    protected abstract Task<ScopedServiceProvider> BuildScopedServiceProviderAsync(
        MethodInfo testMethod, 
        ISecretsRepository secrets, 
        CancellationToken cancellation);

    /// <summary>
    /// Called just before each test is run.
    /// </summary>
    /// <param name="test">The test method that is about to be run.</param>
    /// <remarks>
    /// Override this method to perform setup operations before each test is run.
    /// </remarks>
    protected virtual void OnTestStarting(MethodInfo test){}

    /// <summary>
    /// Called just after each test is run.
    /// </summary>
    /// <param name="test">The test method that was just run.</param>
    /// <remarks>
    /// Override this method to perform cleanup operations after each test is run.
    /// </remarks>
    protected virtual void OnTestCompleted(MethodInfo test) {}


    /// <summary>
    /// Runs all public instance methods of this class.
    /// </summary>
    /// <param name="secrets">Secrets repository for the tests.</param>
    /// <param name="cancellation">Token to observe for cancellation.</param>
    public async Task RunAllAsync(
        ISecretsRepository secrets, 
        CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        var handlers =
            GetType()
                .GetMethods(
                    BindingFlags.Instance | 
                    BindingFlags.Public | 
                    BindingFlags.DeclaredOnly)
                .ToList();

        foreach (var handler in handlers)
        {
            OnTestStarting(handler);
            cancellation.ThrowIfCancellationRequested();
            using var scope = await BuildScopedServiceProviderAsync(handler, secrets, cancellation);

            var parameters = handler.GetParameters();
            var args = new object[parameters.Length];
            for (int i = 0; i < args.Length; ++i)
            {
                args[i] = ThrowIf.NullReference(
                    scope.GetService(parameters[i].ParameterType));
            }


            var result = handler.Invoke(this, args);
            if (result is Task task)
            {
                await task;
            }

            OnTestCompleted(handler);
        }

    }

    /// <summary>
    /// Runs all tests in the given assembly.
    /// </summary>
    /// <param name="assembly">The assembly containing the tests.</param>
    /// <param name="secrets">Secrets repository for the tests.</param>
    /// <param name="serviceProviderFactory">Optional factory for creating the service provider.</param>
    /// <param name="cancellation">Token to observe for cancellation.</param>
    [DebuggerStepThrough]
    public static Task RunAllAsync(
        Assembly assembly,
        ISecretsRepository secrets,
        Func<Type[], IServiceProvider>? serviceProviderFactory = null,
        CancellationToken cancellation = default)
    {
        var assemblies = FluentArray.Create(assembly);
        return RunAllAsync(assemblies, secrets, serviceProviderFactory, cancellation);
    }

    /// <summary>
    /// Runs all tests in the given assemblies.
    /// </summary>
    /// <param name="assemblies">The assemblies containing the tests.</param>
    /// <param name="secrets">Secrets repository for the tests.</param>
    /// <param name="serviceProviderFactory">Optional factory for creating the service provider.</param>
    /// <param name="cancellation">Token to observe for cancellation.</param>
    public static async Task RunAllAsync(
        IEnumerable<Assembly> assemblies,
        ISecretsRepository secrets,
        Func<Type[], IServiceProvider>? serviceProviderFactory = null,
        CancellationToken cancellation = default)
    {
        cancellation.ThrowIfCancellationRequested();
        var tests = Load(assemblies, serviceProviderFactory);
        foreach (var test in tests)
        {
            cancellation.ThrowIfCancellationRequested();
            await test.RunAllAsync(secrets, cancellation);
        }
    }

    /// <summary>
    /// Gets the types of all non-abstract subclasses of IntegrationTest in the given assemblies.
    /// </summary>
    /// <param name="assemblies">The assemblies to search.</param>
    /// <returns>The types of the subclasses.</returns>
    public static IEnumerable<Type> GetTypes(
        IEnumerable<Assembly> assemblies)
    {
        return assemblies
            .Distinct()
            .SelectMany(_ => _.GetTypes())
            .Where(t => t.IsSubclassOf(typeof(IntegrationTest)))
            .Where(t => t.IsAbstract == false);
    }

    /// <summary>
    /// Loads instances of all non-abstract subclasses of IntegrationTest in the given assemblies.
    /// </summary>
    /// <param name="assemblies">The assemblies to load the subclasses from.</param>
    /// <param name="serviceProviderFactory">Optional factory for creating the service provider.</param>
    /// <returns>The loaded instances of the subclasses.</returns>
    public static IEnumerable<IntegrationTest> Load(
        IEnumerable<Assembly> assemblies,
        Func<Type[], IServiceProvider>? serviceProviderFactory = null)
    {
        serviceProviderFactory ??= (Type[] types) => new ActivatorServiceProvider();

        var types = GetTypes(assemblies).ToArray();
        var provider = serviceProviderFactory.Invoke(types);
        foreach (var type in types)
        {
            var instance = ThrowIf.NullReference(provider.GetService(type));
            yield return (IntegrationTest)instance;
        }
    }
}