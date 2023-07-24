using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

public delegate Task<bool> HttpTransactionCallback(HttpResponseMessage request, CancellationToken cancellation = default);