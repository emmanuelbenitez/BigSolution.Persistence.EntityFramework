using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace BigSolution.Infra.Persistence
{
    public abstract class DbContextBase<TDbContext> : DbContext
        where TDbContext : DbContextBase<TDbContext>
    {
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        protected DbContextBase(DbContextOptions<TDbContext> options)
            : base(options) { }

        protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}
