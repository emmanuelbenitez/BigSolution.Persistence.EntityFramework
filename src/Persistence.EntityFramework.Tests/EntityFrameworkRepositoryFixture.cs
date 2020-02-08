using System;
using System.Diagnostics.CodeAnalysis;
using BigSolution.Infra.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BigSolution.Infra.Persistence
{
    public class EntityFrameworkRepositoryFixture
    {
        [Fact]
        public void AddFailed()
        {
            var mockedContext = new Mock<DbContext>();
            var repository = new FakeRepository(mockedContext.Object);
            Action action = () => repository.Add(null);
            action.Should().ThrowExactly<ArgumentNullException>().Where(exception => exception.ParamName == "entity");
        }

        [Fact]
        public void AddSucceeds()
        {
            var mockedContext = new Mock<DbContext>();
            var repository = new FakeRepository(mockedContext.Object);
            var entity = new FakeEntity();
            repository.Add(entity);
            mockedContext.Verify(context => context.Add(It.IsIn(entity)), Times.Once);
        }

        [Fact]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void CreateFailed()
        {
            Action action = () => new FakeRepository(null);
            action.Should().ThrowExactly<ArgumentNullException>().Where(exception => exception.ParamName == "dbContext");
        }

        [Fact]
        [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
        public void CreateSucceeds()
        {
            Action action = () => new FakeRepository(new Mock<DbContext>().Object);
            action.Should().NotThrow();
        }

        [Fact]
        public void DeleteFailed()
        {
            var mockedContext = new Mock<DbContext>();
            var repository = new FakeRepository(mockedContext.Object);
            Action action = () => repository.Delete(null);
            action.Should().ThrowExactly<ArgumentNullException>().Where(exception => exception.ParamName == "entity");
        }

        [Fact]
        public void DeleteSucceeds()
        {
            var mockedContext = new Mock<DbContext>();
            var repository = new FakeRepository(mockedContext.Object);
            var entity = new FakeEntity();
            repository.Delete(entity);
            mockedContext.Verify(context => context.Remove(It.IsIn(entity)), Times.Once);
        }

        [Fact]
        public void UpdateFailed()
        {
            var mockedContext = new Mock<DbContext>();
            var repository = new FakeRepository(mockedContext.Object);
            Action action = () => repository.Update(null);
            action.Should().ThrowExactly<ArgumentNullException>().Where(exception => exception.ParamName == "entity");
        }

        [Fact]
        public void UpdateSucceeds()
        {
            var mockedContext = new Mock<DbContext>();
            var repository = new FakeRepository(mockedContext.Object);
            var entity = new FakeEntity();
            repository.Update(entity);
            mockedContext.Verify(context => context.Update(It.IsIn(entity)), Times.Once);
        }

        [Fact]
        public void GetEntitiesSucceeds()
        {
            var mockedContext = new Mock<DbContext>();
            mockedContext.Setup(context => context.Set<FakeEntity>())
                .Returns(new Mock<DbSet<FakeEntity>>().Object);
            var repository = new FakeRepository(mockedContext.Object);
            repository.Entities.Should().NotBeNull();
            mockedContext.Verify(context => context.Set<FakeEntity>(), Times.Once);
        }

        [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
        public sealed class FakeEntity : Entity<int>, IAggregateRoot { }

        private sealed class FakeRepository : EntityFrameworkRepository<DbContext, FakeEntity>
        {
            public FakeRepository(DbContext context) : base(context) { }
        }
    }
}
