#region Copyright & License

// Copyright © 2020 - 2021 Emmanuel Benitez
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

using Microsoft.EntityFrameworkCore;

namespace BigSolution.Persistence;

public class FakeDbContext : DbContext
{
    public FakeDbContext()
        : this(
            new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase("WithSchema")
                .EnableServiceProviderCaching(false)
                .Options) { }

    public FakeDbContext(DbContextOptions<FakeDbContext> options) : base(options) { }

    #region Base Class Member Overrides

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ModelCreator?.Invoke(modelBuilder);
    }

    #endregion

    public Action<ModelBuilder> ModelCreator { get; set; }
}