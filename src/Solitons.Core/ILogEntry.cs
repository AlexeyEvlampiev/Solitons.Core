using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Solitons
{
    public interface ILogEntry  
    {
        LogLevel Level { get; }

        string Message { get; }

        DateTimeOffset Created { get; } 

        string Details { get; }

        IEnumerable<string> Tags { get; }

        IEnumerable<string> Properties { get; }

        string GetProperty(string name);

        public IReadOnlyDictionary<string, string> GetProperties()
        {
            return Properties.Any()
                ? new ReadOnlyDictionary<string, string>(Properties
                    .Select(name=> KeyValuePair.Create(name, GetProperty(name)))
                    .ToDictionary(StringComparer.Ordinal))
                : EmptyPropertiesDictionary;
        }

        [DebuggerStepThrough]
        public ILogEntry AsLogEntry()
        {
            if (this is RelayLogEntry entry) return entry;
            return new RelayLogEntry(this);
        }


        private static readonly IReadOnlyDictionary<string, string> EmptyPropertiesDictionary =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
    }
}
