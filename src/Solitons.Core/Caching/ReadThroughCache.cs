using System;
using System.Diagnostics;
using System.Reactive.Subjects;

namespace Solitons.Caching
{
    /// <summary>
    /// 
    /// </summary>
    public static class ReadThroughCache
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IConnectableObservable<T> Publish<T>(IObservable<T> source)
        {
            if (source is ReadThroughCacheConnectedObservable<T> connected)
            {
                return connected;
            }

            return new ReadThroughCacheConnectedObservable<T>(source);
        }
    }
}
