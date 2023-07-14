using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

public sealed class HttpMessageSerializationException : Exception
{
    public HttpMessageSerializationException()
    {
    }

    public HttpMessageSerializationException(string message)
        : base(message)
    {
    }

    public HttpMessageSerializationException(string message, Exception inner)
        : base(message, inner)
    {
    }
}