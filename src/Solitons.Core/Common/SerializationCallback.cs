using System.Diagnostics;
using System.Runtime.Serialization;
using Solitons.Data;

namespace Solitons.Common;

/// <summary>
/// Provides a base class for types that need to implement serialization callback methods.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="SerializationCallback"/> class is designed to be used as a base class for types that require serialization callback functionality.
/// It implements the <see cref="ISerializationCallback"/> interface, which provides methods to be called during the serialization and deserialization process.
/// </para>
/// <para>
/// Inheriting from this class allows derived types to define custom serialization callback methods that are called at specific points during the serialization and deserialization process.
/// By overriding the <see cref="OnSerializing"/>, <see cref="OnSerialized"/>, and <see cref="OnDeserialization"/> methods, derived types can perform additional operations before and after the serialization or deserialization of an object.
/// </para>
/// </remarks>
public abstract class SerializationCallback : ISerializationCallback
{
    /// <summary>
    /// Performs additional operations before the deserialization of an object.
    /// </summary>
    /// <param name="sender">The object that is being deserialized.</param>
    protected virtual void OnDeserialization(object? sender)
    {
        // Derived classes can override this method to handle deserialization callbacks.
    }

    /// <summary>
    /// Performs additional operations after the serialization of an object.
    /// </summary>
    /// <param name="sender">The object that has been serialized.</param>
    protected virtual void OnSerialized(object? sender)
    {
        // Derived classes can override this method to handle serialization callbacks.
    }
    /// <summary>
    /// Performs additional operations before the serialization of an object.
    /// </summary>
    /// <param name="sender">The object that is being serialized.</param>
    protected virtual void OnSerializing(object? sender)
    {
        // Derived classes can override this method to handle serialization callbacks.
    }

    [DebuggerStepThrough]
    void IDeserializationCallback.OnDeserialization(object? sender) => OnDeserialization(sender);

    [DebuggerStepThrough]
    void ISerializationCallback.OnSerialized(object? sender) => OnSerialized(sender);

    [DebuggerStepThrough]
    void ISerializationCallback.OnSerializing(object? sender) => OnSerializing(sender);
}
