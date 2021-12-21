using System.Net;

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
