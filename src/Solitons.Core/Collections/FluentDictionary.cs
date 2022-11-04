using System.Collections.Generic;
using System.Diagnostics;
using Solitons.Collections.Common;

namespace Solitons.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public sealed class FluentDictionary<TKey, TValue> : DictionaryProxy<TKey, TValue>
    {
        internal FluentDictionary(IDictionary<TKey, TValue> innerDictionary) : base(innerDictionary)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public new FluentDictionary<TKey, TValue> Add(TKey key, TValue value)
        {
            ((IDictionary<TKey, TValue>)this).Add(key, value);
            return this;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class FluentDictionary
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static FluentDictionary<TKey, TValue> Create<TKey, TValue>() where TKey : notnull => 
            new (new Dictionary<TKey, TValue>());

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="comparer"></param>
        /// <returns></returns>
        public static FluentDictionary<TKey, TValue> Create<TKey, TValue>(IEqualityComparer<TKey> comparer) where TKey : notnull =>
            new(new Dictionary<TKey, TValue>(comparer));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="innerDictionary"></param>
        /// <returns></returns>
        public static FluentDictionary<TKey, TValue> Create<TKey, TValue>(IDictionary<TKey, TValue> innerDictionary) where TKey : notnull =>
            new(innerDictionary);
    }
}
