using System;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// 
        /// </summary>
        public static IClock System => Clock.System;

        /// <summary>
        /// 
        /// </summary>
        DateTimeOffset Now { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTimeOffset UtcNow { get; }
    }
}
