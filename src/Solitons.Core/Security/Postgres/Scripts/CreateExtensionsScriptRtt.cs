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
    public partial class CreateExtensionsScriptRtt : Solitons.Text.Sql.PgRuntimeTextTemplate
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public override string TransformText()
        {
 foreach(var schema in Schemas){ 
            this.Write("  \r\nCREATE SCHEMA IF NOT EXISTS ");
            this.Write(this.ToStringHelper.ToStringWithCulture(schema));
            this.Write(" AUTHORIZATION ");
            this.Write(this.ToStringHelper.ToStringWithCulture(DbAdminRole));
            this.Write("; ");
 } 
            this.Write(" \r\n\r\n");
 foreach(var extension in this.Extensions){ 
            this.Write("  \r\nCREATE EXTENSION IF NOT EXISTS ");
            this.Write(this.ToStringHelper.ToStringWithCulture(extension));
            this.Write(" SCHEMA ");
            this.Write(this.ToStringHelper.ToStringWithCulture(GetSchema(extension)));
            this.Write("; ");
 } 
            this.Write(" ");
            return this.GenerationEnvironment.ToString();
        }
    }
}