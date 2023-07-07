using System;
using System.Diagnostics;

namespace Solitons;

/// <summary>
/// Abstract base class for creating scoped, disposable service providers.
/// </summary>
public abstract class ScopedServiceProvider : IServiceProvider, IDisposable
{
    /// <summary>
    /// Retrieves a service of the specified type.
    /// </summary>
    /// <param name="serviceType">The type of service to retrieve.</param>
    /// <returns>The service instance, or null if the service cannot be resolved.</returns>
    public abstract object? GetService(Type serviceType);

    /// <summary>
    /// Releases all resources used by the current instance of the ScopedServiceProvider class.
    /// </summary>
    protected abstract void Dispose();

    /// <summary>
    /// Creates a new instance of the <see cref="IServiceProvider"/> class,
    /// providing a non-disposable IServiceProvider interface for the current instance.
    /// </summary>
    /// <returns>A non-disposable proxy to the current instance.</returns>
    public IServiceProvider AsServiceProvider() => new ServiceProviderProxy(this);

    /// <inheritdoc/>
    object? IServiceProvider.GetService(Type serviceType) => GetService(serviceType);

    /// <inheritdoc/>
    void IDisposable.Dispose() => Dispose();

    /// <summary>
    /// Provides a proxy for ScopedServiceProvider that hides the IDisposable interface.
    /// </summary>
    sealed class ServiceProviderProxy : IServiceProvider
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly ScopedServiceProvider _provider;

        public ServiceProviderProxy(ScopedServiceProvider provider)
        {
            _provider = provider;
        }

        public object? GetService(Type serviceType)
        {
            return ((IServiceProvider)_provider).GetService(serviceType);
        }

        public override string ToString() => _provider.ToString() ?? _provider.GetType().FullName ?? String.Empty;

        public override int GetHashCode() => _provider.GetHashCode();

        public override bool Equals(object? obj) => _provider.Equals(obj);
    }
}