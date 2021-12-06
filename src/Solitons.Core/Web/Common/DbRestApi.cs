using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Web.Common
{
    public abstract class DbRestApi<THttpTrigger, TDbTransaction> : RestApi
        where THttpTrigger : IHttpTriggerMetadata
        where TDbTransaction : IDbTransactionMetadata
    {

        private readonly Dictionary<THttpTrigger, TDbTransaction> _httpTriggers;
        private readonly IWebQueryConverter _webQueryConverter;


        protected DbRestApi(DomainContext domainContext)
        {
            if (domainContext == null) throw new ArgumentNullException(nameof(domainContext));
            _httpTriggers = domainContext.GetHttpTriggers<THttpTrigger, TDbTransaction>();
            _webQueryConverter = domainContext.GetWebQueryConverter<THttpTrigger>();
            Serializer = domainContext.GetSerializer();
        }


        protected abstract Task<IWebResponse> ExecAsync(
            IHttpTriggerMetadata trigger,
            IDbTransactionMetadata handler, 
            object webQuery, 
            IWebRequest request, 
            IAsyncLogger logger, 
            CancellationToken cancellation);

        protected IDomainSerializer Serializer { get; }

        protected sealed override IObservable<IWebResponse> GetResponses(IWebRequest request, IAsyncLogger logger, CancellationToken cancellation)
        {
            return _httpTriggers
                .ToObservable()
                .Do(pair => Debug.WriteLine($"{GetType().Name}: processing {request.Uri}. Match: {pair.Key.IsUriMatch(request.Uri)}"))
                .Where(pair =>
                {
                    var trigger = pair.Key;
                    return trigger.IsUriMatch(request.Uri) && 
                           trigger.IsMethodMatch(request.Method);
                })
                .SelectMany(pair =>
                {
                    var webQuery = _webQueryConverter.ToDataTransferObject(request);
                    var (trigger, handler) = (pair.Key, pair.Value);
                    return ExecAsync(trigger, handler, webQuery, request, logger, cancellation);
                });
        }
    }

    public abstract class DbRestApi<TDbTransaction> : DbRestApi<IHttpTriggerMetadata, TDbTransaction>
        where TDbTransaction : IDbTransactionMetadata
    {
        protected DbRestApi(DomainContext domainContext) : base(domainContext)
        {
        }
    }
}
