using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Solitons.Data
{
    internal abstract class DatabaseRpcDispatchProxy : DispatchProxy
    {
        private static readonly ConcurrentDictionary<Type, Dictionary<MethodInfo, DbCommandHandler>> MethodAnnotationsTableCache = new();

        protected static Dictionary<MethodInfo, DbCommandHandler> GetMethodAnnotationsTable(
            Type interfaceType,
            IDataContractSerializer serializer)
        {
            if (false == MethodAnnotationsTableCache.TryGetValue(interfaceType, out var routes))
            {
                routes = BuildAnnotationsTable(interfaceType, serializer);
                MethodAnnotationsTableCache.TryAdd(interfaceType, routes!);
            }

            return routes;
        }

        private static Dictionary<MethodInfo, DbCommandHandler> BuildAnnotationsTable(Type interfaceType,
            IDataContractSerializer serializer)
        {
            var methods = interfaceType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
            var result = new Dictionary<MethodInfo, DbCommandHandler>();
            foreach (var method in methods)
            {
                result.Add(method, DbCommandHandler.Create(method, serializer));
            }
            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DatabaseRpcDispatchProxy<T> : DatabaseRpcDispatchProxy
    {
        private IDatabaseRpcProvider? _provider;
        private IDataContractSerializer? _serializer;
        private Dictionary<MethodInfo, DbCommandHandler>? _annotations;
        

        public static T Create(IDatabaseRpcProvider provider, IDataContractSerializer serializer)
        {
            var type = typeof(T);
            if (type.IsInterface == false)
                throw new InvalidOperationException($"{type} is not an interface.");
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            var routes = GetMethodAnnotationsTable(typeof(T), serializer);


            object obj = DispatchProxy.Create<T, DatabaseRpcDispatchProxy<T>>()!;
            var proxy = (DatabaseRpcDispatchProxy<T>)obj;
            proxy.Initialize(provider, serializer, routes);
            return (T)obj;
        }


        private void Initialize(
            IDatabaseRpcProvider provider, 
            IDataContractSerializer serializer, 
            Dictionary<MethodInfo, DbCommandHandler> routes)
        {
            _provider = provider;
            _serializer = serializer;
            _annotations = routes;
        }

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            Debug.Assert(_provider != null);
            Debug.Assert(_annotations != null);
            var handler = _annotations[targetMethod!];
            return handler.InvokeAsync(_provider, _serializer, args);
        }


        public sealed override string ToString() => _provider?.ToString() ?? base.ToString()!;


    }
}
