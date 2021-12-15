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
    public sealed class QueryParameterNotFoundException : FormatException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        public QueryParameterNotFoundException(string parameterName) :
            base($"'{parameterName}' query parameter is missing.")
        {
            QueryParameterName = parameterName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="message"></param>
        public QueryParameterNotFoundException(string parameterName, string message) :
            base(message)
        {
            QueryParameterName = parameterName;
        }

        /// <summary>
        /// 
        /// </summary>
        public string QueryParameterName { get; }
    }
}
