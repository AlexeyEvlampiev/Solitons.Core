using System.Collections.Generic;

namespace Solitons.Collections
{
    public static class SingleValue
    {
        public static IEnumerable<T> Enumerable<T>(T value)
        {
            yield return value;
        }
    }
}
