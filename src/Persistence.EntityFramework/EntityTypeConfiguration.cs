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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BigSolution.Domain;
using BigSolution.Persistence.Conventions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Persistence
{
    [UsedImplicitly]
    public abstract class EntityTypeConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
        where TEntity : Entity<TId>
    {
        #region IEntityTypeConfiguration<TEntity> Members

        public void Configure([JetBrains.Annotations.NotNull] EntityTypeBuilder<TEntity> builder)
        {
            foreach (var convention in Conventions ?? Enumerable.Empty<IEntityTypeBuilderConvention<TEntity>>())
            {
                convention.Apply(builder);
            }

            ConfigureInternal(builder);
        }

        #endregion

        [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global", Justification = "Public API")]

        protected virtual IEnumerable<IEntityTypeBuilderConvention<TEntity>> Conventions => _defaultConventions;

        protected abstract void ConfigureInternal([JetBrains.Annotations.NotNull] EntityTypeBuilder<TEntity> builder);

        private static readonly IEntityTypeBuilderConvention<TEntity>[] _defaultConventions = {
            new IdEntityTypeConvention<TEntity, TId>(true),
            new AuditShadowPropertiesConvention<TEntity>()
        };
    }
}
