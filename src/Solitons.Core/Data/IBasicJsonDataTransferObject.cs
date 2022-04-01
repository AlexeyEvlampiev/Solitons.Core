using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text.Json;

namespace Solitons.Data
{
    /// <summary>
    /// A marker interface automatically adding the <see cref="ToJsonString"/> method to implementing types.
    /// </summary>
    /// <seealso cref="Parse{T}(string)"/>
    /// <seealso cref="Parse(string, Type)"/>
    /// <seealso cref="BasicXmlDataTransferObject"/>
    /// <seealso cref="BasicJsonMediaTypeSerializer"/>
    public interface IBasicJsonDataTransferObject
    {
        /// <summary>
        /// Converts this instance to its JSON- string representation.
        /// </summary>
        /// <returns>The JSON- string object representation</returns>
        [DebuggerStepThrough]
        public string ToJsonString()
        {
            var callback = this as ISerializationCallback;
            callback?.OnSerializing(null);
            var json = JsonSerializer.Serialize(this, this.GetType());
            callback?.OnSerialized(null);
            return json;
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="jsonString">The JSON to deserialize.</param>
        /// <returns>The JSON- deserialized object.</returns>
        [DebuggerNonUserCode]
        public static T Parse<T>(string jsonString) where T : IBasicJsonDataTransferObject
        {
            var obj = JsonSerializer.Deserialize<T>(jsonString);
            if (obj is IDeserializationCallback callback)
                callback.OnDeserialization(null);
            return (T)obj;
        }

        /// <summary>
        /// Deserializes the JSON to the specified .NET type.
        /// </summary>
        /// <param name="jsonString">The JSON to deserialize.</param>
        /// <param name="returnType">The type of the object to deserialize to.</param>
        /// <returns>The JSON- deserialized object.</returns>
        [DebuggerStepThrough]
        public static object Parse(string jsonString, Type returnType)
        {
            var obj = JsonSerializer.Deserialize(jsonString,returnType);
            if (obj is IDeserializationCallback callback)
                callback.OnDeserialization(null);
            return obj;
        }
    }

    public static partial class Extensions
    {
        /// <summary>
        /// Converts this instance to its JSON- string representation.
        /// </summary>
        /// <param name="self">The object to serialize</param>
        /// <returns>The JSON- string object representation</returns>
        [DebuggerStepThrough]
        public static string ToJsonString(this IBasicJsonDataTransferObject self) => self.ToJsonString();
    }
}
