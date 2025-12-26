#region Copyright & License

// Copyright © 2020 - 2025 Emmanuel Benitez
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
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Persistence;

/// <summary>
/// Provides extension methods for configuring reference collection properties in Entity Framework Core.
/// </summary>
/// <remarks>
/// This static class contains helper methods to simplify and enhance the configuration of navigation properties
/// and their access modes in the Entity Framework Core model.
/// </remarks>
[UsedImplicitly]
public static class ReferenceCollectionBuilderExtensions
{
    /// <summary>
    /// Configures the access mode for a navigation property in the Entity Framework Core model.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity containing the navigation property.</typeparam>
    /// <typeparam name="TProperty">The type of the navigation property.</typeparam>
    /// <param name="builder">
    /// The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.
    /// </param>
    /// <param name="propertyExpression">
    /// An expression that identifies the navigation property to configure.
    /// </param>
    /// <param name="propertyAccessMode">
    /// The <see cref="PropertyAccessMode"/> to set for the navigation property. Pass <c>null</c> to use the default access mode.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="builder"/> or <paramref name="propertyExpression"/> is <c>null</c>.
    /// </exception>
    /// <remarks>
    /// This method simplifies the configuration of navigation property access modes, allowing developers to specify
    /// whether properties should be accessed via their field, property, or a combination of both.
    /// </remarks>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static void SetNavigationPropertyAccessMode<TEntity, TProperty>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        PropertyAccessMode? propertyAccessMode)
        where TEntity : class
    {
        (builder ?? throw new ArgumentNullException(nameof(builder))).Metadata
            .FindNavigation((propertyExpression ?? throw new ArgumentNullException(nameof(propertyExpression))).GetPropertyAccess())
            ?.SetPropertyAccessMode(propertyAccessMode);
    }
}
