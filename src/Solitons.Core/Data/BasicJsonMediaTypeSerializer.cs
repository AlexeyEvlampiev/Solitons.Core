using System;
using System.Diagnostics;
using System.Text.Json;
using Solitons.Data.Common;

namespace Solitons.Data;

sealed class BasicJsonMediaTypeSerializer : MediaTypeSerializer
{
    public BasicJsonMediaTypeSerializer() : base("application/json")
    {
    }

    [DebuggerStepThrough]
    protected override string Serialize(object obj) => JsonSerializer.Serialize(obj);

    protected override object? Deserialize(string content, Type targetType) =>
        JsonSerializer.Deserialize(content, targetType);
}