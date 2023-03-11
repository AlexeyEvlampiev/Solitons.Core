using System.Threading;
using System.Threading.Tasks;

namespace Solitons.Net.Http;

/// <summary>
/// Represents a method that intercepts an HTTP transaction.
/// </summary>
/// <param name="callback">The callback that represents the HTTP transaction.</param>
/// <param name="cancellation">The cancellation token.</param>
/// <returns>A task that represents the asynchronous operation.</returns>

public delegate Task HttpTransactionInterceptor(IHttpTransactionCallback callback, CancellationToken cancellation);