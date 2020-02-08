using BigSolution.Infra.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xunit;

namespace BigSolution.Infra.Persistence
{
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

            protected override void ConfigureInternal(EntityTypeBuilder<ChildFakeEntity> builder) { }

            #endregion
        }

        private class FakeDbContext : DbContext
        {
            public FakeDbContext(DbContextOptions<FakeDbContext> options) : base(options) { }

            #region Base Class Member Overrides

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.ApplyConfiguration(new ChildFakeEntityConfiguration());
            }

            #endregion
        }
    }
}
