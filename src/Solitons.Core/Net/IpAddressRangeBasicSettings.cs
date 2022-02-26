using System;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Solitons.Configuration;

namespace Solitons.Net
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class IpAddressRangeBasicSettings : BasicSettings, IFormattable
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private IPAddress? _end;

        /// <summary>
        /// 
        /// </summary>
        [DebuggerNonUserCode]
        public IpAddressRangeBasicSettings()
        {
            Start = IPAddress.None;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        [DebuggerStepThrough]
        public IpAddressRangeBasicSettings(IPAddress address) : this(address.ThrowIfNullArgument(nameof(address)), address){}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public IpAddressRangeBasicSettings(IPAddress start, IPAddress? end)
        {
            end ??= start;

            var comparer = IpAddressComparer.Default;
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
        [BasicSetting("Start", IsRequired = true, Pattern = @"(?is)^(?:start|from)(?:-?address)$")]
        public IPAddress Start { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [BasicSetting("End", IsRequired = false, Pattern = @"(?is)^(?:end|till|to|untill)(?:-?address)$")]
        public IPAddress End
        {
            get => _end ?? Start;
            set => _end = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
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
        public static string GetSynopsis()
        {
            var template = GetSynopsis<IpAddressRangeBasicSettings>();
            return $"{{start}}-{{end}} or {{address}} or {template}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="format"></param>
        /// <param name="formatProvider"></param>
        /// <returns></returns>
        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            return format switch
            {
                "g" => _end is null ? Start.ToString() : $"{Start}-{End}",
                _=> this.ToString()
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="settings"></param>
        public static implicit operator IpAddressRange?(IpAddressRangeBasicSettings? settings)
        {
            if (settings is null) return null;
            return new IpAddressRange(settings.Start, settings.End);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="range"></param>
        public static implicit operator IpAddressRangeBasicSettings?(IpAddressRange range)
        {
            return new IpAddressRangeBasicSettings(range.Start, range.Start);
        }
    }
}
