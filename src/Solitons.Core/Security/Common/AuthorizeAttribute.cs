using System;
using System.Diagnostics;
using System.Security.Claims;

namespace Solitons.Security.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AuthorizeAttribute : Attribute, IAuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected abstract bool IsAuthorized(ClaimsPrincipal user);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        bool IAuthorizeAttribute.IsAuthorized(ClaimsPrincipal user) => user is not null && IsAuthorized(user);
    }
}
