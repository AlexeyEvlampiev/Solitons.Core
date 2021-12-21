using System.Security;

namespace Solitons.Security
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ClaimNotFoundException : SecurityException
    {        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimType"></param>
        public ClaimNotFoundException(string claimType) 
            : base($"'{claimType}' claim not found.")
        {
            ClaimType = claimType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="claimType"></param>
        /// <param name="message"></param>
        public ClaimNotFoundException(string claimType, string message) 
            : base(message)
        {
            ClaimType=claimType;
        }

        /// <summary>
        /// 
        /// </summary>
        public string ClaimType { get; }
    }
}
