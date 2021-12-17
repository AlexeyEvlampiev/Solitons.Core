using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Data.Postgres
{
    public partial class CreateRoleCommandRtt
    {
        private readonly CreateRoleOptions _options;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public CreateRoleCommandRtt(string role, CreateRoleOptions options)
        {
            Role = role.ThrowIfNullOrWhiteSpaceArgument(nameof(role));
            _options = options;
            if (_options.HasFlag(CreateRoleOptions.Login))
                throw new InvalidOperationException($"{CreateRoleOptions.Login} option can be used only in combination with a password.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="password"></param>
        /// <param name="options"></param>
        /// <param name="connectionLimit"></param>
        /// <exception cref="InvalidOperationException"></exception>
        public CreateRoleCommandRtt(string role, string password, CreateRoleOptions options, int connectionLimit = -1)
        {
            Role = role.ThrowIfNullOrWhiteSpaceArgument(nameof(role));
            Pwd = password.ThrowIfNullOrWhiteSpaceArgument(nameof(password));
            _options = options |= CreateRoleOptions.Login;
            ConnectionLimit = connectionLimit;
        }

        public int ConnectionLimit { get; }
        private string Role { get; }
        private string Pwd { get; }

        private string Login => _options.HasFlag(CreateRoleOptions.Login) ? "LOGIN" : "NOLOGIN";
        private string Superuser => _options.HasFlag(CreateRoleOptions.Superuser) ? "SUPERUSER" : "NOSUPERUSER";
        private string CreateDb => _options.HasFlag(CreateRoleOptions.CreateDb) ? "CREATEDB" : "NOCREATEDB";
        private string CreateRole => _options.HasFlag(CreateRoleOptions.CreateRole) ? "CREATEROLE" : "NOCREATEROLE";
        private string Inherit => _options.HasFlag(CreateRoleOptions.Inherit) ? "INHERIT" : "NOINHERIT";

        private bool WithLogin => _options.HasFlag(CreateRoleOptions.Login);
    }
}
