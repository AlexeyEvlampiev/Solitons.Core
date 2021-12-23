using System;
using System.Collections.Generic;
using System.Data;

namespace Solitons.Security.Postgres.Scripts
{
    public partial class CreateExtensionsScriptRtt
    {
        private readonly PgExtensionListBuilder _extensions;

        private CreateExtensionsScriptRtt(string databaseName, PgExtensionListBuilder extensions)
        {
            DatabaseName = databaseName
                .ThrowIfNullOrWhiteSpaceArgument(nameof(databaseName));
            _extensions = extensions.ThrowIfNullArgument(nameof(extensions));
        }


        private string GetSchema(string extension) => _extensions.GetExtensionsSchema(extension);

        public string DatabaseName { get; }
        public IEnumerable<string> Schemas => _extensions.Schemas;

        public IEnumerable<string> Extensions => _extensions.Extensions;

        public string DbAdminRole => $"{DatabaseName}_admin";





        internal static void Execute(IDbConnection connection, string databaseName, PgExtensionListBuilder extensions)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (databaseName == null) throw new ArgumentNullException(nameof(databaseName));
            if (extensions == null) throw new ArgumentNullException(nameof(extensions));
            using var command = connection.CreateCommand();
            command.CommandText = new CreateExtensionsScriptRtt(databaseName, extensions);
            command.ExecuteNonQuery();
        }


    }
}
