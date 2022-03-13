using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// <typeparam name="T"></typeparam>
    internal class DatabaseRpcDispatchProxy<T> : DispatchProxy
    {
        private IDatabaseRpcProvider? _provider;
        private Dictionary<MethodInfo, DbCommandAttribute>? _annotations;
        private static readonly ConcurrentDictionary<Type, Dictionary<MethodInfo, DbCommandAttribute>> RouteByContractType = new();


        public static T Create(IDatabaseRpcProvider provider)
        {
            var type = typeof(T);
            if (type.IsInterface == false)
                throw new InvalidOperationException($"{type} is not an interface.");
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            if (false == RouteByContractType.TryGetValue(type, out var routes))
            {
                routes = BuildRoutingTable(type);
                RouteByContractType.TryAdd(type, routes!);
            }


            object obj = DispatchProxy.Create<T, DatabaseRpcDispatchProxy<T>>()!;
            var proxy = (DatabaseRpcDispatchProxy<T>)obj;
            proxy.Initialize(provider, routes);
            return (T)obj;
        }

        private static Dictionary<MethodInfo, DbCommandAttribute> BuildRoutingTable(Type type)
        {
            var methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            var result = new Dictionary<MethodInfo, DbCommandAttribute>();
            foreach (var method in methods)
            {
                var att = method
                    .GetCustomAttribute<DbCommandAttribute>()
                    .ThrowIfNull(()=> new InvalidOperationException(new StringBuilder($"{typeof(DbCommandAttribute)} method annotation is missing.")
                        .Append($" See method {typeof(T)}.{method.Name}")
                        .ToString()));

                var parameters = method.GetParameters();
                if (parameters.Length != 2 || parameters[1].ParameterType != typeof(CancellationToken))
                {
                    throw new InvalidOperationException(new StringBuilder("Invalid parameter list.")
                        .Append($" Expected parameters: (object request, CancellationToken cancellation)")
                        .Append($" See method {typeof(T)}.{method.Name}")
                        .ToString());
                }

                var returnParameter = method.ReturnParameter;
                if (false == typeof(Task).IsAssignableFrom(returnParameter.ParameterType) ||
                    false == returnParameter.ParameterType.IsGenericType)
                {
                    throw new InvalidOperationException(new StringBuilder("Invalid return parameter.")
                        .Append($" Expected: Task<TResponse>")
                        .Append($" See method {typeof(T)}.{method.Name}")
                        .ToString());
                }
                var responseType = returnParameter.ParameterType.GetGenericArguments().Single();
                var relayType = typeof(DbCommandRelay<>).MakeGenericType(responseType);
                var relayInstance = (DbCommandRelay)Activator.CreateInstance(relayType)!;

                att.ResponseType = method.ReturnType.GetGenericArguments().Single();
                att.RequestType = parameters[0].ParameterType;
                att.DbCommandRelay = relayInstance;
                result.Add(method, att);
            }
            return result;
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
            var request = args[0] ?? throw new NullReferenceException("Request dto object is null.");
            var cancellation = (CancellationToken)args[1]!;

            cancellation.ThrowIfCancellationRequested();
            var annotation = _annotations[targetMethod!];
            return annotation.DbCommandRelay.Invoke(_provider, annotation, request, cancellation);
        }


        public sealed override string ToString() => _provider?.ToString() ?? base.ToString()!;
    }

    abstract class DbCommandRelay
    {
        public abstract object Invoke(IDatabaseRpcProvider provider, DbCommandAttribute annotation, object request, CancellationToken cancellation);
    }

    sealed class DbCommandRelay<TResponse> : DbCommandRelay
    {
        [DebuggerStepThrough]
        public override object Invoke(IDatabaseRpcProvider provider, DbCommandAttribute annotation, object request, CancellationToken cancellation)
        {
            cancellation.ThrowIfCancellationRequested();
            return InvokeAsync(provider, annotation, request, cancellation);
        }

        private async Task<TResponse> InvokeAsync(IDatabaseRpcProvider provider, DbCommandAttribute annotation, object request, CancellationToken cancellation)
        {
            var requestText = provider.Serialize(request, annotation.RequestContentType);
            var responseText = await provider.InvokeAsync(annotation, requestText, cancellation);
            var response = (TResponse)provider.Deserialize(responseText, annotation.RequestContentType, typeof(TResponse));
            return response;
        }
    }
}
