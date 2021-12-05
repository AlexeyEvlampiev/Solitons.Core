using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solitons.Collections
{
    public abstract class CollectionProxy<T> : ICollection<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly ICollection<T> _innerCollection;

        [DebuggerStepThrough]
        protected CollectionProxy(ICollection<T> innerCollection)
        {
            _innerCollection = innerCollection ?? throw new ArgumentNullException(nameof(innerCollection));
        }

        [DebuggerStepThrough]
        public IEnumerator<T> GetEnumerator() => _innerCollection.GetEnumerator();

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _innerCollection).GetEnumerator();

        [DebuggerStepThrough]
        public virtual void Add(T item) => _innerCollection.Add(item);

        [DebuggerStepThrough]
        public virtual void Clear() => _innerCollection.Clear();

        [DebuggerStepThrough]
        public bool Contains(T item) => _innerCollection.Contains(item);

        [DebuggerStepThrough]
        public void CopyTo(T[] array, int arrayIndex) => _innerCollection.CopyTo(array, arrayIndex);

        [DebuggerStepThrough]
        public virtual bool Remove(T item) => _innerCollection.Remove(item);

        public int Count
        {
            [DebuggerStepThrough]
            get => _innerCollection.Count;
        }

        public bool IsReadOnly 
        {
            [DebuggerStepThrough]
            get => _innerCollection.IsReadOnly;
        }

        [DebuggerStepThrough]
        public override string ToString() => _innerCollection.ToString();

        [DebuggerStepThrough]
        public override bool Equals(object? obj) => _innerCollection.Equals(obj);

        [DebuggerStepThrough]
        public override int GetHashCode() => _innerCollection.GetHashCode();
    }
}
