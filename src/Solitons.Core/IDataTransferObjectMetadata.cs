using System;

namespace Solitons
{
    public interface IDataTransferObjectMetadata
    {
        Type SerializerType { get; }

        bool IsDefault { get; }

    }
}
