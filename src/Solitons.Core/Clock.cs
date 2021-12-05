using System;

namespace Solitons
{
    sealed class Clock : IClock
    {
        public static readonly IClock System = new Clock();

        private Clock() { }

        public DateTimeOffset Now => DateTimeOffset.Now;
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
