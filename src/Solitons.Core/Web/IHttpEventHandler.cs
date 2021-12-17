using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
