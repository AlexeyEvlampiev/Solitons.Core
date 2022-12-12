using System;

namespace Solitons
{
    /// <summary>
    /// 
    /// </summary>
    public class NullOrEmptyArgumentException : ArgumentException
    {
        /// <summary>
        /// 
        /// </summary>
        public NullOrEmptyArgumentException()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public NullOrEmptyArgumentException(string message) : base(message)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="paramName"></param>
        public NullOrEmptyArgumentException(string message, string paramName) : base(message, paramName)
        {

        }

    }
}
