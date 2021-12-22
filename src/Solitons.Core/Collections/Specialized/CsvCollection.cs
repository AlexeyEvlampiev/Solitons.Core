using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Solitons.Collections.Specialized
{
    public static class CsvCollection
    {
        public static CsvCollection<string> Create() => new();

        public static CsvCollection<string> Create(string value)
        {
            value.ThrowIfNullOrWhiteSpace(() => new ArgumentNullException(nameof(value)));
            var result = Create();
            result.Add(value);
            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Solitons.Collections.CollectionProxy&lt;T&gt;" />
    public sealed class CsvCollection<T> : CollectionProxy<T>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Func<T, string> _formatItem;

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvCollection{T}"/> class.
        /// </summary>
        /// <param name="innerCollection">The inner collection.</param>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="formatItem">The format item.</param>
        /// <exception cref="System.ArgumentNullException">innerCollection</exception>
        public CsvCollection(ICollection<T> innerCollection, string delimiter = null, Func<T, string> formatItem = null) : base(innerCollection)
        {
            Delimiter = delimiter ?? ",";
            _formatItem = formatItem ?? NoFormatting;
            string NoFormatting(T input) => input.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CsvCollection{T}"/> class.
        /// </summary>
        /// <param name="delimiter">The delimiter.</param>
        /// <param name="formatItem">The format item.</param>
        public CsvCollection(string delimiter = null, Func<T, string> formatItem = null) 
            : this(new List<T>(), delimiter, formatItem)
        {
        }

        /// <summary>
        /// Gets the delimiter.
        /// </summary>
        /// <value>
        /// The delimiter.
        /// </value>
        public string Delimiter { get; }

        /// <summary>
        /// Performs an implicit conversion from <see cref="CsvCollection{T}"/> to <see cref="System.String"/>.
        /// </summary>
        /// <param name="csvCollection">The CSV list.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(CsvCollection<T> csvCollection) => csvCollection?.ToString();

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public sealed override string ToString()
        {
            if (Count == 1) return _formatItem.Invoke(this.First());
            var builder = new StringBuilder();
            this.ForEach((item, index) =>
            {
                var stringItem = _formatItem.Invoke(item);
                builder.Append(index == 0 ? stringItem : $"{Delimiter}{stringItem}");
            });

            return builder.ToString();
        }

        public void AddRange(IEnumerable<T> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            values.ForEach(Add);
        }
    }
}
