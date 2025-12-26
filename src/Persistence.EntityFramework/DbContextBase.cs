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
using Microsoft.EntityFrameworkCore;

namespace BigSolution.Persistence;

/// <summary>
/// Represents the base class for a database context in the BigSolution.Persistence namespace.
/// </summary>
/// <typeparam name="TDbContext">
/// The type of the derived database context. It must inherit from <see cref="DbContextBase{TDbContext}"/>.
/// </typeparam>
/// <remarks>
/// This class provides a foundation for Entity Framework Core database contexts, including functionality
/// for applying configurations from the assembly and setting a default schema.
/// </remarks>
[method: SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
public abstract class DbContextBase<TDbContext>(DbContextOptions<TDbContext> options) : DbContext(options)
    where TDbContext : DbContextBase<TDbContext>
{
    #region Base Class Member Overrides

    /// <summary>
    /// Configures the model for the database context.
    /// </summary>
    /// <param name="modelBuilder">
    /// The <see cref="ModelBuilder"/> used to configure the model for the database context.
    /// </param>
    /// <remarks>
    /// This method applies all configurations from the assembly containing the derived database context
    /// and sets the default schema if <see cref="SchemaName"/> is not null or whitespace.
    /// </remarks>
    protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        if (!string.IsNullOrWhiteSpace(SchemaName)) modelBuilder.HasDefaultSchema(SchemaName);
        base.OnModelCreating(modelBuilder);
    }

    #endregion

    /// <summary>
    /// Gets the name of the database schema to be used as the default schema for the context.
    /// </summary>
    /// <value>
    /// A <see cref="string"/> representing the schema name. If <c>null</c> or whitespace, no default schema is set.
    /// </value>
    /// <remarks>
    /// This property can be overridden in derived classes to specify a custom schema name.
    /// </remarks>
    protected virtual string? SchemaName { get; } = null;
}