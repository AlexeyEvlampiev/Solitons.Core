using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Solitons.Collections;

namespace Solitons.Web.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Solitons.Web.IUriParser" />
    public class UriParser : IUriParser
    {
        private static readonly Regex UriRegex;
        private static readonly Regex QueryRegex;

        static UriParser()
        {
            UriRegex = new Regex(@"^(?<resource>[^?]+)?(?:\?(?<query>.+))?$");
            QueryRegex = new Regex(@"^[?]?(?:@param(?:&@param)*)?$"
                .Replace("@param", "(?<name>[^&=]+)=(?<value>[^&=]+)"));
        }

        /// <summary>
        /// Parses the query string.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static KeyValuePairCollection<string, string> ParseQueryString(string queryString)
        {
            queryString = queryString
                .ThrowIfNull(() => new ArgumentException($"Input query string is required.", nameof(queryString)))
                .Trim();
            var match = QueryRegex.Match(queryString);
            if (match.Success == false) throw new FormatException();

            return KeyValuePairCollection.Create(match
                .ZipCaptures("name", "value")
                .Select(pair =>
                {
                    var key = HttpUtility.UrlDecode(pair.Key.Value);
                    var value = HttpUtility.UrlDecode(pair.Value.Value);
                    return KeyValuePair.Create(key, value);
                }));
        }

        /// <summary>
        /// Tries the parse.
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <param name="resourceUri">The resource URI.</param>
        /// <param name="queryString">The query string.</param>
        /// <returns></returns>
        public static bool TryParse(string uri, out string resourceUri, out string queryString)
        {
            resourceUri = null;
            queryString = null;
            if (uri is null) return false;
            uri = uri.Trim();
            var match = UriRegex.Match(uri);
            if (match.Success == false)
                return false;
            resourceUri = match.Groups["resource"].Value.Trim();
            queryString = match.Groups["query"].Value.Trim();
            return true;
        }

        public static bool TryParseQueryString(string queryString, out KeyValuePairCollection<string, string> queryParameters)
        {
            queryParameters = null;
            if (queryString is null) return false;
            queryString = queryString.Trim();
            var match = QueryRegex.Match(queryString);
            if (match.Success == false) return false;

            queryParameters = KeyValuePairCollection.Create(match
                .ZipCaptures("name", "value")
                .Select(pair =>
                {
                    var key = HttpUtility.UrlDecode(pair.Key.Value);
                    var value = HttpUtility.UrlDecode(pair.Value.Value);
                    return KeyValuePair.Create(key, value);
                }));
            return true;
        }

        [DebuggerStepThrough]
        bool IUriParser.TryParse(string rawUri, out string resourceUri, out string queryString) =>
            TryParse(rawUri, out resourceUri, out queryString);

        [DebuggerStepThrough]
        bool IUriParser.TryParseQueryString(string queryString, out KeyValuePairCollection<string, string> queryParameters) =>
            TryParseQueryString(queryString, out queryParameters);

    }
}
