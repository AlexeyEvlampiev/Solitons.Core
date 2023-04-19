namespace Solitons.Data;

/// <summary>
/// Represents a large object queue that can both receive and send data.
/// </summary>
public interface ILargeObjectQueue : ILargeObjectQueueProducer, ILargeObjectQueueConsumer
{
    // This interface inherits all the members of ILargeObjectQueueProducer and ILargeObjectQueueConsumer.
}