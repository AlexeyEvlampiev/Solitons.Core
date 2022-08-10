using System;

namespace Solitons
{
    public sealed class NullOrEmptyStringException : NullOrWhiteSpaceStringException
    {
        public NullOrEmptyStringException()
        {
        }

        public NullOrEmptyStringException(string message) : base(message)
        {
        }

        public NullOrEmptyStringException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
