using System;

namespace Solitons.Security
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum BlobSasPermissions
    {
        /// <summary>
        /// 
        /// </summary>
        Read = 1,

        /// <summary>
        /// 
        /// </summary>
        Create = 2,

        /// <summary>
        /// 
        /// </summary>
        Write = 4,

        /// <summary>
        /// 
        /// </summary>
        Delete = 8
    }
}
