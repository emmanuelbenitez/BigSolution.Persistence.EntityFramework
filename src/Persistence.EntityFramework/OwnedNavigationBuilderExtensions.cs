using System;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Infra.Persistence
{
    public static class OwnedNavigationBuilderExtensions
    {
        public static OwnedNavigationBuilder<TEntity, TRelatedEntity> Configure<TEntity, TRelatedEntity>(
            this OwnedNavigationBuilder<TEntity, TRelatedEntity> builder,
            Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> configureAction)
            where TRelatedEntity : class
            where TEntity : class
        {
            Requires.NotNull(builder, nameof(builder));

            configureAction?.Invoke(builder);

            return builder;
        }
    }
}
