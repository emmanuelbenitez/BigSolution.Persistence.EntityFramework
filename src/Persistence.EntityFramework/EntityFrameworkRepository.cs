using System.Linq;
using BigSolution.Infra.Domain;
using Microsoft.EntityFrameworkCore;

namespace BigSolution.Infra.Persistence
{
    public abstract class EntityFrameworkRepository<TDbContext, TAggregate> : IRepository<TAggregate>
        where TAggregate : class, IAggregateRoot
        where TDbContext : DbContext
    {
        private readonly TDbContext _dbContext;

        protected EntityFrameworkRepository(TDbContext dbContext)
        {
            Requires.NotNull(dbContext, nameof(dbContext));

            _dbContext = dbContext;
        }

        public IQueryable<TAggregate> Entities => _dbContext.Set<TAggregate>();

        public void Add(TAggregate entity)
        {
            Requires.NotNull(entity, nameof(entity));

            _dbContext.Add(entity);
        }

        public void Delete(TAggregate entity)
        {
            Requires.NotNull(entity, nameof(entity));

            _dbContext.Remove(entity);
        }

        public void Update(TAggregate entity)
        {
            Requires.NotNull(entity, nameof(entity));

            _dbContext.Update(entity);
        }
    }
}
