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
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Persistence;

/// <summary>
/// Provides extension methods for configuring owned navigation properties in Entity Framework Core.
/// </summary>
public static class OwnedNavigationBuilderExtensions
{
    /// <summary>
    /// Configures the specified <see cref="OwnedNavigationBuilder{TEntity,TRelatedEntity}"/> using the provided action.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity that owns the navigation property.</typeparam>
    /// <typeparam name="TRelatedEntity">The type of the related entity being configured.</typeparam>
    /// <param name="builder">
    /// The <see cref="OwnedNavigationBuilder{TEntity,TRelatedEntity}"/> to configure. 
    /// Cannot be <c>null</c>.
    /// </param>
    /// <param name="configureAction">
    /// An optional action to configure the <see cref="OwnedNavigationBuilder{TEntity,TRelatedEntity}"/>. 
    /// If <c>null</c>, no additional configuration is applied.
    /// </param>
    /// <returns>The configured <see cref="OwnedNavigationBuilder{TEntity,TRelatedEntity}"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="builder"/> is <c>null</c>.</exception>
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public static OwnedNavigationBuilder<TEntity, TRelatedEntity> Configure<TEntity, TRelatedEntity>(
        this OwnedNavigationBuilder<TEntity, TRelatedEntity>? builder,
        Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>>? configureAction)
        where TRelatedEntity : class
        where TEntity : class
    {
        configureAction?.Invoke(builder ?? throw new ArgumentNullException(nameof(builder)));

        return builder!;
    }
}
