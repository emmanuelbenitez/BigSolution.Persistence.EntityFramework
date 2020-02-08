using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BigSolution.Infra.Persistence
{
    public abstract class DbInitializer<TContext> : IDbInitializer
        where TContext : DbContext
    {
        private readonly TContext _context;

        protected DbInitializer(TContext context)
        {
            Requires.NotNull(context, nameof(context));

            _context = context;
        }

        public void Seed()
        {
            if (_context.Database.GetMigrations().Any())
                _context.Database.Migrate();
            else
                _context.Database.EnsureCreated();

            SeedData(_context);
        }

        protected virtual void SeedData(TContext context)
        {
        }
    }
}