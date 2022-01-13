using System;
using System.Threading.Tasks;

namespace Solitons.Reflection
{
    internal abstract class SuperCast
    {
        public static SuperCast Create(Type type)
        {
            var converter = (SuperCast)typeof(SuperCast<>)
                .MakeGenericType(type)
                .GetConstructor(Array.Empty<Type>())!
                .Invoke(Array.Empty<object>());
            return converter;
        }


        public abstract Task Cast(Task<object> task);
    }

    sealed class SuperCast<T> : SuperCast
    {
        public override Task Cast(Task<object> task)
        {
            async Task<T> CastAsync()
            {
                var result = await task;
                return (T)result;
            }

            return CastAsync();
        }
    }
}
