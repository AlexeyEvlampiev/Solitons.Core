using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Solitons.Data.Postgres
{
    public partial class CreateExtensionsScriptRtt
    {
        private readonly Dictionary<string, string> _schemasByExtension;

        private CreateExtensionsScriptRtt(string databaseName, Dictionary<string, string> schemasByExtension)
        {
            _schemasByExtension = schemasByExtension;
            DatabaseName = databaseName;
        }

        public sealed class ExtensionsAppender
        {
            private readonly Dictionary<string, string> _extensionsBySchema;

            internal ExtensionsAppender(Dictionary<string, string> extensionsBySchema)
            {
                _extensionsBySchema = extensionsBySchema;
            }

            public ExtensionsAppender With(string extension)
            {
                _extensionsBySchema.Add(extension, "extensions");
                return this;
            }
        }


        private string GetSchema(string extension)
        {
            return _schemasByExtension[extension];
        }

        public string DatabaseName { get; }
        public IEnumerable<string> Schemas => _schemasByExtension.Values.Distinct(StringComparer.Ordinal);

        public IEnumerable<string> Extensions => _schemasByExtension.Keys;
        public string DbAdminRole => $"{DatabaseName}_admin";


        public static void Execute(IDbConnection connection, string databaseName, Action<ExtensionsAppender> config)
        {
            if (connection == null) throw new ArgumentNullException(nameof(connection));
            if (databaseName == null) throw new ArgumentNullException(nameof(databaseName));
            if (config == null) throw new ArgumentNullException(nameof(config));
            var extensionsBySchema = new Dictionary<string, string>();
            var extensions = new ExtensionsAppender(extensionsBySchema);
            config.Invoke(extensions);
            using var command = connection.CreateCommand();
            command.CommandText = new CreateExtensionsScriptRtt(databaseName, extensionsBySchema);
            command.ExecuteNonQuery();
        }


    }
}
