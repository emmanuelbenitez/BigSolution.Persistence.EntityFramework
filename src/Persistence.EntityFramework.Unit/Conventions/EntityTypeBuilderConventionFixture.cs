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

using System;
using BigSolution.Domain;
using BigSolution.Persistence.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Persistence.Unit.Conventions
{
    public abstract class EntityTypeBuilderConventionFixture<TConvention, TEntity> : DbContextFixture
        where TConvention : class, IEntityTypeBuilderConvention<TEntity>
        where TEntity : class, IEntity
    {
        #region Nested Type: FakeEntityTypeConfiguration

        private class FakeEntityTypeConfiguration : IEntityTypeConfiguration<TEntity>
        {
            public FakeEntityTypeConfiguration(TConvention convention, bool isKeyless)
            {
                _convention = convention ?? throw new ArgumentNullException(nameof(convention));
                _isKeyless = isKeyless;
            }

            #region IEntityTypeConfiguration<TEntity> Members

            public void Configure(EntityTypeBuilder<TEntity> builder)
            {
                if (_isKeyless) builder.HasNoKey();
                _convention.Apply(builder);
            }

            #endregion

            private readonly TConvention _convention;
            private readonly bool _isKeyless;
        }

        #endregion

        protected IEntityType Apply(TConvention convention, bool isKeyless = false)
        {
            _context.ModelCreator = builder => builder.ApplyConfiguration(new FakeEntityTypeConfiguration(convention, isKeyless));
            return _context.Model.FindEntityType(typeof(TEntity));
        }
    }
}
