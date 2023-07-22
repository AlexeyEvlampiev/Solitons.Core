using System;

namespace Solitons.Net.Http;

/// <summary>
/// Represents errors that occur during the conversion of HTTP messages 
/// from and to their binary representations using the <see cref="HttpMessageBinaryFormatter"/>.
/// </summary>
public sealed class HttpMessageBinaryFormatterException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpMessageBinaryFormatterException"/> class.
    /// </summary>
    public HttpMessageBinaryFormatterException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpMessageBinaryFormatterException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public HttpMessageBinaryFormatterException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="HttpMessageBinaryFormatterException"/> class 
    /// with a specified error message and a reference to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="inner">The exception that is the cause of the current exception. If the innerException parameter is not a null reference, 
    /// the current exception is raised in a catch block that handles the inner exception.</param>
    public HttpMessageBinaryFormatterException(string message, Exception inner)
        : base(message, inner)
    {
    }
}
