using System;
using System.Linq;
using System.Net.Http;

namespace Solitons.Data;


/// <summary>
/// Represents a specialized HttpClient designed to work with database operations. 
/// This class ensures the last handler in the chain is of type DbHttpMessageHandler. 
/// </summary>
public class DbHttpClient : HttpClient
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbHttpClient"/> class.
    /// </summary>
    /// <param name="handler">An <see cref="HttpMessageHandler"/> that will handle sending HTTP requests and receiving HTTP responses.</param>
    /// <exception cref="ArgumentException">Thrown when the last handler in the chain is not of type DbHttpMessageHandler.</exception>
    public DbHttpClient(HttpMessageHandler handler) 
        : base(handler)
    {
        var last = handler
            .UnrollHandlerChain()
            .Last();
        if (last is not DbHttpMessageHandler)
        {
            throw new ArgumentException($"The last handler in the chain must be of type {typeof(DbHttpMessageHandler)}");
        }
    }
}