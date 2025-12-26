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

using System.Linq.Expressions;
using BigSolution.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Persistence.Conventions;

/// <summary>
/// Represents a convention that configures the primary key for an entity type.
/// </summary>
/// <typeparam name="TEntity">
/// The type of the entity for which the convention is applied. Must be a class that implements <see cref="IEntity"/>.
/// </typeparam>
/// <typeparam name="TKey">
/// The type of the key property for the entity.
/// </typeparam>
/// <remarks>
/// This convention ensures that the specified key property is configured as the primary key and applies additional
/// configurations based on whether the key is automatically generated.
/// </remarks>
public class KeyEntityTypeConvention<TEntity, TKey> : IEntityTypeBuilderConvention<TEntity> where TEntity : class, IEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="KeyEntityTypeConvention{TEntity, TKey}"/> class.
    /// </summary>
    /// <param name="keyPropertyExpression">
    /// An expression that specifies the key property of the entity.
    /// </param>
    /// <param name="isAutomaticallyGenerated">
    /// A value indicating whether the key is automatically generated.
    /// </param>
    /// <remarks>
    /// This constructor sets up the key property expression and determines whether the key is automatically generated.
    /// </remarks>
    protected KeyEntityTypeConvention(Expression<Func<TEntity, TKey>> keyPropertyExpression, bool isAutomaticallyGenerated)
    {
        _keyPropertyExpression = keyPropertyExpression;
        IsAutomaticallyGenerated = isAutomaticallyGenerated;
    }

    #region IEntityTypeBuilderConvention<TEntity> Members

    /// <summary>
    /// Applies the convention to configure the primary key for the specified entity type.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.
    /// </param>
    /// <remarks>
    /// This method configures the primary key for the entity type using the key property expression provided during
    /// the construction of the convention. It also sets whether the key is automatically generated or not.
    /// </remarks>
    public void Apply(EntityTypeBuilder<TEntity> builder)
    {
        var idProperty = builder.Property(_keyPropertyExpression)
            .IsRequired()
            .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction);

        if (IsAutomaticallyGenerated) idProperty.ValueGeneratedOnAdd();
        else idProperty.ValueGeneratedNever();

        builder.HasKey(_keyPropertyExpression.ToObjectExpression()!);
    }

    #endregion

    /// <summary>
    /// Gets a value indicating whether the key for the entity is automatically generated.
    /// </summary>
    /// <value>
    /// <c>true</c> if the key is automatically generated; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    /// This property determines whether the primary key for the entity is configured to be automatically generated
    /// by the database or explicitly set by the application.
    /// </remarks>
    private bool IsAutomaticallyGenerated { get; }

    /// <summary>
    /// Represents an expression that specifies the key property of the entity.
    /// </summary>
    /// <remarks>
    /// This field is used to configure the primary key for the entity type. The expression defines the property
    /// that serves as the key, and it is utilized during the application of the convention to ensure the key
    /// is properly configured.
    /// </remarks>
    private readonly Expression<Func<TEntity, TKey>> _keyPropertyExpression;
}