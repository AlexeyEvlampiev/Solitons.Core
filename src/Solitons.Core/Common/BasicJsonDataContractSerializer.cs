using System;
using System.Diagnostics;
using System.Text.Json;

namespace Solitons.Common
{
    public sealed class BasicJsonDataContractSerializer : DataContractSerializer
    {
        public BasicJsonDataContractSerializer() : base("application/json")
        {
        }

        [DebuggerStepThrough]
        protected override string Serialize(object obj) => JsonSerializer.Serialize(obj);

        protected override object Deserialize(string content, Type targetType) =>
            JsonSerializer.Deserialize(content, targetType);
    }
}
