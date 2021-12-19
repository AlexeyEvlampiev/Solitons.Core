namespace Solitons.Security.Postgres
{
	using System;

	/// <summary>
    /// CREATE ROLE options
    /// </summary>
	[Flags]
    public enum CreateRoleOptions
    {
		/// <summary>
        /// 
        /// </summary>
        None = 0, 

		/// <summary>
        /// Determine whether the new role is a 'superuser'
        /// </summary>
		Superuser = 2, 

		/// <summary>
        /// Defines a role's ability to create databases. If CREATEDB is specified, the role being defined will be allowed to create new databases.
        /// </summary>
		CreateDb = 4, 

		/// <summary>
        /// Determine whether a role will be permitted to create new roles (that is, execute CREATE ROLE). A role with CREATEROLE privilege can also alter and drop other roles.
        /// </summary>
		CreateRole = 8, 

		/// <summary>
        /// Determine whether a role “inherits” the privileges of roles it is a member of. A role with the INHERIT attribute can automatically use whatever database privileges have been granted to all roles it is directly or indirectly a member of.
        /// </summary>
		Inherit = 16, 

		/// <summary>
        /// Determine whether a role is allowed to log in; that is, whether the role can be given as the initial session authorization name during client connection. A role having the LOGIN attribute can be thought of as a user. Roles without this attribute are useful for managing database privileges, but are not users in the usual sense of the word
        /// </summary>
		Login = 32
    }
}
