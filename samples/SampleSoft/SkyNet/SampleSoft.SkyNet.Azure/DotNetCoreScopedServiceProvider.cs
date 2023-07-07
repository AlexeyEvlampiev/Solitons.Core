using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Solitons;

namespace SampleSoft.SkyNet.Azure;

/// <summary>
/// Provides an implementation of <see cref="ScopedServiceProvider"/> that wraps an <see cref="IServiceScope"/>.
/// </summary>
public sealed class DotNetCoreScopedServiceProvider : ScopedServiceProvider
{
    private readonly IServiceScope _scope;

    /// <summary>
    /// Initializes a new instance of the <see cref="DotNetCoreScopedServiceProvider"/> class
    /// using the provided <see cref="IServiceScope"/>.
    /// </summary>
    /// <param name="scope">The service scope to wrap.</param>
    [DebuggerNonUserCode]
    public DotNetCoreScopedServiceProvider(IServiceScope scope)
    {
        _scope = scope;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DotNetCoreScopedServiceProvider"/> class
    /// creating a new <see cref="IServiceScope"/> using the provided <see cref="IServiceProvider"/>.
    /// </summary>
    /// <param name="provider">The service provider to create a new scope from.</param>
    [DebuggerNonUserCode]
    public DotNetCoreScopedServiceProvider(IServiceProvider provider)
    {
        _scope = provider.CreateScope();
    }

    /// <summary>
    /// Retrieves a service of the specified type from the underlying <see cref="IServiceScope"/>.
    /// </summary>
    /// <param name="serviceType">The type of service to retrieve.</param>
    /// <returns>The service instance, or null if the service cannot be resolved.</returns>
    public override object? GetService(Type serviceType) => _scope.ServiceProvider.GetService(serviceType);

    /// <summary>
    /// Releases all resources used by the underlying <see cref="IServiceScope"/>.
    /// </summary>
    protected override void Dispose() => _scope.Dispose();
}