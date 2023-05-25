using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons;

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

    /// <summary>
    /// Delays the execution for the specified time interval, taking into account cancellation.
    /// </summary>
    /// <param name="duration">The time interval to delay the execution.</param>
    /// <param name="cancellation">The cancellation token to observe for cancellation requests.</param>
    Task DelayAsync(TimeSpan duration, CancellationToken cancellation = default);
}

public partial interface IClock
{
    /// <summary>
    /// Represents the system clock.
    /// </summary>
    public static readonly IClock System = new SystemClock();

    /// <summary>
    /// Creates a new <see cref="IClock"/> instance by applying the given time offset to this instance.
    /// </summary>
    /// <param name="offset">The time offset to apply.</param>
    /// <returns>A new <see cref="IClock"/> instance adjusted with the specified offset.</returns>
    [DebuggerNonUserCode]
    public IClock WithOffset(TimeSpan offset) => new ClockWithOffset(this, offset);

    /// <summary>
    /// Creates a new <see cref="IClock"/> instance adjusted using the given current-UTC reference value.
    /// </summary>
    /// <param name="utcNow">The reference UTC value.</param>
    /// <returns>A new <see cref="IClock"/> instance adjusted with the specified current-UTC reference value.</returns>
    [DebuggerNonUserCode]
    public IClock WithUtcNow(DateTime utcNow)
    {
        var offset = utcNow - this.UtcNow.UtcDateTime;
        return WithOffset(offset);
    }

    /// <summary>
    /// Creates a new <see cref="IClock"/> instance adjusted using the given current-UTC reference ticks.
    /// </summary>
    /// <param name="ticks">The reference UTC value ticks.</param>
    /// <returns>A new <see cref="IClock"/> instance adjusted with the specified current-UTC reference ticks.</returns>
    [DebuggerNonUserCode]
    public IClock WithUtcNow(long ticks) => WithUtcNow(new DateTime(ticks));

    /// <summary>
    /// Abstract base class for clocks.
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

        /// <summary>
        /// Gets the name of the clock.
        /// </summary>
        protected abstract string Name { get; }

        /// <summary>
        /// Delays the execution for the specified time interval, taking into account cancellation.
        /// </summary>
        /// <param name="duration">The time interval to delay the execution.</param>
        /// <param name="cancellation">The cancellation token to observe for cancellation requests.</param>
        /// <returns>A task representing the asynchronous delay operation.</returns>
        [DebuggerNonUserCode]
        public virtual Task DelayAsync(TimeSpan duration, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return Task.Delay(duration, cancellation);
        }

        /// <summary>
        /// Returns a string representation of the clock.
        /// </summary>
        /// <returns>A string representation of the clock.</returns>
        public sealed override string ToString() => $"{UtcNow:O} ({Name})";
    }

    /// <summary>
    /// System clock implementation.
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
        /// Gets the name of the clock.
        /// </summary>
        protected override string Name => "System clock";
    }

    /// <summary>
    /// Clock implementation with an offset.
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
        /// Gets a <see cref="DateTimeOffset"/> object adjusted with the offset.
        /// </summary>
        public override DateTimeOffset UtcNow => _innerClock.UtcNow.Add(_offset);

        /// <summary>
        /// Gets a <see cref="DateTimeOffset"/> object adjusted with the offset.
        /// </summary>
        public override DateTimeOffset Now => _innerClock.Now.Add(_offset);

        /// <summary>
        /// Gets the name of the clock with the offset information.
        /// </summary>
        protected override string Name => $"Adjusted clock. Offset: {_offset}";
    }

}