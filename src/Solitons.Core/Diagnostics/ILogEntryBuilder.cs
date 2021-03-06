using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solitons.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface ILogEntryBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        ILogEntryBuilder WithProperty(string name, string value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        ILogEntryBuilder WithTags(string tag);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag0"></param>
        /// <param name="tag1"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public ILogEntryBuilder WithTags(string tag0, string tag1)
        {
            return WithTags(tag0)
                .WithTags(tag1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag0"></param>
        /// <param name="tag1"></param>
        /// <param name="tag2"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public ILogEntryBuilder WithTags(string tag0, string tag1, string tag2)
        {
            return WithTags(tag0)
                .WithTags(tag1)
                .WithTags(tag2);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>

        [DebuggerStepThrough]
        public ILogEntryBuilder WithTags(params string[] tags)
        {
            ILogEntryBuilder entry = this;
            foreach (var tag in tags)
            {
                entry = entry.WithTags(tag);
            }

            return entry;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        ILogEntryBuilder WithProperties(IEnumerable<KeyValuePair<string, string>>? properties);
    }

    public partial interface ILogEntryBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public ILogEntryBuilder WithProperties(Action<IDictionary<string, string>> config)
        {
            var properties = new Dictionary<string, string>();
            config.Invoke(properties);
            return WithProperties(properties);
        }
    }
}
