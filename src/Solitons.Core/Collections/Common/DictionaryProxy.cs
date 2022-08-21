using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solitons.Collections.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class DictionaryProxy<TKey, TValue> : IDictionary<TKey, TValue>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IDictionary<TKey, TValue> _innerDictionary;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerDictionary"></param>
        [DebuggerStepThrough]
        protected DictionaryProxy(IDictionary<TKey, TValue> innerDictionary)
        {
            _innerDictionary = innerDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_innerDictionary).GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        [DebuggerStepThrough]
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _innerDictionary.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        [DebuggerStepThrough]
        public void Clear()
        {
            _innerDictionary.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _innerDictionary.Contains(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        [DebuggerStepThrough]
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _innerDictionary.CopyTo(array, arrayIndex);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _innerDictionary.Remove(item);
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count => _innerDictionary.Count;

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly => _innerDictionary.IsReadOnly;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [DebuggerStepThrough]
        public void Add(TKey key, TValue value)
        {
            _innerDictionary.Add(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool ContainsKey(TKey key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool Remove(TKey key)
        {
            return _innerDictionary.Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool TryGetValue(TKey key, out TValue value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            [DebuggerStepThrough]
            get => _innerDictionary[key];
            [DebuggerStepThrough]
            set => _innerDictionary[key] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<TKey> Keys => _innerDictionary.Keys;

        /// <summary>
        /// 
        /// </summary>
        public ICollection<TValue> Values => _innerDictionary.Values;
    }
}
