using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Web.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class HttpEventListener : IHttpEventListener
    {
        protected abstract bool CanProcess(WebRequest request);
        protected abstract Task<WebResponse> ProcessAsync(WebRequest request, CancellationToken cancellation);
        

        [DebuggerStepThrough]
        IObservable<WebResponse> IHttpEventListener.ToObservable(WebRequest webRequest)
        {
            var args = webRequest?.HttpEventArgs;
            if (args is null || CanProcess(webRequest) == false)
                return Observable.Empty<WebResponse>();
            return Observable.Create<WebResponse>(async (observer, cancellation) =>
            {
                try
                {
                    var response = await ProcessAsync(webRequest, cancellation);
                    observer.OnNext(response);
                    observer.OnCompleted();
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            });
        }
    }
}
