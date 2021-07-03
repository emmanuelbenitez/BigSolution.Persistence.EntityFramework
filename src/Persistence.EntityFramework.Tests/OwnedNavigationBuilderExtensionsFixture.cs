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
using BigSolution.Persistence.Unit;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Moq;
using Xunit;

namespace BigSolution.Persistence
{
    public class OwnedNavigationBuilderExtensionsFixture : DbContextFixture
    {
        [SuppressMessage("ReSharper", "AssignNullToNotNullAttribute", Justification = "Testing purpose")]
        [Fact]
        public void ConfigureFailed()
        {
            Action action = () => ((OwnedNavigationBuilder<FakeEntity, OwnedProperty>) null).Configure(null);
            action.Should().ThrowExactly<ArgumentNullException>().Where(exception => exception.ParamName == "builder");
        }

        [Fact]
        public void ConfigureSucceedsWithAction()
        {
            var mockedAction = new Mock<Action<OwnedNavigationBuilder<FakeEntity, OwnedProperty>>>();
            _context.ModelCreator = builder => builder.ApplyConfiguration(new FakeEntityConfiguration(mockedAction.Object));
            _context.Model.FindEntityType(typeof(FakeEntity));
            mockedAction.Verify(action => action.Invoke(It.IsAny<OwnedNavigationBuilder<FakeEntity, OwnedProperty>>()), Times.Once);
        }

        [Fact]
        public void ConfigureSucceedsWithoutAction()
        {
            _context.ModelCreator = builder => builder.ApplyConfiguration(new FakeEntityConfiguration(null));
            _context.Model.FindEntityType(typeof(FakeEntity));
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
            public OwnedProperty OwnedProperty { get; [UsedImplicitly] set; }
        }

        [UsedImplicitly]
        public sealed class OwnedProperty { }
    }
}
