using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Solitons.Common
{
    [Guid("d6dcaaac-c0b7-42fd-b5c9-b879c08accc2"), DataTransferObject]
    public sealed class LogEntryData : ILogEntry
    {
        public LogEntryData()
        {
        }

        public LogEntryData(ILogEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));
            Level = entry.Level;
            Message = entry.Message;
            Details = entry.Details;
            Tags = entry.Tags?
                .Distinct(StringComparer.Ordinal)?
                .ToList();
            Properties = entry.Properties?
                .ToDictionary(
                    name => name,
                    entry.GetProperty,
                    StringComparer.Ordinal);
        }

        [JsonPropertyName("level")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LogLevel Level { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("created")]
        public DateTimeOffset Created { get; set; }

        [JsonPropertyName("details")]
        public string Details { get; set; }

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, string> Properties { get; set; }

        string ILogEntry.GetProperty(string name) => Properties[name];

        IEnumerable<string> ILogEntry.Tags => Tags ?? Enumerable.Empty<string>();

        IEnumerable<string> ILogEntry.Properties => Properties?.Keys ?? Enumerable.Empty<string>();

    }
}
