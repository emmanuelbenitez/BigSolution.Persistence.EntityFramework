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

using System.Diagnostics.CodeAnalysis;
using BigSolution.Domain;
using BigSolution.Persistence.Conventions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Persistence;

/// <summary>
/// Represents the base class for configuring the entity type in the Entity Framework Core model.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
/// <typeparam name="TId">The type of the identifier for the entity.</typeparam>
/// <remarks>
/// This class provides a mechanism to apply conventions and custom configurations to the entity type.
/// </remarks>
[UsedImplicitly]
public abstract class EntityTypeConfiguration<TEntity, TId> : IEntityTypeConfiguration<TEntity>
    where TEntity : Entity<TId>
{
    #region IEntityTypeConfiguration<TEntity> Members

    /// <summary>
    /// Configures the entity type using the specified <see cref="EntityTypeBuilder{TEntity}"/>.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.
    /// </param>
    /// <remarks>
    /// This method applies any conventions defined in <see cref="Conventions"/> and delegates additional configuration
    /// to the <see cref="ConfigureInternal"/> method.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="builder"/> parameter is <c>null</c>.
    /// </exception>
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        foreach (var convention in Conventions)
        {
            convention.Apply(builder);
        }

        ConfigureInternal(builder);
    }

    #endregion

    /// <summary>
    /// Gets the collection of conventions to be applied to the entity type during its configuration.
    /// </summary>
    /// <remarks>
    /// These conventions define reusable rules or behaviors that can be applied to the 
    /// <see cref="Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder{TEntity}"/> 
    /// to standardize and simplify the configuration process.
    /// </remarks>
    /// <value>
    /// A collection of <see cref="IEntityTypeBuilderConvention{TEntity}"/> instances representing the conventions.
    /// </value>
    [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global", Justification = "Public API")]

    protected virtual IEnumerable<IEntityTypeBuilderConvention<TEntity>> Conventions => _defaultConventions;

    /// <summary>
    /// Configures the internal details of the entity type.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.
    /// </param>
    /// <remarks>
    /// This method must be implemented in derived classes to define the specific configuration
    /// for the entity type.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="builder"/> parameter is <c>null</c>.
    /// </exception>
    protected abstract void ConfigureInternal(EntityTypeBuilder<TEntity> builder);

    private static readonly IEntityTypeBuilderConvention<TEntity>[] _defaultConventions = [
        new IdEntityTypeConvention<TEntity, TId>(true),
        new AuditShadowPropertiesConvention<TEntity>()
    ];
}