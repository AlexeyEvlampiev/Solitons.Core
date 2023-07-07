using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Solitons;

/// <summary>
/// Provides a basic implementation of <see cref="IServiceProvider"/> that creates service instances using <see cref="Activator.CreateInstance(Type)"/>.
/// </summary>
/// <remarks>
/// This service provider implementation uses the <see cref="Activator.CreateInstance(Type)"/> method for creating service instances,
/// which creates a new instance of a type each time a service is requested. This behavior differs from a traditional IoC container,
/// which may manage the lifetime of instances (transient, singleton, etc.) and resolve complex dependency chains.
///
/// Therefore, while this class is suitable for simple scenarios where services have no dependencies or their lifetimes don't need to be managed,
/// it is not suitable for more complex scenarios. If you need to manage service lifetimes, or if your services have dependencies that need to be injected,
/// consider using a full-fledged IoC container.
///
/// Be aware that using this class could lead to unexpected behavior if used improperly. For example, if you register a service that has disposable resources
/// but don't dispose of them correctly, you could have resource leaks.
///
/// Furthermore, since it uses reflection to create instances, this approach may have performance implications if used heavily. In performance-critical paths,
/// consider other ways of service registration and resolution.
/// </remarks>
public sealed class ActivatorServiceProvider : IServiceProvider
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Func<Type, object?> _activatorCallback;

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivatorServiceProvider"/> class with the default activator factory.
    /// </summary>
    [DebuggerNonUserCode]
    public ActivatorServiceProvider()
    {
        _activatorCallback = Activator.CreateInstance;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivatorServiceProvider"/> class with the activator factory that uses the provided arguments.
    /// </summary>
    /// <param name="args">The arguments to pass to the constructor.</param>
    [DebuggerNonUserCode]
    public ActivatorServiceProvider(params object?[]? args)
    {
        args = args ?? Array.Empty<object>();
        _activatorCallback = (type) => Activator.CreateInstance(type, args);
    }


    /// <summary>
    /// Initializes a new instance of the <see cref="ActivatorServiceProvider"/> class with the activator factory that uses the provided binding attributes, binder, arguments and culture.
    /// </summary>
    /// <param name="bindingAttr">The binding attributes to use.</param>
    /// <param name="binder">The binder to use.</param>
    /// <param name="args">The arguments to pass to the constructor.</param>
    /// <param name="culture">The culture to use.</param>
    [DebuggerNonUserCode]
    public ActivatorServiceProvider(BindingFlags bindingAttr, Binder binder, object?[]? args, CultureInfo culture)
    {
        args = args ?? Array.Empty<object>();
        _activatorCallback = (type) => Activator.CreateInstance(type, bindingAttr, binder, args ?? Array.Empty<object>(), culture);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ActivatorServiceProvider"/> class with the activator factory that uses the provided binding attributes, binder, arguments, culture, and activation attributes.
    /// </summary>
    /// <param name="bindingAttr">The binding attributes to use.</param>
    /// <param name="binder">The binder to use.</param>
    /// <param name="args">The arguments to pass to the constructor.</param>
    /// <param name="culture">The culture to use.</param>
    /// <param name="activationAttributes">The activation attributes to use.</param>
    [DebuggerNonUserCode]
    public ActivatorServiceProvider(BindingFlags bindingAttr, Binder binder, object?[]? args, CultureInfo culture, object[]? activationAttributes)
    {
        args = args ?? Array.Empty<object>();
        _activatorCallback = (type) => Activator.CreateInstance(type, bindingAttr, binder, args, culture, activationAttributes);
    }

    /// <summary>
    /// Gets the service of the specified type.
    /// </summary>
    /// <param name="serviceType">The type of the service to get.</param>
    /// <returns>A service of the requested type, or null if the service cannot be resolved.</returns>
    [DebuggerStepThrough]
    public object? GetService(Type serviceType) => _activatorCallback.Invoke(serviceType);
}