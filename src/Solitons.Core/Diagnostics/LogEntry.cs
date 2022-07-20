using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;

namespace Solitons.Diagnostics
{
    sealed class LogEntry : ILogEntry, ILogEntryBuilder
    {
        private HashSet<string>? _tags;
        private Dictionary<string, string>? _properties;


        public LogEntry(LogLevel level, string message, string? details)
        {
            Level = level;
            Message = message;
            Detail = details;
        }

        public LogLevel Level { get; }

        public string Message { get; }

        public string? Detail { get; }

        public DateTimeOffset Created { get; } = DateTimeOffset.UtcNow;

        public IEnumerable<string> Tags => _tags  ?? Enumerable.Empty<string>();

        public IEnumerable<string> PropertyNames => _properties?.Keys ?? Enumerable.Empty<string>();

        public string? GetProperty(string name) => _properties != null
            ? _properties[name]
            : throw new KeyNotFoundException($"{nameof(name)} property not found.");


        public ILogEntryBuilder WithTags(string tag)
        {
            LazyInitializer.EnsureInitialized(ref _tags, () => new HashSet<string>(StringComparer.Ordinal));
            _tags.Add(tag);
            return this;
        }

        public ILogEntryBuilder WithProperty(string name, string value)
        {
            LazyInitializer.EnsureInitialized(ref _properties,()=> new Dictionary<string, string>(StringComparer.Ordinal));
            _properties[name] = value;
            return this;
        }


        public ILogEntryBuilder WithProperties(IEnumerable<KeyValuePair<string, string>>? properties)
        {
            if (properties is null) return this;

            foreach (var keyValuePair in properties)
            {
                LazyInitializer.EnsureInitialized(ref _properties, () => new Dictionary<string, string>(StringComparer.Ordinal));
                _properties[keyValuePair.Key] = keyValuePair.Value;
            }
            return this;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(new LogEntryData(this));
        }
    }
}
