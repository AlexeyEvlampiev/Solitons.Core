using System;

namespace Solitons
{
    public class NullOrWhiteSpaceStringException : Exception
    {
        public NullOrWhiteSpaceStringException()
        {
        }

        public NullOrWhiteSpaceStringException(string message) : base(message)
        {
        }

        public NullOrWhiteSpaceStringException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
