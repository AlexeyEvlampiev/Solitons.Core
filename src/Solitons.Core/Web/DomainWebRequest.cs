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
    public sealed class DomainWebRequest
    {
        internal DomainWebRequest(object httpEventArgs, object messageBody)
        {
            HttpEventArgs = httpEventArgs.ThrowIfNullArgument(nameof(httpEventArgs));
            MessageBody = messageBody;
        }

        public object HttpEventArgs { get; }
        public object MessageBody { get; }
    }
}
