using System;
using System.Diagnostics;

namespace Solitons.Data;

sealed class MediaTypeSerializerProxy : IMediaTypeSerializer
{
    [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
    private readonly IMediaTypeSerializer _innerSerializer;

    private MediaTypeSerializerProxy(IMediaTypeSerializer innerSerializer)
    {
        _innerSerializer = innerSerializer;
    }

    [DebuggerNonUserCode]
    public static IMediaTypeSerializer Wrap(IMediaTypeSerializer innerSerialize)
    {
        return innerSerialize is MediaTypeSerializerProxy proxy
            ? proxy
            : new MediaTypeSerializerProxy(innerSerialize);
    }


    public string TargetContentType
    {
        [DebuggerStepThrough]
        get => _innerSerializer
            .TargetContentType
            .ThrowIfNullOrWhiteSpace(() => new InvalidOperationException($"{_innerSerializer.GetType()}.{nameof(TargetContentType)} returned null or empty result."));
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
            .ThrowIfNull($"{_innerSerializer.GetType()}.{nameof(Serialize)} returned null.");
    }

    [DebuggerStepThrough]
    public override string ToString() => _innerSerializer.ToString();

    [DebuggerStepThrough]
    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || _innerSerializer.Equals(obj);

    [DebuggerStepThrough]
    public override int GetHashCode() => _innerSerializer.GetHashCode();
}