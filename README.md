# BetterResult

[![NuGet](https://img.shields.io/nuget/v/BetterResult.svg)](https://www.nuget.org/packages/BetterResult) [![BuildStatus](https://github.com/cds-git/BetterResult/actions/workflows/dotnet.yml/badge.svg)](https://github.com/cds-git/BetterResult/actions/workflows/dotnet.yml)

**BetterResult** is a lightweight library that adopts a functional programming approach to error handling. By representing both successes and errors as explicit values, it forces developers to acknowledge and manage errors at every step, resulting in clearer, more robust, and maintainable code.

---

## Table of Contents

- [Installation](#installation)
- [Why Use The Result Pattern?](#why-use-the-result-pattern)
- [Usage](#usage)
- [Methods](#methods)
  - [Bind](#bind)
  - [Map](#map)
  - [MapError](#maperror)
  - [Tap](#tap)
  - [TapError](#tapError)
  - [Match](#match)
- [Error Type](#error-type)
- [Credits - Inspiration](#credits---inspiration)

---

## Installation

You can install the **BetterResult** NuGet package via the NuGet Package Manager, .NET CLI, or by adding the package reference to your `.csproj` file.

### NuGet Package Manager

```bash
Install-Package BetterResult
```

### .NET CLI

```bash
dotnet add package BetterResult
```

### Package Reference

```xml
<PackageReference Include="BetterResult" Version="x.x.x" />
```

Replace `x.x.x` with the latest version available on [NuGet](https://www.nuget.org/packages/BetterResult).

---

## Why Use The Result Pattern?

The Result Pattern embraces a functional approach to error handling by treating both successes and failures as explicit values. Instead of relying on exceptions—which incur performance costs and can obscure the flow of error handling—this pattern requires every operation to return a well-defined result. This enforcement means that developers must address potential errors at every step, either by handling them immediately or by deliberately propagating them.

This approach offers several key benefits:

- **Improved Performance:** Representing errors as values avoids the overhead associated with exception handling and stack trace generation.
- **Explicit Error Handling:** Developers are compelled to consider and manage error conditions, leading to more robust and predictable code.
- **Enhanced Composability:** Functional composition of operations becomes straightforward, as each function explicitly returns a result that can be seamlessly chained with others.
- **Increased Code Clarity:** By making error handling an integral part of the function's contract, the intent of the code becomes clearer, resulting in better maintainability.

Overall, the Result Pattern encourages disciplined error management, ensuring that failures are neither ignored nor hidden, but are treated as first-class citizens in your application's design.

---

## Usage

### Basic Usage

#### `Result<TValue>`

Represents either a successful outcome carrying a value of type `TValue` or a failure carrying an `Error`.
You can create instances by implicit casting or factory methods:

```csharp
Result<int> success = 42;
Result<int> failure = Error.Failure("E001", "An error occurred");

var explicitSuccess = Result.Success(100);
var explicitFailure = Result.Failure<int>(Error.Failure("E002", "Something went wrong"));
```

#### Accessing Values and Errors

```csharp
if (result.IsSuccess)
{
    Console.WriteLine($"Value: {result.Value}");
}
else
{
    Console.WriteLine($"Error: {result.Error.Message}");
}
```

#### `Result` (No Value)

Ideal for operations that only indicate success or failure (similar to `void`).

```csharp
public Result DoWork()
{
    return someCondition
        ? Result.Success()
        : Result.Failure(Error.Failure("E003", "Work failed"));
}
```

---

## Methods

These extension methods allow functional composition, transformation, side effects, and recovery in a fluent style.

### Bind

Chains a successful `Result<T1>` into a `Result<T2>`, propagating errors.

```csharp
var result = Result.Success(10);
var doubled = result.Bind(x => Result.Success(x * 2));  // Success(20)
var failureChain = result.Bind(x => Result.Failure<int>(Error.Failure("E004", "Bad")));
```

**Async:**

```csharp
var asyncChain = await result.BindAsync(x => Task.FromResult(Result.Success(x + 5)));
```

### Map

Transforms the success value, propagating errors.

```csharp
var text = result.Map(x => x.ToString());
```

**Async:**

```csharp
var asyncText = await result.MapAsync(x => Task.FromResult(x.ToString()));
```

### MapError

Recovers from failures by mapping an `Error` into a new `Result<T>`.

```csharp
var recovered = result.MapError(e => Result.Success(default));
```

**Async:**

```csharp
var asyncRecover = await result.MapErrorAsync(e => Task.FromResult(Result.Success(0)));
```

### Tap

Executes side-effects on success without altering the result.

```csharp
result.Tap(value => Console.WriteLine(value));

bool mutatableBoolean = false;
result.Tap(value => mutatableBoolean = value is not null);
```

**Async:**

```csharp
await result.TapAsync(v => Task.Run(() => Log(v)));

Task<Result<T>> GetResultAsync<T>(T value) => Task.FromResult(Result.Success(value));
await GetResultAsync(42).TapAsync(v => Log(v));
await GetResultAsync(42).TapAsync(v => Task.Run(() => Log(v)));
```

### TapError

Executes side-effects on error without altering the result.

```csharp
result.TapError(error => Console.WriteLine(error.Message));
```

**Async:**

```csharp
await result.TapErrorAsync(error => Task.Run(() => Log(error)));

Task<Result<T>> GetResultAsync<T>() => Task.FromResult(Result.Failure<T>(Error.Validation()));
await GetResultAsync<int>().TapErrorAsync(error => Log(error));
await GetResultAsync<int>().TapErrorAsync(error => Task.Run(() => Log(error)));
```

### Match

Folds a `Result<T>` into a value of type `TResult` by providing handlers for both success and error.

```csharp
var message = result.Match(
    value => $"Value is {value}",
    error => $"Failed: {error.Message}"
);
```

**Async:**

```csharp
var asyncMessage = await result.MatchAsync(
    value => Task.FromResult($"Value: {v}"),
    error => $"Error: {e.Message}"
);
```

```csharp
var asyncMessage = await result.MatchAsync(
    value => SomeAsyncOperation(value),
    error => $"Error: {e.Message}"
);
```

```csharp
var asyncMessage = await result.MatchAsync(
    value => "MyValue",
    error => SomeAsyncOperationBasedOnTheError(error)
);
```

---

## Error Type

The `Error` struct encapsulates failure information with rich metadata and a variety of factory and extension methods.

### Properties

- `ErrorType Type` – Category of the error (e.g., `Failure`, `Unexpected`, `Validation`, etc.).
- `string Code` – Error code that can be used to decifer it e.g. HTTP status codes or localization key.
- `string Message` – Error description.
- `Dictionary<string, object>? Metadata` – Optional contextual data.

### Metadata Accessors

- `T? GetMetadata<T>(string key)` – Retrieves the metadata value for `key`, or `default` if absent (throws `InvalidCastException` if type mismatch).
- `T? GetMetadata<T>()` – Returns the first metadata value matching type `T`, or `default` if none.

### Factory Methods

Use the static factory methods to create standard error instances:

```csharp
var failure    = Error.Failure("E001", "General failure");
var unexpected = Error.Unexpected("E100", "Something went wrong");
var validation = Error.Validation("E102", "Invalid input");
var notFound   = Error.NotFound("E404", "Item not found");
var conflict   = Error.Conflict("E409", "Conflict detected");
var unauthorized = Error.Unauthorized("E401", "Access denied");
var unavailable  = Error.Unavailable("E503", "Service unavailable");
var timeout      = Error.Timeout("E408", "Operation timed out");

// Custom error with specific type
var custom = Error.Create(ErrorType.Validation, "E123", "Custom validation error");
```

### Extension Methods

Fluently augment or transform existing errors:

```csharp
var error = Error.Failure("E001", "Base error");

// Prepend or replace the message
var withMessage  = error.WithMessage("Additional context");

// Merge metadata dictionary
var mergedMeta   = error.WithMetadata(new Dictionary<string, object> {{ "UserId", 42 }});

// Add or update single key
var keyedMeta    = error.WithMetadata("RetryCount", 3);

// Add metadata using type name as key
var typedMeta    = error.WithMetadata(TimeSpan.FromSeconds(30));
```

---

## TODO

- Clean up Do.cs and go through all XML documentation
- Check if tests are sufficient
- Add ToError extension method on Exceptions
- Add alternative where its possible to specify custom error return type

---

## Credits - Inspiration

- [Zoran Horvat](https://www.youtube.com/@zoran-horvat)
- [Gui Ferreira](https://www.youtube.com/watch?v=C_u1WottRA0)
- [Milan Jovanović](https://www.youtube.com/watch?v=WCCkEe_Hy2Y)
- [ErrorOr](https://github.com/amantinband/error-or)
