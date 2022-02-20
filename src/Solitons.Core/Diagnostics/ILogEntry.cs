using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace Solitons.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface ILogEntry 
    {
        /// <summary>
        /// 
        /// </summary>
        LogLevel Level { get; }

        /// <summary>
        /// 
        /// </summary>
        string Message { get; }

        /// <summary>
        /// 
        /// </summary>
        DateTimeOffset Created { get; } 

        /// <summary>
        /// 
        /// </summary>
        string? Details { get; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<string> Tags { get; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<string> PropertyNames { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string? GetProperty(string name);
    }

    public partial interface ILogEntry
    {
        private static readonly IReadOnlyDictionary<string, string> EmptyPropertiesDictionary =
            new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IReadOnlyDictionary<string, string> GetProperties()
        {
            return PropertyNames.Any()
                ? new ReadOnlyDictionary<string, string>(PropertyNames
                    .Select(name => KeyValuePair.Create(name, GetProperty(name)!))
                    .ToDictionary(StringComparer.Ordinal))
                : EmptyPropertiesDictionary;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public LogEntryData AsDataTransferObject()
        {
            return this is LogEntryData data 
                ? data 
                : new LogEntryData(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="indented"></param>
        /// <returns></returns>
        public string ToJsonString(bool indented = false)
        {
            return JsonSerializer.Serialize(
                this.AsDataTransferObject(),
                new JsonSerializerOptions() { WriteIndented = indented });
        }
    }
}
