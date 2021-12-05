using System;
using System.Diagnostics;
using System.Net;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISecureAccessSigner
    {
        /// <summary>
        /// Transforms <paramref name="dto"/> properties marked with <see cref="BlobSecureAccessSignatureMetadata"/> into SAS protected absolute urls calculated from the original property values.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>Signed values count</returns>
        int Sign(object dto);


        /// <summary>
        /// Transforms <paramref name="dto"/> properties marked with <see cref="BlobSecureAccessSignatureMetadata"/> into SAS protected absolute urls calculated from the original property values.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ip"></param>
        /// <param name="endAddress"></param>
        /// <returns>Signed values count</returns>
        int Sign(object dto, IPAddress ip, IPAddress endAddress);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="declaration"></param>
        /// <returns></returns>
        bool CanSign(ISecureAccessSignatureMetadata declaration);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="protection"></param>
        /// <returns></returns>
        Uri Sign(string resource, ISecureAccessSignatureMetadata protection);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="ip"></param>
        /// <param name="endAddress"></param>
        /// <param name="protection"></param>
        /// <returns></returns>
        Uri Sign(string resource, IPAddress ip, IPAddress endAddress, ISecureAccessSignatureMetadata protection);


        /// <summary>
        /// Transforms <paramref name="dto"/> properties marked with <see cref="BlobSecureAccessSignatureMetadata"/> into SAS protected absolute urls calculated from the original property values.
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="ip"></param>
        /// <returns>Signed values count</returns>
        [DebuggerStepThrough]
        public int Sign(object dto, IPAddress ip)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));
            if (ip == null) throw new ArgumentNullException(nameof(ip));
            return Sign(dto, ip, ip);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resource"></param>
        /// <param name="ip"></param>
        /// <param name="protection"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        Uri Sign(string resource, IPAddress ip, ISecureAccessSignatureMetadata protection)
        {
            resource.ThrowIfNullOrWhiteSpaceArgument(nameof(resource));
            ip.ThrowIfNullArgument(nameof(ip));
            protection.ThrowIfNullArgument(nameof(protection));
            return Sign(resource, ip, ip, protection);
        }
    }
}