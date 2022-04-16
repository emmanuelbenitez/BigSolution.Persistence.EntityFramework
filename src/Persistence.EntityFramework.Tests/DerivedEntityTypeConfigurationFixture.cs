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
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xunit;

namespace BigSolution.Persistence;

public class DerivedEntityTypeConfigurationFixture
{
    [Fact]
    public void ConfigurationSucceeds()
    {
        var options = new DbContextOptionsBuilder<FakeDbContext>()
            .UseInMemoryDatabase("Default")
            .EnableServiceProviderCaching(false)
            .Options;

        using var dbContext = new FakeDbContext(options);
        var entityType = dbContext.Model.FindEntityType(typeof(FakeEntity));
        var childEntityType = dbContext.Model.FindEntityType(typeof(ChildFakeEntity));
        childEntityType.BaseType.Should().Be(entityType);
    }

    private abstract class FakeEntity : Entity<int> { }

    private sealed class ChildFakeEntity : FakeEntity { }

    private sealed class ChildFakeEntityConfiguration : DerivedEntityTypeConfiguration<ChildFakeEntity, int, FakeEntity>
    {
        #region Base Class Member Overrides

        protected override void ConfigureInternal(EntityTypeBuilder<ChildFakeEntity> builder)
        {
        }

        #endregion
    }

    private class FakeDbContext : DbContext
    {
        public FakeDbContext(DbContextOptions<FakeDbContext> options) : base(options) { }

        #region Base Class Member Overrides

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FakeEntity>()
                .HasNoKey();
            modelBuilder.ApplyConfiguration(new ChildFakeEntityConfiguration());
        }

        #endregion
    }
}