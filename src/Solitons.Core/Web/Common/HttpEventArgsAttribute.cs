using Solitons.Text;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Solitons.Web.Common
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true, AllowMultiple = true)]
    public abstract class HttpEventArgsAttribute : Attribute, IHttpEventArgsAttribute
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
        protected HttpEventArgsAttribute(
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
                        "uuid" => RegexPatterns.Uuid.LooseWithoutBrakets,
                        "guid" => RegexPatterns.Uuid.LooseWithoutBrakets,
                        _ => throw new NotSupportedException(match.Value)
                    };
                });
            UriRegexp = uriRegexp;
            _methodRegex = new Regex($"^(?:{methodRegexp})$", RegexOptions.IgnoreCase);
            _uriRegex = new Regex(uriRegexp, RegexOptions.IgnoreCase);
            _versionRegex = new Regex(VersionRegexp, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        public string VersionRegexp { get; }

        /// <summary>
        /// 
        /// </summary>
        public string UriRegexp { get; }

        /// <summary>
        /// 
        /// </summary>
        public string MethodRegexp { get; }

        /// <summary>
        /// 
        /// </summary>
        public Type PayloadObjectType { get; set; }

        public Type ResponseObjectType { get; set; }

        [DebuggerNonUserCode]
        bool IHttpEventArgsAttribute.IsMethodMatch(string method) => _methodRegex.IsMatch(method ?? String.Empty);

        [DebuggerNonUserCode]
        bool IHttpEventArgsAttribute.IsUriMatch(string requestUri) => _uriRegex.IsMatch(requestUri ?? string.Empty);

        [DebuggerNonUserCode]
        bool IHttpEventArgsAttribute.IsVersionMatch(string version) => _versionRegex.IsMatch(version ?? String.Empty);

        Match IHttpEventArgsAttribute.MatchUri(string requestUri) => _uriRegex.Match(requestUri ?? String.Empty);
    }
}
