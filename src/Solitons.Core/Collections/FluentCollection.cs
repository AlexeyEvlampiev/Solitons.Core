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
    /// <typeparam name="T"></typeparam>
    public sealed class FluentCollection<T> : ICollection<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly ICollection<T> _collection;

        /// <summary>
        /// 
        /// </summary>
        [DebuggerNonUserCode]
        public FluentCollection() : this(new List<T>())
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode]
        public FluentCollection(ICollection<T> collection)
        {
            _collection = collection ?? throw new ArgumentNullException(nameof(collection));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumerable"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode]
        public FluentCollection(IEnumerable<T> enumerable)
        {
            if (enumerable == null) throw new ArgumentNullException(nameof(enumerable));
            _collection = enumerable is ICollection<T> collection
                ? collection
                : enumerable.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IEnumerator<T> GetEnumerator() => _collection.GetEnumerator();

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_collection).GetEnumerator();

        [DebuggerStepThrough]
        void ICollection<T>.Add(T item) => _collection.Add(item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public FluentCollection<T> Add(T item)
        {
            _collection.Add(item);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public FluentCollection<T> AddRange(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            foreach (var item in collection)
            {
                _collection.Add(item);
            }

            return this;
        }

        [DebuggerStepThrough]
        void ICollection<T>.Clear() => _collection.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public FluentCollection<T> Clear()
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
        public bool Contains(T item) => _collection.Contains(item);


        [DebuggerStepThrough]
        void ICollection<T>.CopyTo(T[] array, int arrayIndex) => _collection.CopyTo(array, arrayIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public FluentCollection<T> CopyTo(T[] array, int arrayIndex)
        {
            _collection.CopyTo(array, arrayIndex);
            return this;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool Remove(T item) => _collection.Remove(item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="removed"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public FluentCollection<T> Remove(T item, out bool removed)
        {
            removed = Remove(item);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public FluentCollection<T> Remove(Func<T, bool> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            foreach (var item in _collection.Where(selector).ToList())
            {
                _collection.Remove(item);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selector"></param>
        /// <param name="removed"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public FluentCollection<T> Remove(Func<T, bool> selector, out int removed)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            removed = 0;
            foreach (var item in _collection.Where(selector).ToList())
            {
                removed++;
                _collection.Remove(item);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        [DebuggerNonUserCode]
        public int Count => _collection.Count;

        /// <summary>
        /// 
        /// </summary>
        [DebuggerNonUserCode]
        public bool IsReadOnly => _collection.IsReadOnly;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public FluentCollection<T> ForEach(Action<T> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            foreach (var item in _collection)
            {
                action.Invoke(item);
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public FluentCollection<T> ForEach(Action<T, int> action)
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            int index = 0;
            foreach (var item in _collection)
            {
                action.Invoke(item, index++);
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public override int GetHashCode() => _collection.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(_collection, obj)) return true;
            if (ReferenceEquals(null, obj)) return false;
            return _collection.Equals(obj);
        }
    }
}
