using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace Solitons.Diagnostics;


/// <summary>
/// Represents information about the calling member, file, and line number, 
/// obtained through attributes that can be applied to optional parameters.
/// </summary>
public readonly struct CallerInfo : IEquatable<CallerInfo>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CallerInfo"/> struct.
    /// </summary>
    /// <param name="memberName">The name of the calling member.</param>
    /// <param name="filePath">The full path of the source file that contains the caller.</param>
    /// <param name="lineNumber">The line number in the source file at which the method is called.</param>
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
    /// Creates a new instance of the <see cref="CallerInfo"/> struct with the calling member,
    /// file, and line number information obtained through attributes that can be applied to optional parameters.
    /// </summary>
    /// <param name="memberName">The name of the calling member.</param>
    /// <param name="filePath">The full path of the source file that contains the caller.</param>
    /// <param name="lineNumber">The line number in the source file at which the method is called.</param>
    /// <returns>A new instance of the <see cref="CallerInfo"/> struct.</returns>
    public static CallerInfo Create(
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = -1)
    {
        return new CallerInfo()
        {
            MemberName = memberName,
            FilePath = filePath,
            LineNumber = lineNumber
        };
    }

    /// <summary>
    /// Gets or sets the name of the calling member.
    /// </summary>
    [JsonPropertyName("name")]
    public string MemberName { get; init; }

    /// <summary>
    /// Gets or sets the full path of the source file that contains the caller.
    /// </summary>
    [JsonPropertyName("file")]
    public string FilePath { get; init; }

    /// <summary>
    /// Gets or sets the line number in the source file at which the method is called.
    /// </summary>
    [JsonPropertyName("line")]
    public int LineNumber { get; init; }

    /// <summary>
    /// Determines whether this instance of the <see cref="CallerInfo"/> struct and another specified
    /// instance of the struct have the same values for the <see cref="MemberName"/>, <see cref="FilePath"/>,
    /// and <see cref="LineNumber"/> properties.
    /// </summary>
    /// <param name="other">The other <see cref="CallerInfo"/> instance to compare to this instance.</param>
    /// <returns>True if the two <see cref="CallerInfo"/> instances are equal; otherwise, false.</returns>
    public bool Equals(CallerInfo other)
    {
        return MemberName == other.MemberName && FilePath == other.FilePath && LineNumber == other.LineNumber;
    }

    /// <summary>
    /// Determines whether this instance of the <see cref="CallerInfo"/> struct and a specified object,
    /// which must also be a <see cref="CallerInfo"/> instance, have the same values for the <see cref="MemberName"/>,
    /// <see cref="FilePath"/>, and <see cref="LineNumber"/> properties.
    /// </summary>
    /// <param name="obj">The object to compare to this instance.</param>
    /// <returns>True if the specified object is a <see cref="CallerInfo"/> instance and the two instances are equal; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        return obj is CallerInfo other && Equals(other);
    }

    /// <summary>
    /// Returns a hash code for this instance of the <see cref="CallerInfo"/> struct, based on the values of
    /// the <see cref="MemberName"/>, <see cref="FilePath"/>, and <see cref="LineNumber"/> properties.
    /// </summary>
    /// <returns>A hash code for this instance of the <see cref="CallerInfo"/> struct.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(MemberName, FilePath, LineNumber);
    }

    /// <summary>
    /// Returns a string that represents this instance of the <see cref="CallerInfo"/> struct,
    /// containing the values of the <see cref="FilePath"/>, <see cref="LineNumber"/>,
    /// and <see cref="MemberName"/> properties.
    /// </summary>
    /// <returns>A string that represents this instance of the <see cref="CallerInfo"/> struct.</returns>
    public override string ToString()
    {
        return new StringBuilder()
            .Append($"file={FilePath};")
            .Append($"line={LineNumber};")
            .Append($"member={MemberName}")
            .ToString();
    }

    /// <summary>
    /// Writes the <see cref="MemberName"/>, <see cref="FilePath"/>, and <see cref="LineNumber"/>
    /// properties of this instance of the <see cref="CallerInfo"/> struct to a binary writer.
    /// </summary>
    /// <param name="writer">The binary writer to write to.</param>
    public void Write(BinaryWriter writer)
    {
        writer.Write(MemberName);
        writer.Write(FilePath);
        writer.Write(LineNumber);
    }

    /// <summary>
    /// Computes a hash for this instance of the <see cref="CallerInfo"/> struct using the specified hash algorithm.
    /// </summary>
    /// <param name="algorithm">The hash algorithm to use.</param>
    /// <returns>A byte array that represents the hash value of this instance of the <see cref="CallerInfo"/> struct.</returns>
    public byte[] ComputeHash(HashAlgorithm algorithm)
    {
        using var memory = new MemoryStream();
        using var writer = new BinaryWriter(memory);
        Write(writer);
        var hash = algorithm.ComputeHash(memory.ToArray());
        return hash;
    }

    /// <summary>
    /// Computes a hash for this instance of the <see cref="CallerInfo"/> struct using the SHA256 hash algorithm.
    /// </summary>
    /// <returns>A byte array that represents the SHA256 hash value of this instance of the <see cref="CallerInfo"/> struct.</returns>
    public byte[] ComputeHash()
    {
        using var sha256 = SHA256.Create();
        return ComputeHash(sha256);
    }

    /// <summary>
    /// Returns a new <see cref="CallerInfo"/> struct with the <see cref="FilePath"/> property
    /// transformed using the specified function.
    /// </summary>
    /// <param name="transform">The function used to transform the <see cref="FilePath"/> property.</param>
    /// <returns>A new <see cref="CallerInfo"/> struct with the transformed <see cref="FilePath"/> property.</returns>
    public CallerInfo TransformFilePath(Func<string, string> transform) => this with
    {
        FilePath = transform(FilePath)
    };


    /// <summary>
    /// Determines whether two <see cref="CallerInfo"/> instances have the same values for
    /// the <see cref="MemberName"/>, <see cref="FilePath"/>, and <see cref="LineNumber"/> properties.
    /// </summary>
    /// <param name="left">The first <see cref="CallerInfo"/> instance to compare.</param>
    /// <param name="right">The second <see cref="CallerInfo"/> instance to compare.</param>
    /// <returns>True if the two <see cref="CallerInfo"/> instances are equal; otherwise, false.</returns>
    public static bool operator ==(CallerInfo left, CallerInfo right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two <see cref="CallerInfo"/> instances have different values for
    /// the <see cref="MemberName"/>, <see cref="FilePath"/>, and <see cref="LineNumber"/> properties.
    /// </summary>
    /// <param name="left">The first <see cref="CallerInfo"/> instance to compare.</param>
    /// <param name="right">The second <see cref="CallerInfo"/> instance to compare.</param>
    /// <returns>True if the two <see cref="CallerInfo"/> instances are not equal; otherwise, false.</returns>
    public static bool operator !=(CallerInfo left, CallerInfo right)
    {
        return !left.Equals(right);
    }
}