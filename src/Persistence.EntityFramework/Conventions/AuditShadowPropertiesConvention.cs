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
using BigSolution.Persistence.ValueGenerators;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Persistence.Conventions;

/// <summary>
/// Represents a convention that configures audit-related shadow properties for an entity type.
/// </summary>
/// <typeparam name="TEntity">
/// The type of the entity for which the convention is applied. Must be a class implementing <see cref="IEntity"/>.
/// </typeparam>
/// <remarks>
/// This convention adds the following shadow properties to the entity type:
/// <list type="bullet">
/// <item>
/// <description>
/// <c>CreationDate</c>: A required property of type <see cref="DateTimeOffset"/> that is automatically generated on add.
/// </description>
/// </item>
/// <item>
/// <description>
/// <c>LastUpdateDate</c>: An optional property of type <see cref="DateTimeOffset?"/> that is automatically generated on update.
/// </description>
/// </item>
/// <item>
/// <description>
/// <c>RowVersion</c>: A property of type <see cref="byte[]"/> that is used as a row version and concurrency token.
/// </description>
/// </item>
/// </list>
/// </remarks>
public class AuditShadowPropertiesConvention<TEntity> : IEntityTypeBuilderConvention<TEntity>
    where TEntity : class, IEntity
{
    #region IEntityTypeBuilderConvention<TEntity> Members

    /// <summary>
    /// Applies the audit-related shadow properties convention to the specified entity type builder.
    /// </summary>
    /// <param name="builder">
    /// The <see cref="EntityTypeBuilder{TEntity}"/> used to configure the entity type.
    /// </param>
    /// <remarks>
    /// This method configures the following shadow properties for the entity type:
    /// <list type="bullet">
    /// <item>
    /// <description>
    /// <c>CreationDate</c>: A required property of type <see cref="DateTimeOffset"/> that is automatically generated on add.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <c>LastUpdateDate</c>: An optional property of type <see cref="DateTimeOffset?"/> that is automatically generated on update.
    /// </description>
    /// </item>
    /// <item>
    /// <description>
    /// <c>RowVersion</c>: A property of type <see cref="byte[]"/> that is used as a row version and concurrency token.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the <paramref name="builder"/> parameter is <c>null</c>.
    /// </exception>
    public void Apply(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property<DateTimeOffset>("CreationDate")
            .IsRequired()
            .ValueGeneratedOnAdd()
            .HasValueGenerator<NowDateTimeOffsetValueGenerator>();
        builder.Property<DateTimeOffset?>("LastUpdateDate")
            .ValueGeneratedOnUpdate()
            .HasValueGenerator<NowDateTimeOffsetValueGenerator>();
        builder.Property<byte[]>("RowVersion")
            .IsRowVersion();
    }

    #endregion
}