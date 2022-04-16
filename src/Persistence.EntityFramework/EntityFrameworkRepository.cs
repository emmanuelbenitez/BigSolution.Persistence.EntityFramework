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

public abstract class EntityFrameworkRepository<TDbContext, TAggregate> : IRepository<TAggregate>
    where TAggregate : class, IAggregateRoot
    where TDbContext : DbContext
{
    protected EntityFrameworkRepository([NotNull] TDbContext dbContext)
    {
        Requires.Argument(dbContext, nameof(dbContext))
            .IsNotNull()
            .Check();

        _dbContext = dbContext;
    }

    #region IRepository<TAggregate> Members

    public void Add(TAggregate entity)
    {
        Requires.Argument(entity, nameof(entity))
            .IsNotNull()
            .Check();

        _dbContext.Add(entity);
    }

    public void Delete(TAggregate entity)
    {
        Requires.Argument(entity, nameof(entity))
            .IsNotNull()
            .Check();

        _dbContext.Remove(entity);
    }

    public IQueryable<TAggregate> Entities => _dbContext.Set<TAggregate>();

    public void Update(TAggregate entity)
    {
        Requires.Argument(entity, nameof(entity))
            .IsNotNull()
            .Check();

        _dbContext.Update(entity);
    }

    #endregion

    private readonly TDbContext _dbContext;
}