using System;
using System.Net;

namespace Solitons.Net
{
    /// <summary>
    /// Represents a range of allowed IP addresses.
    /// </summary>
    public readonly struct IpAddressRange : IEquatable<IpAddressRange>
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
        /// Creates a new <see cref="IpAddressRange"/>.
        /// </summary>
        /// <param name="start">
        /// The range's start <see cref="IPAddress"/>.
        /// </param>
        /// <param name="end">
        /// The range's optional end <see cref="IPAddress"/>.
        /// </param>
        public IpAddressRange(IPAddress start, IPAddress? end = null)
        {
            end ??= start;
            
            if (IpAddressComparer.Default.Compare(end, start) >= 0)
            {

                Start = start;
                End = end;
            }
            else
            {
                Start = end;
                End = start;
            }
            
        }

        /// <summary>
        /// Check if an <see cref="IPAddress"/> was not provided.
        /// </summary>
        /// <param name="address">The address to check.</param>
        /// <returns>True if it's empty, false otherwise.</returns>
        private static bool IsEmpty(IPAddress address) => Equals(address, IPAddress.None);

        /// <summary>
        /// Creates a string representation of an <see cref="IpAddressRange"/>.
        /// </summary>
        /// <returns>
        /// A string representation of an <see cref="IpAddressRange"/>.
        /// </returns>
        public override string ToString() =>
            IsEmpty(Start) ? string.Empty :
            IsEmpty(End) ? Start.ToString() :
            Start + "-" + End;

        /// <summary>
        /// Parse an IP range string into a new <see cref="IpAddressRange"/>.
        /// </summary>
        /// <param name="s">IP range string to parse.</param>
        /// <returns>The parsed <see cref="IpAddressRange"/>.</returns>
        public static IpAddressRange Parse(string s)
        {
            var dashIndex = s.IndexOf('-');
            return dashIndex == -1 ?
                new IpAddressRange(IPAddress.Parse(s)) :
                new IpAddressRange(
                    IPAddress.Parse(s.Substring(0, dashIndex)),
                    IPAddress.Parse(s.Substring(dashIndex + 1)));
        }

        /// <summary>
        /// Check if two <see cref="IpAddressRange"/> instances are equal.
        /// </summary>
        /// <param name="obj">The instance to compare to.</param>
        /// <returns>True if they're equal, false otherwise.</returns>
        public override bool Equals(object? obj) =>
            obj is IpAddressRange other && Equals(other);

        /// <summary>
        /// Get a hash code for the <see cref="IpAddressRange"/>.
        /// </summary>
        /// <returns>Hash code for the <see cref="IpAddressRange"/>.</returns>
        public override int GetHashCode() =>
            (Start?.GetHashCode() ?? 0) ^ (End?.GetHashCode() ?? 0);

        /// <summary>
        /// Check if two <see cref="IpAddressRange"/> instances are equal.
        /// </summary>
        /// <param name="left">The first instance to compare.</param>
        /// <param name="right">The second instance to compare.</param>
        /// <returns>True if they're equal, false otherwise.</returns>
        public static bool operator ==(IpAddressRange left, IpAddressRange right) =>
            left.Equals(right);

        /// <summary>
        /// Check if two <see cref="IpAddressRange"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first instance to compare.</param>
        /// <param name="right">The second instance to compare.</param>
        /// <returns>True if they're not equal, false otherwise.</returns>
        public static bool operator !=(IpAddressRange left, IpAddressRange right) =>
            !(left == right);

        /// <summary>
        /// Check if two <see cref="IpAddressRange"/> instances are equal.
        /// </summary>
        /// <param name="other">The instance to compare to.</param>
        /// <returns>True if they're equal, false otherwise.</returns>
        public bool Equals(IpAddressRange other) =>
            ((IsEmpty(Start) && IsEmpty(other.Start)) ||
             (Start != null && Start.Equals(other.Start))) &&
            ((IsEmpty(End) && IsEmpty(other.End)) ||
             (End != null && End.Equals(other.End)));
    }
}
