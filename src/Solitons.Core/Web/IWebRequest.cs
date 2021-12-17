using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace Solitons.Web
{
    public partial interface IWebRequest
    {
        string Uri { get; }

        string Method { get; }
        Version ClientVersion { get; }

        string Accept { get; }

        ClaimsPrincipal Caller { get; }

        IEnumerable<string> QueryParameterNames { get; }
        IPAddress? IPAddress { get; }
        string ContentType { get; }

        IEnumerable<string> GetQueryParameterValues(string name);

        [DebuggerStepThrough]
        public IWebRequest AsWebRequest() => WebRequestProxy.Wrap(this);

        [DebuggerStepThrough]
        static IWebRequest Wrap(IWebRequest request) => WebRequestProxy.Wrap(request);
        Stream GetBody();
    }

    public partial interface IWebRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public bool AcceptsAll => Accept?.Contains("*/*") == true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public bool Accepts(string contentType) =>
            Accept?.Contains(contentType, StringComparison.OrdinalIgnoreCase) == true ||
            AcceptsAll;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentTypes"></param>
        /// <returns></returns>
        public bool AcceptsAny(IEnumerable<string> contentTypes) => contentTypes is not null &&
            contentTypes.Any(Accepts);
    }
}
