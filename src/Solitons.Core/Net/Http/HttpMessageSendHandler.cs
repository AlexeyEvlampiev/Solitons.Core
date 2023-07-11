using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

/// <summary>
/// Represents the method that handles sending an HTTP request message.
/// </summary>
/// <param name="request">The HTTP request message to be sent.</param>
/// <param name="cancellation">A cancellation token to observe while waiting for the task to complete.</param>
/// <returns>A task that represents the asynchronous operation. The task result is the HTTP response message.</returns>
public delegate Task<HttpResponseMessage> HttpMessageSendHandler(
    HttpRequestMessage request, 
    CancellationToken cancellation);