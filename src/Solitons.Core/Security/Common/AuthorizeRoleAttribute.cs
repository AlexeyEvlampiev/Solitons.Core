using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Solitons.Security.Common
{
    public abstract class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        private readonly string[] _authorizedRoles;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authorizedRoles"></param>
        protected AuthorizeRoleAttribute(IEnumerable<string> authorizedRoles)
        {
            _authorizedRoles = authorizedRoles
                .ThrowIfNullArgument(nameof(authorizedRoles))
                .ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        protected override bool IsAuthorized(ClaimsPrincipal user)
        {
            return user.Claims
                .Where(c=> c.Type == ClaimTypes.Role)
                .Any(c=> _authorizedRoles.Contains(c.Value, StringComparer.OrdinalIgnoreCase));
        }
    }

}
