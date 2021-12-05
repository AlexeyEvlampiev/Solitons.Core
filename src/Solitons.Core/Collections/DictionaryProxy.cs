using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Solitons.Collections
{
    public abstract class DictionaryProxy<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _innerDictionary;

        [DebuggerStepThrough]
        protected DictionaryProxy() : this(new Dictionary<TKey, TValue>())
        {
        }

        [DebuggerStepThrough]
        protected DictionaryProxy(IEqualityComparer<TKey> comparer) 
            : this(new Dictionary<TKey, TValue>(comparer
            .ThrowIfNullArgument(nameof(comparer))))
        {

        }

        [DebuggerStepThrough]
        protected DictionaryProxy(IDictionary<TKey, TValue> innerDictionary)
        {
            _innerDictionary = innerDictionary
                .ThrowIfNullArgument(nameof(innerDictionary));
        }


        [DebuggerStepThrough]
        public override string ToString() => _innerDictionary.ToString();

        [DebuggerStepThrough]
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable) _innerDictionary).GetEnumerator();

        [DebuggerStepThrough]
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _innerDictionary.GetEnumerator();

        [DebuggerStepThrough]
        public override bool Equals(object? obj) => _innerDictionary.Equals(obj);

        [DebuggerStepThrough]
        public override int GetHashCode() => _innerDictionary.GetHashCode();

        [DebuggerStepThrough]
        public virtual void Add(KeyValuePair<TKey, TValue> item) => _innerDictionary.Add(item);

        [DebuggerStepThrough]
        public virtual void Clear() => _innerDictionary.Clear();

        [DebuggerStepThrough]
        public bool Contains(KeyValuePair<TKey, TValue> item) => _innerDictionary.Contains(item);

        [DebuggerStepThrough]
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => _innerDictionary.CopyTo(array, arrayIndex);

        [DebuggerStepThrough]
        public virtual bool Remove(KeyValuePair<TKey, TValue> item) => _innerDictionary.Remove(item);


        public int Count
        {
            [DebuggerStepThrough]
            get => _innerDictionary.Count;
        }

        public bool IsReadOnly
        {
            [DebuggerStepThrough]
            get => _innerDictionary.IsReadOnly;
        }

        [DebuggerStepThrough]
        public void Add(TKey key, TValue value) => Add(KeyValuePair.Create(key, value));

        [DebuggerStepThrough]
        public bool ContainsKey(TKey key) => _innerDictionary.ContainsKey(key);

        [DebuggerStepThrough]
        public virtual bool Remove(TKey key) => _innerDictionary.Remove(key);

        [DebuggerStepThrough]
        public virtual bool TryGetValue(TKey key, out TValue value) => _innerDictionary.TryGetValue(key, out value);

        public virtual TValue this[TKey key]
        {
            [DebuggerStepThrough]
            get => _innerDictionary[key];
            [DebuggerStepThrough]
            set => _innerDictionary[key] = value;
        }

        public ICollection<TKey> Keys
        {
            [DebuggerStepThrough]
            get => _innerDictionary.Keys;
        }

        public ICollection<TValue> Values
        {
            [DebuggerStepThrough]
            get => _innerDictionary.Values;
        }
    }
}
