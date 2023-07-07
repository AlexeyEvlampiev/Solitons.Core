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
/// Base class for integration tests.
/// </summary>
public abstract class IntegrationTest
{
    /// <summary>
    /// Builds a scoped service provider for the given test method.
    /// </summary>
    /// <param name="testMethod">The method to be tested.</param>
    /// <param name="secrets">Secrets repository for the test.</param>
    /// <param name="cancellation">Token to observe for cancellation.</param>
    /// <returns>A scoped service provider.</returns>
    protected abstract Task<ScopedServiceProvider> BuildScopedServiceProviderAsync(
        MethodInfo testMethod, 
        ISecretsRepository secrets, 
        CancellationToken cancellation);

    /// <summary>
    /// Invoked just before each test is run.
    /// </summary>
    /// <param name="test">The test method about to be run.</param>
    protected virtual void OnTestStarting(MethodInfo test){}

    /// <summary>
    /// Invoked just after each test is run.
    /// </summary>
    /// <param name="test">The test method that was run.</param>
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