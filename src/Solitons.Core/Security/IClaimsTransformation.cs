using System.Security.Claims;
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
