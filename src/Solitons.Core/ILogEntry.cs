using Solitons.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Solitons
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
        string Details { get; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<string> Tags { get; }

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<string> Properties { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string GetProperty(string name);
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
            return Properties.Any()
                ? new ReadOnlyDictionary<string, string>(Properties
                    .Select(name => KeyValuePair.Create(name, GetProperty(name)))
                    .ToDictionary(StringComparer.Ordinal))
                : EmptyPropertiesDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public ILogEntry AsLogEntry() => this is LogEntryProxy entry ? entry : new LogEntryProxy(this);
        
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
    }
}
