using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solitons.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class CollectionProxy<T> : ICollection<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly ICollection<T> _innerCollection;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerCollection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        protected CollectionProxy(ICollection<T> innerCollection)
        {
            _innerCollection = innerCollection ?? throw new ArgumentNullException(nameof(innerCollection));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IEnumerator<T> GetEnumerator() => _innerCollection.GetEnumerator();


        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _innerCollection).GetEnumerator();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual CollectionProxy<T> Add(T item)
        {
            _innerCollection.Add(item);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual CollectionProxy<T> Clear()
        {
            _innerCollection.Clear();
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool Contains(T item) => _innerCollection.Contains(item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        [DebuggerStepThrough]
        public void CopyTo(T[] array, int arrayIndex) => _innerCollection.CopyTo(array, arrayIndex);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public virtual bool Remove(T item) => _innerCollection.Remove(item);

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            [DebuggerStepThrough]
            get => _innerCollection.Count;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly 
        {
            [DebuggerStepThrough]
            get => _innerCollection.IsReadOnly;
        }


        [DebuggerStepThrough]
        void ICollection<T>.Add(T item) => _innerCollection.Add(item);

        [DebuggerStepThrough]
        void ICollection<T>.Clear() => _innerCollection.Clear();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public override string ToString() => _innerCollection.ToString();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public override bool Equals(object? obj) => _innerCollection.Equals(obj);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public override int GetHashCode() => _innerCollection.GetHashCode();
    }
}
