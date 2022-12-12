using System;


namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NullOrEmptyValueException : NullOrWhiteSpaceStringException
    {
        /// <summary>
        /// 
        /// </summary>
        public NullOrEmptyValueException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public NullOrEmptyValueException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public NullOrEmptyValueException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
