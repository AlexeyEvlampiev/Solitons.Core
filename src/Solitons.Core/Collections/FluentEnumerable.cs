using System.Collections.Generic;

namespace Solitons.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public static class FluentEnumerable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static IEnumerable<T> Yield<T>(T item)
        {
            yield return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static IEnumerable<T> Yield<T>(T item1, T item2)
        {
            yield return item1;
            yield return item2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <returns></returns>
        public static IEnumerable<T> Yield<T>(T item1, T item2, T item3)
        {
            yield return item1;
            yield return item2;
            yield return item3;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <param name="item4"></param>
        /// <returns></returns>
        public static IEnumerable<T> Yield<T>(T item1, T item2, T item3, T item4)
        {
            yield return item1;
            yield return item2;
            yield return item3;
            yield return item4;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IEnumerable<T> Yield<T>(params T[] items) => items;

    }
}
