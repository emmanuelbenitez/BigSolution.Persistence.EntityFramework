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
using BigSolution.Persistence.ValueGenerators;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Persistence.Conventions;

public class AuditShadowPropertiesConvention<TEntity> : IEntityTypeBuilderConvention<TEntity>
    where TEntity : class, IEntity
{
    #region IEntityTypeBuilderConvention<TEntity> Members

    public void Apply(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property<DateTimeOffset>("CreationDate")
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasValueGenerator<NowDateTimeOffsetValueGenerator>();
        builder.Property<DateTimeOffset?>("LastUpdateDate")
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<NowDateTimeOffsetValueGenerator>();
        builder.Property<byte[]>("RowVersion")
            .IsRowVersion();
    }

    #endregion
}