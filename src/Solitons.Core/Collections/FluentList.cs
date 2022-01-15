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
    public sealed class FluentList<T> : IList<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IList<T> _list;

        /// <summary>
        /// 
        /// </summary>
        [DebuggerNonUserCode]
        public FluentList()
        {
            _list = new List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        [DebuggerNonUserCode]
        public FluentList(int capacity)
        {
            _list = new List<T>(capacity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode]
        public FluentList(IEnumerable<T> collection)
        {
            _list = new List<T>(collection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        [DebuggerNonUserCode]
        public static explicit operator FluentList<T>(List<T> list) => list == null ? null : new FluentList<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        [DebuggerNonUserCode]
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_list).GetEnumerator();

        [DebuggerNonUserCode]
        void ICollection<T>.Add(T item) => _list.Add(item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public FluentList<T> Add(T item)
        {
            _list.Add(item);
            return this;
        }

        [DebuggerNonUserCode]
        void ICollection<T>.Clear() => _list.Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public FluentList<T> Clear()
        {
            _list.Clear();
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public bool Contains(T item) => _list.Contains(item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public bool Contains(T item, out int index)
        {
            index = IndexOf(item);
            return index >= 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        [DebuggerNonUserCode]
        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        [DebuggerNonUserCode]
        bool ICollection<T>.Remove(T item) => _list.Remove(item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public FluentList<T> Remove(T item)
        {
            _list.Remove(item);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="removed"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public FluentList<T> Remove(T item, out bool removed)
        {
            removed = _list.Remove(item);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        [DebuggerNonUserCode]
        public int Count => _list.Count;

        [DebuggerNonUserCode]
        bool ICollection<T>.IsReadOnly => _list.IsReadOnly;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public int IndexOf(T item) => _list.IndexOf(item);

        [DebuggerNonUserCode]
        void IList<T>.Insert(int index, T item) => _list.Insert(index, item);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public FluentList<T> Insert(int index, T item)
        {
            _list.Insert(index, item);
            return this;
        }

        [DebuggerNonUserCode]
        void IList<T>.RemoveAt(int index) => _list.RemoveAt(index);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public FluentList<T> RemoveAt(int index)
        {
            _list.RemoveAt(index);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            [DebuggerNonUserCode]
            get => _list[index];
            [DebuggerNonUserCode]
            set => _list[index] = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public override int GetHashCode() => _list.GetHashCode();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj)) return true;
            if (ReferenceEquals(_list, obj)) return true;
            if (ReferenceEquals(null, obj)) return false;
            return _list.Equals(obj);
        }
    }
}
