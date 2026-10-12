# BetterResult

[![NuGet](https://img.shields.io/nuget/v/BetterResult.svg)](https://www.nuget.org/packages/BetterResult) [![BuildStatus](https://github.com/cds-git/BetterResult/actions/workflows/dotnet.yml/badge.svg)](https://github.com/cds-git/BetterResult/actions/workflows/dotnet.yml)

A small Result type for C#. Instead of throwing for expected failures, a method returns a
`Result<T>` that holds either a value or an `Error`, so the failure path shows up in the signature
and the compiler keeps you honest about handling it.

## Table of Contents

- [Installation](#installation)
- [Why the Result pattern](#why-the-result-pattern)
- [Usage](#usage)
  - [Result&lt;T&gt;](#resultt)
  - [Operations without a return value](#operations-without-a-return-value)
- [Operations](#operations)
  - [Bind](#bind)
  - [Map](#map)
  - [MapError](#maperror)
  - [Tap](#tap)
  - [TapError](#taperror)
  - [TapAlways](#tapalways)
  - [Match](#match)
  - [Try](#try)
  - [Ensure](#ensure)
  - [Recover](#recover)
  - [Sequence](#sequence)
  - [Traverse](#traverse)
  - [Partition](#partition)
  - [Combine](#combine)
  - [Zip](#zip)
- [Error](#error)
- [Credits](#credits)

## Installation

```bash
dotnet add package BetterResult
```

Or add the reference directly:

```xml
<PackageReference Include="BetterResult" Version="x.x.x" />
```

Replace `x.x.x` with the latest version on [NuGet](https://www.nuget.org/packages/BetterResult).

Targets `netstandard2.0` and `net10.0`.

## Why the Result pattern

Exceptions hide the failure paths. Nothing in the signature tells the caller what can go wrong:

```csharp
public User GetUser(int id)
{
    var user = database.FindUser(id);      // may throw SqlException
    if (user is null)
        throw new UserNotFoundException(); // may throw
    ValidateUser(user);                    // may throw ValidationException
    return user;
}

var user = GetUser(123); // no idea what can come back at you
```

With a Result the failure is part of the return type, so the caller has to deal with it:

```csharp
public Result<User> GetUser(int id) =>
    FindUserInDatabase(id)
        .Ensure(IsValid, Error.Validation("INVALID_USER", "User failed validation"))
        .Tap(user => logger.LogSuccess(user));

Result<User> user = GetUser(123); // success or Error, nothing else
```

That buys you three things: failures compose along a pipeline instead of unwinding the stack,
errors are plain data that is easy to assert on in tests, and expected failures cost nothing at
runtime. Keep exceptions for the genuinely exceptional.

## Usage

### `Result<T>`

Holds either a value of type `T` or an `Error`.

```csharp
// Implicit conversions
Result<int> success = 42;
Result<int> failure = Error.Failure("E001", "Invalid input");

// Explicit factories
Result<int> explicitSuccess = Result.Success(100);
Result<int> explicitFailure = Result.Failure<int>(Error.NotFound("E404", "User not found"));
```

Read the value once you know which side you are on:

```csharp
Result<int> age = CalculateAge(birthDate);

if (age.IsSuccess)
    Console.WriteLine($"Age: {age.Value}");
else
    Console.WriteLine($"Error: {age.Error.Message}");
```

`Value` throws if the result is a failure and `Error` throws if it is a success, so check
`IsSuccess` or `IsFailure` first. Most of the time you do not need to: chain the operations below
and end with `Match`.

```csharp
string description = CalculateAge(birthDate)
    .Map(age => age >= 18 ? "Adult" : "Minor")
    .Match(
        category => $"Person is classified as: {category}",
        error => $"Could not determine age: {error.Message}");
```

> **Note:** `Result<Error>`, where the success type *is* `Error`, cannot use the implicit
> conversions. Both operators collapse to the same signature and the compiler reports `CS0457`.
> Use `Result<Error>.Success(...)` and `Result<Error>.Failure(...)` instead.

### Operations without a return value

Deleting a record or sending an email either works or fails, but has nothing to return. Use
`Result<NoValue>` for those.

```csharp
public Result<NoValue> DeleteUser(int userId)
{
    if (userId <= 0)
        return Error.Validation("INVALID_ID", "User ID must be positive");

    if (!UserExists(userId))
        return Error.NotFound("USER_NOT_FOUND", $"User {userId} does not exist");

    database.Delete(userId);
    return NoValue.Instance;
}
```

It composes with valued results like anything else. Use `_` in lambdas where the `NoValue` does not
matter:

```csharp
string outcome = await ValidatePermissionsAsync(currentUser, targetUserId)  // Task<Result<NoValue>>
    .BindAsync(_ => DeleteUser(targetUserId))                              // Result<NoValue>
    .TapAsync(_ => auditService.LogDeletion(targetUserId))
    .MatchAsync(
        _ => "User deleted",
        error => $"Delete failed: {error.Message}");
```

## Operations

Every operation has sync and async variants. The ones that work on the value (`Bind`, `Map`, `Tap`,
`Ensure`, `Try`, `Zip`, `Combine`) short-circuit: once a result is a failure their callback is
skipped and the error falls through to the end of the chain. The ones that exist to deal with
failure (`TapError`, `MapError`, `Recover`, `Match`) run on exactly the branch you would expect, and
`TapAlways` runs on both.

On naming, use `XAsync` when the callback is async. The operations that hang off a `Result<T>` also
take `XAsync` when you chain from a `Task<Result<T>>`, so a plain sync lambda still works after
`await GetUserAsync(id)`. `Zip`, `Sequence`, `Traverse` and `Partition` are the exceptions: they
keep their plain name when only the source is a task.

### Bind

Chains an operation that can itself fail.

```csharp
Result<string> ProcessUser(int userId) =>
    GetUser(userId)             // Result<User>
        .Bind(ValidateUser)     // Result<NoValue>
        .Bind(_ => GetUser(userId))
        .Bind(FormatUserData);  // Result<string>

await GetUserAsync(userId)
    .BindAsync(user => ApplyDefaults(user))   // sync callback
    .BindAsync(user => SaveUserAsync(user));  // async callback
```

Each step gets the unwrapped value from the one before. The first failure ends the chain.

### Map

Transforms the value with a function that cannot fail.

```csharp
Result<UserDto> dto = GetUser(userId)
    .Map(user => user.Name)
    .Map(name => name.Trim().ToUpper())
    .Map(name => new UserDto { Name = name });

await GetUserAsync(userId)
    .MapAsync(user => FormatNameAsync(user.Name));
```

`Map` takes a plain value and returns a plain value. Use `Bind` when the function returns a
`Result<T>`, otherwise you end up with `Result<Result<T>>`.

### MapError

Rewrites the error. The result stays a failure.

```csharp
Result<Data> data = LoadData()
    .MapError(error => Error.Validation("LOAD_001", $"Failed to load: {error.Message}"));

// Add context but keep code, type and metadata
Result<Order> order = PlaceOrder(request)
    .MapError(error => error.WithMessage("Checkout failed"));
```

Use it to re-code an external error into your own code space, re-categorise it, or add context. It
cannot turn a failure into a success. That is [`Recover`](#recover).

### Tap

Runs a side effect on success and returns the result untouched.

```csharp
var result = GetUser(userId)
    .Tap(user => logger.LogInfo($"Retrieved user: {user.Name}"))
    .Map(user => user.Email);

await LoadExpensiveDataAsync()
    .TapAsync(data => cache.SetAsync(cacheKey, data));
```

### TapError

The same, on the failure branch. The error is preserved for whoever handles it downstream.

```csharp
var result = GetUser(userId)
    .TapError(error => logger.LogError($"Failed to get user {userId}: {error.Message}"))
    .TapError(error => metrics.IncrementCounter("user.fetch.errors"));

await UploadFileAsync(file)
    .TapErrorAsync(async error =>
    {
        if (error.Code == "QUOTA_EXCEEDED")
            await cleanupService.FreeSpace();
    });
```

### TapAlways

Runs a side effect on both branches. Reach for it when the two branches do the *same* thing, so the
closure is not duplicated across `Tap` and `TapError` and cannot drift.

```csharp
var stopwatch = Stopwatch.StartNew();

var result = await ProcessOrderAsync(order)
    .TapAlwaysAsync(_ => stopwatch.Stop())
    .TapAsync(o => logger.LogInfo($"Order {o.Id} placed in {stopwatch.Elapsed}"))
    .TapErrorAsync(e => logger.LogError($"Order failed after {stopwatch.Elapsed}: {e.Message}"));

await ChargeCardAsync(payment)
    .TapAlwaysAsync(r => metrics.IncrementCounter(
        "payments.attempted",
        tags: new() { ["outcome"] = r.IsSuccess ? "success" : "failure" }));
```

Two things to know:

- The action gets the whole `Result<T>`, not the value. Check `IsSuccess` or `IsFailure` before
  reading `Value` or `Error`.
- It runs at its position in the chain, and only once a result exists. If an earlier step throws,
  the exception unwinds straight past it:

  ```csharp
  ThrowingStep()                  // throws, so there is no Result
      .TapAlways(_ => cleanup()); // never runs
  ```

  Use `using` or `try`/`finally` for resource cleanup. `TapAlways` is for instrumentation.

### Match

Collapses the result into a plain value. Both handlers must return the same type, and exactly one
of them runs.

```csharp
string message = GetUser(userId).Match(
    user => $"Welcome, {user.Name}!",
    error => $"Error: {error.Message}");

IActionResult response = ProcessRequest(request).Match<IActionResult>(
    data => Ok(data),
    error => error.Type switch
    {
        ErrorType.Validation => BadRequest(error.Message),
        ErrorType.NotFound => NotFound(error.Message),
        ErrorType.Unauthorized => Unauthorized(error.Message),
        ErrorType.Forbidden => Forbid(error.Message),
        _ => StatusCode(500, "Internal server error")
    });
```

The explicit `<IActionResult>` is needed whenever the two handlers return different concrete types.
Without it there is nothing for the compiler to infer `U` from.

### Try

Wraps code that throws, so third-party and BCL APIs can join a pipeline.

```csharp
Result<int> parsed = Result<string>.Success("  123  ")
    .Map(s => s.Trim())
    .Try(s => int.Parse(s))   // FormatException becomes an Error
    .Map(x => x * 2);

Result<User> user = Result<string>.Success(json)
    .Try(
        json => JsonSerializer.Deserialize<User>(json),
        ex => Error.Validation("INVALID_JSON", $"Failed to parse JSON: {ex.Message}"));
```

- The default mapping produces `Error.Unexpected("EXCEPTION", ex.Message)` with `ExceptionType` in
  the metadata. **Stack traces are left out on purpose.** They leak file paths, line numbers and
  the internal call hierarchy, which is a CWE-209 disclosure risk once an error crosses a trust
  boundary. Add them yourself through the `errorMapper` overload if you need them for an internal
  log:

  ```csharp
  .Try(x => int.Parse(x), ex => Error.Unexpected("EXCEPTION", ex.Message)
      .WithMetadata("StackTrace", ex.StackTrace ?? string.Empty))
  ```

- `OperationCanceledException` and `TaskCanceledException` are not caught, so cooperative
  cancellation keeps working. Wrap the pipeline in your own `try`/`catch` if you want cancellation
  as a Result.
- If your `errorMapper` returns `default(Error)`, the default mapping is used instead. `Try` never
  throws.

### Ensure

Turns a success into a failure when a predicate rejects the value.

```csharp
Result<Order> order = GetOrder(orderId)
    .Ensure(o => o.Items.Any(), Error.Validation("EMPTY_ORDER", "Order must contain at least one item"))
    .Ensure(o => o.Total > 0, Error.Validation("INVALID_TOTAL", "Order total must be positive"));

// The error factory overload sees the value, for messages that need context
Result<User> user = GetUser(userId)
    .Ensure(u => u.Age >= 18, u => Error.Validation("UNDERAGE", $"{u.Name} is only {u.Age}"));

Result<Order> stocked = await GetOrder(orderId)
    .EnsureAsync(
        async o => await inventoryService.HasStock(o.Items),
        Error.Conflict("OUT_OF_STOCK", "Insufficient inventory for order"));
```

It stops at the first predicate that fails.

### Recover

Turns a failure back into a success.

```csharp
// Any failure
Result<Config> config = LoadConfig()
    .Recover(error => LoadDefaultConfig());

// A single error type, with a fallback value
Result<User> user = GetUser(userId)
    .Recover(ErrorType.NotFound, User.Guest);

// Several error types, one handler
Result<Data> data = await FetchDataAsync()
    .RecoverAsync(
        [ErrorType.Timeout, ErrorType.Unavailable],
        async error => await FetchFromBackupAsync());

// A custom predicate
Result<Response> response = await SendRequestAsync(request)
    .RecoverAsync(
        error => error.Type == ErrorType.Timeout && error.GetMetadata<int>("AttemptNumber") < 3,
        async error => await RetryRequestAsync(request));

// Layered fallbacks
Result<ImageData> image = LoadHighResImage(imageId)
    .Recover(ErrorType.NotFound, _ => LoadMediumResImage(imageId))
    .Recover(ErrorType.NotFound, _ => LoadThumbnail(imageId))
    .Recover(ErrorType.NotFound, ImageData.Placeholder);
```

The condition can be omitted, a single `ErrorType`, an `IEnumerable<ErrorType>`, or a
`Func<Error, bool>`. The recovery itself may return another failure, which is what makes layered
fallbacks work.

**Recover or MapError?** `MapError` relabels an error and stays a failure. `Recover` handles the
failure and can get back to a value. Neither changes the value type: to recover into a different
`T`, transform the success side with `Map` or `Bind` first.

### Sequence

Aggregates `IEnumerable<Result<T>>` into `Result<IReadOnlyList<T>>`. All or nothing.

```csharp
Result<IReadOnlyList<int>> allValid = new[]
{
    ValidateAge(user.Age),
    ValidateEmail(user.Email),
    ValidatePhone(user.Phone)
}.Sequence();

Result<IReadOnlyList<Data>> allData = await urls
    .Select(url => FetchDataAsync(url))
    .SequenceAsync();
```

It stops at the first failure and returns that error. Use [`Partition`](#partition) if you need
every item processed.

### Traverse

`Select` and `Sequence` in one step: apply a function that returns a `Result<U>` to each element and
collect the values.

```csharp
Result<IReadOnlyList<User>> users = userIds.Traverse(id => GetUser(id));

Result<IReadOnlyList<Email>> emails = addresses.Traverse(Email.TryCreate);

Result<IReadOnlyList<Product>> products = await productIds
    .TraverseAsync(id => FetchProductAsync(id));
```

Also stops at the first failure. Use `Sequence` when you already hold results, `Traverse` when you
hold the inputs.

### Partition

Processes everything and hands back both sides.

```csharp
var (imported, failed) = userIds.Select(id => ImportUser(id)).Partition();

Console.WriteLine($"Imported {imported.Count}, failed {failed.Count}");

foreach (var user in imported)
    await SendWelcomeEmail(user);

foreach (var error in failed)
    logger.LogError($"Import failed: {error.Code} - {error.Message}");
```

Returns `(IReadOnlyList<T> Successes, IReadOnlyList<Error> Failures)`. Unlike `Sequence` and
`Traverse` it never short-circuits, so use it for bulk imports, batch jobs, and anywhere partial
success is a real outcome.

### Combine

Combines 2 to 8 independent results, with or without a selector.

```csharp
// Without a selector you get a tuple
Result<(User, Settings)> combined = Result.Combine(GetUser(userId), GetSettings(userId));

// With one, the values go straight into it
Result<Dashboard> dashboard = Result.Combine(
    GetUser(userId),
    GetSettings(userId),
    GetPermissions(userId),
    (u, s, p) => new Dashboard(u, s, p));

// Parallel work: start the tasks first, then combine
var userTask = GetUserAsync(userId);
var settingsTask = GetSettingsAsync(userId);
await Task.WhenAll(userTask, settingsTask);

var profile = Result.Combine(await userTask, await settingsTask, (u, s) => new Profile(u, s));

// CombineAsync takes an async selector
var report = await Result.CombineAsync(
    FetchSalesData(),
    FetchInventoryData(),
    async (sales, inventory) => await BuildReportAsync(sales, inventory));
```

The arguments are evaluated at the call site, so every result is computed before `Combine` sees
them. It returns the first error it finds and drops the rest.

### Zip

`Combine` for two results, in fluent form.

```csharp
var profile = GetUser(userId)
    .Zip(GetSettings(userId), (user, settings) => new Profile(user, settings));

// Chain to build up progressively
var dashboard = GetUser(userId)
    .Zip(GetSettings(userId), (u, s) => (User: u, Settings: s))
    .Zip(GetPermissions(userId), (us, p) => new Dashboard(us.User, us.Settings, p));

// Without a selector you get a tuple
Result<(string, string)> name = GetFirstName().Zip(GetLastName());

var result = await GetUserAsync(userId)
    .ZipAsync(GetSettingsAsync(userId), async (u, s) =>
    {
        await LogProfileCreation(u.Id);
        return new Profile(u, s);
    });
```

Use `Zip` to chain, `Result.Combine` when you have three or more to combine at once.

## Error

A readonly record struct with four members:

| Member | Description |
| --- | --- |
| `ErrorType Type` | The category. Drives `Recover`'s error-type overloads. |
| `string Code` | Your code for the failure, such as an HTTP status or a localization key. |
| `string Message` | The description. |
| `IReadOnlyDictionary<string, object>? Metadata` | Optional context, or `null`. |

### Factories

```csharp
var failure      = Error.Failure("E001", "General failure");
var unexpected   = Error.Unexpected("E100", "Something went wrong");
var validation   = Error.Validation("E102", "Invalid input");
var notFound     = Error.NotFound("E404", "Item not found");
var conflict     = Error.Conflict("E409", "Conflict detected");
var unauthorized = Error.Unauthorized("E401", "Not authenticated");
var forbidden    = Error.Forbidden("E403", "Insufficient permission");
var unavailable  = Error.Unavailable("E503", "Service unavailable");
var timeout      = Error.Timeout("E408", "Operation timed out");

// Any type, when none of the above fit the name you want
var custom = Error.Create(ErrorType.Validation, "E123", "Custom validation error");
```

#### Unauthorized or Forbidden?

Easy to mix up, and worth getting right because `Recover` dispatches on `ErrorType`.

- `Unauthorized`: the caller is not authenticated. Credentials are missing or invalid, so
  re-authenticating may help.
- `Forbidden`: the caller is authenticated but lacks permission. Re-authenticating changes nothing.

```csharp
// Only a real authentication failure triggers a token refresh.
// A Forbidden error flows straight through instead of retrying pointlessly.
var profile = await FetchProfile(token)
    .RecoverAsync(ErrorType.Unauthorized, _ => RefreshTokenAndRetry());
```

> **Note:** `ErrorType.Forbidden` sits last in the enum so the numeric values of the older members
> stay stable for anyone storing or transmitting `ErrorType` as an integer.

### Adding context

Every extension returns a new `Error`.

```csharp
var error = Error.Failure("E001", "Base error");

error.WithMessage("Additional context");    // "Additional context: Base error"
error.WithCode("E002");                     // re-code, keep the rest
error.WithType(ErrorType.Validation);       // re-categorise, keep the rest
error.WithMetadata("RetryCount", 3);        // add or overwrite one key
error.WithMetadata(TimeSpan.FromSeconds(30)); // keyed by the type's full name
error.WithMetadata(new Dictionary<string, object> { ["UserId"] = 42 }); // merge a dictionary
```

Read it back with `GetMetadata<T>(key)`, which throws `InvalidCastException` on a type mismatch, or
`GetMetadata<T>()`, which returns the first value of that type. Both return `default` when there is
nothing to find.

### Equality

`Error` uses the default record struct equality, so `Type`, `Code` and `Message` compare by value
but `Metadata` compares by reference. Two errors built separately with no metadata are equal. Two
carrying metadata are not, even if you pass the same dictionary to both: the constructor copies it,
so each error ends up with its own instance. Compare `Type`, `Code` and `Message` directly when you
want identity.

