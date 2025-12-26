# BigSolution.Persistence.EntityFramework

[![NuGet](https://img.shields.io/nuget/v/BigSolution.Infra.Persistence.EntityFramework)](https://www.nuget.org/packages/BigSolution.Infra.Persistence.EntityFramework)
[![License: Apache-2.0](https://img.shields.io/badge/license-Apache%202.0-blue.svg)](LICENSE)

A comprehensive Entity Framework Core implementation of the repository pattern with support for unit of work, transactions, and data access conventions.

## Overview

This library provides a robust foundation for building data access layers using Entity Framework Core. It implements the repository and unit of work patterns, along with support for advanced EF Core features like value generators, entity configurations, and type builder conventions.

## Features

- **Repository Pattern**: Generic repository implementation for aggregate root entities
- **Unit of Work Pattern**: Transaction management and change tracking across repositories
- **Entity Configurations**: Fluent API for entity type mapping and configuration
- **Builder Conventions**: Extensible conventions system for entity type configuration
- **Value Generators**: Custom value generators (e.g., DateTimeOffset)
- **Audit Support**: Shadow properties for audit trails
- **Multi-Framework Support**: Targets .NET 8.0, .NET 9.0, and .NET 10.0

## Installation

Install via NuGet:

```bash
dotnet add package BigSolution.Infra.Persistence
```

Or using Package Manager Console:

```powershell
Install-Package BigSolution.Infra.Persistence
```

## Quick Start

### Basic Repository Usage

```csharp
// Create a repository for your aggregate root
public class OrderRepository : EntityFrameworkRepository<MyDbContext, Order>
{
    public OrderRepository(MyDbContext context) : base(context)
    {
    }
}

// Use the repository
var repository = new OrderRepository(dbContext);
var order = new Order { /* ... */ };
repository.Add(order);
```

### Unit of Work

```csharp
var unitOfWork = new EntityFrameworkUnitOfWork(dbContext);

// Perform operations across multiple repositories
var orderRepository = new OrderRepository(dbContext);
var customerRepository = new CustomerRepository(dbContext);

orderRepository.Add(newOrder);
customerRepository.Update(updatedCustomer);

// Save all changes in a single transaction
await unitOfWork.SaveAsync();
```

### Entity Type Configuration

```csharp
public class OrderConfiguration : EntityTypeConfiguration<Order>
{
    public override void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.OrderNumber).IsRequired();
        builder.Property(o => o.CreatedAt).HasValueGenerator<NowDateTimeOffsetValueGenerator>();
    }
}
```

### Type Builder Conventions

```csharp
public class IdEntityTypeConvention : IEntityTypeBuilderConvention<IEntity>
{
    public void Apply(EntityTypeBuilder<IEntity> builder)
    {
        // Apply conventions automatically to all entities
        builder.Property(e => e.Id).HasDefaultValueSql("NEWID()");
    }
}
```

## Project Structure

```
src/
├── Persistence.EntityFramework/          # Main library
│   ├── Conventions/                      # Entity type builder conventions
│   ├── Extensions/                       # Extension methods
│   ├── ValueGenerators/                  # Custom value generators
│   ├── DbContextBase.cs                  # Base DbContext implementation
│   ├── EntityFrameworkRepository.cs      # Generic repository
│   ├── EntityFrameworkUnitOfWork.cs      # Unit of work implementation
│   ├── EntityFrameworkTransaction.cs     # Transaction management
│   ├── EntityTypeConfiguration.cs        # Entity configuration base class
│   └── DbInitializer.cs                  # Database initialization
├── Persistence.EntityFramework.Unit/     # Unit tests
└── Persistence.EntityFramework.Tests/    # Integration tests
```

## Core Components

### EntityFrameworkRepository

Generic repository implementation providing CRUD operations:
- `Add(TAggregate entity)` - Add an entity
- `Delete(TAggregate entity)` - Remove an entity
- `Update(TAggregate entity)` - Update an entity
- `Entities` - Get queryable collection

### EntityFrameworkUnitOfWork

Manages transactions and coordinates multiple repositories:
- `SaveAsync()` - Save all pending changes
- `BeginTransactionAsync()` - Start a transaction
- `CommitAsync()` - Commit the transaction
- `RollbackAsync()` - Rollback changes

### DbContextBase

Abstract base class for your DbContext implementation with built-in convention support.

### Conventions

Extensible convention system for automatically configuring entity types:
- `IdEntityTypeConvention` - Configure Id properties
- `KeyEntityTypeConvention` - Configure key properties
- `AuditShadowPropertiesConvention` - Add audit timestamp properties
- `IEntityTypeBuilderConvention<T>` - Interface for custom conventions

## Supported Frameworks

- .NET 8.0
- .NET 9.0
- .NET 10.0

## Dependencies

- [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/)
- [Microsoft.EntityFrameworkCore.Relational](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Relational/)
- [BigSolution.Infra.Domain](https://github.com/emmanuelbenitez/BigSolution.Domain)

## License

Licensed under the Apache License, Version 2.0. See the [LICENSE](LICENSE) file for details.

## Contributing

Contributions are welcome! Please feel free to submit a pull request or open an issue for bugs and feature requests.

## Repository

[https://github.com/emmanuelbenitez/BigSolution.Persistence.EntityFramework](https://github.com/emmanuelbenitez/BigSolution.Persistence.EntityFramework)
