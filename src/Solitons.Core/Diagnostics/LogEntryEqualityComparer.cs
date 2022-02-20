using System;
using System.Collections.Generic;
using System.Linq;

namespace Solitons.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LogEntryEqualityComparer : EqualityComparer<ILogEntry>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool Equals(ILogEntry? x, ILogEntry? y)
        {
            if (x is null) return (y is null);
            if (y is null) return false;
            if (x.Level != y.Level) return false;
            if(x.Created != y.Created) return false;
            if (false == StringComparer.Ordinal.Equals(x.Message, y.Message)) return false;
            if (false == StringComparer.Ordinal.Equals(x.Details, y.Details)) return false;

            if(x.PropertyNames.Count() != y.PropertyNames.Count() ||
               x.Tags.Count() != y.Tags.Count())
            {
                return false;
            }

            if (x.PropertyNames.Except(y.PropertyNames, StringComparer.Ordinal).Any()) return false;
            if (x.Tags.Except(y.Tags, StringComparer.Ordinal).Any()) return false;

            if (x.PropertyNames.Any(key => false == StringComparer.Ordinal.Equals(x.GetProperty(key), y.GetProperty(key))))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override int GetHashCode(ILogEntry obj) => obj.Message.GetHashCode();
    }
}
