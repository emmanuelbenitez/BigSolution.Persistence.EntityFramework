// ReSharper disable All
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BigSolution.Infra.Persistence
{
    //public class EntityFrameworkQueryableExtensionsAdapter : IQueryableExtensionsAdapter
    //{
    //    #region Nested Type: IncludableQueryable

    //    private class IncludableQueryable<TEntity, TProperty> : IIncludableQueryable<TEntity, TProperty>
    //    {
    //        public IncludableQueryable(
    //            Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, TProperty> queryable)
    //        {
    //            _queryable = queryable;
    //        }

    //        #region IIncludableQueryable<TEntity,TProperty> Members

    //        public Type ElementType => _queryable.ElementType;

    //        public Expression Expression => _queryable.Expression;

    //        public IQueryProvider Provider => _queryable.Provider;

    //        public IEnumerator<TEntity> GetEnumerator()
    //        {
    //            return _queryable.GetEnumerator();
    //        }

    //        IEnumerator IEnumerable.GetEnumerator()
    //        {
    //            return GetEnumerator();
    //        }

    //        #endregion

    //        internal readonly Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<TEntity, TProperty> _queryable;
    //    }

    //    #endregion

    //    #region IQueryableExtensionsAdapter Members

    //    public IIncludableQueryable<TEntity, TProperty> IncludeProperty<TEntity, TProperty>(
    //        IQueryable<TEntity> source,
    //        Expression<Func<TEntity, TProperty>> includedProperty)
    //        where TEntity : class
    //    {
    //        return new IncludableQueryable<TEntity, TProperty>(
    //            EntityFrameworkQueryableExtensions.Include(source, includedProperty));
    //    }

    //    public IIncludableQueryable<TEntity, TProperty> ThenIncludeProperty<TEntity, TPreviousProperty, TProperty>(
    //        IIncludableQueryable<TEntity, TPreviousProperty> source,
    //        Expression<Func<TPreviousProperty, TProperty>> includedProperty)
    //        where TEntity : class
    //    {
    //        var queryable = source as IncludableQueryable<TEntity, TPreviousProperty>;

    //        Ensures.NotNull(queryable, string.Empty);

    //        return new IncludableQueryable<TEntity, TProperty>(queryable._queryable.ThenInclude(includedProperty));
    //    }

    //    public IIncludableQueryable<TEntity, TProperty> ThenIncludeProperty<TEntity, TPreviousProperty, TProperty>(
    //        IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source,
    //        Expression<Func<TPreviousProperty, TProperty>> includedProperty) where TEntity : class
    //    {
    //        var queryable = source as IncludableQueryable<TEntity, IEnumerable<TPreviousProperty>>;

    //        Ensures.NotNull(queryable, string.Empty);

    //        return new IncludableQueryable<TEntity, TProperty>(queryable._queryable.ThenInclude(includedProperty));
    //    }

    //    #endregion
    //}
}
