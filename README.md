# EvilBaschdi.Testing

## Source Code of EvilBaschdi.Testing

[![License: MIT](https://img.shields.io/badge/License-MIT-green.svg?style=for-the-badge&)](LICENSE)

### Package Feeds

|                                | Feed Url                                                         |
| :----------------------------- | :--------------------------------------------------------------- |
| ![myget.org][myGetBadge]       | <https://www.myget.org/F/evilbaschdi/api/v3/index.json>          |
| ![codeberg.org][codebergBadge] | <https://codeberg.org/api/packages/evilbaschdi/nuget/index.json> |

### Build and Codefactor

| main                                                         | develop                                                            |
| :----------------------------------------------------------- | :----------------------------------------------------------------- |
| ![evilbaschdi Azure DevOps Build Status][buildStatusMain]    | ![evilbaschdi Azure DevOps Build Status][buildStatusDevelop]       |
| [![CodeFactor][codeFactorMainBadge]][codeFactorMainOverview] | [![CodeFactor][codeFactorDevelopBadge]][codeFactorDevelopOverview] |

### Packages

#### EvilBaschdi.Testing

|                main                | develop                               |
| :--------------------------------: | :------------------------------------ |
| ![MyGet Version][myGetVersionMain] | ![MyGet Version][myGetVersionDevelop] |

|                                | Package Url                                                             |
| :----------------------------- | :---------------------------------------------------------------------- |
| ![myget.org][myGetBadge]       | <https://myget.org/feed/evilbaschdi/package/nuget/evilbaschdi.testing>  |
| ![codeberg.org][codebergBadge] | <https://codeberg.org/evilbaschdi/-/packages/nuget/evilbaschdi.testing> |

[myGetBadge]: https://img.shields.io/badge/MyGet.org-gray?style=for-the-badge&logo=myget
[codebergBadge]: https://img.shields.io/badge/Codeberg-gray?style=for-the-badge&logo=codeberg

[buildStatusMain]: https://img.shields.io/azure-devops/build/evilbaschdi/main/20/main?style=for-the-badge
[buildStatusDevelop]: https://img.shields.io/azure-devops/build/evilbaschdi/main/20/develop?style=for-the-badge
[myGetVersionMain]: https://img.shields.io/myget/evilbaschdi/v/EvilBaschdi.Testing?style=for-the-badge&label=EvilBaschdi.Testing
[myGetVersionDevelop]: https://img.shields.io/myget/evilbaschdi/vpre/EvilBaschdi.Testing?style=for-the-badge&label=EvilBaschdi.Testing
[codeFactorMainBadge]: https://www.codefactor.io/repository/github/evilbaschdi/evilbaschdi.testing/badge/main?style=for-the-badge
[codeFactorMainOverview]: https://www.codefactor.io/repository/github/evilbaschdi/evilbaschdi.testing/overview/main
[codeFactorDevelopBadge]: https://www.codefactor.io/repository/github/evilbaschdi/evilbaschdi.testing/badge/develop?style=for-the-badge
[codeFactorDevelopOverview]: https://www.codefactor.io/repository/github/evilbaschdi/evilbaschdi.testing/overview/develop

## Installation

```
PM> Install-Package EvilBaschdi.Testing
```

## Features

This library provides a comprehensive set of testing utilities and assertion extensions:

- **Guard Clause Assertions** - Verify null guards on async methods
- **Fluent Assertions for Microsoft.Extensions.DependencyInjection** - Assert service configurations
- **FluentAssertions Extensions** - Enhanced testing for dependency injection

## AutoFixture Attributes

### NSubstituteOmitAutoPropertiesTrueAutoDataAttribute

A custom xUnit `[Theory]` attribute that combines AutoFixture's `AutoDataAttribute` with NSubstitute automatic mocking and sets `OmitAutoProperties = true`. This means the fixture will create mocks but will not automatically populate properties—you must explicitly configure them.

```csharp
using EvilBaschdi.Testing;
using Xunit;

[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
public void MyTest(IMyService service, MyDependency dependency)
{
    // service and dependency are automatically created and injected
    // properties are not auto-populated - configure them as needed
}
```

### NSubstituteOmitAutoPropertiesTrueInlineAutoDataAttribute

Combines `InlineAutoDataAttribute` with NSubstitute automatic mocking and `OmitAutoProperties = true`. Allows you to provide inline data values alongside auto-generated dependencies.

```csharp
using EvilBaschdi.Testing;
using Xunit;

[Theory]
[NSubstituteOmitAutoPropertiesTrueInlineAutoData("value1", 42)]
public void MyTest(string inlineValue, int inlineNumber, IMyService service)
{
    // inlineValue and inlineNumber come from the attribute parameters
    // service is automatically created by the fixture
}
```

## Guard Clause Assertions

### Overview

`GuardClauseAssertionExtensions` provides extension methods for verifying that all public asynchronous methods on a specified type have appropriate null guards for their reference type parameters.

```csharp
using EvilBaschdi.Testing.Extensions;
using AutoFixture.Xunit3;

[Theory, NSubstituteOmitAutoPropertiesTrueAutoData]
public void VerifyAllAsyncMethodsHaveNullGuards(GuardClauseAssertion assertion)
{
    assertion.VerifyTask<MyAsyncService>(
        typeof(MyAsyncService).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance));
}
```

### VerifyTask<T>

Verifies that all public async methods on type `T` have null guards for their reference type parameters.

```csharp
var assertion = new GuardClauseAssertion(new Fixture());
assertion.VerifyTask<MyService>(methodInfos);
```

- **Parameters:**
  - `assertion` - The `GuardClauseAssertion` instance
  - `methodInfos` - Collection of methods to verify
- **Throws:** `GuardClauseException` if a method doesn't have proper null guards
- **Note:** Value types and non-reference parameters are automatically skipped

## Fluent Assertions for Microsoft.Extensions.DependencyInjection

This library contains fluent assertion extensions for testing `Microsoft.Extensions.DependencyInjection` service configurations.

> **Note**: This library is based on [FluentAssertions.Microsoft.Extensions.DependencyInjection](https://github.com/zachdean/FluentAssertions.Microsoft.Extensions.DependencyInjection) by [zachdean](https://github.com/zachdean).

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

#### `WithFactory()`

Asserts that the service is registered with a factory function (e.g., via `AddSingleton(provider => ...)`, `AddScoped(provider => ...)`, `AddTransient(provider => ...)`).

```csharp
services.Should()
    .HaveService<ISomeService>()
    .WithFactory();
```

#### `WithFactory(Func<IServiceProvider, TService> expectedFactory)`

Asserts that the service is registered with a specific factory function.

```csharp
services.Should()
    .HaveService<ISomeService>()
    .WithFactory(provider => new SomeService());
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
