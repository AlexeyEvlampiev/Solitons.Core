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
    public interface IHttpEventListener
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="webRequest"></param>
        /// <returns></returns>
        IObservable<WebResponse> ToObservable(WebRequest webRequest);
    }
}
