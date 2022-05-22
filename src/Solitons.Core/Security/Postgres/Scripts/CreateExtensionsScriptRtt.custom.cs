using System.Collections.Generic;

namespace Solitons.Security.Postgres.Scripts
{
    public partial class CreateExtensionsScriptRtt
    {
        private readonly PgNamingRules _namingRules;
        private readonly PgExtensionListBuilder _extensions;

        internal CreateExtensionsScriptRtt(string databaseName, PgExtensionListBuilder extensions, PgNamingRules namingRules)
        {
            DatabaseName = databaseName
                .ThrowIfNullOrWhiteSpaceArgument(nameof(databaseName));
            _extensions = extensions.ThrowIfNullArgument(nameof(extensions));
            _namingRules = namingRules.ThrowIfNullArgument(nameof(namingRules));
        }


        private string GetSchema(string extension) => _extensions.GetExtensionsSchema(extension);

        internal string DatabaseName { get; }
        internal IEnumerable<string> Schemas => _extensions.Schemas;

        internal IEnumerable<string> Extensions => _extensions.Extensions;

        internal string DbAdminRole => _namingRules.BuildRoleFullName(DatabaseName, "admin");

    }
}
