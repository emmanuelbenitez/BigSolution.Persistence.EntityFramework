using BigSolution.Infra.Domain;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Infra.Persistence
{
    [UsedImplicitly]
    public abstract class EntityTypeConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity<TId>
    {
        #region IEntityTypeConfiguration<TEntity> Members

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable(null, SchemaName);

            var idProperty = builder.Property(x => x.Id)
                .IsRequired()
                .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);

            if (IsIdAutomaticallyGenerated) idProperty.ValueGeneratedOnAdd();
            else idProperty.ValueGeneratedNever();

            builder.HasKey(x => x.Id);

            ConfigureInternal(builder);
        }

        #endregion

        protected virtual bool IsIdAutomaticallyGenerated => true;

        protected virtual string SchemaName => null;

        protected abstract void ConfigureInternal(EntityTypeBuilder<TEntity> builder);
    }
}
