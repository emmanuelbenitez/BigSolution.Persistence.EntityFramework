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

namespace BigSolution.Persistence.Conventions;

/// <summary>
/// Represents a convention that configures the primary key for an entity type based on its <c>Id</c> property.
/// </summary>
/// <typeparam name="TEntity">
/// The type of the entity for which the convention is applied. Must inherit from <see cref="Entity{TId}"/>.
/// </typeparam>
/// <typeparam name="TId">
/// The type of the identifier for the entity.
/// </typeparam>
/// <remarks>
/// This convention ensures that the <c>Id</c> property is configured as the primary key and applies additional
/// configurations based on whether the identifier is automatically generated.
/// </remarks>
public class IdEntityTypeConvention<TEntity, TId>(bool isAutomaticallyGenerated) : KeyEntityTypeConvention<TEntity, TId>(entity => entity.Id, isAutomaticallyGenerated)
    where TEntity : Entity<TId>;