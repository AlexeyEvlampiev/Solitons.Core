using Solitons.Security.Postgres.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Security.Postgres
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="PgRoleAttribute"/>
    /// <seealso cref="IPgRoleAttribute"/>
    public sealed class BasicPgRoleAttribute : PgRoleAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="options"></param>
        public BasicPgRoleAttribute(string roleName, CreateRoleOptions options) 
            : base(roleName, options)
        {
        }
    }
}
