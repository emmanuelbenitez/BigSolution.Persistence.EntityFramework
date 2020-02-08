using System;
using BigSolution.Infra.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moq;
using Xunit;

namespace BigSolution.Infra.Persistence
{
    public class OwnedNavigationBuilderExtensionsFixture
    {
        [Fact]
        public void ConfigureFailed()
        {
            Action action = () => ((OwnedNavigationBuilder<FakeEntity, OwnedProperty>)null).Configure(null);
            action.Should().ThrowExactly<ArgumentNullException>().Where(exception => exception.ParamName == "builder");
        }

        [Fact]
        public void ConfigureSucceedsWithAction()
        {
            var mockedAction = new Mock<Action<OwnedNavigationBuilder<FakeEntity, OwnedProperty>>>();
            var options = new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase("WithSchema")
                .EnableServiceProviderCaching(false)
                .Options;
            using var dbContext = new FakeDbContext(options);
            dbContext.ModelCreating += (sender, builder) => builder.ApplyConfiguration(new FakeEntityConfiguration(mockedAction.Object));
            dbContext.Model.FindEntityType(typeof(FakeEntity));
            mockedAction.Verify(action => action.Invoke(It.IsAny<OwnedNavigationBuilder<FakeEntity, OwnedProperty>>()), Times.Once);
        }

        [Fact]
        public void ConfigureSucceedsWithoutAction()
        {
            var options = new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase("WithSchema")
                .EnableServiceProviderCaching(false)
                .Options;
            using var dbContext = new FakeDbContext(options);
            dbContext.ModelCreating += (sender, builder) => builder.ApplyConfiguration(new FakeEntityConfiguration(null));
            dbContext.Model.FindEntityType(typeof(FakeEntity));
        }

        private class FakeDbContext : DbContext
        {
            public FakeDbContext(DbContextOptions<FakeDbContext> options) : base(options) { }

            #region Base Class Member Overrides

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                ModelCreating?.Invoke(this, modelBuilder);
            }

            #endregion

            #region Events

            public event EventHandler<ModelBuilder> ModelCreating;

            #endregion
        }

        private class FakeEntityConfiguration : EntityTypeConfiguration<FakeEntity, int>
        {
            public FakeEntityConfiguration(Action<OwnedNavigationBuilder<FakeEntity, OwnedProperty>> configureAction)
            {
                _configureAction = configureAction;
            }

            #region Base Class Member Overrides

            protected override void ConfigureInternal(EntityTypeBuilder<FakeEntity> builder)
            {
                builder.OwnsOne(fakeEntity => fakeEntity.OwnedProperty)
                    .Configure(_configureAction);
            }

            #endregion

            private readonly Action<OwnedNavigationBuilder<FakeEntity, OwnedProperty>> _configureAction;
        }

        public sealed class FakeEntity : Entity<int>
        {
            public OwnedProperty OwnedProperty { get; set; }
        }

        public sealed class OwnedProperty { }
    }
}
