# BetterResult

[![NuGet](https://img.shields.io/nuget/v/BetterResult.svg)](https://www.nuget.org/packages/BetterResult) [![BuildStatus](https://github.com/cds-git/BetterResult/actions/workflows/dotnet.yml/badge.svg)](https://github.com/cds-git/BetterResult/actions/workflows/dotnet.yml)

**BetterResult** is a lightweight library that adopts a functional programming approach to error handling. 
By representing both successes and errors as explicit values, 
it forces developers to acknowledge and manage errors at every step, resulting in clearer, more robust, and maintainable code.

---

## Table of Contents

- [Installation](#installation)
- [Why Use The Result Pattern?](#why-use-the-result-pattern)
- [Usage](#usage)
- [Operations](#operations)
  - [Bind - Monadic Chaining](#bind---monadic-chaining)
  - [Map - Value Transformation](#map---value-transformation)
  - [MapError - Error Recovery](#maperror---error-recovery)
  - [Tap - Side Effects on Success](#tap---side-effects-on-success)
  - [TapError - Side Effects on Failure](#taperror---side-effects-on-failure)
  - [Match - Result Folding](#match---result-folding)
- [Error Type](#error-type)
- [Credits - Inspiration](#credits---inspiration)

---

## Installation

You can install the **BetterResult** NuGet package via the NuGet Package Manager, .NET CLI, or by adding the package reference to your `.csproj` file.

### .NET CLI

```bash
dotnet add package BetterResult
```

### NuGet Package Manager

```bash
Install-Package BetterResult
```

### Package Reference

```xml
<PackageReference Include="BetterResult" Version="x.x.x" />
```

Replace `x.x.x` with the latest version available on [NuGet](https://www.nuget.org/packages/BetterResult).

---

## Why Use The Result Pattern?

The Result Pattern revolutionizes error handling by treating both successes and failures as explicit, first-class values in your type system. 
This functional approach eliminates the unpredictability of exceptions while forcing developers to consciously handle every possible failure scenario.

### The Problem with Traditional Exception Handling

```csharp
// Traditional approach - hidden failure paths
public User GetUser(int id)
{
    var user = database.FindUser(id); // Could throw SqlException
    if (user == null) 
        throw new UserNotFoundException(); // Could throw
    
    ValidateUser(user); // Could throw ValidationException
    return user; // Success path unclear
}

// Caller has no idea what could go wrong
var user = GetUser(123); // 🎰 Mystery box of potential failures
```

### The Result Pattern Solution

```csharp
// Result approach - explicit failure paths
public Result<User> GetUser(int id) =>
    FindUserInDatabase(id)                    // Result<User>
        .Bind(ValidateUser)                   // Result<User>
        .Tap(user => logger.LogSuccess(user)); // Side effects

// Caller knows exactly what they're getting
Result<User> userResult = GetUser(123); // 📦 Explicit success or failure
```

### Key Benefits

**🚀 Performance & Predictability**
- Zero exception overhead - no stack traces or unwinding costs
- Predictable control flow that's easy to reason about
- Better JIT optimization opportunities

**🛡️ Type-Safe Error Handling**
- Compiler enforces error handling - forgotten error checks become compile errors
- Self-documenting APIs that clearly communicate failure modes
- Impossible to accidentally ignore errors

**🔗 Functional Composition**
- Chain operations seamlessly with automatic error propagation
- Build complex pipelines from simple, testable functions
- Error handling becomes part of the data flow, not separate control structures

**🧪 Enhanced Testability**
- Errors are just data - easy to create, assert, and verify in tests
- No need for complex exception mocking or try-catch blocks in tests
- Clear separation between business logic and error scenarios

This pattern transforms error handling from a necessary evil into an elegant part of your application's architecture, 
making your code more robust, maintainable, and enjoyable to work with.

---

## Usage

### Basic Usage

#### `Result<T>`

Represents either a successful outcome carrying a value of type `T` or a failure carrying an `Error`.

**Creating Results:**
```csharp
// Implicit conversions (recommended for clean code)
Result<int> success = 42;
Result<int> failure = Error.Failure("E001", "Invalid input");

// Explicit factory methods
Result<int> explicitSuccess = Result.Success(100);
Result<int> explicitFailure = Result.Failure<int>(Error.NotFound("E404", "User not found"));

// From operations that might fail
Result<User> GetUser(int id) =>
    id > 0 
        ? Result.Success(new User { Id = id, Name = "John" })
        : Result.Failure<User>(Error.Validation("INVALID_ID", "User ID must be positive"));
```

**Working with Results:**
```csharp
Result<int> CalculateAge(DateTime birthDate)
{
    if (birthDate > DateTime.Now)
        return Error.Validation("FUTURE_DATE", "Birth date cannot be in the future");
    
    var age = DateTime.Now.Year - birthDate.Year;
    return age; // Implicit conversion to Result<int>
}

// Safe value access
Result<int> ageResult = CalculateAge(new DateTime(1990, 5, 15));

if (ageResult.IsSuccess)
{
    Console.WriteLine($"Age: {ageResult.Value}");
    // Can safely access Value property
}
else
{
    Console.WriteLine($"Error: {ageResult.Error.Message}");
    // Can safely access Error property
}

// Functional pipeline (no manual checking needed)
string description = ageResult
    .Map(age => age >= 18 ? "Adult" : "Minor")
    .Map(category => $"Person is classified as: {category}")
    .Match(
        success => success,
        error => $"Could not determine age: {error.Message}"
    );
```


#### `Result` (Void Operations)

Perfect for operations that indicate success or failure without returning a value - think of it as a functional replacement for `void` methods that can fail.

```csharp
// Traditional void method with exceptions
public void DeleteUser(int userId)
{
    if (userId <= 0) throw new ArgumentException("Invalid ID");
    if (!userExists(userId)) throw new UserNotFoundException();
    
    database.Delete(userId); // Could throw SqlException
}

// Result-based approach
public Result DeleteUser(int userId)
{
    if (userId <= 0) 
        return Error.Validation("INVALID_ID", "User ID must be positive");
    
    if (!UserExists(userId))
        return Error.NotFound("USER_NOT_FOUND", $"User {userId} does not exist");
    
    return database.TryDelete(userId)
        ? Result.Success()
        : Error.Unexpected("DELETE_FAILED", "Database operation failed");
}

// Usage with explicit error handling
Result deleteResult = DeleteUser(42);
if (deleteResult.IsFailure)
{
    // Handle the specific error
    logger.LogError($"Delete failed: {deleteResult.Error.Message}");
    return;
}

// Or use in functional pipelines
await ValidatePermissions(currentUser, targetUserId)
    .BindAsync(permission => DeleteUser(targetUserId))
    .TapAsync(() => auditService.LogDeletion(targetUserId))
    .MatchAsync(
        () => Ok("User deleted successfully"),
        error => BadRequest(error.Message)
    );
```

---

## Operations

These operations enable functional composition, transformation, side effects, and recovery in a fluent, chainable style. 
All operations support both synchronous and asynchronous variants, with automatic error propagation.

### Bind - Monadic Chaining

**Purpose**: Chains operations that can fail, automatically propagating errors without manual checking.

**When to use**: When you need to perform a sequence of operations where each step depends on the previous one's success, and any step might fail.

```csharp
// Synchronous chaining
Result<string> ProcessUser(int userId) =>
    GetUser(userId)                    // Result<User>
        .Bind(user => ValidateUser(user))      // Result<User> 
        .Bind(user => FormatUserData(user));   // Result<string>

// Asynchronous chaining  
async Task<Result<string>> ProcessUserAsync(int userId) =>
    await GetUserAsync(userId)         // Task<Result<User>>
        .BindAsync(ValidateUserAsync)          // Task<Result<User>>
        .BindAsync(FormatUserDataAsync);       // Task<Result<string>>

// Mixed sync/async
await GetUserAsync(userId)
    .BindAsync(user => ValidateUser(user))     // Sync validation
    .BindAsync(user => SaveUserAsync(user));   // Back to async
```

**Key point**: Each operation in the chain receives the unwrapped value from the previous step. 
If any step fails, the chain short-circuits and the error propagates to the final result.

### Map - Value Transformation

**Purpose**: Transforms the value inside a successful result without the possibility of failure.

**When to use**: When you need to convert or transform data, but the transformation itself cannot fail (pure functions).

```csharp
// Simple transformations
Result<int> number = 42;
Result<string> text = number.Map(x => x.ToString());
Result<string> upper = text.Map(s => s.ToUpper());

// Chaining transformations
Result<UserDto> dto = GetUser(userId)
    .Map(user => user.Name)                    // Extract name
    .Map(name => name.Trim().ToUpper())        // Clean and format
    .Map(name => new UserDto { Name = name }); // Create DTO

// Async transformations
await GetUserAsync(userId)
    .MapAsync(user => FormatNameAsync(user.Name))  // Async formatting
    .MapAsync(name => CreateDtoAsync(name));       // Async DTO creation

// Transform void Result to valued Result
Result voidResult = DoWork();
Result<string> message = voidResult.Map(() => "Work completed successfully");
```

**Key point**: Map operations receive unwrapped values and return plain values (not Results). 
Errors automatically propagate without executing the map operation.

### MapError - Error Recovery

**Purpose**: Transforms or recovers from failures by examining the error and potentially converting it to a success.

**When to use**: When you want to provide fallback values, retry logic, or transform one error type into another.

```csharp
// Provide fallback values
Result<User> user = GetUser(userId)
    .MapError(error => error.Code == "NOT_FOUND" 
        ? Result.Success(User.Guest)     // Convert to success
        : error);                        // Keep original error

// Error transformation
Result<Data> data = LoadData()
    .MapError(error => Error.Validation("LOAD_001", $"Failed to load: {error.Message}"));

// Retry logic
Result<string> content = DownloadContent()
    .MapError(async error => 
    {
        if (error.Code == "TIMEOUT" && retryCount < 3)
            return await RetryDownload();
        return error;
    });

// Graceful degradation
Result<WeatherData> weather = GetWeatherFromApi()
    .MapError(error => GetCachedWeather())     // Fallback to cache
    .MapError(error => GetDefaultWeather());   // Ultimate fallback
```

**Key point**: MapError only executes on failures and can convert them back to successes. Successful results pass through unchanged.

### Tap - Side Effects on Success

**Purpose**: Executes side effects (logging, notifications, debugging) on successful results without modifying the result.

**When to use**: When you need to perform actions like logging, caching, or notifications while maintaining the functional pipeline.

```csharp
// Logging and debugging
var result = GetUser(userId)
    .Tap(user => logger.LogInfo($"Retrieved user: {user.Name}"))
    .Map(user => user.Email)
    .Tap(email => Console.WriteLine($"Processing email: {email}"));

// Caching successful results
var data = await LoadExpensiveDataAsync()
    .TapAsync(data => cache.SetAsync(cacheKey, data))
    .MapAsync(data => TransformData(data));

// Notifications and metrics
await ProcessOrderAsync(order)
    .TapAsync(async result => await notificationService.NotifySuccess(result))
    .TapAsync(result => metrics.IncrementCounter("orders.processed"));

// Multiple side effects
GetUser(userId)
    .Tap(user => auditService.LogAccess(user.Id))
    .Tap(user => analytics.TrackUserActivity(user))
    .Tap(user => cache.Warm(user.PreferredSettings));
```

**Key point**: Tap actions receive the unwrapped value but don't affect the result. The original result passes through unchanged, making it perfect for pipeline instrumentation.

### TapError - Side Effects on Failure

**Purpose**: Executes side effects on failed results without modifying the result.

**When to use**: When you need to log errors, send alerts, or perform cleanup operations while preserving the error for downstream handling.

```csharp
// Error logging and monitoring
var result = GetUser(userId)
    .TapError(error => logger.LogError($"Failed to get user {userId}: {error.Message}"))
    .TapError(error => metrics.IncrementCounter("user.fetch.errors"))
    .TapError(error => alertService.SendAlert(error));

// Async error handling
await ProcessPaymentAsync(payment)
    .TapErrorAsync(async error => await auditService.LogFailedPayment(payment, error))
    .TapErrorAsync(async error => await notificationService.NotifyPaymentFailure(error));

// Conditional error actions
await UploadFileAsync(file)
    .TapErrorAsync(async error => 
    {
        if (error.Code == "QUOTA_EXCEEDED")
            await cleanupService.FreeSpace();
        if (error.Code == "NETWORK_ERROR") 
            await diagnosticsService.TestConnection();
    });

// Debugging and development
var pipeline = LoadData()
    .TapError(error => Console.WriteLine($"Step 1 failed: {error}"))
    .MapError(error => RetryLogic(error))
    .TapError(error => Console.WriteLine($"Retry also failed: {error}"));
```

**Key point**: TapError actions receive the error but don't modify the result. Failed results remain failed, successful results are untouched.

### Match - Result Folding

**Purpose**: Converts a Result into a concrete value by providing handlers for both success and failure cases.

**When to use**: When you need to extract a final value from a Result, typically at the end of a functional pipeline or when interfacing with non-Result code.

```csharp
// Simple value extraction
string message = GetUser(userId).Match(
    user => $"Welcome, {user.Name}!",
    error => $"Error: {error.Message}"
);

// HTTP response mapping
return await ProcessRequestAsync(request).MatchAsync(
    async data => Ok(await SerializeAsync(data)),
    error => BadRequest(error.Message)
);

// Complex branching logic
var actionResult = await ValidateAndProcessAsync(input).MatchAsync(
    async result => 
    {
        await notificationService.NotifySuccess(result);
        return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
    },
    error => error.Type switch
    {
        ErrorType.Validation => BadRequest(error.Message),
        ErrorType.NotFound => NotFound(error.Message),
        ErrorType.Unauthorized => Unauthorized(error.Message),
        _ => StatusCode(500, "Internal server error")
    }
);

// Aggregating multiple results
string summary = await Task.WhenAll(
        ProcessDataAsync(data1),
        ProcessDataAsync(data2), 
        ProcessDataAsync(data3)
    )
    .ContinueWith(tasks => tasks.Result
        .Select(result => result.Match(
            success => $"✓ {success}",
            error => $"✗ {error.Message}"
        ))
        .Aggregate((a, b) => $"{a}\n{b}")
    );

// Early termination patterns
return users.Match(
    userList => userList.Any() 
        ? ProcessUsers(userList) 
        : Result.Failure(Error.NotFound("NO_USERS", "No users to process")),
    error => error
);
```

**Key point**: Match is terminal - it extracts the final value from the Result monad. 
Both success and failure handlers must return the same type, and one of them will always execute.

### Operation Composition Examples

```csharp
// Complete pipeline combining all operations
async Task<IActionResult> UpdateUserProfileAsync(int userId, UpdateProfileRequest request)
{
    return await GetUserAsync(userId)
        .TapAsync(user => logger.LogInfo($"Updating profile for user {user.Id}"))
        .BindAsync(user => ValidateUpdateRequestAsync(request, user))
        .TapAsync(validatedData => auditService.LogValidation(validatedData))
        .BindAsync(data => UpdateUserInDatabaseAsync(data))
        .TapAsync(updatedUser => cache.InvalidateUser(updatedUser.Id))
        .TapErrorAsync(error => logger.LogError($"Profile update failed: {error.Message}"))
        .MatchAsync(
            user => Ok(new { message = "Profile updated successfully", user = user.ToDto() }),
            error => error.Type switch
            {
                ErrorType.NotFound => NotFound(error.Message),
                ErrorType.Validation => BadRequest(error.Message),
                ErrorType.Unauthorized => Forbid(error.Message),
                _ => StatusCode(500, "Internal server error")
            }
        );
}

// Error handling with fallbacks
Result<Configuration> config = LoadPrimaryConfig()
    .TapError(error => logger.LogWarning($"Primary config failed: {error.Message}"))
    .MapError(error => LoadBackupConfig())
    .TapError(error => logger.LogWarning($"Backup config failed: {error.Message}"))
    .MapError(error => LoadDefaultConfig())
    .TapError(error => logger.LogError($"All config sources failed: {error.Message}"));
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

## Roadmap

Future enhancements planned to make BetterResult even more powerful for functional programming:

### High Priority - Core Functional Operations

**`Or` / `OrElse` - Fallback Results**
- Simple fallback chaining: `GetFromApi().Or(() => GetFromCache()).Or(() => GetDefault())`
- Essential for resilient systems with multiple data sources

**`Try` - Exception Integration** 
- Convert exception-throwing code to Results: `Result.Try(() => riskyOperation())`
- Bridge between traditional .NET code and functional error handling

**`Filter` / `Where` - Conditional Success**
- Convert success to failure based on predicates: `result.Filter(x => x > 0, "Must be positive")`
- Perfect for validation pipelines

### Medium Priority - Collection Operations

**`Traverse` / `Sequence` - Collection Handling**
- Transform `IEnumerable<Result<T>>` to `Result<IEnumerable<T>>`
- Essential for processing collections where all items must succeed

**`Combine` - Result Aggregation**
- Merge multiple results into one: `Result.Combine(result1, result2, result3)`
- Useful for operations requiring multiple inputs

### Lower Priority - Advanced Patterns

**`SelectMany` - LINQ Integration**
- Enable LINQ query syntax for Result compositions
- Syntactic sugar for complex Bind chains

**`Apply` - Applicative Pattern**
- Apply functions wrapped in Results to values wrapped in Results
- Advanced functional composition scenarios

**`Zip` - Result Pairing**
- Combine two results into a tuple result
- Specialized composition for paired operations

---

## Credits - Inspiration

- [Zoran Horvat](https://www.youtube.com/@zoran-horvat)
- [Gui Ferreira](https://www.youtube.com/watch?v=C_u1WottRA0)
- [Milan Jovanović](https://www.youtube.com/watch?v=WCCkEe_Hy2Y)
- [ErrorOr](https://github.com/amantinband/error-or)
