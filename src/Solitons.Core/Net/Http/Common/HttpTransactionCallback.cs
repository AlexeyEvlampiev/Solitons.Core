using System.Data.Common;
using System.Data;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Solitons.Net.Http.Common
{
    public class HttpTransactionCallback : HttpTransactionCallbackBase
    {
        private readonly IDbTransaction _transaction;

        protected HttpTransactionCallback(
            HttpRequestMessage request,
            HttpResponseMessage response,
            IDbTransaction transaction) : base(request, response)
        {
            _transaction = transaction;
        }


        public override async Task<bool> RollbackIfActiveAsync()
        {
            var connection = _transaction.Connection;
            if (connection?.State == ConnectionState.Open)
            {
                await RollbackAsync(_transaction);
                return true;
            }

            return false;
        }

        protected override async Task<bool> CommitIfActiveAsync()
        {
            var connection = _transaction.Connection;
            if (connection?.State == ConnectionState.Open)
            {
                if (_transaction is DbTransaction common)
                {
                    await common.CommitAsync();
                }
                else
                {
                    _transaction.Commit();
                }
                return true;
            }

            return false;
        }

        [DebuggerStepThrough]
        protected virtual Task RollbackAsync(IDbTransaction transaction)
        {
            if (transaction is DbTransaction common)
            {
                return common.RollbackAsync();
            }
            transaction.Rollback();
            return Task.CompletedTask;
        }
    }
}
