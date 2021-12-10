using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Solitons.Common;
using Solitons.Text;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Struct, Inherited = true, AllowMultiple = true)]
    public sealed class HttpEventArgsAttribute : Attribute, IHttpEventArgsMetadata
    {
        private readonly Regex _uriRegex;
        private readonly Regex _versionRegex;
        private readonly Regex _methodRegex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionRegexp"></param>
        /// <param name="methodRegexp"></param>
        /// <param name="uriRegexp"></param>
        /// <exception cref="NotSupportedException"></exception>
        public HttpEventArgsAttribute(
            string versionRegexp,
            string methodRegexp,            
            string uriRegexp)
        {
            VersionRegexp = versionRegexp.ThrowIfNullOrWhiteSpaceArgument(nameof(versionRegexp));
            MethodRegexp = methodRegexp.ThrowIfNullOrWhiteSpaceArgument(nameof(methodRegexp));            
            uriRegexp = uriRegexp
                .ThrowIfNullOrWhiteSpaceArgument(nameof(uriRegexp))
                .Replace(new Regex(@"rgx:(\w+)"), match =>
                {
                    return match.Groups[1].Value.ToLower() switch
                    {
                        "uuid"=> RegexPatterns.Uuid.LooseWithoutBrakets,
                        "guid" => RegexPatterns.Uuid.LooseWithoutBrakets,
                        _ => throw new NotSupportedException(match.Value)
                    };
                });
            UriRegexp = uriRegexp;
            _methodRegex = new Regex($"^(?:{methodRegexp})$", RegexOptions.IgnoreCase);
            _uriRegex = new Regex(uriRegexp, RegexOptions.IgnoreCase);
            _versionRegex = new Regex(VersionRegexp, RegexOptions.IgnoreCase);
        }
        
        public string VersionRegexp { get; }

        public string UriRegexp { get; }

        public string MethodRegexp { get; }

        [DebuggerNonUserCode]
        bool IHttpEventArgsMetadata.IsMethodMatch(string method) => _methodRegex.IsMatch(method ?? String.Empty);

        [DebuggerNonUserCode]
        bool IHttpEventArgsMetadata.IsUriMatch(string requestUri) => _uriRegex.IsMatch(requestUri ?? string.Empty);

        [DebuggerNonUserCode]
        bool IHttpEventArgsMetadata.IsVersionMatch(string version) => _versionRegex.IsMatch(version ?? String.Empty);

        Match IHttpEventArgsMetadata.MatchUri(string requestUri) => _uriRegex.Match(requestUri ?? String.Empty);
    }
}
