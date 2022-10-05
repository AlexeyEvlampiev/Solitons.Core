using System;
using System.Diagnostics;

namespace Solitons
{
    /// <summary>
    /// Represents a point in time, typically expressed as a date and time of day, relative to Coordinated Universal Time (UTC).
    /// </summary>
    public partial interface IClock
    {
        /// <summary>
        /// Gets a <see cref="DateTimeOffset"/> object that is set to the current date and time on the current computer, with the offset set to the local time's offset from Coordinated Universal Time (UTC).
        /// </summary>
        DateTimeOffset Now { get; }

        /// <summary>
        /// Gets a <see cref="DateTimeOffset"/> object whose date and time are set to the current Coordinated Universal Time (UTC) date and time and whose offset is Zero.
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
        /// Creates a new <see cref="IClock"/> instance by applying the given time offset to this instance.
        /// </summary>
        /// <param name="offset">The time offset.</param>
        [DebuggerNonUserCode]
        public IClock WithOffset(TimeSpan offset) => new ClockWithOffset(this, offset);

        /// <summary>
        /// Creates a new <see cref="IClock"/> instance adjusted using the given current-UTC reference value.
        /// </summary>
        /// <param name="utcNow">Reference UTC value</param>
        [DebuggerNonUserCode]
        public IClock WithUtcNow(DateTime utcNow)
        {
            var offset = utcNow - this.UtcNow.UtcDateTime;
            return WithOffset(offset);
        }

        /// <summary>
        /// Creates a new <see cref="IClock"/> instance adjusted using the given current-UTC reference ticks.
        /// </summary>
        /// <param name="ticks">Reference UTC value</param>
        [DebuggerNonUserCode]
        public IClock WithUtcNow(long ticks) => WithUtcNow(new DateTime(ticks));

        /// <summary>
        /// 
        /// </summary>
        abstract class Clock : IClock
        {
            /// <summary>
            /// Gets a <see cref="DateTimeOffset"/> object that is set to the current date and time on the current computer, with the offset set to the local time's offset from Coordinated Universal Time (UTC).
            /// </summary>
            public abstract DateTimeOffset Now { get; }

            /// <summary>
            /// Gets a <see cref="DateTimeOffset"/> object whose date and time are set to the current Coordinated Universal Time (UTC) date and time and whose offset is Zero.
            /// </summary>
            public abstract DateTimeOffset UtcNow { get; }

            protected abstract string Name { get; }

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public sealed override string ToString() => $"{UtcNow:O} ({Name})";
        }

        /// <summary>
        /// System clock implementation
        /// </summary>
        sealed class SystemClock : Clock
        {
            /// <summary>
            /// Gets a <see cref="DateTimeOffset"/> object whose date and time are set to the current Coordinated Universal Time (UTC) date and time and whose offset is Zero.
            /// </summary>
            public override DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

            /// <summary>
            /// Gets a <see cref="DateTimeOffset"/> object that is set to the current date and time on the current computer, with the offset set to the local time's offset from Coordinated Universal Time (UTC).
            /// </summary>
            public override DateTimeOffset Now => DateTimeOffset.Now;

            /// <summary>
            /// 
            /// </summary>
            protected override string Name => "System clock";
        }

        /// <summary>
        /// 
        /// </summary>
        sealed class ClockWithOffset : Clock
        {
            private readonly IClock _innerClock;
            private readonly TimeSpan _offset;

            internal ClockWithOffset(IClock innerClock, TimeSpan offset)
            {
                _innerClock = innerClock;
                _offset = offset;
            }

            /// <summary>
            /// 
            /// </summary>
            public override DateTimeOffset UtcNow => _innerClock.UtcNow.Add(_offset);

            /// <summary>
            /// 
            /// </summary>
            public override DateTimeOffset Now => _innerClock.Now.Add(_offset);

            /// <summary>
            /// 
            /// </summary>
            protected override string Name => $"Adjusted clock. Offset: {_offset}";
        }
    }
}
