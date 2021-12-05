using System;
using System.Reflection;
using System.Threading.Tasks;
using Solitons.Reflection;

namespace Solitons
{
    public static partial class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static Func<object[], object> AsInvokeHandler(this Func<object[], Task<object>> self, Type returnType)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (returnType == null) throw new ArgumentNullException(nameof(returnType));
            var asyncHandler = new DispatchProxyAsyncHandler(self);
            var proxy = new DispatchProxyAsyncHandlerProxy(asyncHandler);
            return proxy.Cast(returnType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public static Func<object[], object> AsInvokeHandler(this DispatchProxyAsyncHandler self, Type returnType)
        {
            if (self == null) throw new ArgumentNullException(nameof(self));
            if (returnType == null) throw new ArgumentNullException(nameof(returnType));
            var proxy = new DispatchProxyAsyncHandlerProxy(self);
            return proxy.Cast(returnType);
        }



        class DispatchProxyAsyncHandlerProxy
        {
            private readonly DispatchProxyAsyncHandler _innerHandler;

            public DispatchProxyAsyncHandlerProxy(DispatchProxyAsyncHandler innerHandler)
            {
                _innerHandler = innerHandler ?? throw new ArgumentNullException(nameof(innerHandler));
            }

            public Func<object[], object> Cast(Type returnType)
            {
                var method = GetType()
                    .GetMethod(nameof(InvokeAsync), BindingFlags.NonPublic | BindingFlags.Instance)
                    .ThrowIfNull(()=> new InvalidOperationException($"{nameof(InvokeAsync)} method not found."))
                    .MakeGenericMethod(returnType);

                Task RunAsync(object[] parameters)
                {
                    var result = method.Invoke(this, new[] { parameters });
                    return (Task)result;
                }
                return RunAsync;
            }

            async Task<T> InvokeAsync<T>(object[] parameters)
            {
                var result = await _innerHandler.Invoke(parameters);
                return (T)result;
            }
        }
    }
}
