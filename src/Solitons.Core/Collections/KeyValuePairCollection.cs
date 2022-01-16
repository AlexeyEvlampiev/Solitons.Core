using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public static class KeyValuePairCollection
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static KeyValuePairCollection<TKey, TValue> Create<TKey, TValue>() => new();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static KeyValuePairCollection<string,TValue> Create<TValue>() => new();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static KeyValuePairCollection<string, string> Create() => new();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static KeyValuePairCollection<TKey, TValue> Add<TKey, TValue>(TKey key, TValue value)
        {
            var result = Create<TKey,TValue>();
            result.Add(KeyValuePair.Create(key, value));
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static KeyValuePairCollection<TKey, TValue> Create<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> collection) => 
            new(collection.ThrowIfNullArgument(nameof(collection)).ToList());

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static KeyValuePairCollection<TKey, TValue> Wrap<TKey, TValue>(
            ICollection<KeyValuePair<TKey, TValue>> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            return collection is KeyValuePairCollection<TKey, TValue> other
                ? other
                : new KeyValuePairCollection<TKey, TValue>(collection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static KeyValuePairCollection<TKey, TValue> WrapOrCreate<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            return collection is KeyValuePairCollection<TKey, TValue> other
                ? other
                : new KeyValuePairCollection<TKey, TValue>(collection);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class KeyValuePairCollection<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly ICollection<KeyValuePair<TKey, TValue>> _collection;

        [DebuggerStepThrough]
        internal KeyValuePairCollection()
        {
            _collection = new List<KeyValuePair<TKey, TValue>>();
        }

        [DebuggerStepThrough]
        internal KeyValuePairCollection(IEnumerable<KeyValuePair<TKey, TValue>> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            if (enumerable is KeyValuePairCollection<TKey, TValue> other)
            {
                _collection = other._collection;
            }
            else if(enumerable is ICollection<KeyValuePair<TKey, TValue>> collection)
            {
                _collection = collection;
            }
            else
            {
                _collection = new List<KeyValuePair<TKey, TValue>>(enumerable);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _collection.GetEnumerator();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_collection).GetEnumerator();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        [DebuggerStepThrough]
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item) => _collection.Add(item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public KeyValuePairCollection<TKey, TValue> Add(KeyValuePair<TKey, TValue> item)
        {
            _collection.Add(item);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        [DebuggerStepThrough]
        void ICollection<KeyValuePair<TKey, TValue>>.Clear() => _collection.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public KeyValuePairCollection<TKey, TValue> Clear()
        {
            _collection.Clear();
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool Contains(KeyValuePair<TKey, TValue> item) => _collection.Contains(item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        [DebuggerStepThrough]
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool Remove(KeyValuePair<TKey, TValue> item) => _collection.Remove(item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="removed"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public KeyValuePairCollection<TKey, TValue> Remove(KeyValuePair<TKey, TValue> item, out bool removed)
        {
            removed = _collection.Remove(item);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count => _collection.Count;

        /// <summary>
        /// 
        /// </summary>

        public bool IsReadOnly => _collection.IsReadOnly;
    }

}
