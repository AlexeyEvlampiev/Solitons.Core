using System;
using System.Diagnostics;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public static class TryCatch
    {
        [DebuggerStepThrough]
        public static TResult Invoke<TResult>(Func<TResult> func, Action<Exception> onError) =>
            Invoke<TResult, Exception>(func, onError);

        public static TResult Invoke<TResult, TException>(Func<TResult> func, Action<TException> onError) where TException : Exception
        {
            try
            {
                return func.Invoke();
            }
            catch (Exception e) when(e is TException exception)
            {
                onError.Invoke(exception);
                throw;
            }
        }
    }
}
