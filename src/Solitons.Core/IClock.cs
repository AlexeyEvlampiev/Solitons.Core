using System;

namespace Solitons
{
    /// <summary>
    /// Represents a point in time, typically expressed as a date and time of day, relative to Coordinated Universal Time (UTC).
    /// </summary>
    public partial interface IClock
    {
        /// <summary>
        /// 
        /// </summary>
        DateTimeOffset Now { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTimeOffset UtcNow { get; }
    }

    public partial interface IClock
    {
        /// <summary>
        /// System clock
        /// </summary>
        public static readonly IClock System = new SystemClock();

        /// <summary>
        /// System clock implementation
        /// </summary>
        sealed class SystemClock : IClock
        {
            /// <summary>
            /// 
            /// </summary>
            public DateTimeOffset Now => DateTimeOffset.Now;

            /// <summary>
            /// 
            /// </summary>
            public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
        }
    }
}
