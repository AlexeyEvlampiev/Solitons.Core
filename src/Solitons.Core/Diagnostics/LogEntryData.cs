using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Solitons.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    [Guid("d6dcaaac-c0b7-42fd-b5c9-b879c08accc2")]    
    public sealed class LogEntryData : BasicJsonDataTransferObject,
        ILogEntry
    {
        /// <summary>
        /// 
        /// </summary>
        public LogEntryData()
        {
            Message = "?";
            Created = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        public LogEntryData(ILogEntry entry)
        {
            if (entry == null) throw new ArgumentNullException(nameof(entry));
            Created = entry.Created;
            Level = entry.Level;
            Message = entry.Message;
            Details = entry.Details;
            Tags = entry.Tags?
                .Distinct(StringComparer.Ordinal)?
                .ToList();
            Properties = entry.PropertyNames?
                .ToDictionary(
                    name => name,
                    entry.GetProperty,
                    StringComparer.Ordinal);
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("level")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LogLevel Level { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("created")]
        public DateTimeOffset Created { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("details")]
        public string? Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("tags")]
        public List<string>? Tags { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("properties")]
        public Dictionary<string, string?>? Properties { get; set; }



        string? ILogEntry.GetProperty(string name) => Properties?.TryGetValue(name, out var value) == true ? value : null;

        IEnumerable<string> ILogEntry.Tags => Tags as IEnumerable<string> ?? Enumerable.Empty<string>();

        IEnumerable<string> ILogEntry.PropertyNames => Properties?.Keys ?? Enumerable.Empty<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static LogEntryData? Parse(string json) => JsonSerializer.Deserialize<LogEntryData>(json);
    }
}
