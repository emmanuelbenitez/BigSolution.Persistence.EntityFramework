using System.Threading;
using System.Threading.Tasks;
using BigSolution.Infra.Domain;
using Microsoft.EntityFrameworkCore.Storage;

namespace BigSolution.Infra.Persistence
{
    public sealed class EntityFrameworkTransaction : ITransaction
    {
        public EntityFrameworkTransaction(IDbContextTransaction transaction)
        {
            Requires.NotNull(transaction, nameof(transaction));

            _transaction = transaction;
        }

        #region ITransaction Members

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.CommitAsync(cancellationToken);
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.RollbackAsync(cancellationToken);
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        #endregion

        private readonly IDbContextTransaction _transaction;
    }
}
