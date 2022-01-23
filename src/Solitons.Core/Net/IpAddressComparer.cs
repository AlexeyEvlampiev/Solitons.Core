using System;
using System.Collections.Generic;
using System.Net;

namespace Solitons.Net
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class IpAddressComparer : Comparer<IPAddress>
    {
        /// <summary>
        /// 
        /// </summary>
        public new static IpAddressComparer Default => Instance;

        /// <summary>
        /// 
        /// </summary>
        public static readonly IpAddressComparer Instance = new();

        private IpAddressComparer()
        {
            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public override int Compare(IPAddress? lhs, IPAddress? rhs)
        {
            if (lhs is null) throw new ArgumentNullException(nameof(lhs));
            if (rhs is null) throw new ArgumentNullException(nameof(rhs));
            if (IPAddress.IsLoopback(lhs))
                lhs = lhs.MapToIPv4();
            if (IPAddress.IsLoopback(rhs))
                rhs = rhs.MapToIPv4();
            var lhsVersion = Version.Parse(lhs.ToString());
            var rhsVersion = Version.Parse(rhs.ToString());
            return lhsVersion.CompareTo(rhsVersion);
        }
    }
}
