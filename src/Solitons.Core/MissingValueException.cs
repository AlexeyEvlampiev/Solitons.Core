using System;

namespace Solitons
{
    public sealed class MissingValueException : Exception
    {
        public MissingValueException()
        {
        }

        public MissingValueException(string message) : base(message)
        {
        }

        public MissingValueException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
