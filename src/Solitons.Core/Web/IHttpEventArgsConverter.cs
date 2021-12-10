using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    public partial interface IHttpEventArgsConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        object Convert(IWebRequest request);
    }


    public partial interface IHttpEventArgsConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static IHttpEventArgsConverter FromTypes(IEnumerable<Type> types)
        {
            return new HttpEventArgsConverter(types
                .ThrowIfNullArgument(nameof(types)));
        }
    }
}
