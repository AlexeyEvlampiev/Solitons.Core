using System;
using System.Net;

namespace Solitons.Net
{
    /// <summary>
    /// Represents a range of allowed IP addresses.
    /// </summary>
    public readonly struct IpRange : IEquatable<IpRange>
    {
        /// <summary>
        /// Gets the start of the IP range.  Not specified if equal to null or
        /// <see cref="IPAddress.None"/>.
        /// </summary>
        public IPAddress Start { get; }

        /// <summary>
        /// Gets the optional end of the IP range.  Not specified if equal to
        /// null or <see cref="IPAddress.None"/>.
        /// </summary>
        public IPAddress End { get; }

        /// <summary>
        /// Creates a new <see cref="IpRange"/>.
        /// </summary>
        /// <param name="start">
        /// The range's start <see cref="IPAddress"/>.
        /// </param>
        /// <param name="end">
        /// The range's optional end <see cref="IPAddress"/>.
        /// </param>
        public IpRange(IPAddress start, IPAddress end = null)
        {
            start ??= end;
            end ??= start;
            
            if (IpAddressComparer.Default.Compare(end, start) >= 0)
            {

                Start = start ?? IPAddress.None;
                End = end ?? IPAddress.None;
            }
            else
            {
                Start = end ?? IPAddress.None;
                End = start ?? IPAddress.None;
            }
            
        }

        /// <summary>
        /// Check if an <see cref="IPAddress"/> was not provided.
        /// </summary>
        /// <param name="address">The address to check.</param>
        /// <returns>True if it's empty, false otherwise.</returns>
        private static bool IsEmpty(IPAddress address) =>
            address == null || Equals(address, IPAddress.None);

        /// <summary>
        /// Creates a string representation of an <see cref="IpRange"/>.
        /// </summary>
        /// <returns>
        /// A string representation of an <see cref="IpRange"/>.
        /// </returns>
        public override string ToString() =>
            IsEmpty(Start) ? string.Empty :
            IsEmpty(End) ? Start.ToString() :
            Start + "-" + End;

        /// <summary>
        /// Parse an IP range string into a new <see cref="IpRange"/>.
        /// </summary>
        /// <param name="s">IP range string to parse.</param>
        /// <returns>The parsed <see cref="IpRange"/>.</returns>
        public static IpRange Parse(string s)
        {
            var dashIndex = s.IndexOf('-');
            return dashIndex == -1 ?
                new IpRange(IPAddress.Parse(s)) :
                new IpRange(
                    IPAddress.Parse(s.Substring(0, dashIndex)),
                    IPAddress.Parse(s.Substring(dashIndex + 1)));
        }

        /// <summary>
        /// Check if two <see cref="IpRange"/> instances are equal.
        /// </summary>
        /// <param name="obj">The instance to compare to.</param>
        /// <returns>True if they're equal, false otherwise.</returns>
        public override bool Equals(object obj) =>
            obj is IpRange other && Equals(other);

        /// <summary>
        /// Get a hash code for the <see cref="IpRange"/>.
        /// </summary>
        /// <returns>Hash code for the <see cref="IpRange"/>.</returns>
        public override int GetHashCode() =>
            (Start?.GetHashCode() ?? 0) ^ (End?.GetHashCode() ?? 0);

        /// <summary>
        /// Check if two <see cref="IpRange"/> instances are equal.
        /// </summary>
        /// <param name="left">The first instance to compare.</param>
        /// <param name="right">The second instance to compare.</param>
        /// <returns>True if they're equal, false otherwise.</returns>
        public static bool operator ==(IpRange left, IpRange right) =>
            left.Equals(right);

        /// <summary>
        /// Check if two <see cref="IpRange"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first instance to compare.</param>
        /// <param name="right">The second instance to compare.</param>
        /// <returns>True if they're not equal, false otherwise.</returns>
        public static bool operator !=(IpRange left, IpRange right) =>
            !(left == right);

        /// <summary>
        /// Check if two <see cref="IpRange"/> instances are equal.
        /// </summary>
        /// <param name="other">The instance to compare to.</param>
        /// <returns>True if they're equal, false otherwise.</returns>
        public bool Equals(IpRange other) =>
            ((IsEmpty(Start) && IsEmpty(other.Start)) ||
             (Start != null && Start.Equals(other.Start))) &&
            ((IsEmpty(End) && IsEmpty(other.End)) ||
             (End != null && End.Equals(other.End)));
    }
}
