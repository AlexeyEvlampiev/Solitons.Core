using System;

namespace Solitons.Security
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class BlobSasUriAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="permissions"></param>
        /// <param name="expiresAfter"></param>
        public BlobSasUriAttribute(string scope, BlobSasPermissions permissions, string expiresAfter)
        {
            Scope = scope.ThrowIfNullOrWhiteSpaceArgument(nameof(scope)).Trim();
            Permissions = permissions;
            ExpiresAfter = TimeSpan.Parse(expiresAfter);
        }


        /// <summary>
        /// 
        /// </summary>
        public string Scope { get; }

        /// <summary>
        /// 
        /// </summary>
        public BlobSasPermissions Permissions { get; }

        /// <summary>
        /// 
        /// </summary>
        public IpRangeRestriction IpRangeRestriction { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan ExpiresAfter { get; }
    }
}
