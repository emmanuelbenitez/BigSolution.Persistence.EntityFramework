﻿#region Copyright & License

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
using Microsoft.EntityFrameworkCore.Storage;

namespace BigSolution.Persistence;

public sealed class EntityFrameworkTransaction : ITransaction
{
    public EntityFrameworkTransaction([NotNull] IDbContextTransaction transaction)
    {
        Requires.Argument(transaction, nameof(transaction))
            .IsNotNull()
            .Check();

        _transaction = transaction;
    }

    #region ITransaction Members

    public void Dispose()
    {
        _transaction.Dispose();
    }

    public void Commit()
    {
        _transaction.Commit();
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.CommitAsync(cancellationToken);
    }

    public void Rollback()
    {
        _transaction.Rollback();
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        await _transaction.RollbackAsync(cancellationToken);
    }

    #endregion

    private readonly IDbContextTransaction _transaction;
}