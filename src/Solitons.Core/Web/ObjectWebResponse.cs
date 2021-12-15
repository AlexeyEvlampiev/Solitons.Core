using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectWebResponse : WebResponse
    {
        internal ObjectWebResponse(HttpStatusCode status, object obj) : base(status)
        {
            Object = obj.ThrowIfNullArgument(nameof(obj));
        }

        public object Object { get; }
    }
}
