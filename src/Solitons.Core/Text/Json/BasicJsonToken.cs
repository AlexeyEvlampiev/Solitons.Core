using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Solitons.Text.Json
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BasicJsonToken : IEquatable<BasicJsonToken>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public sealed override string ToString()
        {
            var json = JsonSerializer.Serialize(this, GetType());
            return json.ToBase64(Encoding.UTF8);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        [DebuggerStepThrough]
        protected static T Parse<T>(string input) where T : BasicJsonToken, new()
        {
            var json = ThrowIf.ArgumentNullOrWhiteSpace(input, nameof(input))
                .AsBase64Bytes()
                .ToUtf8String();
            return JsonSerializer.Deserialize<T>(json) 
                   ?? throw new FormatException();
        }

        /// <summary>
        /// 
        /// </summary>
        protected interface IEncoder
        {
            /// <summary>
            /// 
            /// </summary>
            /// <param name="json"></param>
            /// <returns></returns>
            byte[] Encode(string json);

            /// <summary>
            /// 
            /// </summary>
            /// <param name="bytes"></param>
            /// <returns></returns>
            string Decode(byte[] bytes);
        }

        sealed class Utf8Encoder : IEncoder
        {
            public byte[] Encode(string json) => Encoding.UTF8.GetBytes(json);

            public string Decode(byte[] bytes) => Encoding.UTF8.GetString(bytes);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(BasicJsonToken? other)
        {
            if (other is null) return false;
            if (other.GetType() != this.GetType()) return false;
            return StringComparer.Ordinal.Equals(this.ToString(), other.ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BasicJsonToken)obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => ToString().GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(BasicJsonToken? left, BasicJsonToken? right) => Equals(left, right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(BasicJsonToken? left, BasicJsonToken? right) => !Equals(left, right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public static implicit operator string?(BasicJsonToken? token) => token?.ToString();
    }
}
