using Solitons.Security;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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
        /// <param name="metadata"></param>
        /// <returns></returns>
        ///<exception cref="ClaimNotFoundException"></exception>
        ///<exception cref="QueryParameterNotFoundException"></exception>
        ///<exception cref="ArgumentException"></exception>
        ///<exception cref="ArgumentNullException"></exception>
        object Convert(IWebRequest request, out IHttpEventArgsAttribute metadata);
    }


    public partial interface IHttpEventArgsConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ///<exception cref="ClaimNotFoundException"></exception>
        ///<exception cref="QueryParameterNotFoundException"></exception>
        ///<exception cref="ArgumentException"></exception>
        ///<exception cref="ArgumentNullException"></exception>
        [DebuggerStepThrough]
        public object Convert(IWebRequest request) => Convert(request, out _);

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
