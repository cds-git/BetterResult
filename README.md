# BetterResult

[![NuGet](https://img.shields.io/nuget/v/BetterResult.svg)](https://www.nuget.org/packages/BetterResult) [![BuildStatus](https://github.com/cds-git/BetterResult/actions/workflows/dotnet.yml/badge.svg)](https://github.com/cds-git/BetterResult/actions/workflows/dotnet.yml)

**BetterResult** is a lightweight library that adopts a functional programming approach to error handling. By representing both successes and errors as explicit values, it forces developers to acknowledge and manage errors at every step, resulting in clearer, more robust, and maintainable code.

---

## Table of Contents

- [Installation](#installation)
  - [NuGet Package Manager](#nuget-package-manager)
  - [.NET CLI](#net-cli)
  - [Package Reference](#package-reference)
- [Why Use The Result Pattern?](#why-use-the-result-pattern)
- [Usage](#usage)
  - [Basic Usage](#basic-usage)
    - [`Result<TValue>`](#resulttvalue)
    - [Accessing Values and Errors](#accessing-values-and-errors)
    - [`Result` (Base Result)](#result-base-result)
  - [Methods](#methods)
    - [Match](#match)
    - [Switch](#switch)
    - [Then](#then)
    - [Else](#else)
    - [Tap](#tap)
    - [TryCatch](#trycatch)
- [Creating and Extending Errors](#creating-and-extending-errors)
  - [Creating an Error](#creating-an-error)
  - [Extending the Error](#extending-the-error)
  - [Retrieving Metadata](#retrieving-metadata)
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
`Result<TValue>` represents either a successful outcome carrying a value of type `TValue` or a failure carrying an `Error`. You can create `Result<TValue>` either by implicit casting or via factory methods.

1. **Using Implicit Casting:**
   ```csharp
   // Implicitly cast a value to a success result
   Result<int> result = 42;

   // Implicitly cast an Error to a failure result
   Result<int> result = Error.Failure("E001", "Some error occurred");
   ```

2. **Using Factory Methods:**
   ```csharp
   // Create an explicit success result
   var result = Result.Success(42);

   // Create an explicit failure result
   var result = Error.Failure("E001", "Some error occurred");
   ```

#### Accessing Values and Errors
After obtaining a result, you can check its state and access the success value or error details accordingly:

```csharp
if (result.IsSuccess)
{
    Console.WriteLine($"Success! Value: {result.Value}");
}
else
{
    Console.WriteLine($"Failure. Error: {result.Error.Description}");
}
```

#### `Result` (Base Result)
The base `Result` type is ideal for operations that do not return a value (similar to `void`), but still indicate success or failure.

1. **Using Factory Methods:**
   ```csharp
   public Result SomeOperation()
   {
       if (someCondition)
       {
           return Result.Success();  // Success with no value
       }
       else
       {
           return Result.Failure(Error.Failure("E002", "An error occurred"));
       }
   }
   ```

2. **Accessing the Error:**
   ```csharp
   var operationResult = SomeOperation();

   if (operationResult.IsFailure)
   {
       Console.WriteLine($"Operation failed: {operationResult.Error.Description}");
   }
   else
   {
       Console.WriteLine("Operation succeeded!");
   }
   ```

---

### Methods

These methods allow you to compose operations in a functional style—executing further actions based on the success or failure of the current result.

#### Match
`Match` lets you handle both success and failure cases by executing one of two provided functions based on the result state.

```csharp
var result = Result.Success(42);

var message = result.Match(
    onSuccess: value => $"Success with value {value}",
    onFailure: error => $"Failure: {error.Description}"
);

Console.WriteLine(message);  // Output: Success with value 42
```

##### Match Async
```csharp
var message = await result.MatchAsync(
    onSuccess: value => Task.FromResult($"Success with value {value}"),
    onFailure: error => Task.FromResult($"Failure: {error.Description}")
);
```

#### Switch
`Switch` works like `Match`, but it executes actions (without returning a value) for success or failure.

```csharp
var result = Result.Success(42);

result.Switch(
    onSuccess: value => Console.WriteLine($"Success with value {value}"),
    onFailure: error => Console.WriteLine($"Failure: {error.Description}")
);
```

##### Switch Async
```csharp
await result.SwitchAsync(
    onSuccess: value => Task.Run(() => Console.WriteLine($"Success with value {value}")),
    onFailure: error => Task.Run(() => Console.WriteLine($"Failure: {error.Description}"))
);
```

#### Then
`Then` chains operations that return a `Result`. If the current result is successful, the provided function is executed and its result is returned. 
If the current result is a failure, the error is propagated without invoking the function.

```csharp
var result = Result.Success(42);

var finalResult = result.Then(value => Result.Success(value * 2));  // Success(84)

var failureResult = result.Then(value => Result.Failure<int>(Error.Failure("E002", "Calculation failed")));  // Failure
```

##### Then Async
```csharp
var finalResult = await result.ThenAsync(value => Task.FromResult(Result.Success(value * 2)));
```

#### Else
`Else` handles failure cases by executing an alternative function if the current result is a failure. If the result is successful, it remains unchanged.

```csharp
var result = Result.Failure(Error.Failure("E003", "Initial failure"));

var alternativeResult = result.Else(error => Result.Success());  // Returns an alternative success result

var unchangedResult = Result.Success().Else(error => Result.Failure(Error.Failure("E004", "Handled error")));  // Remains successful
```

##### Else Async
```csharp
var alternativeResult = await result.ElseAsync(error => Task.FromResult(Result.Success()));
```


#### Tap
`Tap` is used for executing side effects when the result is successful without altering the result itself.

```csharp
var result = Result.Success(42);

result.Tap(value => Console.WriteLine($"Logging value: {value}"));  // Logs: Logging value: 42
```

##### Tap Async
```csharp
await result.TapAsync(value => Task.Run(() => Console.WriteLine($"Logging value: {value}")));
```

#### TryCatch
`TryCatch` handles exceptions within operations. It executes the provided function, returning a failure with the specified error if an exception occurs, or a success result if the operation completes without error.

```csharp
var result = Result.Success(42);

var finalResult = result.TryCatch(
    onSuccess: value => $"Processed value: {value}",
    error: Error.Failure("E002", "Error occurred")
);
```

##### TryCatch Async
```csharp
var finalResult = await result.TryCatchAsync(
    onSuccess: value => Task.FromResult($"Processed value: {value}"),
    error: Error.Failure("E002", "Error occurred")
);
```

---

### Creating and Extending Errors

**BetterResult** provides a robust `Error` struct that you can extend with rich metadata for enhanced error tracing and debugging.

#### Creating an Error

```csharp
var error = Error.Failure("E001", "An error occurred");
```

#### Extending the Error

You can enhance an error with additional context or metadata:

```csharp
var error = Error.Failure("E001", "An error occurred");

// Extend the error message
var extendedError = error.WithMessage("Additional context");

// Add metadata
var errorWithMetadata = extendedError.WithMetadata("RetryCount", 3);
```

You may also add multiple metadata items using a dictionary:

```csharp
var additionalMetadata = new Dictionary<string, object>
{
    { "UserId", 12345 },
    { "Source", "API Gateway" }
};

var finalError = errorWithMetadata.WithMetadata(additionalMetadata);
```

#### Retrieving Metadata

Retrieve metadata from an error either by key or by type:

```csharp
var retryCount = finalError.GetMetadataByKey<int>("RetryCount");

var userId = finalError.GetMetadataByType<int>();
```

---

## TODO

- Refactor implementation to be more modern and functional style
- Add ToError extension method on Exceptions
- Rename methods to make more sense for a functional style
- Update GetMetadata methods to return null rather than throw exceptions
- Rename Error.Description to Error.Message

---

## Credits - Inspiration

- [Gui Ferreira](https://www.youtube.com/watch?v=C_u1WottRA0)
- [Milan Jovanović](https://www.youtube.com/watch?v=WCCkEe_Hy2Y)
- [ErrorOr](https://github.com/amantinband/error-or)
- [OneOf](https://github.com/mcintyre321/OneOf)

