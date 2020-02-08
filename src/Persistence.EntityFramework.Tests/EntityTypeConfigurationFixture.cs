using System;
using BigSolution.Infra.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Xunit;

namespace BigSolution.Infra.Persistence
{
    public class EntityTypeConfigurationFixture
    {
        [Fact]
        public void ConfigurationSucceeds()
        {
            var options = new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase("Default")
                .EnableServiceProviderCaching(false)
                .Options;

            using var dbContext = new FakeDbContext(options);
            dbContext.ModelCreating += (sender, builder) => builder.ApplyConfiguration(new FakeEntityConfiguration());
            var entityType = dbContext.Model.FindEntityType(typeof(FakeEntity));
            entityType.GetSchema().Should().BeNullOrEmpty();

            var idProperty = entityType.FindProperty("Id");
            idProperty.Should().Match<IProperty>(o => !o.IsNullable && o.IsPrimaryKey() && o.ValueGenerated == ValueGenerated.OnAdd);
        }

        [Fact]
        public void ConfigurationSucceedsWithNoIdGenerated()
        {
            var options = new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase("WithNoIdGenerated")
                .EnableServiceProviderCaching(false)
                .Options;
            using var dbContext = new FakeDbContext(options);
            dbContext.ModelCreating += (sender, builder) => builder.ApplyConfiguration(new FakeEntityConfiguration(false));

            var entityType = dbContext.Model.FindEntityType(typeof(FakeEntity));
            entityType.GetSchema().Should().BeNullOrEmpty();

            var idProperty = entityType.FindProperty("Id");
            idProperty.Should().Match<IProperty>(o => !o.IsNullable && o.IsPrimaryKey() && o.ValueGenerated == ValueGenerated.Never);
        }

        [Fact]
        public void ConfigurationSucceedsWithSchema()
        {
            var options = new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase("WithSchema")
                .EnableServiceProviderCaching(false)
                .Options;
            using var dbContext = new FakeDbContext(options);
            const string schemaName = "schema";
            dbContext.ModelCreating += (sender, builder) => builder.ApplyConfiguration(new FakeEntityConfiguration(schemaName: schemaName));

            var entityType = dbContext.Model.FindEntityType(typeof(FakeEntity));
            entityType.GetSchema().Should().Be(schemaName);

            var idProperty = entityType.FindProperty("Id");
            idProperty.Should().Match<IProperty>(o => !o.IsNullable && o.IsPrimaryKey() && o.ValueGenerated == ValueGenerated.OnAdd);
        }

        private sealed class FakeEntity : Entity<int> { }

        private sealed class FakeEntityConfiguration : EntityTypeConfiguration<FakeEntity, int>
        {
            public FakeEntityConfiguration(bool isIdAutomaticallyGenerated = true, string schemaName = null)
            {
                IsIdAutomaticallyGenerated = isIdAutomaticallyGenerated;
                SchemaName = schemaName;
            }

            #region Base Class Member Overrides

            protected override void ConfigureInternal(EntityTypeBuilder<FakeEntity> builder) { }

            protected override bool IsIdAutomaticallyGenerated { get; }

            protected override string SchemaName { get; }

            #endregion
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
    }
}
