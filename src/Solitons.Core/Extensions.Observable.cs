using System;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;


namespace Solitons
{
    public static partial class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="self"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IObservable<T> Skip<T>(this IObservable<T> self, Func<T, bool> predicate)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));
            return self.Where(item => false == predicate.Invoke(item));
        }


        
    }
}
