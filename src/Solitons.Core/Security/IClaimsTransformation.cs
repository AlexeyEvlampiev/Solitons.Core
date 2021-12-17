using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Security
{
    /// <summary>
    /// 
    /// </summary>
    public interface IClaimsTransformation
    {
        Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal);
    }
}
