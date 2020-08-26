#region Copyright & License

// Copyright © 2020 - 2020 Emmanuel Benitez
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

using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Infra.Persistence
{
    public static class ReferenceCollectionBuilderExtensions
    {
        public static void SetNavigationPropertyAccessMode<TEntity, TProperty>(
            this EntityTypeBuilder<TEntity> builder,
            Expression<Func<TEntity, TProperty>> propertyExpression,
            PropertyAccessMode? propertyAccessMode)
            where TEntity : class
        {
            Requires.NotNull(builder, nameof(builder));
            Requires.NotNull(propertyExpression, nameof(propertyExpression));

            builder.Metadata.FindNavigation(propertyExpression.GetPropertyAccess())
                .SetPropertyAccessMode(propertyAccessMode);
        }
    }
}
