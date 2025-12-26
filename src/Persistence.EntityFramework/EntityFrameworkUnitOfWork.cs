#region Copyright & License

// Copyright © 2020 - 2022 Emmanuel Benitez
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
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using static System.GC;

namespace BigSolution.Persistence;

/// <summary>
/// Provides an implementation of the <see cref="BigSolution.Domain.IUnitOfWork"/> interface using Entity Framework.
/// </summary>
/// <typeparam name="TDbContext">
/// The type of the <see cref="Microsoft.EntityFrameworkCore.DbContext"/> used by this unit of work.
/// </typeparam>
/// <remarks>
/// This abstract class manages the lifecycle of a database context and provides methods to handle transactions
/// and save changes to the database. It ensures proper disposal of the underlying <see cref="Microsoft.EntityFrameworkCore.DbContext"/>.
/// </remarks>
public abstract class EntityFrameworkUnitOfWork<TDbContext>(TDbContext dbContext) : IUnitOfWork, IDisposable
    where TDbContext : DbContext
{
    #region IDisposable Members

    /// <summary>
    /// Releases all resources used by the current instance of the <see cref="EntityFrameworkUnitOfWork{TDbContext}"/> class.
    /// </summary>
    /// <remarks>
    /// This method disposes of the underlying <see cref="Microsoft.EntityFrameworkCore.DbContext"/> to free up resources.
    /// It should be called when the unit of work is no longer needed.
    /// </remarks>
    public void Dispose()
    {
        _dbContext.Dispose();
        SuppressFinalize(_dbContext);
    }

    #endregion

    #region IUnitOfWork Members

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    /// <returns>
    /// An instance of <see cref="BigSolution.Domain.ITransaction"/> representing the initiated transaction.
    /// </returns>
    /// <remarks>
    /// This method creates a new transaction using the underlying <see cref="Microsoft.EntityFrameworkCore.DbContext"/>.
    /// The returned transaction must be explicitly committed or rolled back to ensure proper database state management.
    /// </remarks>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown if the underlying <see cref="Microsoft.EntityFrameworkCore.DbContext"/> is not properly configured
    /// or if a transaction cannot be started.
    /// </exception>
    public ITransaction BeginTransaction()
    {
        return new EntityFrameworkTransaction(_dbContext.Database.BeginTransaction());
    }

    /// <summary>
    /// Saves all changes made in the context to the database.
    /// </summary>
    /// <remarks>
    /// This method commits all tracked changes in the underlying <see cref="Microsoft.EntityFrameworkCore.DbContext"/> 
    /// to the database. It should be called after making modifications to entities to persist those changes.
    /// </remarks>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">
    /// Thrown if an error occurs while saving changes to the database.
    /// </exception>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
    /// Thrown if a concurrency conflict occurs while saving changes to the database.
    /// </exception>
    public void Save()
    {
        _dbContext.SaveChanges();
    }

    /// <summary>
    /// Asynchronously saves all changes made in this unit of work to the underlying database.
    /// </summary>
    /// <param name="cancellationToken">
    /// A <see cref="System.Threading.CancellationToken"/> to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous save operation.
    /// </returns>
    /// <remarks>
    /// This method ensures that all changes tracked by the underlying <see cref="Microsoft.EntityFrameworkCore.DbContext"/>
    /// are persisted to the database. It supports cancellation through the provided <paramref name="cancellationToken"/>.
    /// </remarks>
    /// <exception cref="System.OperationCanceledException">
    /// Thrown if the operation is canceled via the <paramref name="cancellationToken"/>.
    /// </exception>
    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion

    /// <summary>
    /// The underlying <see cref="Microsoft.EntityFrameworkCore.DbContext"/> instance used by this unit of work.
    /// </summary>
    /// <remarks>
    /// This field is initialized during the construction of the <see cref="EntityFrameworkUnitOfWork{TDbContext}"/>.
    /// It is used to manage database operations such as transactions and saving changes.
    /// </remarks>
    private readonly DbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
}