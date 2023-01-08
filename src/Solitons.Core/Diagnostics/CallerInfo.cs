using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Solitons.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    public readonly struct CallerInfo : IEquatable<CallerInfo>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>
        public CallerInfo(
            [CallerMemberName] string memberName = "",
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = -1)
        {
            MemberName = memberName;
            FilePath = filePath;
            LineNumber = lineNumber;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberName"></param>
        /// <param name="filePath"></param>
        /// <param name="lineNumber"></param>
        /// <returns></returns>
        public static CallerInfo Create(
            [CallerMemberName]string memberName = "", 
            [CallerFilePath]string filePath = "", 
            [CallerLineNumber]int lineNumber = -1)
        {
            return new CallerInfo()
            {
                MemberName = memberName,
                FilePath = filePath,
                LineNumber = lineNumber
            };
        }
        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("name")]
        public string MemberName { get; init; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("file")]
        public string FilePath { get; init; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("line")]
        public int LineNumber { get; init; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CallerInfo other)
        {
            return MemberName == other.MemberName && FilePath == other.FilePath && LineNumber == other.LineNumber;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return obj is CallerInfo other && Equals(other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(
                MemberName, 
                FilePath, 
                LineNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return new StringBuilder()
                .Append($"file={FilePath};")
                .Append($"line={LineNumber};")
                .Append($"member={LineNumber}")
                .ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public void Write(BinaryWriter writer)
        {
            writer.Write(MemberName);
            writer.Write(FilePath);
            writer.Write(LineNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="algorithm"></param>
        /// <returns></returns>
        public byte[] ComputeHash(HashAlgorithm algorithm)
        {
            using var memory = new MemoryStream();
            using var writer = new BinaryWriter(memory);
            Write(writer);
            var hash = algorithm.ComputeHash(memory.ToArray());
            return hash;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public byte[] ComputeHash()
        {
            using var sha256 = SHA256.Create();
            return ComputeHash(sha256);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transform"></param>
        /// <returns></returns>
        public CallerInfo TransformFilePath(Func<string, string> transform) => this with
        {
            FilePath = transform(FilePath)
        };


        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(CallerInfo left, CallerInfo right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(CallerInfo left, CallerInfo right)
        {
            return !left.Equals(right);
        }
    }
}
