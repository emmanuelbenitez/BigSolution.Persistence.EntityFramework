using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Moq;
using Xunit;

namespace BigSolution.Infra.Persistence
{
    public class DbInitializerFixture
    {
        [Fact]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void CreateFailed()
        {
            Action action = () => new FakeDbInitializer(null);
            action.Should().ThrowExactly<ArgumentNullException>().Where(exception => exception.ParamName == "context");
        }

        [Fact]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void CreateSucceeds()
        {
            Action action = () => new FakeDbInitializer(new Mock<DbContext>().Object);
            action.Should().NotThrow();
        }

        [Fact]
        public void MigrateSucceeds()
        {
            var mockedMigrationAssembly = new Mock<IMigrationsAssembly>();
            mockedMigrationAssembly.SetupGet(assembly => assembly.Migrations)
                .Returns(new Dictionary<string, TypeInfo> { { "", typeof(string).GetTypeInfo() } });
            var mockedMigrator = new Mock<IMigrator>();
            var mockedServiceProvider = new Mock<IServiceProvider>();
            mockedServiceProvider.Setup(provider => provider.GetService(typeof(IMigrationsAssembly)))
                .Returns(() => mockedMigrationAssembly.Object);
            mockedServiceProvider.Setup(provider => provider.GetService(typeof(IMigrator)))
                .Returns(() => mockedMigrator.Object);
            var mockedContext = new Mock<DbContext>();
            var mockedDatabase = new Mock<DatabaseFacade>(mockedContext.Object);
            mockedDatabase.As<IInfrastructure<IServiceProvider>>()
                .SetupGet(infrastructure => infrastructure.Instance)
                .Returns(() => mockedServiceProvider.Object);
            mockedContext.SetupGet(context => context.Database)
                .Returns(mockedDatabase.Object);
            var dbInitializer = new FakeDbInitializer(mockedContext.Object);

            Action action = () => dbInitializer.Seed();
            action.Should().NotThrow();
            mockedMigrator.Verify(migrator => migrator.Migrate(It.IsAny<string>()), Times.Once);
            mockedDatabase.Verify(database => database.EnsureCreated(), Times.Never);
        }

        [Fact]
        public void CreatedSucceeds()
        {
            var mockedMigrationAssembly = new Mock<IMigrationsAssembly>();
            mockedMigrationAssembly.SetupGet(assembly => assembly.Migrations)
                .Returns(new Dictionary<string, TypeInfo>());
            var mockedMigrator = new Mock<IMigrator>();
            var mockedServiceProvider = new Mock<IServiceProvider>();
            mockedServiceProvider.Setup(provider => provider.GetService(typeof(IMigrationsAssembly)))
                .Returns(() => mockedMigrationAssembly.Object);
            mockedServiceProvider.Setup(provider => provider.GetService(typeof(IMigrator)))
                .Returns(() => mockedMigrator.Object);
            var mockedContext = new Mock<DbContext>();
            var mockedDatabase = new Mock<DatabaseFacade>(mockedContext.Object);
            mockedDatabase.As<IInfrastructure<IServiceProvider>>()
                .SetupGet(infrastructure => infrastructure.Instance)
                .Returns(() => mockedServiceProvider.Object);
            mockedContext.SetupGet(context => context.Database)
                .Returns(mockedDatabase.Object);
            var dbInitializer = new FakeDbInitializer(mockedContext.Object);

            Action action = () => dbInitializer.Seed();
            action.Should().NotThrow();
            mockedMigrator.Verify(migrator => migrator.Migrate(It.IsAny<string>()), Times.Never);
            mockedDatabase.Verify(database => database.EnsureCreated(), Times.Once);
        }

        private sealed class FakeDbInitializer : DbInitializer<DbContext>
        {
            public FakeDbInitializer(DbContext context) : base(context) { }
        }
    }
}
