using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

internal delegate Task<HttpResponseMessage> SendAsyncHandler(HttpRequestMessage request, CancellationToken cancellation);