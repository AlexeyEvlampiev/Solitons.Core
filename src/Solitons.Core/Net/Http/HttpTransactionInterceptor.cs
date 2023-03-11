using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

public delegate Task HttpTransactionInterceptor(IHttpTransactionCallback callback, CancellationToken cancellation);