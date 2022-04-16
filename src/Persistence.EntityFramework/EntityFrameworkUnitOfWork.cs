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

namespace BigSolution.Persistence;

public abstract class EntityFrameworkUnitOfWork<TDbContext> : IUnitOfWork, IDisposable
    where TDbContext : DbContext
{
    protected EntityFrameworkUnitOfWork([NotNull] TDbContext dbContext)
    {
        Requires.Argument(dbContext, nameof(dbContext))
            .IsNotNull()
            .Check();

        _dbContext = dbContext;
    }

    #region IDisposable Members

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    #endregion

    #region IUnitOfWork Members

    public ITransaction BeginTransaction()
    {
        return new EntityFrameworkTransaction(_dbContext.Database.BeginTransaction());
    }

    public void Save()
    {
        _dbContext.SaveChanges();
    }

    public async Task SaveAsync(CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    #endregion

    private readonly DbContext _dbContext;
}