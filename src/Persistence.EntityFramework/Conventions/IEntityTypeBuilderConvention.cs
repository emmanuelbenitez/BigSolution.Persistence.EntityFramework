#region Copyright & License

// Copyright © 2020 - 2022 Emmanuel Benitez
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
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Persistence.Conventions;

/// <summary>
/// Defines a convention that can be applied to an <see cref="Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder{TEntity}"/>.
/// </summary>
/// <typeparam name="TEntity">
/// The type of the entity to which the convention applies. This type must implement <see cref="BigSolution.Domain.IEntity"/>.
/// </typeparam>
public interface IEntityTypeBuilderConvention<TEntity>
    where TEntity : class, IEntity
{
    /// <summary>
    /// Applies the convention to the specified <see cref="Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder{TEntity}"/>.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder{TEntity}"/> to which the convention is applied.
    /// </param>
    void Apply(EntityTypeBuilder<TEntity> builder);
}