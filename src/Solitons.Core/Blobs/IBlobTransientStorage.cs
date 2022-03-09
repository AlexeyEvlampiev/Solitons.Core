using System;
using System.Net;
using Solitons.Data;

namespace Solitons.Blobs
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBlobTransientStorage : ITransientStorage
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="expiresOn"></param>
        /// <param name="startAddress"></param>
        /// <param name="endAddress"></param>
        /// <returns></returns>
        Uri Sign(string blobName, DateTimeOffset expiresOn, IPAddress startAddress, IPAddress endAddress);
    }
}
