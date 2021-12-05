using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Solitons.Reflection
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GenericTaskConverter
    {
        private readonly ConcurrentDictionary<Type, Func<object, Task>> _converters = new();


        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public Task Convert(object target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (target is Task task)
                return task;
            var factory = _converters.GetOrAdd(target.GetType(), CreateTaskConverter);
            return factory.Invoke(target);
        }

        private Func<object, Task> CreateTaskConverter(Type resultType)
        {
            if (resultType == null) throw new ArgumentNullException(nameof(resultType));

            if (typeof(Exception).IsAssignableFrom(resultType))
            {
                // ReSharper disable once PossibleNullReferenceException
                var mi = typeof(Task).GetMethod(nameof(Task.FromException)).MakeGenericMethod(resultType);
                [DebuggerNonUserCode]
                Task FromException(object obj) => (Task)mi.Invoke(null, new[] { obj });
                return FromException;
            }
            else
            {
                // ReSharper disable once PossibleNullReferenceException
                var mi = typeof(Task).GetMethod(nameof(Task.FromResult)).MakeGenericMethod(resultType);
                [DebuggerNonUserCode]
                Task FromResult(object obj) => (Task)mi.Invoke(null, new[] { obj });
                return FromResult;
            }
        }
    }
}
