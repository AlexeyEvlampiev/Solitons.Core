using System;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Solitons.Security.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class JsonToken : IEquatable<JsonToken>
    {
        private readonly IEncoder _encoder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encoder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected JsonToken(IEncoder encoder)
        {
            _encoder = encoder ?? throw new ArgumentNullException(nameof(encoder));
        }

        /// <summary>
        /// 
        /// </summary>
        protected JsonToken() : this(new Utf8Encoder())
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public sealed override string ToString()
        {
            var json = JsonSerializer.Serialize(this, GetType());
            return _encoder.Encode(json).ToBase64String();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="encoder"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="FormatException"></exception>
        [DebuggerStepThrough]
        protected static T Parse<T>(string input, IEncoder? encoder = null) where T : JsonToken, new()
        {
            input = input.ThrowIfNullOrWhiteSpaceArgument(nameof(input));
            encoder ??= new Utf8Encoder();

            var bytes = input
                .ThrowIfNullOrWhiteSpaceArgument(nameof(input))
                .AsBase64Bytes();
            var json = encoder.Decode(bytes);
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
        public bool Equals(JsonToken? other)
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
            return Equals((JsonToken)obj);
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
        public static bool operator ==(JsonToken? left, JsonToken? right) => Equals(left, right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(JsonToken? left, JsonToken? right) => !Equals(left, right);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        public static implicit operator string?(JsonToken? token) => token?.ToString();
    }
}
