using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Solitons.Common
{
    sealed class LogEntry : ILogEntry, ILogEntryBuilder
    {
        private HashSet<string> _tags;
        private Dictionary<string, string> _properties;
        private StringBuilder _details;

        public LogLevel Level { get; set; }

        public string Message { get; set; }

        public DateTimeOffset Created { get; } = DateTimeOffset.UtcNow;

        public string Details { get; set; }

        public IEnumerable<string> Tags => _tags?.AsEnumerable() ?? Enumerable.Empty<string>();

        public IEnumerable<string> Properties => _properties?.Keys ?? Enumerable.Empty<string>();

        public string GetProperty(string name) => _properties != null
            ? _properties[name]
            : throw new KeyNotFoundException($"{nameof(name)} property not found.");


        public ILogEntryBuilder WithTag(string tag)
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

        public ILogEntryBuilder WithDetails(string details)
        {
            if (details.IsNullOrWhiteSpace()) return this;
            LazyInitializer.EnsureInitialized(ref _details, () => new StringBuilder());
            _details
                .AppendLine()
                .Append(details);
            return this;
        }

        public ILogEntryBuilder WithProperties(IEnumerable<KeyValuePair<string, string>> properties)
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
