using BigSolution.Infra.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Infra.Persistence {
    public abstract class DerivedEntityTypeConfiguration<TEntity, TId, TBaseType> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity<TId>, TBaseType
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasBaseType<TBaseType>();

            ConfigureInternal(builder);
        }

        protected abstract void ConfigureInternal(EntityTypeBuilder<TEntity> builder);
    }
}
