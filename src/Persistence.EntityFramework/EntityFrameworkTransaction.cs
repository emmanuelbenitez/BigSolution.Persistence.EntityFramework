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
using Microsoft.EntityFrameworkCore.Storage;

namespace BigSolution.Persistence;

/// <summary>
/// Represents a transaction that is managed by Entity Framework.
/// </summary>
/// <remarks>
/// This class provides methods to commit or roll back a transaction, either synchronously or asynchronously.
/// It wraps an <see cref="Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction"/> instance to integrate with Entity Framework.
/// </remarks>
public sealed class EntityFrameworkTransaction(IDbContextTransaction transaction) : ITransaction
{
    #region ITransaction Members

    /// <summary>
    /// Releases all resources used by the current instance of <see cref="EntityFrameworkTransaction"/>.
    /// </summary>
    /// <remarks>
    /// This method disposes the underlying <see cref="Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction"/> instance.
    /// It should be called when the transaction is no longer needed to free up resources.
    /// </remarks>
    public void Dispose()
    {
        _transaction.Dispose();
    }

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    /// <remarks>
    /// This method finalizes the transaction by saving all changes made during the transaction scope.
    /// If the commit operation fails, an exception will be thrown.
    /// </remarks>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">
    /// Thrown if an error occurs while committing the transaction.
    /// </exception>
    public void Commit()
    {
        _transaction.Commit();
    }

    /// <summary>
    /// Asynchronously commits the current transaction.
    /// </summary>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to cancel the commit operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous commit operation.
    /// </returns>
    /// <remarks>
    /// This method finalizes the transaction by saving all changes made during the transaction scope.
    /// If the commit operation fails, an exception will be thrown.
    /// </remarks>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">
    /// Thrown if an error occurs while committing the transaction.
    /// </exception>
    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.CommitAsync(cancellationToken);
    }

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <remarks>
    /// This method reverts all changes made during the transaction scope.
    /// It should be used when an error occurs, or the transaction cannot be completed successfully.
    /// </remarks>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">
    /// Thrown if an error occurs while rolling back the transaction.
    /// </exception>
    public void Rollback()
    {
        _transaction.Rollback();
    }

    /// <summary>
    /// Asynchronously rolls back the current transaction.
    /// </summary>
    /// <param name="cancellationToken">
    /// A <see cref="CancellationToken"/> that can be used to cancel the rollback operation.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous rollback operation.
    /// </returns>
    /// <remarks>
    /// This method undoes all changes made during the transaction scope.
    /// If the rollback operation fails, an exception will be thrown.
    /// </remarks>
    /// <exception cref="Microsoft.EntityFrameworkCore.DbUpdateException">
    /// Thrown if an error occurs while rolling back the transaction.
    /// </exception>
    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.RollbackAsync(cancellationToken);
    }

    #endregion

    /// <summary>
    /// Represents the underlying Entity Framework transaction.
    /// </summary>
    /// <remarks>
    /// This field holds the <see cref="Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction"/> instance
    /// that is used to manage the database transaction.
    /// </remarks>
    private readonly IDbContextTransaction _transaction = transaction ?? throw new ArgumentNullException(nameof(transaction));
}
