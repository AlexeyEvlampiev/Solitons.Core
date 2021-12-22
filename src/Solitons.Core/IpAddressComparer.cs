﻿using System;
using System.Collections.Generic;
using System.Net;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class IpAddressComparer : Comparer<IPAddress>
    {
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
            var lhsVersion = Version.Parse(lhs.ToString());
            var rhsVersion = Version.Parse(rhs.ToString());
            return lhsVersion.CompareTo(rhsVersion);
        }
    }
}
