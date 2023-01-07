using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Solitons.Collections;

namespace Solitons.Diagnostics
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface ILogStringBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        ILogStringBuilder WithProperty(string name, object value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        ILogStringBuilder WithTags(string tag);


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string ToString();
    }

    public partial interface ILogStringBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag0"></param>
        /// <param name="tag1"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public sealed ILogStringBuilder WithTags(string tag0, string tag1)
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
        public sealed ILogStringBuilder WithTags(string tag0, string tag1, string tag2)
        {
            return WithTags(tag0)
                .WithTags(tag1)
                .WithTags(tag2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual ILogStringBuilder WithProperties(IEnumerable<KeyValuePair<string, object>> properties)
        {
            foreach (var property in properties)
            {
                WithProperty(property.Key, property.Value);
            }
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public sealed ILogStringBuilder WithProperties(
            Action<FluentDictionary<string, object>> config)
        {
            var properties = new Dictionary<string, object>();
            var fluentProperties = FluentDictionary.Create(properties);
            config.Invoke(fluentProperties);
            return WithProperties(properties);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tags"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public sealed ILogStringBuilder WithTags(params string[] tags)
        {
            ILogStringBuilder entry = this;
            foreach (var tag in tags)
            {
                entry = entry.WithTags(tag);
            }

            return entry;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual ILogStringBuilder WithProperty(string name, bool value) => this.WithProperty(name, (object)value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual ILogStringBuilder WithProperty(string name, TimeSpan value)
        {
            WithProperty(name, (object)XmlConvert.ToString(value));
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual ILogStringBuilder WithProperty(string name, DateTime value) => this.WithProperty(name, (object)value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual ILogStringBuilder WithProperty(string name, DateTimeOffset value) => this.WithProperty(name, (object)value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual ILogStringBuilder WithProperty(string name, Guid value) => this.WithProperty(name, (object)value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual ILogStringBuilder WithProperty(string name, int value) => this.WithProperty(name, (object)value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual ILogStringBuilder WithProperty(string name, uint value) => this.WithProperty(name, (object)value);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual ILogStringBuilder WithProperty(string name, short value) => this.WithProperty(name, (object)value);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual ILogStringBuilder WithProperty(string name, ushort value) => this.WithProperty(name, (object)value);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual ILogStringBuilder WithProperty(string name, double value) => this.WithProperty(name, (object)value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual ILogStringBuilder WithProperty(string name, float value) => this.WithProperty(name, (object)value);
    }
}
