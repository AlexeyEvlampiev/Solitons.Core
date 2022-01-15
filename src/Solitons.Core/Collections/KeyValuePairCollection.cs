using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class KeyValuePairCollection<TKey, TValue> : Collection<KeyValuePair<TKey, TValue>>
    {
        #region ctor

        [DebuggerStepThrough]
        public KeyValuePairCollection()
        {
        }

        [DebuggerStepThrough]
        public KeyValuePairCollection(Append init)
        {
            if (init == null) throw new ArgumentNullException(nameof(init));
            init.Invoke(new Builder(this));
        }

        [DebuggerStepThrough]
        public KeyValuePairCollection(params KeyValuePair<TKey, TValue>[] items) : this(items.AsEnumerable())
        {
        }

        [DebuggerStepThrough]
        public KeyValuePairCollection(IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            source ??= Enumerable.Empty<KeyValuePair<TKey, TValue>>();
            foreach (var pair in source)
            {
                Add(pair);
            }
        }

        public delegate void Append(IBuilder builder);

        public interface IBuilder
        {
            IBuilder With(TKey key, TValue value);
        }

        sealed class Builder : IBuilder
        {
            private readonly KeyValuePairCollection<TKey, TValue> _target;

            public Builder(KeyValuePairCollection<TKey, TValue> target)
            {
                _target = target ?? throw new ArgumentNullException(nameof(target));
            }
            [DebuggerStepThrough]
            public IBuilder With(TKey key, TValue value)
            {
                _target.Add(key, value);
                return this;
            }
        }
        

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(TKey key, TValue value) => Add(KeyValuePair.Create(key, value));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public void Remove(IEnumerable<KeyValuePair<TKey, TValue>> items) => 
            items?.ForEach(item=> Remove(item));
    }

    /// <summary>
    /// 
    /// </summary>
    public class KeyValuePairCollection : KeyValuePairCollection<string, string>
    {
        /// <summary>
        /// 
        /// </summary>
        [DebuggerStepThrough]
        public KeyValuePairCollection()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="init"></param>
        [DebuggerStepThrough]
        public KeyValuePairCollection(Append init) : base(init)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        [DebuggerStepThrough]
        public KeyValuePairCollection(params KeyValuePair<string, string>[] items) : base(items)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        [DebuggerStepThrough]
        public KeyValuePairCollection(IEnumerable<KeyValuePair<string, string>> source) : base(source)
        {

        }

    }
}
