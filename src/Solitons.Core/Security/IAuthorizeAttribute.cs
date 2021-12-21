using System.Security.Claims;

namespace Solitons.Security
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        bool IsAuthorized(ClaimsPrincipal user);
    }
}
