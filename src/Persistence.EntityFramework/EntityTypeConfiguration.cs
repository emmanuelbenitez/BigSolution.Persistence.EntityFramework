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

using BigSolution.Domain;
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

        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.ToTable(null, SchemaName);

            var idProperty = builder.Property(x => x.Id)
                .IsRequired()
                .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);

            if (IsIdAutomaticallyGenerated) idProperty.ValueGeneratedOnAdd();
            else idProperty.ValueGeneratedNever();

            builder.HasKey(x => x.Id);

            ConfigureInternal(builder);
        }

        #endregion

        protected virtual bool IsIdAutomaticallyGenerated => true;

        protected virtual string SchemaName => null;

        protected abstract void ConfigureInternal(EntityTypeBuilder<TEntity> builder);
    }
}
