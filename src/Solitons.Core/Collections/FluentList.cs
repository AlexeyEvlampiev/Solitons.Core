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
    public static class FluentList
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public static FluentList<T> Create<T>() => new();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode]
        public static FluentList<T> Create<T>(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            return new FluentList<T>(collection.ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public static FluentList<T> Create<T>(T item) => Wrap(new List<T>(){item});

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public static FluentList<T> Create<T>(T item1, T item2) => Wrap(new List<T>() { item1, item2});

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public static FluentList<T> Create<T>(T item1, T item2, T item3) => Wrap(new List<T>() { item1, item2, item3 });

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public static FluentList<T> Create<T>(params T[] items) => Wrap(new List<T>(items));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="capacity"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public static FluentList<T> CreateWithCapacity<T>(int capacity, T item) => Wrap(new List<T>(capacity) { item });

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="capacity"></param>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public static FluentList<T> CreateWithCapacity<T>(int capacity, T item1, T item2) => Wrap(new List<T>(capacity) { item1, item2 });

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="capacity"></param>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public static FluentList<T> CreateWithCapacity<T>(int capacity, T item1, T item2, T item3) => Wrap(new List<T>(capacity) { item1, item2, item3 });

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="capacity"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public static FluentList<T> CreateWithCapacity<T>(int capacity, params T[] items)
        {
            var list = new List<T>(capacity);
            list.AddRange(items);
            return Wrap(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="capacity"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public static FluentList<T> CreateWithCapacity<T>(int capacity) => new(new List<T>(capacity));

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="capacity"></param>
        /// <param name="collection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode]
        public static FluentList<T> CreateWithCapacity<T>(int capacity, IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            var list = new List<T>(capacity);
            list.AddRange(collection);
            return new FluentList<T>(list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode]
        public static FluentList<T> Wrap<T>(IList<T> list)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            return list is FluentList<T> fluentList
                ? fluentList
                : new FluentList<T>(list);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode]
        public static FluentList<T> WrapOrCreate<T>(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            return new FluentList<T>(collection);
        }
    }

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
        internal FluentList()
        {
            _list = new List<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        [DebuggerNonUserCode]
        internal FluentList(int capacity)
        {
            _list = new List<T>(capacity);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode]
        internal FluentList(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));
            if (collection is FluentList<T> other)
            {
                _list = other._list;
            }
            else
            {
                _list = collection is IList<T> list
                    ? list
                    : new List<T>(collection);
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public FluentList<T> Add(T item1, T item2)
        {
            _list.Add(item1);
            _list.Add(item2);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public FluentList<T> Add(T item1, T item2, T item3)
        {
            _list.Add(item1);
            _list.Add(item2);
            _list.Add(item3);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode]
        public FluentList<T> Add(params T[] items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (var item in items)
            {
                _list.Add(item);
            }
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [DebuggerNonUserCode]
        public FluentList<T> AddRange(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));
            foreach (var item in items)
            {
                _list.Add(item);
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public bool Remove(T item) => _list.Remove(item);


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
