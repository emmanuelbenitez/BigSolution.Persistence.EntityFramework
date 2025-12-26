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
/// Provides a base implementation for initializing a database context of type <typeparamref name="TContext"/>.
/// </summary>
/// <typeparam name="TContext">
/// The type of the database context, which must derive from <see cref="Microsoft.EntityFrameworkCore.DbContext"/>.
/// </typeparam>
/// <remarks>
/// This class ensures that the database is either created or migrated to the latest version before seeding data.
/// Derived classes can override the <see cref="SeedData"/> method to provide custom data seeding logic.
/// </remarks>
public abstract class DbInitializer<TContext> : IDbInitializer
    where TContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DbInitializer{TContext}"/> class.
    /// </summary>
    /// <param name="context">
    /// The database context of type <typeparamref name="TContext"/> to be initialized. 
    /// This parameter must not be <c>null</c>.
    /// </param>
    /// <exception cref="System.ArgumentNullException">
    /// Thrown when the <paramref name="context"/> is <c>null</c>.
    /// </exception>
    protected DbInitializer([NotNull] TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    #region IDbInitializer Members

    /// <summary>
    /// Ensures that the database associated with the context is properly initialized.
    /// </summary>
    /// <remarks>
    /// If the database supports migrations, it applies any pending migrations to bring the database
    /// to the latest version. Otherwise, it ensures that the database is created if it does not already exist.
    /// Afterward, it invokes the <see cref="SeedData"/> method to populate the database with initial data.
    /// </remarks>
    /// <exception cref="System.InvalidOperationException">
    /// Thrown if the database context is not properly configured or if an error occurs during initialization.
    /// </exception>
    public void Seed()
    {
        if (_context.Database.GetMigrations().Any()) _context.Database.Migrate();
        else _context.Database.EnsureCreated();

        SeedData(_context);
    }

    #endregion

    /// <summary>
    /// Seeds the database with initial data for the specified context.
    /// </summary>
    /// <param name="context">
    /// The database context of type <typeparamref name="TContext"/> to seed data into. 
    /// This parameter must not be <see langword="null"/>.
    /// </param>
    /// <remarks>
    /// Derived classes can override this method to implement custom data seeding logic.
    /// This method is invoked after ensuring that the database is created or migrated to the latest version.
    /// </remarks>
    [SuppressMessage("ReSharper", "VirtualMemberNeverOverridden.Global")]
    [SuppressMessage("ReSharper", "UnusedParameter.Global")]
    protected virtual void SeedData(TContext context) { }

    private readonly TContext _context;
}