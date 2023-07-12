using System.Runtime.Serialization;

namespace Solitons.Data;

/// <summary>
/// Defines methods to handle serialization and deserialization events.
/// </summary>
/// <remarks>
/// <para>
/// The <see cref="ISerializationCallback"/> interface provides methods to handle serialization and deserialization events for an object.
/// It serves as a contract for types that need to perform additional operations before or after serialization and deserialization.
/// </para>
/// <para>
/// The <see cref="OnSerializing"/> method is invoked before an object is serialized. Implement this method to perform any custom actions or transformations on the object prior to serialization.
/// </para>
/// <para>
/// The <see cref="OnSerialized"/> method is invoked after an object has been serialized. Implement this method to perform any custom actions or cleanup operations after serialization has completed.
/// </para>
/// </remarks>
internal interface ISerializationCallback : IDeserializationCallback
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