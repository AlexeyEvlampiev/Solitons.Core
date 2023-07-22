using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solitons;

/// <summary>
/// Represents a pseudo-random number generator, which is an algorithm that produces a sequence of numbers that meet certain statistical requirements for randomness.
/// </summary>
public partial interface IRandom
{
    /// <summary>
    /// Returns instance of the <see cref="global::System.Guid"/> structure.
    /// </summary>
    Guid NextGuid();

    /// <summary>
    /// Returns a non-negative random integer.
    /// </summary>
    /// <returns>A 32-bit signed integer that is greater than or equal to 0 and less than <see cref="Int32.MaxValue"/>.</returns>
    int NextInt32();

    /// <summary>
    /// Returns a non-negative random integer that is less than the specified maximum.
    /// </summary>
    /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to 0.</param>
    /// <returns>A 32-bit signed integer that is greater than or equal to 0,
    /// and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily includes 0 but not <paramref name="maxValue"/>.
    /// However, if <paramref name="maxValue"/> equals 0, <paramref name="maxValue"/> is returned.</returns>
    int NextInt32(int maxValue);

    /// <summary>
    /// The inclusive lower bound of the random number returned.
    /// </summary>
    /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
    /// <param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.</param>
    /// <returns>A 32-bit signed integer greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>.
    /// If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.</returns>
    int NextInt32(int minValue, int maxValue);

    /// <summary>
    /// Fills the elements of a specified array of bytes with random numbers.
    /// </summary>
    /// <param name="buffer">The array to be filled with random numbers.</param>
    /// <exception cref="ArgumentNullException"><paramref name="buffer"/> is null.</exception>
    void NextBytes(byte[] buffer);

    /// <summary>
    /// Fills the elements of a specified span of bytes with random numbers.
    /// </summary>
    /// <param name="buffer">The array to be filled with random numbers.</param>
    /// <remarks>
    /// Each element of the span of bytes is set to a random number greater than or equal to 0 and
    /// less than or equal to <see cref="byte.MaxValue"/>.
    /// </remarks>
    void NextBytes(Span<byte> buffer);

    /// <summary>
    /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
    /// </summary>
    /// <returns>A double-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
    double NextDouble();

    /// <summary>
    /// Returns a non-negative random integer.
    /// </summary>
    /// <returns>A 64-bit signed integer that is greater than or equal to 0 and less than <see cref="Int64.MaxValue"/>.</returns>
    long NextInt64();

    /// <summary>
    /// Returns a non-negative random integer that is less than the specified maximum.
    /// </summary>
    /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to 0.</param>
    /// <returns>A 64-bit signed integer that is greater than or equal to 0, and less than <paramref name="maxValue"/>;
    /// that is, the range of return values ordinarily includes 0 but not <paramref name="maxValue"/>.
    /// However, if <paramref name="maxValue"/> equals 0, <paramref name="maxValue"/> is returned.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than 0.</exception>
    long NextInt64(long maxValue);

    /// <summary>
    /// Returns a random integer that is within a specified range.
    /// </summary>
    /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
    /// <param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.</param>
    /// <returns>A 64-bit signed integer greater than or equal to minValue and less than <paramref name="maxValue"/>;
    /// that is, the range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>.
    /// If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="minValue"/> is greater than <paramref name="maxValue"/>.</exception>
    long NextInt64(long minValue, long maxValue);

    /// <summary>
    /// Returns a random floating-point number that is greater than or equal to 0.0, and less than 1.0.
    /// </summary>
    /// <returns>A single-precision floating point number that is greater than or equal to 0.0, and less than 1.0.</returns>
    float NextSingle();
}

public partial interface IRandom
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public sealed T Choice<T>(IReadOnlyList<T> list)
    {
        var index = NextInt32(0, list.Count - 1);
        return list[index];
    }
}

public partial interface IRandom
{
    /// <summary>
    /// Provides a thread-safe <see cref="IRandom"/> instance that may be used concurrently from any thread.
    /// </summary>
    /// <returns></returns>
    public static readonly IRandom System = Wrap(Random.Shared);

    /// <summary>
    /// Creates a new instance of <see cref="IRandom"/> default implementation.
    /// </summary>
    /// <param name="generator">The inner <see cref="Random"/> generator.</param>
    /// <returns></returns>
    public static IRandom Wrap(Random generator) => new RelayGenerator(generator);

    /// <summary>
    /// Default <see cref="IRandom"/> implementation.
    /// </summary>
    sealed class RelayGenerator : IRandom
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly Random _innerGenerator;

        [DebuggerNonUserCode]
        internal RelayGenerator(Random innerGenerator)
        {
            _innerGenerator = innerGenerator;
        }

        [DebuggerNonUserCode]
        Guid IRandom.NextGuid() => Guid.NewGuid();

        [DebuggerStepThrough]
        int IRandom.NextInt32() => _innerGenerator.Next();

        [DebuggerStepThrough]
        int IRandom.NextInt32(int maxValue) => _innerGenerator.Next(maxValue);

        [DebuggerStepThrough]
        int IRandom.NextInt32(int minValue, int maxValue) => _innerGenerator.Next(minValue, maxValue);

        [DebuggerStepThrough]
        void IRandom.NextBytes(byte[] buffer) => _innerGenerator.NextBytes(buffer);

        [DebuggerStepThrough]
        void IRandom.NextBytes(Span<byte> buffer) => _innerGenerator.NextBytes(buffer);

        [DebuggerStepThrough]
        double IRandom.NextDouble() => _innerGenerator.NextDouble();

        [DebuggerStepThrough]
        long IRandom.NextInt64() => _innerGenerator.NextInt64();

        [DebuggerStepThrough]
        long IRandom.NextInt64(long maxValue) => _innerGenerator.NextInt64(maxValue);

        [DebuggerStepThrough]
        long IRandom.NextInt64(long minValue, long maxValue) => _innerGenerator.NextInt64(minValue, maxValue);

        [DebuggerStepThrough]
        float IRandom.NextSingle() => _innerGenerator.NextSingle();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() => _innerGenerator.ToString() ?? GetType().FullName ?? nameof(RelayGenerator);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is RelayGenerator other)
            {
                return _innerGenerator.Equals(other._innerGenerator);
            }

            if (obj is Random random)
            {
                return _innerGenerator.Equals(random);
            }

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => _innerGenerator.GetHashCode();
    }
}