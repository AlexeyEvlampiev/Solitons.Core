using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Claims;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WebRequest : IWebRequest
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly IWebRequest _innerRequest;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="httpEventArgs"></param>
        /// <param name="messageBody"></param>
        internal WebRequest(IWebRequest request, object httpEventArgs, object messageBody)
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

        public string Accept => _innerRequest.Accept;

        [DebuggerStepThrough]
        public Stream GetBody() => _innerRequest.GetBody();

        [DebuggerStepThrough]
        public IEnumerable<string> GetQueryParameterValues(string name) => _innerRequest.GetQueryParameterValues(name);

        /// <summary>
        /// 
        /// </summary>
        public bool AcceptsAll => _innerRequest.AcceptsAll;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public bool Accepts(string contentType) => _innerRequest.Accepts(contentType);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentTypes"></param>
        /// <returns></returns>
        public bool AcceptsAny(IEnumerable<string> contentTypes) => _innerRequest.AcceptsAny(contentTypes);
    }
}
