#region Copyright & License

// Copyright © 2020 - 2021 Emmanuel Benitez
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

using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace BigSolution.Persistence
{
    public abstract class DbInitializer<TContext> : IDbInitializer
        where TContext : DbContext
    {
        protected DbInitializer(TContext context)
        {
            Requires.Argument(context, nameof(context))
                .IsNotNull()
                .Check();

            _context = context;
        }

        #region IDbInitializer Members

        public void Seed()
        {
            if (_context.Database.GetMigrations().Any()) _context.Database.Migrate();
            else _context.Database.EnsureCreated();

            SeedData(_context);
        }

        #endregion

        [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
        [SuppressMessage("ReSharper", "UnusedParameter.Global")]
        protected virtual void SeedData(TContext context) { }

        private readonly TContext _context;
    }
}
