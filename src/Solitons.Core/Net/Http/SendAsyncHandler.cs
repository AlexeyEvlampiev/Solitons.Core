using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

internal delegate Task<HttpResponseMessage> SendAsyncHandler(HttpRequestMessage request, CancellationToken cancellation);