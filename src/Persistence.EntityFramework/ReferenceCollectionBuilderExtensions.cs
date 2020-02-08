using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Infra.Persistence
{
    public static class ReferenceCollectionBuilderExtensions
    {
        public static void SetNavigationPropertyAccessMode<TEntity, TProperty>(
            this EntityTypeBuilder<TEntity> builder,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            PropertyAccessMode? propertyAccessMode)
            where TEntity : class
        {
            Requires.NotNull(builder, nameof(builder));
            Requires.NotNull(propertyExpression, nameof(propertyExpression));

            builder.Metadata.FindNavigation(propertyExpression.GetPropertyAccess())
                .SetPropertyAccessMode(propertyAccessMode);
        }
    }
}
