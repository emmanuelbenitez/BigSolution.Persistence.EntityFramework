﻿#region Copyright & License

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
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BigSolution.Persistence;

public static class OwnedNavigationBuilderExtensions
{
    [SuppressMessage("ReSharper", "UnusedMethodReturnValue.Global")]
    public static OwnedNavigationBuilder<TEntity, TRelatedEntity> Configure<TEntity, TRelatedEntity>(
        [JetBrains.Annotations.NotNull] this OwnedNavigationBuilder<TEntity, TRelatedEntity> builder,
        Action<OwnedNavigationBuilder<TEntity, TRelatedEntity>> configureAction)
        where TRelatedEntity : class
        where TEntity : class
    {
        Requires.Argument(builder, nameof(builder))
            .IsNotNull()
            .Check();

        configureAction?.Invoke(builder);

        return builder;
    }
}