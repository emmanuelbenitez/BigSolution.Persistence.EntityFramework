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
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xunit;

namespace BigSolution.Persistence;

public class EntityTypeConfigurationFixture : DbContextFixture
{
    [Fact]
    public void ConfigurationSucceeds()
    {
        _context.ModelCreator = builder => builder.ApplyConfiguration(new FakeEntityConfiguration());
        var entityType = _context.Model.FindEntityType(typeof(FakeEntity));
        entityType!.GetSchema().Should().BeNullOrEmpty();

        var idProperty = entityType.FindProperty("Id");
        idProperty!.IsNullable.Should().BeFalse();
        idProperty.IsPrimaryKey().Should().BeTrue();
        idProperty.ValueGenerated.Should().Be(ValueGenerated.OnAdd);
        var creationDateProperty = entityType.FindProperty("CreationDate");
        creationDateProperty!.IsNullable.Should().BeFalse();
        creationDateProperty.IsShadowProperty().Should().BeTrue();
        creationDateProperty.ClrType.Should().Be<DateTimeOffset>();
        creationDateProperty.ValueGenerated.Should().Be(ValueGenerated.OnAdd);
        var lastUpdateDateProperty = entityType.FindProperty("LastUpdateDate");
        lastUpdateDateProperty!.IsNullable.Should().BeTrue();
        lastUpdateDateProperty.IsShadowProperty().Should().BeTrue();
        lastUpdateDateProperty.ClrType.Should().Be<DateTimeOffset?>();
        lastUpdateDateProperty.ValueGenerated.Should().Be(ValueGenerated.OnUpdate);
        var rowVersionProperty = entityType.FindProperty("RowVersion");
        rowVersionProperty!.IsShadowProperty().Should().BeTrue();
        rowVersionProperty.ClrType.Should().Be<byte[]>();
        rowVersionProperty.ValueGenerated.Should().Be(ValueGenerated.OnAddOrUpdate);
        rowVersionProperty.IsConcurrencyToken.Should().BeTrue();
    }

    private sealed class FakeEntity : Entity<int> { }

    private sealed class FakeEntityConfiguration : EntityTypeConfiguration<FakeEntity, int>
    {
        #region Base Class Member Overrides

        protected override void ConfigureInternal(EntityTypeBuilder<FakeEntity> builder) { }

        #endregion
    }
}