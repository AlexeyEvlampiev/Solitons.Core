using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Data
{
    abstract class DbCommandHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="annotation"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected DbCommandHandler(DbCommandAttribute annotation)
        {
            Annotation = annotation ?? throw new ArgumentNullException(nameof(annotation));
        }

        public DbCommandAttribute Annotation { get; }


        public abstract Task InvokeAsync(
            IDatabaseRpcProvider provider, 
            IDataContractSerializer dataContractSerializer,
            object[] args);

        public static DbCommandHandler Create(MethodInfo method, IDataContractSerializer serializer)
        {
            Debug.Assert(method.DeclaringType?.IsInterface == true);

            var annotation = method.GetCustomAttribute<DbCommandAttribute>()
                .ThrowIfNull(() => new InvalidOperationException(new StringBuilder($"{typeof(DbCommandAttribute)} method annotation is missing.")
                    .Append($" See method {method.DeclaringType}.{method.Name}")
                    .ToString()));

            var parameters = method.GetParameters();
            
            if (parameters.Any(p => typeof(MulticastDelegate).IsAssignableFrom(p.ParameterType)))
            {
                return CreateCallbackHandler(annotation, method, serializer);
            }

            return CreateRpcHandler(annotation, method, serializer);
        }

        private static DbCommandHandler CreateRpcHandler(
            DbCommandAttribute annotation, 
            MethodInfo method,
            IDataContractSerializer serializer)
        {
            var parameters = method.GetParameters();
            if (parameters.Length != 2 || parameters[1].ParameterType != typeof(CancellationToken))
            {
                throw new InvalidOperationException(new StringBuilder("Invalid parameter list.")
                    .Append($" Expected parameters: ({{TRequest}} request, CancellationToken cancellation)")
                    .Append($" See method {method.DeclaringType}.{method.Name}")
                    .ToString());
            }

            var returnParameter = method.ReturnParameter;
            if (false == typeof(Task).IsAssignableFrom(returnParameter.ParameterType) ||
                false == returnParameter.ParameterType.IsGenericType)
            {
                throw new InvalidOperationException(new StringBuilder("Invalid return parameter.")
                    .Append($" Expected: Task<TResponse>")
                    .Append($" See method {method.DeclaringType}.{method.Name}")
                    .ToString());
            }

            var requestType = parameters[0].ParameterType;
            if (false == serializer.CanSerialize(requestType, annotation.RequestContentType))
            {
                throw new InvalidOperationException(new StringBuilder("Required content type is not supported.")
                    .Append($" The {requestType} request type cannot be serialized to the '{annotation.RequestContentType}' media content type.")
                    .Append($" See method {method.DeclaringType}.{method.Name}")
                    .ToString());
            }


            var responseType = returnParameter.ParameterType.GetGenericArguments().Single();
            if (false == serializer.CanSerialize(responseType, annotation.ResponseContentType))
            {
                throw new InvalidOperationException(new StringBuilder("Required content type is not supported.")
                    .Append($" The {responseType} response type cannot be deserialized from the '{annotation.ResponseContentType}' media content type.")
                    .Append($" See method {method.DeclaringType}.{method.Name}")
                    .ToString());
            }

            var handlerType = typeof(DbCommandRpcHandler<>).MakeGenericType(responseType);
            var ctor = handlerType.GetConstructor(new Type[]{ typeof(DbCommandAttribute)})!;
            var handler = (DbCommandHandler)ctor.Invoke(new object[]{annotation});
            annotation.RequestType = requestType;
            annotation.ResponseType = responseType;
            return handler;
        }

        private static DbCommandHandler CreateCallbackHandler(
            DbCommandAttribute annotation,
            MethodInfo method, 
            IDataContractSerializer serializer)
        {
            string InvalidSignatureMessage() =>
                new StringBuilder("Invalid parameter list.")
                    .Append($" Expected parameters: ({{TRequest}} request, Func<{{TResponse}}, Task> callback, CancellationToken cancellation)")
                    .Append($" See method {method.DeclaringType}.{method.Name}")
                    .ToString();

            var parameters = method.GetParameters();
            if (parameters.Length != 3 ||
                false == typeof(MulticastDelegate).IsAssignableFrom(parameters[1].ParameterType) ||
                false == parameters[1].ParameterType.IsGenericType ||
                parameters[1].ParameterType.GetGenericTypeDefinition() != typeof(Func<,>) ||
                parameters[2].ParameterType != typeof(CancellationToken))
            {
                throw new InvalidOperationException(InvalidSignatureMessage());
            }

            var returnParameter = method.ReturnParameter;
            if (returnParameter.ParameterType != typeof(Task))
            {
                throw new InvalidOperationException(new StringBuilder("Invalid return parameter.")
                    .Append($" Expected: Task. Actual: {returnParameter.ParameterType}")
                    .Append($" See method {method.DeclaringType}.{method.Name}")
                    .ToString());
            }

            var requestType = parameters[0].ParameterType;
            if (false == serializer.CanSerialize(requestType, annotation.RequestContentType))
            {
                throw new InvalidOperationException(new StringBuilder("Required content type is not supported.")
                    .Append($" The {requestType} request type cannot be serialized to the '{annotation.RequestContentType}' media content type.")
                    .Append($" See method {method.DeclaringType}.{method.Name}")
                    .ToString());
            }

            var callbackGenericArguments = parameters[1].ParameterType.GetGenericArguments();
            Debug.Assert(callbackGenericArguments.Length == 2);

            if (callbackGenericArguments[1] != typeof(Task))
            {
                throw new InvalidOperationException(InvalidSignatureMessage());
            }

            var responseType = callbackGenericArguments[0];
            if (false == serializer.CanDeserialize(callbackGenericArguments[0], annotation.ResponseContentType))
            {
                throw new InvalidOperationException(new StringBuilder("Required content type is not supported.")
                    .Append($" The {responseType} response type cannot be deserialized from the '{annotation.ResponseContentType}' media content type.")
                    .Append($" See method {method.DeclaringType}.{method.Name}")
                    .ToString());
            }


            var handlerType = typeof(DbCommandInterceptedRpcHandler<>).MakeGenericType(responseType);
            var ctor = handlerType.GetConstructor(new Type[] { typeof(DbCommandAttribute)})!;
            var handler = (DbCommandHandler)ctor.Invoke(new object[] { annotation });
            annotation.RequestType = requestType;
            annotation.ResponseType = responseType;
            return handler;
        }
    }


    sealed class DbCommandRpcHandler<TResponse> : DbCommandHandler
    {
        public DbCommandRpcHandler(DbCommandAttribute annotation) : base(annotation)
        {
        }

        [DebuggerStepThrough]
        public override Task InvokeAsync(
            IDatabaseRpcProvider provider, 
            IDataContractSerializer serializer,
            object[] args) =>
            InvokeRpcAsync(provider, serializer, args);

        private async Task<TResponse> InvokeRpcAsync(
            IDatabaseRpcProvider provider,
            IDataContractSerializer serializer,
            object[] args)
        {
            var request = args[0];
            var cancellation = (CancellationToken)args[1];
            var response = (TResponse)await provider.InvokeAsync(Annotation, request, serializer, null, cancellation);
            return response;
        }
    }

    sealed class DbCommandInterceptedRpcHandler<TResponse> : DbCommandHandler
    {
        public DbCommandInterceptedRpcHandler(DbCommandAttribute annotation) : base(annotation)
        {
        }

        public override Task InvokeAsync(
            IDatabaseRpcProvider provider, 
            IDataContractSerializer serializer,
            object[] args)
        {
            var request = args[0];
            var callback = (Func<TResponse, Task>)args[1];
            var cancellation = (CancellationToken)args[2];

            [DebuggerNonUserCode]
            Task OnResponse(object response) => callback.Invoke((TResponse)response);

            return provider.InvokeAsync(
                Annotation, 
                request, 
                serializer, 
                OnResponse, 
                cancellation);

        }
    }
}
