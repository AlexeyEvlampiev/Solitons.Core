using System;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    public interface IHttpEventHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="webRequest"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        IObservable<WebResponse> ToObservable(WebRequest webRequest, IAsyncLogger logger);
    }
}
