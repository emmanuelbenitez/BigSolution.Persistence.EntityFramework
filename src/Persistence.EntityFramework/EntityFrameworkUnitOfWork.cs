using System;
using System.Threading;
using System.Threading.Tasks;
using BigSolution.Infra.Domain;
using Microsoft.EntityFrameworkCore;

namespace BigSolution.Infra.Persistence
{
    public abstract class EntityFrameworkUnitOfWork<TDbContext> : IUnitOfWork, IDisposable
        where TDbContext : DbContext
    {
        protected EntityFrameworkUnitOfWork(TDbContext dbContext)
        {
            Requires.NotNull(dbContext, nameof(dbContext));

            _dbContext = dbContext;
        }

        #region IDisposable Members

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        #endregion

        #region IUnitOfWork Members

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public ITransaction BeginTransaction()
        {
            return new EntityFrameworkTransaction(_dbContext.Database.BeginTransaction());
        }

        public async Task SaveAsync(CancellationToken cancellationToken = default)
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        #endregion

        private readonly DbContext _dbContext;
    }
}
