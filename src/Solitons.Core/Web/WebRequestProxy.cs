using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Security.Claims;

namespace Solitons.Web
{
    sealed class WebRequestProxy : IWebRequest
    {
        private readonly IWebRequest _innerRequest;

        [DebuggerNonUserCode]
        public WebRequestProxy(IWebRequest innerRequest)
        {
            _innerRequest = innerRequest ?? throw new ArgumentNullException(nameof(innerRequest));
        }


        [DebuggerNonUserCode]
        public static IWebRequest Wrap(IWebRequest other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            return other is WebRequestProxy proxy ? proxy : new WebRequestProxy(other);
        }

        [DebuggerStepThrough]
        public override string ToString() => _innerRequest.ToString();

        [DebuggerStepThrough]
        public override bool Equals(object? obj) => _innerRequest.Equals(obj);

        [DebuggerStepThrough]
        public override int GetHashCode() => _innerRequest.GetHashCode();

        public string Uri => _innerRequest.Uri.ThrowIfNull(()=> new NullReferenceException($"{_innerRequest.GetType()}.{nameof(Uri)} is null."));
        public string Method => _innerRequest.Method.ThrowIfNullOrWhiteSpace(()=> new InvalidOperationException($"{_innerRequest.Method}.{nameof(_innerRequest.Method)} returned null or a white space string."));

        public Version ClientVersion => _innerRequest.ClientVersion.ThrowIfNull(() => new NullReferenceException($"{_innerRequest.GetType()}.{nameof(ClientVersion)} is null."));

        public IEnumerable<string> QueryParameterNames => _innerRequest.QueryParameterNames.EmptyIfNull();
        public IPAddress IPAddress => _innerRequest.IPAddress;

        public ClaimsPrincipal Caller => _innerRequest.Caller;

        public IEnumerable<string> GetQueryParameterValues(string name) => _innerRequest.GetQueryParameterValues(name).EmptyIfNull();
    }
}
