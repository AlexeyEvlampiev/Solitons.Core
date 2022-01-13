using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Solitons.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class GeneralizedAsyncHandlerObject
    {
        internal GeneralizedAsyncHandlerObject()
        {
        }

        public abstract object Invoke(object[] args);

        [return: NotNull]
        public static GeneralizedAsyncHandlerObject Create(DispatchProxyAsyncHandler innerHandler, Type asyncReturnType)
        {
            var ctor = typeof(GeneralizedAsyncHandlerObject<>)
                .MakeGenericType(asyncReturnType)
                .GetConstructor(new Type[] { typeof(DispatchProxyAsyncHandler) })
                .ThrowIfNull(() => new InvalidOperationException());
            return (GeneralizedAsyncHandlerObject)ctor.Invoke(new object?[] { innerHandler });
        }
    }

    sealed class GeneralizedAsyncHandlerObject<T> : GeneralizedAsyncHandlerObject
    {
        private readonly DispatchProxyAsyncHandler _innerHandler;

        public GeneralizedAsyncHandlerObject(DispatchProxyAsyncHandler innerHandler)
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
}
