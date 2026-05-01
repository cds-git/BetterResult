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
  - [Try - Exception Handling](#try---exception-handling)
  - [Ensure - Validation Guards](#ensure---validation-guards)
  - [Recover - Error Recovery](#recover---error-recovery)
  - [Sequence - Collection Aggregation](#sequence---collection-aggregation)
  - [Traverse - Transform and Collect](#traverse---transform-and-collect)
  - [Partition - Separate Successes from Failures](#partition---separate-successes-from-failures)
  - [Combine - Combine Multiple Independent Results](#combine---combine-multiple-independent-results)
  - [Zip - Fluent Result Combination](#zip---fluent-result-combination)
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
    FindUserInDatabase(id)                      // Result<User>
        .Tap(user => ValidateUser(user))        // Side effect validation
        .Tap(user => logger.LogSuccess(user));  // Side effects

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

---

### Operations Without Return Values

Many operations succeed or fail without returning meaningful data - like deleting a record, sending an email, or updating a setting. For these cases, use `Result<NoValue>`.

**What is NoValue?**

`NoValue` is a special type that represents "no meaningful value". Think of it as a type-safe way to say "this operation succeeded, but there's nothing to return."

```csharp
// Simple example
Result<NoValue> SendEmail(string to, string subject)
{
    if (string.IsNullOrEmpty(to))
        return Error.Validation("EMPTY_TO", "Recipient required");
    
    emailService.Send(to, subject);
    return NoValue.Instance;  // Success, but no data to return
}

// Use _ (discard) in lambdas when the value doesn't matter
Result<NoValue> result = DoWork();
result
    .Tap(_ => Console.WriteLine("Work completed"))   // We don't need the NoValue
    .Map(_ => "Operation successful");                // Transform to Result<string>
```

**Composing with Result<T>**

The real power shows when chaining operations - `Result<NoValue>` works seamlessly with `Result<T>`:

```csharp
Result<string> ProcessUser(int userId) =>
    GetUser(userId)              // Result<User>
        .Bind(ValidateUser)      // Result<NoValue> - validation doesn't return data
        .Bind(_ => GetUser(userId))  // Back to Result<User>
        .Map(user => user.Name); // Result<string>

// Complex pipeline mixing valued and void results
await GetUserEmail(userId)               // Result<string>
    .BindAsync(email => SendWelcomeEmail(email))  // Result<NoValue>
    .BindAsync(_ => LogEmailSent(userId))         // Result<NoValue>
    .MapAsync(_ => "Welcome email sent");         // Result<string>
```

---

```csharp
// Traditional approach
public void DeleteUser(int userId)
{
    if (userId <= 0) throw new ArgumentException("Invalid ID");
    if (!userExists(userId)) throw new UserNotFoundException();
    database.Delete(userId); // Could throw SqlException
}

// With Result<NoValue>
public Result<NoValue> DeleteUser(int userId)
{
    if (userId <= 0) 
        return Error.Validation("INVALID_ID", "User ID must be positive");
    
    if (!UserExists(userId))
        return Error.NotFound("USER_NOT_FOUND", $"User {userId} does not exist");
    
    database.Delete(userId);
    return NoValue.Instance;
}

// Explicit error handling
Result<NoValue> deleteResult = DeleteUser(42);
if (deleteResult.IsFailure)
{
    logger.LogError($"Delete failed: {deleteResult.Error.Message}");
    return;
}

// Functional pipeline
await ValidatePermissions(currentUser, targetUserId)
    .BindAsync(_ => DeleteUser(targetUserId))
    .TapAsync(_ => auditService.LogDeletion(targetUserId))
    .MatchAsync(
        _ => Ok("User deleted successfully"),
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
        .Bind(ValidateUser)                    // Result<NoValue>
        .Bind(_ => GetUser(userId))            // Result<User>
        .Bind(FormatUserData);                 // Result<string>

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

// Transform Result<NoValue> to valued Result
Result<NoValue> workResult = DoWork();
Result<string> message = workResult.Map(_ => "Work completed successfully");
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

### Try - Exception Handling

**Purpose**: Wraps exception-throwing operations into Results, bridging the gap between traditional .NET APIs and the Result pattern.

**When to use**: When you need to call code that throws exceptions (parsing, file I/O, database calls, third-party APIs) and want to handle errors functionally.

```csharp
// Basic exception catching
Result<int> numberResult = Result<string>.Success("42")
    .Try(x => int.Parse(x));  // Catches FormatException

// With custom error mapping
Result<User> userResult = Result<string>.Success(jsonString)
    .Try(
        json => JsonSerializer.Deserialize<User>(json),
        ex => Error.Validation("INVALID_JSON", $"Failed to parse JSON: {ex.Message}")
    );

// Opt-in stack traces (NOT included by default — see security note below)
Result<int> withStackTrace = Result<string>.Success("not a number")
    .Try(
        x => int.Parse(x),
        ex => Error.Unexpected("EXCEPTION", ex.Message)
            .WithMetadata("ExceptionType", ex.GetType().Name)
            .WithMetadata("StackTrace", ex.StackTrace ?? string.Empty)
    );

// Chaining with other operations
Result<int> processedResult = Result<string>.Success("  123  ")
    .Map(s => s.Trim())
    .Try(s => int.Parse(s))                    // Can throw
    .Map(x => x * 2);                          // Only runs if parsing succeeded

// Async operations
Result<string> fileContent = await Result<string>.Success("data.txt")
    .TryAsync(async path => await File.ReadAllTextAsync(path));

// Real-world pipeline
var result = await GetConfigPathAsync()              // Result<string>
    .TryAsync(async path => await File.ReadAllTextAsync(path))  // Result<string>
    .Try(json => JsonSerializer.Deserialize<Config>(json))      // Result<Config>
    .Tap(config => logger.LogInfo($"Loaded config from {config.Source}"))
    .MapAsync(async config => await ValidateConfigAsync(config));
```

**Key points**:

- The default `Try` converts caught exceptions to `Error.Unexpected("EXCEPTION", ex.Message)` with `ExceptionType` in the metadata. **Stack traces are deliberately not included** — they leak file paths, line numbers, and the internal call hierarchy, which is a CWE-209 information-disclosure risk if errors cross a trust boundary (API responses, untrusted log sinks, third-party integrations). If you need the stack trace (e.g. for an internal log), use the `errorMapper` overload as shown above.
- `OperationCanceledException` and `TaskCanceledException` are **not** caught — cancellation propagates so cooperative cancellation keeps working. Wrap the whole pipeline in your own `try/catch` if you need to convert cancellation into a Result.
- Success values pass through unchanged; existing errors propagate without executing the operation.

### Ensure - Validation Guards

**Purpose**: Converts successful results to failures based on validation predicates, making validation chains declarative and composable.

**When to use**: When you need to validate data, enforce business rules, or guard against invalid states while maintaining functional composition.

```csharp
// Simple validation
Result<int> age = GetAge()
    .Ensure(age => age >= 0, Error.Validation("NEGATIVE_AGE", "Age cannot be negative"))
    .Ensure(age => age <= 150, Error.Validation("INVALID_AGE", "Age unrealistic"));

// With error factory for dynamic messages
Result<User> user = GetUser(userId)
    .Ensure(u => u.Age >= 18, u => Error.Validation("UNDERAGE", $"User {u.Name} is only {u.Age} years old"))
    .Ensure(u => u.IsActive, Error.Unauthorized("INACTIVE", "User account is not active"));

// Chaining validations with transformations
Result<Order> order = GetOrder(orderId)
    .Ensure(o => o.Items.Any(), Error.Validation("EMPTY_ORDER", "Order must contain at least one item"))
    .Ensure(o => o.Total > 0, Error.Validation("INVALID_TOTAL", "Order total must be positive"))
    .Map(o => o.WithTax())
    .Ensure(o => o.Total <= 10000, Error.Validation("EXCEEDS_LIMIT", "Order exceeds maximum amount"));

// Async validation
Result<Order> validatedOrder = await GetOrder(orderId)
    .EnsureAsync(
        async o => await inventoryService.HasStock(o.Items),
        Error.Conflict("OUT_OF_STOCK", "Insufficient inventory for order")
    )
    .EnsureAsync(
        async o => await fraudService.IsLegitimate(o),
        Error.Unauthorized("FRAUD_DETECTED", "Order flagged as suspicious")
    );

// Complex validation pipeline
Result<string> processedData = Result<string>.Success("  42  ")
    .Map(s => s.Trim())
    .Try(s => int.Parse(s))
    .Ensure(x => x > 0, Error.Validation("NEGATIVE", "Value must be positive"))
    .Ensure(x => x < 100, Error.Validation("TOO_LARGE", "Value must be less than 100"))
    .Map(x => x * 2)
    .Match(
        success => $"Result: {success}",
        error => $"Error: {error.Message}"
    );

// Multiple guards with different error types
Result<Transaction> transaction = CreateTransaction(data)
    .Ensure(t => t.Amount > 0, Error.Validation("INVALID_AMOUNT", "Amount must be positive"))
    .Ensure(t => t.AccountId != null, Error.Validation("MISSING_ACCOUNT", "Account ID required"))
    .Ensure(t => HasSufficientFunds(t), Error.Conflict("INSUFFICIENT_FUNDS", "Account balance too low"))
    .Ensure(t => IsWithinDailyLimit(t), Error.Conflict("LIMIT_EXCEEDED", "Daily transaction limit reached"));
```

**Key point**: Ensure short-circuits on the first failing predicate and only executes on successful results. Failed results propagate their error without executing any predicates. The error factory variant receives the value for context-aware error messages.

### Recover - Error Recovery

**Purpose**: Converts specific failed results back to success by providing recovery logic based on error type or custom predicates, enabling graceful fallbacks and retry scenarios.

**When to use**: When you want to handle specific error types with fallback values or recovery logic, retry failed operations, or provide default values for expected failures.

```csharp
// Basic recovery with fallback value
Result<User> user = GetUser(userId)
    .Recover(ErrorType.NotFound, User.Guest);  // Use guest user if not found

// Recovery based on error type with logic
Result<Config> config = LoadConfig()
    .Recover(ErrorType.NotFound, error => LoadDefaultConfig())
    .Recover(ErrorType.Unavailable, error => LoadCachedConfig());

// Recovery with custom predicate
Result<string> content = DownloadContent(url)
    .Recover(
        error => error.Code == "TIMEOUT" || error.Code == "NETWORK_ERROR",
        error => RetryDownload(url, maxRetries: 3)
    );

// Async recovery
Result<Data> data = await FetchPrimaryDataAsync()
    .RecoverAsync(
        ErrorType.Unavailable,
        async error => await FetchBackupDataAsync()
    );

// Multiple recovery strategies
Result<WeatherData> weather = GetWeatherFromPrimaryApi()
    .Recover(ErrorType.Timeout, _ => GetWeatherFromSecondaryApi())
    .Recover(ErrorType.Unavailable, _ => GetCachedWeather())
    .Recover(ErrorType.NotFound, WeatherData.Default);

// Recovery in pipeline
Result<Order> order = await ValidateOrderAsync(orderId)
    .BindAsync(o => ProcessPaymentAsync(o))
    .RecoverAsync(
        ErrorType.Unavailable,
        async error => await RetryPaymentWithBackupProviderAsync(orderId)
    )
    .TapAsync(o => logger.LogInfo($"Order {o.Id} completed"))
    .TapErrorAsync(error => logger.LogError($"Order processing failed: {error.Message}"));

// Conditional recovery based on error metadata
Result<Response> response = await SendRequestAsync(request)
    .RecoverAsync(
        error => error.Type == ErrorType.Timeout && 
                 error.GetMetadata<int>("AttemptNumber") < 3,
        async error => 
        {
            var attempts = error.GetMetadata<int>("AttemptNumber") ?? 0;
            return await RetryRequestAsync(request, attempts + 1);
        }
    );

// Graceful degradation with multiple fallbacks
Result<ImageData> image = LoadHighResImage(imageId)
    .TapError(error => logger.LogWarning($"High-res failed: {error.Message}"))
    .Recover(ErrorType.NotFound, _ => LoadMediumResImage(imageId))
    .TapError(error => logger.LogWarning($"Medium-res failed: {error.Message}"))
    .Recover(ErrorType.NotFound, _ => LoadThumbnail(imageId))
    .TapError(error => logger.LogWarning($"Thumbnail failed: {error.Message}"))
    .Recover(ErrorType.NotFound, ImageData.Placeholder);
```

**Key point**: Recover only executes when the result is a failure AND the condition matches (error type or predicate). Success results pass through unchanged. Unlike MapError which examines every error, Recover provides cleaner syntax for specific error handling scenarios.

**Recover vs MapError**:
- **MapError**: Examines every error, must return a `Result<T>`
- **Recover**: Only acts on errors matching specific conditions (type or predicate), ideal for targeted fallbacks and retry logic

### Sequence - Collection Aggregation

**Purpose**: Aggregates a collection of Results into a single Result containing all values, short-circuiting on the first failure.

**When to use**: When you have a collection of Results and need all operations to succeed (all-or-nothing semantics). Perfect for validation scenarios where you need all checks to pass.

```csharp
// Basic sequence - all values or first error
IEnumerable<Result<int>> validations = new[]
{
    ValidateAge(user.Age),
    ValidateEmail(user.Email),
    ValidatePhone(user.Phone)
};

Result<IReadOnlyList<int>> allValid = validations.Sequence();
// Returns: Success with [age, email, phone] if all succeed
//          OR first validation error

// Sequence with LINQ
var userIds = new[] { 1, 2, 3, 4, 5 };
Result<IReadOnlyList<User>> users = userIds
    .Select(id => GetUser(id))
    .ToList()
    .Sequence();

// Async sequence
IEnumerable<Task<Result<Data>>> asyncTasks = urls.Select(url => FetchDataAsync(url));
Result<IReadOnlyList<Data>> allData = await asyncTasks.SequenceAsync();

// Validation pipeline
var validationResults = new[]
{
    Validate.NotEmpty(name, "NAME_REQUIRED"),
    Validate.MinLength(name, 3, "NAME_TOO_SHORT"),
    Validate.MaxLength(name, 50, "NAME_TOO_LONG")
};

Result<IReadOnlyList<NoValue>> validated = validationResults.Sequence();
// If any validation fails, returns that error
// If all pass, returns success with list of NoValue

// Multi-step operations
var operations = new[]
{
    SaveToDatabase(data),
    UpdateCache(data),
    NotifySubscribers(data)
};

Result<IReadOnlyList<NoValue>> completed = operations.Sequence();
// All operations must succeed
```

**Key point**: Sequence short-circuits on the first failure and returns that error immediately. If you need to process all items and collect both successes and failures, use [Partition](#partition---separate-successes-from-failures) instead.

### Traverse - Transform and Collect

**Purpose**: Transforms each element in a collection using a Result-producing function and aggregates the results, short-circuiting on the first failure.

**When to use**: When you need to apply a transformation that might fail to each item in a collection and collect all results (all-or-nothing semantics).

```csharp
// Basic traverse - transform and collect
var userIds = new[] { 1, 2, 3, 4, 5 };
Result<IReadOnlyList<User>> users = Result.Traverse(userIds, id => GetUser(id));
// Transforms each ID to User, stops on first failure

// Parse and validate
var inputs = new[] { "42", "100", "256" };
Result<IReadOnlyList<int>> parsed = Result.Traverse(inputs, s =>
{
    if (int.TryParse(s, out var n))
        return Result<int>.Success(n);
    return Error.Validation("PARSE_ERROR", $"Invalid number: {s}");
});

// Async traverse
var productIds = new[] { 1, 2, 3 };
Result<IReadOnlyList<Product>> products = await Result
    .TraverseAsync(productIds, id => FetchProductAsync(id));

// Chained transformations
var orderIds = new[] { 101, 102, 103 };
Result<IReadOnlyList<OrderSummary>> summaries = Result.Traverse(orderIds, id => GetOrder(id))
    .Bind(orders => Result.Traverse(orders, order => order.ValidateAndProcess()))
    .Map(processed => processed.Select(o => o.ToSummary()).ToList());

// File processing
var filePaths = new[] { "data1.json", "data2.json", "data3.json" };
Result<IReadOnlyList<Config>> configs = Result
    .Traverse(filePaths, path => LoadConfigFromFile(path));

// Database batch insert
var entities = new[] { entity1, entity2, entity3 };
Result<IReadOnlyList<int>> insertedIds = await Result
    .TraverseAsync(entities, e => InsertIntoDatabase(e));
// Stops on first database error

// Validation with transformation
var emailAddresses = new[] { "user1@example.com", "user2@example.com", "invalid" };
Result<IReadOnlyList<Email>> validEmails = Result.Traverse(emailAddresses, email =>
    Email.TryCreate(email) // Returns Result<Email>
);

// Nested traverse
var userGroups = new[] { groupA, groupB, groupC };
Result<IReadOnlyList<IReadOnlyList<User>>> groupUsers = Result.Traverse(
    userGroups, 
    group => Result.Traverse(group.MemberIds, id => GetUser(id))
);
```

**Key point**: Traverse is like LINQ's `Select` but for Result-producing functions. It short-circuits on the first failure. Think of it as "map and sequence combined" - it applies a function to each element and aggregates the results.

**Traverse vs Sequence**:
- **Sequence**: You already have `IEnumerable<Result<T>>`, just aggregate them
- **Traverse**: You have `IEnumerable<T>` and need to apply a Result-producing function `T → Result<U>`, then aggregate

### Partition - Separate Successes from Failures

**Purpose**: Processes all Results in a collection and separates successes from failures (does NOT short-circuit). Returns both collections for separate handling.

**When to use**: When you want to process all items and handle successes and failures separately (best-effort semantics). Perfect for bulk operations, batch imports, and reporting scenarios.

```csharp
// Basic partition - separate successes from failures
IEnumerable<Result<User>> results = userIds.Select(id => ImportUser(id));
var (imported, failed) = results.Partition();

Console.WriteLine($"Successfully imported: {imported.Count}");
Console.WriteLine($"Failed to import: {failed.Count}");

// Process successes and failures separately
foreach (var user in imported)
{
    await SendWelcomeEmail(user);
}

foreach (var error in failed)
{
    logger.LogError($"Import failed: {error.Code} - {error.Message}");
}

// Bulk validation with reporting
var users = GetAllUsers();
var validationResults = users.Select(user => ValidateUser(user));
var (valid, invalid) = validationResults.Partition();

return new ValidationReport
{
    ValidCount = valid.Count,
    InvalidCount = invalid.Count,
    Errors = invalid.Select(e => new ErrorReport
    {
        Code = e.Code,
        Message = e.Message,
        Timestamp = DateTime.UtcNow
    }).ToList()
};

// Batch processing with partial success
var orders = GetPendingOrders();
var processResults = orders.Select(order => ProcessOrder(order));
var (succeeded, failedOrders) = processResults.Partition();

// Continue with successful orders
await NotifyCustomers(succeeded);
await UpdateInventory(succeeded);

// Retry or log failed orders
foreach (var error in failedOrders)
{
    await QueueForRetry(error);
}

// File processing with error collection
var files = Directory.GetFiles("./imports");
var parseResults = files.Select(f => ParseFile(f));
var (parsed, parseErrors) = parseResults.Partition();

// Process successful parses
SaveToDatabase(parsed);

// Generate error report for failed parses
var errorReport = parseErrors.Select(err => new {
    File = err.GetMetadata<string>("FileName"),
    Error = err.Message
});

// Email send with reporting
var recipients = GetSubscribers();
var sendResults = recipients.Select(r => SendEmail(r));
var (sent, failed) = sendResults.Partition();

return new EmailCampaignResult
{
    TotalSent = sent.Count,
    TotalFailed = failed.Count,
    FailureDetails = failed.Select(e => new FailureDetail
    {
        Recipient = e.GetMetadata<string>("Recipient"),
        Reason = e.Message
    })
};

// Data migration with reporting
var records = await GetLegacyRecords();
var migrationResults = records.Select(r => MigrateRecord(r));
var (migrated, migrationFailed) = migrationResults.Partition();

Console.WriteLine($"Migration complete:");
Console.WriteLine($"  ✓ Migrated: {migrated.Count}");
Console.WriteLine($"  ✗ Failed: {migrationFailed.Count}");
```

**Key point**: Unlike Sequence and Traverse which short-circuit on the first error (all-or-nothing), Partition processes the entire collection and separates successes from failures (best-effort). Use Partition when you want to handle partial success scenarios.

**Partition vs Sequence/Traverse**:
- **Sequence/Traverse**: Short-circuit on first error ("all or nothing") - returns `Result<IEnumerable<T>>`
- **Partition**: Process everything ("best effort") - returns `(IReadOnlyList<T> Successes, IReadOnlyList<Error> Failures)`

### Combine - Combine Multiple Independent Results

**Purpose**: Combines 2-8 independent Results into a single Result, applying an optional selector function. Short-circuits on the first failure. Ideal for parallel independent operations.

**When to use**: When you have multiple independent Results (e.g., from parallel async operations) and need all to succeed. Use Combine for static, declarative combination of multiple results.

```csharp
// Basic combine - 2 parameters
var user = GetUser(userId);
var settings = GetSettings(userId);
var combined = Result.Combine(user, settings);
// Returns: Result<(User, Settings)>

// With selector - transform immediately
var result = Result.Combine(
    user,
    settings,
    (u, s) => new Profile { User = u, Settings = s }
);

// Combining 3+ results
var dashboard = Result.Combine(
    GetUser(userId),
    GetSettings(userId),
    GetPermissions(userId),
    (u, s, p) => new Dashboard(u, s, p)
);

// Parallel async operations
var userTask = GetUserAsync(userId);
var settingsTask = GetSettingsAsync(userId);
var permissionsTask = GetPermissionsAsync(userId);

await Task.WhenAll(userTask, settingsTask, permissionsTask);

var result = Result.Combine(
    await userTask,
    await settingsTask,
    await permissionsTask,
    (u, s, p) => new Dashboard(u, s, p)
);

// Validation scenario - all must pass
var validation = Result.Combine(
    ValidateName(name),
    ValidateEmail(email),
    ValidateAge(age),
    ValidatePhone(phone),
    (n, e, a, p) => new UserRegistration(n, e, a, p)
);

// Form validation with 6+ fields
var form = Result.Combine(
    ValidateField1(data.Field1),
    ValidateField2(data.Field2),
    ValidateField3(data.Field3),
    ValidateField4(data.Field4),
    ValidateField5(data.Field5),
    ValidateField6(data.Field6),
    (f1, f2, f3, f4, f5, f6) => new FormData(f1, f2, f3, f4, f5, f6)
);

// Without selector - returns tuple
var tuple = Result.Combine(
    GetFirstName(),
    GetLastName(),
    GetAge()
);
// Returns: Result<(string, string, int)>
if (tuple.IsSuccess)
{
    var (firstName, lastName, age) = tuple.Value;
}

// Combining different types
var report = Result.Combine(
    FetchSalesData(),      // Result<SalesData>
    FetchInventoryData(),  // Result<InventoryData>
    FetchCustomerData(),   // Result<CustomerData>
    CalculateMetrics(),    // Result<Metrics>
    (sales, inventory, customers, metrics) => new MonthlyReport
    {
        Sales = sales,
        Inventory = inventory,
        Customers = customers,
        Metrics = metrics
    }
);

// CombineAsync - with async selector
var enriched = await Result.CombineAsync(
    GetUser(userId),
    GetSettings(userId),
    async (u, s) =>
    {
        await LogAccess(u.Id);
        return new Profile { User = u, Settings = s };
    }
);

// CombineAsync - processing multiple results asynchronously
var report = await Result.CombineAsync(
    FetchSalesData(),
    FetchInventoryData(),
    FetchCustomerData(),
    async (sales, inventory, customers) =>
    {
        // Perform async aggregation/processing
        var metrics = await CalculateMetricsAsync(sales, inventory, customers);
        return new Report(sales, inventory, customers, metrics);
    }
);
```

**Key point**: Combine is static and declarative - pass all Results at once. It short-circuits on the first failure, returning that error immediately. Supports 2-8 parameters with optional selector functions. Use `CombineAsync` when your selector function is async.

### Zip - Fluent Result Combination

**Purpose**: Fluently combines two Results in a method chain. A fluent wrapper around `Combine` for 2 parameters, enabling sequential chaining patterns.

**When to use**: When you want to build up a result by chaining combinations fluently, or when one result depends on the previous. Use Zip for fluent, sequential composition.

```csharp
// Basic zip - fluent chaining
var result = GetUser(userId)
    .Zip(GetSettings(userId), (user, settings) => new Profile(user, settings));

// Multiple chained zips
var dashboard = GetUser(userId)
    .Zip(GetSettings(userId), (u, s) => (User: u, Settings: s))
    .Zip(GetPermissions(userId), (us, p) => new Dashboard(us.User, us.Settings, p));

// Async chaining
var result = await GetUserAsync(userId)
    .Zip(await GetSettingsAsync(userId), (u, s) => new Profile(u, s));

// ZipAsync with async selector
var result = await GetUserAsync(userId)
    .ZipAsync(GetSettingsAsync(userId), async (u, s) =>
    {
        await LogProfileCreation(u.Id);
        return new Profile(u, s);
    });

// Building complex objects fluently
var order = GetProduct(productId)
    .Zip(GetPrice(productId), (product, price) => (product, price))
    .Zip(GetInventory(productId), (pp, inventory) => (pp.product, pp.price, inventory))
    .Zip(GetShipping(shippingId), (ppi, shipping) => new Order
    {
        Product = ppi.product,
        Price = ppi.price,
        Available = ppi.inventory > 0,
        Shipping = shipping
    });

// Without selector - returns tuple
var tuple = GetFirstName()
    .Zip(GetLastName());
// Returns: Result<(string, string)>

// Mixing sync and async
var result = await GetUserAsync(userId)
    .Zip(GetSettings(userId), (u, s) => new Profile(u, s));

// Task extension - both are tasks
var result = await GetUserAsync(userId)
    .Zip(GetSettingsAsync(userId), (u, s) => new Profile(u, s));

// Progressive building in pipeline
var result = ValidateInput(data)
    .Map(validated => Transform(validated))
    .Zip(GetConfiguration(), (transformed, config) => Apply(transformed, config))
    .Bind(SaveToDatabase);

// Combining with other operations
var profile = GetUser(userId)
    .Tap(user => logger.LogInfo($"Retrieved user: {user.Name}"))
    .Zip(GetSettings(userId), (u, s) => new Profile(u, s))
    .TapError(error => logger.LogError($"Failed to create profile: {error.Message}"));
```

**Key point**: Zip is fluent and sequential - chain `.Zip()` calls for readability. Perfect for building complex objects step-by-step. All async variants included for seamless async/await integration.

**Combine vs Zip**:
- **Combine**: Static method for 2-8 parameters - `Result.Combine(r1, r2, r3, ...)`
- **Zip**: Fluent extension for 2 parameters - `r1.Zip(r2, ...)` 
- **Use Combine** when you have 3+ results to combine at once
- **Use Zip** when you want to chain combinations fluently or build progressively

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

## Credits - Inspiration

- [Zoran Horvat](https://www.youtube.com/@zoran-horvat)
- [Gui Ferreira](https://www.youtube.com/watch?v=C_u1WottRA0)
- [Milan Jovanović](https://www.youtube.com/watch?v=WCCkEe_Hy2Y)
- [ErrorOr](https://github.com/amantinband/error-or)
