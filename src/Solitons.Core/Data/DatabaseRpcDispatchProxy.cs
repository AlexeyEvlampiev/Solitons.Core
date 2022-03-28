using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Solitons.Collections;
using Solitons.Reflection;

namespace Solitons.Data
{
    internal abstract class DatabaseRpcDispatchProxy : DispatchProxy
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<MethodInfo, DbCommandAttribute>> MethodAnnotationsTableCache = new();

        protected static Dictionary<MethodInfo, DbCommandAttribute> GetMethodAnnotationsTable(
            Type interfaceType,
            IDatabaseRpcProvider provider)
        {
            if (false == MethodAnnotationsTableCache.TryGetValue(interfaceType, out var routes))
            {
                routes = BuildAnnotationsTable(interfaceType, provider);
                MethodAnnotationsTableCache.TryAdd(interfaceType, routes!);
            }

            return routes;
        }

        private static Dictionary<MethodInfo, DbCommandAttribute> BuildAnnotationsTable(Type interfaceType,
            IDatabaseRpcProvider provider)
        {
            var methods = interfaceType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            var result = new Dictionary<MethodInfo, DbCommandAttribute>();
            foreach (var method in methods)
            {
                var att = method
                    .GetCustomAttribute<DbCommandAttribute>()
                    .ThrowIfNull(() => new InvalidOperationException(new StringBuilder($"{typeof(DbCommandAttribute)} method annotation is missing.")
                        .Append($" See method {interfaceType}.{method.Name}")
                        .ToString()));

                var parameters = method.GetParameters();
                if (parameters.Length != 2 || parameters[1].ParameterType != typeof(CancellationToken))
                {
                    throw new InvalidOperationException(new StringBuilder("Invalid parameter list.")
                        .Append($" Expected parameters: (object request, CancellationToken cancellation)")
                        .Append($" See method {interfaceType}.{method.Name}")
                        .ToString());
                }

                var returnParameter = method.ReturnParameter;
                if (false == typeof(Task).IsAssignableFrom(returnParameter.ParameterType) ||
                    false == returnParameter.ParameterType.IsGenericType)
                {
                    throw new InvalidOperationException(new StringBuilder("Invalid return parameter.")
                        .Append($" Expected: Task<TResponse>")
                        .Append($" See method {interfaceType}.{method.Name}")
                        .ToString());
                }

                var requestType = parameters[0].ParameterType;
                if (false == provider.CanSerialize(requestType, att.RequestContentType))
                {
                    throw new InvalidOperationException(new StringBuilder("Required content type is not supported.")
                        .Append($" The {requestType} request type cannot be serialized to the '{att.RequestContentType}' media content type.")
                        .Append($" See method {interfaceType}.{method.Name}")
                        .ToString());
                }


                var responseType = returnParameter.ParameterType.GetGenericArguments().Single();
                if (false == provider.CanSerialize(responseType, att.ResponseContentType))
                {
                    throw new InvalidOperationException(new StringBuilder("Required content type is not supported.")
                        .Append($" The {responseType} response type cannot be deserialized from the '{att.ResponseContentType}' media content type.")
                        .Append($" See method {interfaceType}.{method.Name}")
                        .ToString());
                }

                var callbackType = typeof(AsyncDatabaseCallback<>).MakeGenericType(responseType);
                var invocationCallback = (InvocationCallback)Activator
                    .CreateInstance(callbackType)
                    .ThrowIfNull(() => new InvalidOperationException($"{callbackType} type could not be created."));

                att.ResponseType = method.ReturnType.GetGenericArguments().Single();
                att.RequestType = parameters[0].ParameterType;
                att.InvocationCallback = invocationCallback.Invoke;
                result.Add(method, att);
            }
            return result;
        }


        sealed class AsyncDatabaseCallback<T> : AsyncInvocationCallback<T>
        {
            [DebuggerStepThrough]
            public override Task<T> InvokeAsync(object[] args)
            {
                var provider = (IDatabaseRpcProvider)args[0];
                var annotation = (DbCommandAttribute)args[1];
                var request = args[2];
                var cancellation = (CancellationToken)args[3];
                return InvokeAsync(provider, annotation, request, cancellation);
            }

            private async Task<T> InvokeAsync(IDatabaseRpcProvider provider, DbCommandAttribute annotation, object request, CancellationToken cancellation)
            {
                if (provider == null) throw new ArgumentNullException(nameof(provider));
                if (annotation == null) throw new ArgumentNullException(nameof(annotation));
                if (request == null) throw new ArgumentNullException(nameof(request));
                cancellation.ThrowIfCancellationRequested();

                try
                {

                    var response = await provider.InvokeAsync(annotation, request, cancellation);
                    return (T)response;
                }
                catch (Exception e)
                {
                    throw new DatabaseRpcInvocationException(annotation, e);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DatabaseRpcDispatchProxy<T> : DatabaseRpcDispatchProxy
    {
        private IDatabaseRpcProvider? _provider;
        private Dictionary<MethodInfo, DbCommandAttribute>? _annotations;
        

        public static T Create(IDatabaseRpcProvider provider)
        {
            var type = typeof(T);
            if (type.IsInterface == false)
                throw new InvalidOperationException($"{type} is not an interface.");
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            var routes = GetMethodAnnotationsTable(typeof(T), provider);


            object obj = DispatchProxy.Create<T, DatabaseRpcDispatchProxy<T>>()!;
            var proxy = (DatabaseRpcDispatchProxy<T>)obj;
            proxy.Initialize(provider, routes);
            return (T)obj;
        }


        private void Initialize(IDatabaseRpcProvider provider, Dictionary<MethodInfo, DbCommandAttribute> routes)
        {
            _provider = provider;
            _annotations = routes;
        }

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            Debug.Assert(_provider != null);
            Debug.Assert(_annotations != null);
            Debug.Assert(args?.Length == 2);
            Debug.Assert(args[1] is CancellationToken);

            var annotation = _annotations[targetMethod!];
            return annotation.InvocationCallback.Invoke(new[]
            {
                _provider,
                annotation,
                args[0]!, // request dto
                args[1]!  // CancellationToken
            });
        }


        public sealed override string ToString() => _provider?.ToString() ?? base.ToString()!;


    }
}
