using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace Solitons.Net
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class IpAddressRangeBasicSettings : BasicSettings
    {
        /// <summary>
        /// 
        /// </summary>
        [DebuggerNonUserCode]
        public IpAddressRangeBasicSettings()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        [DebuggerStepThrough]
        public IpAddressRangeBasicSettings(IPAddress address) : this(address, address){}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public IpAddressRangeBasicSettings(IPAddress start, IPAddress end)
        {
            var comparer = new IpAddressComparer();
            if (comparer.Compare(end, start) >= 0)
            {
                Start = start;
                End = end;
            }
            else
            {
                End = start;
                Start = end;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [BasicSetting("Start", IsRequired = true, Pattern = @"(?is)^start(?:-?address)$")]
        public IPAddress Start { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [BasicSetting("End", IsRequired = true, Pattern = @"(?is)^end(?:-?address)$")]
        public IPAddress End { get; set; }

        public static IpAddressRangeBasicSettings Parse(string text)
        {
            text.ThrowIfNullOrWhiteSpaceArgument(nameof(text));
            if (text.Contains("-"))
            {
                var parts = Regex.Split(text, @"\s*-\s*");
                if (parts.Length != 2)
                    throw new FormatException();
                if(false == IPAddress.TryParse(parts[0], out var start))
                    throw new FormatException();
                if (false == IPAddress.TryParse(parts[1], out var end))
                    throw new FormatException();
                return new IpAddressRangeBasicSettings(start, end);
            }

            if(IPAddress.TryParse(text, out var address))
            {
                return new IpAddressRangeBasicSettings(address);
            }

            return Parse<IpAddressRangeBasicSettings>(text);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetSynopsis()
        {
            var template = GetSynopsis<IpAddressRangeBasicSettings>();
            return $"{template} or {{start}}-{{end}} or {{address}}";
        }
    }
}
