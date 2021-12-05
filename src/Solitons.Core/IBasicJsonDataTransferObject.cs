using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json;

namespace Solitons
{
    public interface IBasicJsonDataTransferObject : IBasicDataTransferObject
    {
        [DebuggerNonUserCode]
        public string ToJsonString() => JsonSerializer.Serialize(this, this.GetType());

        public static T Parse<T>(string jsonString) where T : IBasicJsonDataTransferObject
        {
            var obj = JsonSerializer.Deserialize<T>(jsonString);
            if (obj is IDeserializationCallback callback)
                callback.OnDeserialization(typeof(IBasicJsonDataTransferObject));
            return (T)obj;
        }

        internal static object Parse(string jsonString, Type returnType)
        {
            var obj = JsonSerializer.Deserialize(jsonString,returnType);
            if (obj is IDeserializationCallback callback)
                callback.OnDeserialization(typeof(IBasicJsonDataTransferObject));
            return obj;
        }
    }

    public static partial class Extensions
    {
        [DebuggerStepThrough]
        public static string ToJsonString(this IBasicJsonDataTransferObject self) => self.ToJsonString();
    }
}
