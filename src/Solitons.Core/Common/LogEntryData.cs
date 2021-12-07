using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Solitons.Collections;

namespace Solitons.Common
{
    /// <summary>
    /// 
    /// </summary>
    [Guid("d6dcaaac-c0b7-42fd-b5c9-b879c08accc2"), DataTransferObject]
    [XmlRoot("LogEntry")]
    public sealed class LogEntryData : BasicJsonDataTransferObject,
        ILogEntry
    {
        /// <summary>
        /// 
        /// </summary>
        public LogEntryData()
        {
            Created = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
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

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("level"), XmlAttribute("Level")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LogLevel Level { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("message"), XmlAttribute("Message")]
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("created"), XmlIgnore]
        public DateTimeOffset Created { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("details"), XmlElement("Details")]
        public string Details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("tags")]
        [XmlArray("Tags"), XmlArrayItem("Add")]
        public List<string> Tags { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("properties"), XmlIgnore]
        public IDictionary<string, string> Properties { get; set; }



        string ILogEntry.GetProperty(string name) => Properties[name];

        IEnumerable<string> ILogEntry.Tags => Tags ?? Enumerable.Empty<string>();

        IEnumerable<string> ILogEntry.Properties => Properties?.Keys ?? Enumerable.Empty<string>();

        public sealed class PropertyCollection : DictionaryProxy<string, string>
        {
            public PropertyCollection()
            {
                
            }
            public PropertyCollection(IDictionary<string, string> value) : base(value)
            {
      
            }
        }


    }
}
