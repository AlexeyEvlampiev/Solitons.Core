using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Security.Postgres
{
    internal class PgExtensionListBuilder : IPgExtensionListBuilder
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly Dictionary<string, string> _schemaByExtension = new();
        public IEnumerable<string> Schemas => _schemaByExtension.Values.Distinct(StringComparer.OrdinalIgnoreCase);
        public IEnumerable<string> Extensions => _schemaByExtension.Keys;

        [DebuggerNonUserCode]
        public IPgExtensionListBuilder With(string extension, string? schema = null)
        {
            extension = extension
                .ThrowIfNullOrWhiteSpaceArgument(nameof(extension))
                .Trim();
            schema = schema
                .DefaultIfNullOrWhiteSpace("extensions")
                .Trim();
            _schemaByExtension.Add(extension, schema);
            return this;
        }

        [DebuggerNonUserCode]
        public string GetExtensionsSchema(string extension) => _schemaByExtension[extension];

        public override string ToString() => _schemaByExtension.Keys.Join();
    }
}
