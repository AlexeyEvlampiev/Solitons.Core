using System;
using System.Diagnostics;

namespace Solitons
{
    sealed class DataTransferObjectSerializerProxy : IDataTransferObjectSerializer
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IDataTransferObjectSerializer _innerSerializer;

        private DataTransferObjectSerializerProxy(IDataTransferObjectSerializer innerSerializer)
        {
            _innerSerializer = innerSerializer;
        }

        [DebuggerNonUserCode]
        public static IDataTransferObjectSerializer Wrap(IDataTransferObjectSerializer innerSerialize)
        {
            return innerSerialize is DataTransferObjectSerializerProxy proxy
                ? proxy
                : new DataTransferObjectSerializerProxy(innerSerialize);
        }

        public string ContentType
        {
            [DebuggerStepThrough]
            get => _innerSerializer
                .ContentType
                .ThrowIfNullOrWhiteSpace(() => new InvalidOperationException($"{_innerSerializer.GetType()}.{nameof(ContentType)} returned null or empty result."));
        }

        [DebuggerStepThrough]
        public string Serialize(object obj)
        {
            return _innerSerializer
                .Serialize(obj)
                .ThrowIfNullOrWhiteSpace(() => new InvalidOperationException($"{_innerSerializer.GetType()}.{nameof(Serialize)} returned null or empty result."));
        }

        [DebuggerStepThrough]
        public object Deserialize(string content, Type targetType)
        {
            return _innerSerializer
                .Deserialize(content, targetType)
                .ThrowIfNull(() => new InvalidOperationException($"{_innerSerializer.GetType()}.{nameof(Serialize)} returned null."));
        }

        [DebuggerStepThrough]
        public override string ToString() => _innerSerializer.ToString();

        [DebuggerStepThrough]
        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || _innerSerializer.Equals(obj);

        [DebuggerStepThrough]
        public override int GetHashCode() => _innerSerializer.GetHashCode();
    }
}
