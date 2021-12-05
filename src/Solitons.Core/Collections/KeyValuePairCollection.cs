using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Solitons.Collections
{
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

        public void Add(TKey key, TValue value) => Add(KeyValuePair.Create(key, value));

        public void Remove(IEnumerable<KeyValuePair<TKey, TValue>> items) => 
            items?.ForEach(item=> Remove(item));
    }


    public class KeyValuePairCollection : KeyValuePairCollection<string, string>
    {
        [DebuggerStepThrough]
        public KeyValuePairCollection()
        {

        }

        [DebuggerStepThrough]
        public KeyValuePairCollection(Append init) : base(init)
        {

        }

        [DebuggerStepThrough]
        public KeyValuePairCollection(params KeyValuePair<string, string>[] items) : base(items)
        {

        }

        [DebuggerStepThrough]
        public KeyValuePairCollection(IEnumerable<KeyValuePair<string, string>> source) : base(source)
        {

        }

    }
}
