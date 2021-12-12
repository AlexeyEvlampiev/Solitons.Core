using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DomainWebRequest : IWebRequest
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IWebRequest _innerRequest;

        internal DomainWebRequest(IWebRequest request, object httpEventArgs, object messageBody)
        {
            _innerRequest = WebRequestProxy.Wrap( request.ThrowIfNullArgument(nameof(request)));
            HttpEventArgs = httpEventArgs.ThrowIfNullArgument(nameof(httpEventArgs));            
            MessageBody = messageBody;
        }

        /// <summary>
        /// 
        /// </summary>
        public object HttpEventArgs { get; }

        /// <summary>
        /// 
        /// </summary>
        public object MessageBody { get; }

        public string Uri => _innerRequest.Uri;

        public string Method => _innerRequest.Method;

        public Version ClientVersion => _innerRequest.ClientVersion;

        public ClaimsPrincipal Caller => _innerRequest.Caller;

        public IEnumerable<string> QueryParameterNames => _innerRequest.QueryParameterNames;

        public IPAddress IPAddress => _innerRequest.IPAddress;

        public string ContentType => _innerRequest.ContentType;

        [DebuggerStepThrough]
        public Stream GetBody() => _innerRequest.GetBody();

        [DebuggerStepThrough]
        public IEnumerable<string> GetQueryParameterValues(string name) => _innerRequest.GetQueryParameterValues(name);
    }
}
