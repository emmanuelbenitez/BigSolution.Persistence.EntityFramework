#region Copyright & License

// Copyright © 2020 - 2025 Emmanuel Benitez
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using BigSolution.Domain;
using Microsoft.EntityFrameworkCore;

namespace BigSolution.Persistence;

/// <summary>
/// Represents a base repository implementation using Entity Framework.
/// </summary>
/// <typeparam name="TDbContext">
/// The type of the <see cref="DbContext"/> used by the repository.
/// </typeparam>
/// <typeparam name="TAggregate">
/// The type of the aggregate root entity managed by the repository.
/// </typeparam>
/// <remarks>
/// This class provides basic CRUD operations for aggregate root entities
/// and relies on the underlying <see cref="DbContext"/> for data persistence.
/// </remarks>
public abstract class EntityFrameworkRepository<TDbContext, TAggregate>(TDbContext dbContext) : IRepository<TAggregate>
    where TAggregate : class, IAggregateRoot
    where TDbContext : DbContext
{
    #region IRepository<TAggregate> Members

    /// <summary>
    /// Adds the specified aggregate entity to the repository.
    /// </summary>
    /// <param name="entity">The aggregate entity to add.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This method delegates the addition of the entity to the underlying <see cref="DbContext"/>.
    /// </remarks>
    public void Add(TAggregate entity)
    {
        _dbContext.Add(entity ?? throw new ArgumentNullException(nameof(entity)));
    }

    /// <summary>
    /// Deletes the specified aggregate entity from the repository.
    /// </summary>
    /// <param name="entity">The aggregate entity to delete.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This method delegates the removal of the entity to the underlying <see cref="DbContext"/>.
    /// </remarks>
    public void Delete(TAggregate entity)
    {
        _dbContext.Remove(entity);
    }

    /// <summary>
    /// Gets the queryable collection of aggregate root entities managed by the repository.
    /// </summary>
    /// <value>
    /// An <see cref="IQueryable{T}"/> representing the aggregate root entities.
    /// </value>
    /// <remarks>
    /// This property provides access to the underlying <see cref="DbSet{TEntity}"/> of the <see cref="DbContext"/>.
    /// It allows querying the entities using LINQ.
    /// </remarks>
    public IQueryable<TAggregate> Entities => _dbContext.Set<TAggregate>();

    /// <summary>
    /// Updates the specified aggregate entity in the repository.
    /// </summary>
    /// <param name="entity">The aggregate entity to update.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <paramref name="entity"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This method delegates the update of the entity to the underlying <see cref="DbContext"/>.
    /// </remarks>
    public void Update(TAggregate entity)
    {
        _dbContext.Update(entity ?? throw new ArgumentNullException(nameof(entity)));
    }

    #endregion

    /// <summary>
    /// The <see cref="DbContext"/> instance used by the repository for data persistence.
    /// </summary>
    /// <remarks>
    /// This field is initialized through the constructor and is used to perform CRUD operations
    /// on the aggregate root entities managed by the repository.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the provided <see cref="DbContext"/> instance is <c>null</c>.
    /// </exception>
    private readonly TDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
}
