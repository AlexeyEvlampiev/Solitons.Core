using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    /// <summary>
    /// 
    /// </summary>
    internal abstract class TransactionScriptApi : DispatchProxy
    {
        delegate object GeneralizedAsyncHandler(object[] args);
        delegate Task<object> AsyncHandler(object[] args);

        private readonly Dictionary<MethodInfo, GeneralizedAsyncHandler> _generalizedCallbacks = new();
        private ITransactionScriptProvider? _callback;

        internal TransactionScriptApi() { }


        public static T Create<T>(ITransactionScriptProvider provider, IDomainSerializer serializer) where T : class
        {
            provider.ThrowIfNullArgument(nameof(provider));
            serializer.ThrowIfNullArgument(nameof(serializer));
            if (typeof(T).IsInterface == false)
                throw new InvalidOperationException();
            var instance = Create<T, TransactionScriptApi<T>>();
            var proxy = instance as TransactionScriptApi  ?? throw new InvalidOperationException();
            proxy._callback = provider;

            var methods = typeof(T)
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Except(typeof(object).GetMethods(BindingFlags.Instance | BindingFlags.Public))
                .ToList();

            foreach (var method in methods)
            {
                var procedureInfo = StoredProcedureAttribute
                    .Get(method)
                    .ThrowIfNull(()=> new InvalidOperationException(
                        new StringBuilder($"{typeof(StoredProcedureAttribute)} is required.")
                            .ToString()));

                var parameters = method.GetParameters();

                var requestInfo = StoredProcedureRequestAttribute
                    .Get(parameters[0])
                    .ThrowIfNull(()=> new InvalidOperationException());

                var responseInfo = StoredProcedureResponseAttribute
                    .Get(method)
                    .ThrowIfNull(()=> new InvalidOperationException());

                if (false == serializer.CanSerialize(requestInfo!.ParameterType, requestInfo.ContentType))
                    throw new InvalidOperationException();

                [DebuggerStepThrough]
                async Task<object> InvokeAsync(object[] args)
                {
                    Debug.Assert(args?.Length == 2);
                    var request = args[0];
                    request = request.ThrowIfNullArgument(nameof(request));
                    var cancellation = (CancellationToken)args[1];
                    cancellation.ThrowIfCancellationRequested();
                    request = await provider.OnRequestAsync(request) ?? request;
                    var requestString = serializer.Serialize(request, requestInfo.ContentType);
                    var responseString = await provider.InvokeAsync(
                        procedureInfo, 
                        requestInfo, 
                        responseInfo, 
                        requestString, 
                        cancellation);
                    responseString = responseString.ThrowIfNullOrWhiteSpace(() => new InvalidOperationException());
                    var response = serializer.Deserialize(
                        responseInfo!.AsyncResultType,
                        responseInfo.ContentType,
                        responseString)
                        .ThrowIfNull(()=> new InvalidOperationException());
                    response = await provider.OnResponseAsync(response) ?? response;
                    return response;
                }

                var typedProxy = TypedProxy.Create(InvokeAsync, responseInfo.AsyncResultType);
                proxy._generalizedCallbacks.Add(method, typedProxy.Invoke);

            }

            return instance;
        }
        
        abstract class TypedProxy
        {
            public abstract object Invoke(object[] args);

            [return:NotNull]
            public static TypedProxy Create(AsyncHandler innerHandler, Type asyncReturnType)
            {
                var ctor = typeof(TypedProxy<>)
                    .MakeGenericType(asyncReturnType)
                    .GetConstructor(new Type[] { typeof(AsyncHandler) })
                    .ThrowIfNull(()=> new InvalidOperationException());
                return (TypedProxy)ctor.Invoke(new object?[]{ innerHandler });
            }
        }

        class TypedProxy<T> : TypedProxy
        {
            private readonly AsyncHandler _innerHandler;

            public TypedProxy(AsyncHandler innerHandler)
            {
                _innerHandler = innerHandler;
            }

            [DebuggerStepThrough]
            async Task<T> InvokeAsync(object[] args)
            {
                var response = await _innerHandler.Invoke(args);
                return (T)response;
            }

            [DebuggerStepThrough]
            public override object Invoke(object[] args) => InvokeAsync(args);
        }


        [DebuggerStepThrough]
        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            Debug.Assert(targetMethod is not null);
            Debug.Assert(args?.Length == 2);
            
            if (_generalizedCallbacks.TryGetValue(targetMethod, out var dbCallback))
            {
                return dbCallback.Invoke(args);
            }

            throw new InvalidOperationException();
        }

 


        public sealed override string ToString() => _callback.ToString();

        public sealed override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj) ||
                (obj is ITransactionScriptProvider callback && callback.Equals(_callback))) return true;
            return false;
        }

        public sealed override int GetHashCode() => _callback.GetHashCode();
    }

    internal class TransactionScriptApi<T> : TransactionScriptApi where T : class
    {
    }

}