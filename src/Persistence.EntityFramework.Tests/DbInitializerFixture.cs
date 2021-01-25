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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Moq;
using Xunit;

namespace BigSolution.Persistence
{
    public class DbInitializerFixture
    {
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

            Action act = () => dbInitializer.Seed();
            act.Should().NotThrow();
            mockedMigrator.Verify(migrator => migrator.Migrate(It.IsAny<string>()), Times.Never);
            mockedDatabase.Verify(database => database.EnsureCreated(), Times.Once);
        }

        [Fact]
        [SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "Testing purpose")]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement", Justification = "Testing purpose")]
        public void CreateFailed()
        {
            Action action = () => new FakeDbInitializer(null);
            action.Should().ThrowExactly<ArgumentNullException>().Where(exception => exception.ParamName == "context");
        }

        [Fact]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement", Justification = "Testing purpose")]
        [SuppressMessage("Performance", "CA1806:Do not ignore method results", Justification = "Testing purpose")]
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

        private sealed class FakeDbInitializer : DbInitializer<DbContext>
        {
            public FakeDbInitializer(DbContext context) : base(context) { }
        }
    }
}
