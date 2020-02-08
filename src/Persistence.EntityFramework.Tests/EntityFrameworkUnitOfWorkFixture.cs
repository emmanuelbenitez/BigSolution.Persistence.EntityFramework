using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using Xunit;

namespace BigSolution.Infra.Persistence
{
    public class EntityFrameworkUnitOfWorkFixture
    {
        [Fact]
        public void BeginTransactionSucceeds()
        {
            var mockedContext = new Mock<DbContext>();
            var mockedDatabase = new Mock<DatabaseFacade>(mockedContext.Object);
            mockedDatabase.Setup(facade => facade.BeginTransaction())
                .Returns(new Mock<IDbContextTransaction>().Object);
            mockedContext.SetupGet(context => context.Database)
                .Returns(mockedDatabase.Object);
            var unitOfWork = new FakeUnitOfWork(mockedContext.Object);

            unitOfWork.BeginTransaction().Should().NotBeNull().And.BeOfType<EntityFrameworkTransaction>();
            mockedDatabase.Verify(facade => facade.BeginTransaction(), Times.Once);
        }

        [Fact]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void CreateFailed()
        {
            Action action = () => new FakeUnitOfWork(null);
            action.Should().ThrowExactly<ArgumentNullException>().Where(exception => exception.ParamName == "dbContext");
        }

        [Fact]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void CreateSucceeds()
        {
            Action action = () => new FakeUnitOfWork(new Mock<DbContext>().Object);
            action.Should().NotThrow();
        }

        [Fact]
        public void DisposeSucceeds()
        {
            var mockedContext = new Mock<DbContext>();
            using (new FakeUnitOfWork(mockedContext.Object)) { }
            mockedContext.Verify(context => context.Dispose(), Times.Once);
        }

        [Fact]
        public void SaveAsyncSucceeds()
        {
            var mockedContext = new Mock<DbContext>();
            var unitOfWork = new FakeUnitOfWork(mockedContext.Object);

            var cancellationToken = new CancellationToken();
            unitOfWork.SaveAsync(cancellationToken).Wait(cancellationToken);
            mockedContext.Verify(context => context.SaveChangesAsync(It.IsIn(cancellationToken)), Times.Once);
        }

        [Fact]
        public void SaveSucceeds()
        {
            var mockedContext = new Mock<DbContext>();
            var unitOfWork = new FakeUnitOfWork(mockedContext.Object);

            unitOfWork.Save();
            mockedContext.Verify(context => context.SaveChanges(), Times.Once);
        }

        private sealed class FakeUnitOfWork : EntityFrameworkUnitOfWork<DbContext>
        {
            public FakeUnitOfWork(DbContext dbContext) : base(dbContext) { }
        }
    }
}
