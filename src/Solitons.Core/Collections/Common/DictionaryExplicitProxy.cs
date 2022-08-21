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
    public abstract class DictionaryExplicitProxy<TKey, TValue> : IDictionary<TKey, TValue>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IDictionary<TKey, TValue> _innerDictionary;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerDictionary"></param>
        [DebuggerStepThrough]
        protected DictionaryExplicitProxy(IDictionary<TKey, TValue> innerDictionary)
        {
            _innerDictionary = innerDictionary;
        }

        [DebuggerStepThrough]
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return _innerDictionary.GetEnumerator();
        }

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_innerDictionary).GetEnumerator();
        }

        [DebuggerStepThrough]
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            _innerDictionary.Add(item);
        }

        [DebuggerStepThrough]
        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            _innerDictionary.Clear();
        }

        [DebuggerStepThrough]
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return _innerDictionary.Contains(item);
        }

        [DebuggerStepThrough]
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _innerDictionary.CopyTo(array, arrayIndex);
        }

        [DebuggerStepThrough]
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return _innerDictionary.Remove(item);
        }

        int ICollection<KeyValuePair<TKey, TValue>>.Count => _innerDictionary.Count;


        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => _innerDictionary.IsReadOnly;

        [DebuggerStepThrough]
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            _innerDictionary.Add(key, value);
        }

        [DebuggerStepThrough]
        bool IDictionary<TKey, TValue>.ContainsKey(TKey key)
        {
            return _innerDictionary.ContainsKey(key);
        }

        [DebuggerStepThrough]
        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            return _innerDictionary.Remove(key);
        }

        [DebuggerStepThrough]
        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            return _innerDictionary.TryGetValue(key, out value);
        }

        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            [DebuggerStepThrough]
            get => _innerDictionary[key];
            [DebuggerStepThrough]
            set => _innerDictionary[key] = value;
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => _innerDictionary.Keys;

        ICollection<TValue> IDictionary<TKey, TValue>.Values => _innerDictionary.Values;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public override int GetHashCode() => _innerDictionary.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(obj, null)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (_innerDictionary.Equals(obj)) return true;
            if (obj is DictionaryExplicitProxy<TKey, TValue> other)
            {
                return _innerDictionary.Equals(other._innerDictionary);
            }
            return false;
        }
    }
}
