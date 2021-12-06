using System;
using System.Diagnostics;
using System.Net;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface ISecureAccessSignatureService : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="expiryTime"></param>
        /// <param name="startTime"></param>
        /// <param name="startAddress"></param>
        /// <param name="endAddress"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"><paramref name="queryString"/></exception>
        /// <exception cref="FormatException"><paramref name="queryString"/></exception>
        string SignQueryString(string queryString, 
            DateTime expiryTime,
            DateTime? startTime = null, 
            IPAddress startAddress = null,
            IPAddress endAddress = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="expiryTime"></param>
        /// <param name="startTime"></param>
        /// <param name="startAddress"></param>
        /// <param name="endAddress"></param>
        /// <returns></returns>
        bool IsGenuineQueryString(string queryString, 
            out DateTime? expiryTime,
            out DateTime? startTime,
            out IPAddress startAddress,
            out IPAddress endAddress);
    }

    public partial interface ISecureAccessSignatureService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="expiryTime"></param>
        /// <param name="startTime"></param>
        /// <param name="startAddress"></param>
        /// <param name="endAddress"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public Uri SignUrl(string url,
            DateTime expiryTime,
            DateTime? startTime = null,
            IPAddress startAddress = null,
            IPAddress endAddress = null)
        {
            url = url
                .ThrowIfNullOrWhiteSpaceArgument(nameof(url))
                .ThrowIfNotUri(UriKind.RelativeOrAbsolute, () => new ArgumentException(nameof(url)));
            var parts = url.Split('?');
            var (path, query) = (parts[0], parts[1]);
            query = SignQueryString(query, expiryTime, startTime, startAddress, endAddress);
            return new Uri($"{parts}?{query}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="expiryTime"></param>
        /// <param name="startTime"></param>
        /// <param name="startAddress"></param>
        /// <param name="endAddress"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public Uri SignUrl(Uri url,
           DateTime expiryTime,
           DateTime? startTime = null,
           IPAddress startAddress = null,
           IPAddress endAddress = null) => 
            SignUrl(
               url.ToString(), 
               expiryTime,
               startTime,
               startAddress,
               endAddress);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="expiryTime"></param>
        /// <param name="startTime"></param>
        /// <param name="startAddress"></param>
        /// <param name="endAddress"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool IsValidUrl(string url,
            out DateTime? expiryTime,
            out DateTime? startTime,
            out IPAddress startAddress,
            out IPAddress endAddress)
        {
            url = url
                .ThrowIfNullOrWhiteSpaceArgument(nameof(url))
                .ThrowIfNotUri(UriKind.RelativeOrAbsolute, () => new ArgumentException(nameof(url)));
            var query = url.Substring(url.IndexOf('?'));
            return IsGenuineQueryString(query, out expiryTime, out startTime, out startAddress, out endAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="expiryTime"></param>
        /// <param name="startTime"></param>
        /// <param name="startAddress"></param>
        /// <param name="endAddress"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool IsValidUrl(Uri url,
            out DateTime? expiryTime,
            out DateTime? startTime,
            out IPAddress startAddress,
            out IPAddress endAddress) =>
            IsValidUrl(url.ToString(), out expiryTime, out startTime, out startAddress, out endAddress);
    }
}
