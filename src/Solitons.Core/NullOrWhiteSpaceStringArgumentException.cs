using System;

namespace Solitons
{
    public sealed class NullOrWhiteSpaceStringArgumentException : ArgumentException
    {
        public NullOrWhiteSpaceStringArgumentException()
        {
        }

        public NullOrWhiteSpaceStringArgumentException(string message) 
            : base(message)
        {
        }

        public NullOrWhiteSpaceStringArgumentException(string message, string paramName) 
            : base(message, paramName)
        {
        }
    }
}
