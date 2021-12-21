using Solitons.Security;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Web.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class HttpEventHandler : IHttpEventHandler, IClaimsTransformation
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serializer"></param>
        protected HttpEventHandler(IDomainSerializer serializer)
        {
            Serializer = serializer;
        }

        /// <summary>
        /// 
        /// </summary>
        protected IDomainSerializer Serializer { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="logger"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        protected abstract Task<WebResponse> GetResponseAsync(WebRequest request, IAsyncLogger logger, CancellationToken cancellation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpEventArgs"></param>
        /// <returns></returns>
        protected abstract IHttpEventArgsAttribute FindHttpEventArgsDescriptor(object httpEventArgs);
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected virtual Task<WebResponse> OnResponseObjectTypeMismatchAsync(Type expected, Type actual, IAsyncLogger logger)
        {
            throw new InvalidOperationException(
                new StringBuilder("Structured return object type mismatch.")
                .Append($" {GetType()}.{nameof(GetResponseAsync)} returned {actual}. Expected return type is {expected}.")
                .ToString());
        }


        [DebuggerStepThrough]
        IObservable<WebResponse> IHttpEventHandler.ToObservable(WebRequest webRequest, IAsyncLogger logger)
        {
            var httpEventArgs = webRequest
                .ThrowIfNullArgument(nameof(webRequest))
                .HttpEventArgs
                    .ThrowIfNull(()=> new ArgumentException($"{nameof(webRequest.HttpEventArgs)} is null.", nameof(webRequest)));   
            
            logger = logger
                .ThrowIfNullArgument(nameof(logger))
                .WithProperty(nameof(IHttpEventHandler), GetType().FullName)
                .WithProperty("HttpEventArgs",webRequest.HttpEventArgs.GetType().FullName)
                .WithProperty("Method", webRequest.Method)
                .WithProperty("ClientVersion", webRequest.ClientVersion.ToString())
                .WithProperty("Url", webRequest.Uri)
                .WithProperties(webRequest.Caller.Claims.ToDictionary(c=> c.Type, c=> c.Value));


            var httpEventArgsDescriptor = FindHttpEventArgsDescriptor(httpEventArgs);
            if (httpEventArgsDescriptor is null)
                return Observable.Empty<WebResponse>();

            var expectedResponseObjectType = httpEventArgsDescriptor.ResponseObjectType;
            bool expectedStructuredResponse = !(
                    expectedResponseObjectType is null ||
                    expectedResponseObjectType == typeof(string) ||
                    expectedResponseObjectType == typeof(Stream));
            
            if (expectedStructuredResponse)
            {
                if(Serializer.CanMeetExpectation(expectedResponseObjectType, webRequest.Accept))
                {
                    Debug.WriteLine($"Response expectation can be satisfied.");
                }
                else
                {
                    var contentTypesCsv = Serializer.GetContentTypes(expectedResponseObjectType).Join();
                    return Observable.Return(
                        WebResponse.Create(System.Net.HttpStatusCode.ExpectationFailed,
                            new StringBuilder("Response Content Type expectation cannot be satisfied.")
                            .Append($" Supported Content Types: {contentTypesCsv}")
                            .ToString()));
                }                
            }

            return Observable.Create<WebResponse>(ToObservable);

            async Task ToObservable(IObserver<WebResponse> observer, CancellationToken cancellation)
            {
                cancellation.ThrowIfCancellationRequested();
                try
                {
                    var webResponse = await GetResponseAsync(webRequest, logger, cancellation) ??
                        throw new NullReferenceException($"{GetType()}.{nameof(GetResponseAsync)} returned null async result.");

                    if (webResponse is ObjectWebResponse objectWebResponse)
                    {
                        var actualResponseObjectType = objectWebResponse.Object.GetType();
                        if (expectedStructuredResponse)
                        {                            
                            if (actualResponseObjectType != expectedResponseObjectType)
                            {
                                webResponse = await OnResponseObjectTypeMismatchAsync(expectedResponseObjectType, actualResponseObjectType, logger);
                                webResponse.ThrowIfNull(() => new NullReferenceException($"{GetType()}.{nameof(OnResponseObjectTypeMismatchAsync)} returned null async result."));
                            }
                        }
                        else if(Serializer.CanMeetExpectation(actualResponseObjectType, webRequest.Accept, out var contentType))
                        {
                            var content = Serializer.Serialize(objectWebResponse.Object, contentType);
                            webResponse = WebResponse.Create(webResponse.Status, content, contentType);
                        }
                    }
                    else if (webResponse is ContentWebResponse contentWebResponse)
                    {
                        if((int)contentWebResponse.Status >= 400)
                        {
                            Debug.WriteLine($"{contentWebResponse.Status}: {contentWebResponse.Content}");
                        }                        
                        else if (expectedStructuredResponse)
                        {
                            if(Serializer.CanDeserialize(expectedResponseObjectType, contentWebResponse.ContentType))
                            {
                                var obj = Serializer.Deserialize(
                                    expectedResponseObjectType, 
                                    contentWebResponse.ContentType,
                                    contentWebResponse.Content);
                                webResponse = WebResponse.Create(webResponse.Status, obj);
                            }
                        }
                    }

                    observer.OnNext(webResponse);
                    observer.OnCompleted();
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        protected virtual Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal) => Task.FromResult(principal);

        [DebuggerStepThrough]
        Task<ClaimsPrincipal> IClaimsTransformation.TransformAsync(ClaimsPrincipal principal) => TransformAsync(principal);
    }
}
