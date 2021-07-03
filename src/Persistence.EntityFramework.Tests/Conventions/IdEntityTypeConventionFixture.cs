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
using BigSolution.Persistence.Unit.Conventions;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Xunit;

namespace BigSolution.Persistence.Conventions
{
    public class IdEntityTypeConventionFixture :
        EntityTypeBuilderConventionFixture<IdEntityTypeConvention<IdEntityTypeConventionFixture.FakeEntity, int>, IdEntityTypeConventionFixture.FakeEntity>
    {
        [Theory]
        [InlineData(true, ValueGenerated.OnAdd)]
        [InlineData(false, ValueGenerated.Never)]
        public void ConventionApplied(bool isAutomaticallyGenerated, ValueGenerated expectedValueGenerated)
        {
            var entityType = Apply(new IdEntityTypeConvention<FakeEntity, int>(isAutomaticallyGenerated));

            entityType.Should().NotBeNull();
            var idProperty = entityType.FindProperty("Id");
            idProperty.IsNullable.Should().BeFalse();
            idProperty.IsPrimaryKey().Should().BeTrue();
            idProperty.ValueGenerated.Should().Be(expectedValueGenerated);
        }

        [UsedImplicitly]
        public sealed class FakeEntity : Entity<int> { }
    }
}
