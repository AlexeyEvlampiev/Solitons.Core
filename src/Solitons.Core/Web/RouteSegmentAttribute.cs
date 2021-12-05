using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace Solitons.Web
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class RouteSegmentAttribute : Attribute
    {
        public RouteSegmentAttribute(string regexGroupName)
        {
            RegexGroupName = regexGroupName;
        }

        public string RegexGroupName { get; }
        internal TypeConverter TypeConverter { get; set; }

        [DebuggerNonUserCode]
        public static RouteSegmentAttribute TryGet(PropertyInfo property) => property?.GetCustomAttribute<RouteSegmentAttribute>();
    }
}
