﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Solitons.Security.Postgres.Scripts
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class DropRolesByPrefixScriptRtt : Solitons.Text.Sql.PgRuntimeTextTemplate
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
            this.Write("DO\r\n$$\r\nDECLARE\r\n    _role text;\r\nBEGIN\r\nFOR _role  IN\r\n    SELECT rolname FROM p" +
                    "g_catalog.pg_roles WHERE  rolname ~ \'^");
            this.Write(this.ToStringHelper.ToStringWithCulture(RolePrefix));
            this.Write("\'\r\nLOOP\r\n    RAISE NOTICE \'DROP ROLE %\', _role;\r\n    EXECUTE \'DROP ROLE \' || _rol" +
                    "e;\r\nEND LOOP;\r\nEND;\r\n$$;");
            return this.GenerationEnvironment.ToString();
        }
    }
}
