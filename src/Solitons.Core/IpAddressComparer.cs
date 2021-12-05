using System;
using System.Collections.Generic;
using System.Net;

namespace Solitons
{
    public sealed class IpAddressComparer : Comparer<IPAddress>
    {
        public override int Compare(IPAddress? lhs, IPAddress? rhs)
        {
            if (lhs is null) throw new ArgumentNullException(nameof(lhs));
            if (rhs is null) throw new ArgumentNullException(nameof(rhs));
            var lhsVersion = Version.Parse(lhs.ToString());
            var rhsVersion = Version.Parse(rhs.ToString());
            return lhsVersion.CompareTo(rhsVersion);
        }
    }
}
