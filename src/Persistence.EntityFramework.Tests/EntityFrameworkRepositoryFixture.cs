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
using System.Diagnostics.CodeAnalysis;
using BigSolution.Domain;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace BigSolution.Persistence;

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
        Action act = () => new FakeRepository(null);
        act.Should().ThrowExactly<ArgumentNullException>().Where(exception => exception.ParamName == "dbContext");
    }

    [Fact]
    [SuppressMessage("ReSharper", "ObjectCreationAsStatement")]
    public void CreateSucceeds()
    {
        Action act = () => new FakeRepository(new Mock<DbContext>().Object);
        act.Should().NotThrow();
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
    public void GetEntitiesSucceeds()
    {
        var mockedContext = new Mock<DbContext>();
        mockedContext.Setup(context => context.Set<FakeEntity>())
            .Returns(new Mock<DbSet<FakeEntity>>().Object);
        var repository = new FakeRepository(mockedContext.Object);
        repository.Entities.Should().NotBeNull();
        mockedContext.Verify(context => context.Set<FakeEntity>(), Times.Once);
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

    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public sealed class FakeEntity : Entity<int>, IAggregateRoot { }

    private sealed class FakeRepository : EntityFrameworkRepository<DbContext, FakeEntity>
    {
        public FakeRepository(DbContext context) : base(context) { }
    }
}