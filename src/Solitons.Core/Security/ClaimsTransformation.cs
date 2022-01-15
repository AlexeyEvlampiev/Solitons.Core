using System.Security.Claims;
using System.Threading.Tasks;

namespace Solitons.Security
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ClaimsTransformation
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        protected abstract Task AlterAsync(ClaimsPrincipal principal);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            principal = principal?.Clone() ?? new ClaimsPrincipal();
            await AlterAsync(principal);
            return principal;
        }

    }
}
