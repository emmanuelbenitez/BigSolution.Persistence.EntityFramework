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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Persistence;

/// <summary>
/// Represents an abstract base class for configuring the entity type of a derived entity in Entity Framework Core.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being configured.</typeparam>
/// <typeparam name="TId">The type of the identifier for the entity.</typeparam>
/// <typeparam name="TBaseType">The base type of the entity being configured.</typeparam>
/// <remarks>
/// This class provides a mechanism to configure derived entities by specifying their base type and allowing additional configuration
/// through the <see cref="ConfigureInternal"/> method.
/// </remarks>
public abstract class DerivedEntityTypeConfiguration<TEntity, TId, TBaseType> : IEntityTypeConfiguration<TEntity>
    where TEntity : Entity<TId>, TBaseType
{
    #region IEntityTypeConfiguration<TEntity> Members

    /// <summary>
    /// Configures the entity type for the derived entity in the Entity Framework Core model.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.
    /// </param>
    /// <remarks>
    /// This method sets the base type of the entity and invokes the <see cref="ConfigureInternal"/> method
    /// to apply additional configuration specific to the derived entity.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="builder"/> parameter is <c>null</c>.
    /// </exception>
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasBaseType<TBaseType>();

        ConfigureInternal(builder);
    }

    #endregion

    /// <summary>
    /// Provides a mechanism to configure the properties and relationships of the specified entity type.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.
    /// </param>
    /// <remarks>
    /// This method is intended to be implemented in derived classes to define the specific configuration
    /// for the entity type. It allows customization of the entity's properties and relationships during the model creation process.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="builder"/> parameter is <c>null</c>.
    /// </exception>
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    protected abstract void ConfigureInternal(EntityTypeBuilder<TEntity> builder);
}