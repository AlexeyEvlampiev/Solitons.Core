using System;


namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NullOrEmptyGuidException : NullOrWhiteSpaceStringException
    {
        /// <summary>
        /// 
        /// </summary>
        public NullOrEmptyGuidException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public NullOrEmptyGuidException(string message) 
            : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public NullOrEmptyGuidException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
