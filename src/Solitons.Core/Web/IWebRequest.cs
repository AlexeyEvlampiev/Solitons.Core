using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace Solitons.Web
{
    public interface IWebRequest
    {
        string Uri { get; }

        string Method { get; }
        Version ClientVersion { get; }

        ClaimsPrincipal Caller { get; }

        IEnumerable<string> QueryParameterNames { get; }
        IPAddress IPAddress { get; }
        IEnumerable<string> GetQueryParameterValues(string name);

        [DebuggerStepThrough]
        public IWebRequest AsWebRequest() => WebRequestProxy.Wrap(this);

        [DebuggerStepThrough]
        static IWebRequest Wrap(IWebRequest request) => WebRequestProxy.Wrap(request);
    }
}
