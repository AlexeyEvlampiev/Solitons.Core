using System.Runtime.Serialization;

namespace Solitons.Data;

/// <summary>
/// Defines methods to handle the serialization and deserialization events.
/// </summary>
public interface ISerializationCallback : IDeserializationCallback
{
    /// <summary>
    /// Invoked before an object is serialized.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    void OnSerializing(object? sender);

    /// <summary>
    /// Invoked after an object has been serialized.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    void OnSerialized(object? sender);

}