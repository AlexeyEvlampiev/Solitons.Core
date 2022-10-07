using System;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IRandom
    {
        /// <summary>
        /// Gets a random instance of the <see cref="global::System.Guid"/> structure.
        /// </summary>
        Guid Guid { get; }

        /// <summary>
        /// Returns a non-negative random integer.
        /// </summary>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0 and less than <see cref="Int32.MaxValue"/>.</returns>
        int Next();

        /// <summary>
        /// Returns a non-negative random integer that is less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to 0.</param>
        /// <returns>A 32-bit signed integer that is greater than or equal to 0,
        /// and less than <paramref name="maxValue"/>; that is, the range of return values ordinarily includes 0 but not <paramref name="maxValue"/>.
        /// However, if <paramref name="maxValue"/> equals 0, <paramref name="maxValue"/> is returned.</returns>
        int Next(int maxValue);

        /// <summary>
        /// The inclusive lower bound of the random number returned.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue"/> must be greater than or equal to <paramref name="minValue"/>.</param>
        /// <returns>A 32-bit signed integer greater than or equal to <paramref name="minValue"/> and less than <paramref name="maxValue"/>; that is, the range of return values includes <paramref name="minValue"/> but not <paramref name="maxValue"/>.
        /// If <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.</returns>
        int Next(int minValue, int maxValue);
    }

    public partial interface IRandom
    {
        /// <summary>
        /// 
        /// </summary>
        public static IRandom System => new SystemRandom();

        /// <summary>
        /// 
        /// </summary>
        sealed class SystemRandom : IRandom
        {
            Guid IRandom.Guid => Guid.NewGuid();

            int IRandom.Next() => Random.Shared.Next();

            int IRandom.Next(int maxValue) => Random.Shared.Next(maxValue);

            int IRandom.Next(int minValue, int maxValue) => Random.Shared.Next(maxValue, maxValue);
        }
    }
}
