# EvilBaschdi.Testing

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)
MyGet Feed for NuGet Package: <https://www.myget.org/F/evilbaschdi/api/v3/index.json>

| main                                                         | develop                                                            |
| :----------------------------------------------------------- | :----------------------------------------------------------------- |
| ![evilbaschdi Azure DevOps Build Status][buildStatusMain]    | ![evilbaschdi Azure DevOps Build Status][buildStatusDevelop]       |
| ![MyGet Version][myGetVersionMain]                           | ![MyGet Version][myGetVersionDevelop]                              |
| [![CodeFactor][codeFactorMainBadge]][codeFactorMainOverview] | [![CodeFactor][codeFactorDevelopBadge]][codeFactorDevelopOverview] |

[buildStatusMain]: https://dev.azure.com/evilbaschdi/Main/_apis/build/status/Core/EvilBaschdi.Testing?branchName=main
[buildStatusDevelop]: https://dev.azure.com/evilbaschdi/Main/_apis/build/status/Core/EvilBaschdi.Testing?branchName=develop
[myGetVersionMain]: https://img.shields.io/myget/evilbaschdi/v/EvilBaschdi.Testing?label=MyGet
[myGetVersionDevelop]: https://img.shields.io/myget/evilbaschdi/vpre/EvilBaschdi.Testing?label=MyGet
[codeFactorMainBadge]: https://www.codefactor.io/repository/github/evilbaschdi/evilbaschdi.testing/badge/main
[codeFactorMainOverview]: https://www.codefactor.io/repository/github/evilbaschdi/evilbaschdi.testing/overview/main
[codeFactorDevelopBadge]: https://www.codefactor.io/repository/github/evilbaschdi/evilbaschdi.testing/badge/develop
[codeFactorDevelopOverview]: https://www.codefactor.io/repository/github/evilbaschdi/evilbaschdi.testing/overview/develop

## Fluent Assertions for Microsoft.Extensions.DependencyInjection

This library contains fluent assertion extensions for testing `Microsoft.Extensions.DependencyInjection` service configurations.

> **Note**: This library is based on [FluentAssertions.Microsoft.Extensions.DependencyInjection](https://github.com/zachdean/FluentAssertions.Microsoft.Extensions.DependencyInjection) by [zachdean](https://github.com/zachdean).

### Installation

```
PM> Install-Package EvilBaschdi.Testing
```

### Quick Start

```csharp
using EvilBaschdi.Testing;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<ISomeService, SomeService>();
services.AddTransient<ITransient, Transient>();
services.AddScoped<IScoped, Scoped>();

// Assert that a service is registered with the correct implementation and lifetime
services.Should()
    .HaveService<ISomeService>()
    .WithImplementation<SomeService>()
    .AsSingleton();
```

### API Reference

#### `HaveCount(int expected)`

Asserts that the service collection contains the expected number of service registrations.

```csharp
services.Should().HaveCount(5);
```

#### `HaveService<TService>()`

Asserts that a service of type `TService` is registered in the service collection.

```csharp
services.Should().HaveService<ISomeService>();
```

#### `WithCount(int expected)`

Asserts that the service is registered a specific number of times. Can be chained before lifetime assertions.

```csharp
services.Should()
    .HaveService<ISomeService>()
    .WithCount(2);
```

#### `WithImplementation<TImplementation>()`

Asserts that the service is registered with a specific implementation type. Can be chained for multiple implementations.

```csharp
services.Should()
    .HaveService<ISomeService>()
    .WithImplementation<SomeService>();
```

#### Lifetime Assertions

- **`AsSingleton()`** - Asserts that the service is registered as a singleton
- **`AsScoped()`** - Asserts that the service is registered as scoped
- **`AsTransient()`** - Asserts that the service is registered as transient

```csharp
services.Should()
    .HaveService<ISomeService>()
    .AsSingleton();

services.Should()
    .HaveService<IRepository>()
    .AsTransient();

services.Should()
    .HaveService<IUnitOfWork>()
    .AsScoped();
```

#### Method Chaining with `And()`

Chain multiple assertions together:

```csharp
services.Should()
    .HaveService<ISomeService>()
    .AsSingleton()
    .And()
    .HaveCount(3);
```

### License

Licensed under the MIT License - see LICENSE file for details.
