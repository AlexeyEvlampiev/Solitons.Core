using System.Diagnostics;
using System.Threading.Tasks;

namespace Solitons.Reflection
{
    public abstract class AsyncInvocationCallback : InvocationCallback
    {
        public abstract Task InvokeAsync(object[] args);

        [DebuggerStepThrough]
        public sealed override object Invoke(object[] args) => InvokeAsync(args);

    }

    public abstract class AsyncInvocationCallback<T> : InvocationCallback
    {
        public abstract Task<T> InvokeAsync(object[] args);

        [DebuggerStepThrough]
        public sealed override object Invoke(object[] args) => InvokeAsync(args);
    }
}
