#region Copyright & License

// Copyright © 2020 - 2020 Emmanuel Benitez
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

using System.Linq;
using BigSolution.Infra.Domain;
using Microsoft.EntityFrameworkCore;

namespace BigSolution.Infra.Persistence
{
    public abstract class EntityFrameworkRepository<TDbContext, TAggregate> : IRepository<TAggregate>
        where TAggregate : class, IAggregateRoot
        where TDbContext : DbContext
    {
        protected EntityFrameworkRepository(TDbContext dbContext)
        {
            Requires.NotNull(dbContext, nameof(dbContext));

            _dbContext = dbContext;
        }

        #region IRepository<TAggregate> Members

        public void Add(TAggregate entity)
        {
            Requires.NotNull(entity, nameof(entity));

            _dbContext.Add(entity);
        }

        public void Delete(TAggregate entity)
        {
            Requires.NotNull(entity, nameof(entity));

            _dbContext.Remove(entity);
        }

        public IQueryable<TAggregate> Entities => _dbContext.Set<TAggregate>();

        public void Update(TAggregate entity)
        {
            Requires.NotNull(entity, nameof(entity));

            _dbContext.Update(entity);
        }

        #endregion

        private readonly TDbContext _dbContext;
    }
}
