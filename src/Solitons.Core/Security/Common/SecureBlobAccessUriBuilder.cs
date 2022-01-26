using System;
using System.Diagnostics;
using System.Net;

namespace Solitons.Security.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SecureBlobAccessUriBuilder : ISecureBlobAccessUriBuilder
    {
        private readonly TimeSpan _minExpirationInterval = TimeSpan.FromMilliseconds(100);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="expiredAfter"></param>
        /// <param name="remoteIpAddress"></param>
        /// <returns></returns>
        protected abstract Uri BuildDownloadUri(string relativePath, TimeSpan expiredAfter, IPAddress? remoteIpAddress);



        [DebuggerStepThrough]
        Uri ISecureBlobAccessUriBuilder.BuildDownloadUri(string relativePath, TimeSpan expiredAfter, IPAddress? remoteIpAddress) =>
            BuildDownloadUri(
                relativePath.ThrowIfMalformedUriArgument(UriKind.Relative, nameof(relativePath)),
                expiredAfter.ThrowIfArgumentLessThan(_minExpirationInterval, nameof(expiredAfter)),
                remoteIpAddress);
    }
}
