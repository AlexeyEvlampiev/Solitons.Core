using System;
using System.Net;

namespace Solitons.Security
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISecureBlobAccessUriBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="relativePath"></param>
        /// <param name="expiredAfter"></param>
        /// <param name="remoteIpAddress"></param>
        /// <returns></returns>
        Uri BuildDownloadUri(string relativePath, TimeSpan expiredAfter, IPAddress? remoteIpAddress = null);

    }
}
