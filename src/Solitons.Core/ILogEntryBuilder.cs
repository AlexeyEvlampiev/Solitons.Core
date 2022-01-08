using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solitons
{
    public partial interface ILogEntryBuilder
    {
        ILogEntryBuilder WithTag(string tag);

        ILogEntryBuilder WithProperty(string name, string value);

        ILogEntryBuilder WithDetails(string details);


        [DebuggerStepThrough]
        public ILogEntryBuilder WithTags(string tag0, string tag1)
        {
            return WithTag(tag0)
                .WithTag(tag1);
        }

        [DebuggerStepThrough]
        public ILogEntryBuilder WithTags(string tag0, string tag1, string tag2)
        {
            return WithTag(tag0)
                .WithTag(tag1)
                .WithTag(tag2);
        }

        [DebuggerStepThrough]
        public ILogEntryBuilder WithTags(params string[] tags)
        {
            if (tags is null) return this;
            ILogEntryBuilder entry = this;
            for (int i = 0; i < tags.Length; ++i)
            {
                entry = entry.WithTag(tags[i]);
            }

            return entry;
        }

        
        ILogEntryBuilder WithProperties(IEnumerable<KeyValuePair<string, string>> properties);
    }

    public partial interface ILogEntryBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public ILogEntryBuilder WithTag(Guid correlationId) =>
            this.WithTag(correlationId.ToString());
    }
}
