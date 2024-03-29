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
using Microsoft.EntityFrameworkCore;

namespace BigSolution.Persistence;

public abstract class DbContextBase<TDbContext> : DbContext
    where TDbContext : DbContextBase<TDbContext>
{
    [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
    protected DbContextBase(DbContextOptions<TDbContext> options)
        : base(options) { }

    #region Base Class Member Overrides

    protected sealed override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        if (!string.IsNullOrWhiteSpace(SchemaName)) modelBuilder.HasDefaultSchema(SchemaName);
        base.OnModelCreating(modelBuilder);
    }

    #endregion

    protected virtual string SchemaName { get; } = null;
}