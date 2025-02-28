# BetterResult

[![NuGet](https://img.shields.io/nuget/v/BetterResult.svg)](https://www.nuget.org/packages/BetterResult)
[![BuildStatus](https://github.com/cds-git/BetterResult/actions/workflows/dotnet.yml/badge.svg)](https://github.com/cds-git/BetterResult/actions/workflows/dotnet.yml)

**BetterResult** helps to streamline your code by providing a simple yet powerful way to handle success and failure states without exceptions, offering a clear, functional approach to error handling. With rich error metadata, it also allows you to extend and trace errors efficiently through your application layers.

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

**BetterResult** is a functional result pattern implementation designed for error control flow, improving the handling of success and failure in your code. 
It simplifies the propagation of errors without the need for exceptions and provides an easy way to compose and transform results with rich error metadata. 
This pattern is particularly useful when working with multiple layers of functions and APIs, making it easier to manage success/failure states and improving the readability of your code.

## Usage

### Basic Usage

#### `Result<TValue>`

`Result<TValue>` is used when you want to return a discriminated union of either a success with a value (`TValue`) or a failure with an `Error`. 
You can create `Result<TValue>` using implicit casting or factory methods.

1. **Using Implicit Casting:**
   - You can implicitly cast a value to a `Result<TValue>` for success or an `Error` for failure.

```csharp
// Implicit casting from a value to a success result
Result<int> result = 42;

// Implicit casting from an Error to a failure result
Result<int> result = Error.Failure("E001", "Some error occurred");
```

2. **Using Factory Methods:**
   - You can explicitly create a success or failure result using the `Result.Success()` and `Result.Failure()` factory methods.

```csharp
// Explicit success result creation
var result = Result.Success(42);

// Explicit failure result creation
var result = Error.Failure("E001", "Some error occurred");
```

#### Accessing Values and Errors
Once you have a result, you can check if it’s a success or failure and access the corresponding value or error.

```csharp
if (result.IsSuccess)
{
    Console.WriteLine($"Success! Value: {result.Value}");  // Output: Success! Value: 42
}
else
{
    Console.WriteLine($"Failure. Error: {result.Error.Description}");
}
```

#### `Result` (Base Result)

The base `Result` type is useful when you need a function that doesn’t return a value (normally `void`), but you still want to indicate success or failure. 
This is ideal for methods where the operation can fail, but no data needs to be returned on success.

1. **Using Factory Methods:**
   - You can use `Result.Success()` to indicate success or `Result.Failure(Error)` to indicate failure.

```csharp
public Result SomeOperation()
{
    // Perform some logic
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
   - If the operation fails, you can access the error for logging or handling purposes.

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

This separation of `Result<TValue>` and `Result` makes it easy to handle both value-returning and void-like operations in a consistent, functional manner.

### Then

`Then` is used to chain operations that return a `Result`. If the current result is a success, the provided function is invoked, and the next result is returned. If the result is a failure, it propagates the error without invoking the function.

```csharp
var result = Result.Success(42);

var finalResult = result.Then(value => Result.Success(value * 2));  // Result: Success(84)

var failureResult = result.Then(value => Result.Failure<int>(Error.Failure("E002", "Calculation failed")));  // Result: Failure
```

#### Async Version
```csharp
var finalResult = await result.ThenAsync(value => Task.FromResult(Result.Success(value * 2)));
```

### Match

`Match` is used to handle both success and failure cases. It executes one of two functions depending on the result state (success or failure).

```csharp
var result = Result.Success(42);

var message = result.Match(
    onSuccess: value => $"Success with value {value}",
    onFailure: error => $"Failure: {error.Description}"
);

Console.WriteLine(message);  // Output: Success with value 42
```

#### Async Version
```csharp
var message = await result.MatchAsync(
    onSuccess: value => Task.FromResult($"Success with value {value}"),
    onFailure: error => Task.FromResult($"Failure: {error.Description}")
);
```

### Switch

`Switch` works similarly to `Match` but doesn't return a value. Instead, it executes actions for success and failure states.

```csharp
var result = Result.Success(42);

result.Switch(
    onSuccess: value => Console.WriteLine($"Success with value {value}"),
    onFailure: error => Console.WriteLine($"Failure: {error.Description}")
);
```

#### Async Version
```csharp
await result.SwitchAsync(
    onSuccess: value => Task.Run(() => Console.WriteLine($"Success with value {value}")),
    onFailure: error => Task.Run(() => Console.WriteLine($"Failure: {error.Description}"))
);
```

### Tap

`Tap` is used for side-effects. It executes an action if the result is a success, without modifying the result itself.

```csharp
var result = Result.Success(42);

result.Tap(value => Console.WriteLine($"Logging value: {value}"));  // Logs: Logging value: 42
```

#### Async Version
```csharp
await result.TapAsync(value => Task.Run(() => Console.WriteLine($"Logging value: {value}")));
```

### TryCatch

`TryCatch` helps handle exceptions inside result-producing operations. If an exception occurs, it returns a failure with the specified error; otherwise, it returns a success result.

```csharp
var result = Result.Success(42);

var finalResult = result.TryCatch(
    onSuccess: value => $"Processed value: {value}",
    error: Error.Failure("E002", "Error occurred")
);
```

#### Async Version
```csharp
var finalResult = await result.TryCatchAsync(
    onSuccess: value => Task.FromResult($"Processed value: {value}"),
    error: Error.Failure("E002", "Error occurred")
);
```

---

### Creating and Extending Errors

**BetterResult** provides a rich `Error` struct that can be created using factory methods for different error types (e.g., `Failure`, `Validation`, `NotFound`, etc.). Additionally, you can use extension methods to enhance existing errors with more context or metadata.

#### Creating an Error

```csharp
var error = Error.Failure("E001", "An error occurred");
```

#### Extending the Error

You can extend the error message or add metadata to an existing error, allowing better traceability.

```csharp
var error = Error.Failure("E001", "An error occurred");

// Extend the error message
var extendedError = error.WithMessage("Additional context");

// Add metadata
var errorWithMetadata = extendedError.WithMetadata("RetryCount", 3);
```

You can also extend the metadata using a `Dictionary<string, object>`:

```csharp
var additionalMetadata = new Dictionary<string, object>
{
    { "UserId", 12345 },
    { "Source", "API Gateway" }
};

var finalError = errorWithMetadata.WithMetadata(additionalMetadata);
```

### Retrieving Metadata

You can retrieve metadata from an error by either key or type.

```csharp
var retryCount = finalError.GetMetadataByKey<int>("RetryCount");

var userId = finalError.GetMetadataByType<int>();
```

---

## Credits - Inspiration

- [Gui Ferreira](https://www.youtube.com/watch?v=C_u1WottRA0)
- [Milan Jovanović](https://www.youtube.com/watch?v=WCCkEe_Hy2Y)
- [ErrorOr](https://github.com/amantinband/error-or)
- [OneOf](https://github.com/mcintyre321/OneOf)

