using System;

namespace Solitons.Data
{
    public interface IDataTransferObjectMetadata
    {
        Type SerializerType { get; }

        bool IsDefault { get; }

    }
}
