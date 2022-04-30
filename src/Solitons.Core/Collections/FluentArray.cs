using System.Diagnostics;

namespace Solitons.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public static class FluentArray
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        public static T[] Create<T>(T item) => new T[] { item };

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static T[] Create<T>(T item1, T item2) => new T[] { item1, item2 };

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <returns></returns>
        public static T[] Create<T>(T item1, T item2, T item3) => new T[] { item1, item2, item3 };

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        [DebuggerNonUserCode]
        public static T[] Create<T>(params T[] items) => items;
    }
}
