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

namespace BigSolution.Persistence;

/// <summary>
/// Defines the contract for initializing a database.
/// </summary>
/// <remarks>
/// Implementations of this interface are responsible for ensuring that the database is properly initialized,
/// which may include creating the database schema, applying migrations, and seeding initial data.
/// </remarks>
public interface IDbInitializer
{
    [SuppressMessage("ReSharper", "UnusedMemberInSuper.Global")]
    void Seed();
}