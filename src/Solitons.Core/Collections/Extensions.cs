using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Solitons.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        [return: NotNull]
        public static FluentCollection<T> AsFluent<T>(this ICollection<T> self)
        {
            self = self.ThrowIfNullArgument(nameof(self));
            return self is FluentCollection<T> fluent
                ? fluent
                : new FluentCollection<T>(self);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <returns></returns>
        [return: NotNull]
        public static FluentList<T> AsFluent<T>(this IList<T> self)
        {
            self = self.ThrowIfNullArgument(nameof(self));
            return self is FluentList<T> fluent
                ? fluent
                : new FluentList<T>(self);
        }

    }
}
