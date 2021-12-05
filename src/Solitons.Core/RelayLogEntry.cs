using System;
using System.Collections.Generic;
using System.Linq;

namespace Solitons
{
    sealed class RelayLogEntry : ILogEntry
    {
        private readonly ILogEntry _innerEntry;

        public RelayLogEntry(ILogEntry innerEntry)
        {
            _innerEntry = innerEntry ?? throw new ArgumentNullException(nameof(innerEntry));
        }

        public LogLevel Level => _innerEntry.Level;

        public string Message => _innerEntry.Message;

        public DateTimeOffset Created => _innerEntry.Created;

        public string Details => _innerEntry.Details;

        public IEnumerable<string> Tags => _innerEntry.Tags ?? Enumerable.Empty<string>();
        public IEnumerable<string> Properties => _innerEntry.Properties;

        public string GetProperty(string name)
        {
            return _innerEntry.GetProperty(name);
        }


        public override string ToString() => _innerEntry.ToString();

        public override bool Equals(object? obj) => _innerEntry.Equals(obj);

        public override int GetHashCode() => _innerEntry.GetHashCode();
    }
}
