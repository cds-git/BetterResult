namespace BetterResult.Tests;

public class EnsureTests
{
    [Fact]
    public void Ensure_Should_ReturnOriginalResult_When_PredicateIsTrue()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var ensured = result.Ensure(x => x > 0, Error.Validation("NEGATIVE", "Value must be positive"));

        // Assert
        ensured.IsSuccess.Should().BeTrue();
        ensured.Value.Should().Be(42);
    }

    [Fact]
    public void Ensure_Should_ReturnFailure_When_PredicateIsFalse()
    {
        // Arrange
        var result = Result<int>.Success(-5);
        var error = Error.Validation("NEGATIVE", "Value must be positive");

        // Act
        var ensured = result.Ensure(x => x > 0, error);

        // Assert
        ensured.IsFailure.Should().BeTrue();
        ensured.Error.Should().Be(error);
    }

    [Fact]
    public void Ensure_Should_PropagateError_When_ResultIsFailure()
    {
        // Arrange
        var originalError = Error.NotFound("NOT_FOUND", "User not found");
        var result = Result<int>.Failure(originalError);

        // Act
        var ensured = result.Ensure(x => x > 0, Error.Validation("NEGATIVE", "Value must be positive"));

        // Assert
        ensured.IsFailure.Should().BeTrue();
        ensured.Error.Should().Be(originalError);
    }

    [Fact]
    public void Ensure_WithErrorFactory_Should_ReturnOriginalResult_When_PredicateIsTrue()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var ensured = result.Ensure(x => x > 0, x => Error.Validation("NEGATIVE", $"Value {x} must be positive"));

        // Assert
        ensured.IsSuccess.Should().BeTrue();
        ensured.Value.Should().Be(42);
    }

    [Fact]
    public void Ensure_WithErrorFactory_Should_ReturnFailure_When_PredicateIsFalse()
    {
        // Arrange
        var result = Result<int>.Success(-5);

        // Act
        var ensured = result.Ensure(x => x > 0, x => Error.Validation("NEGATIVE", $"Value {x} must be positive"));

        // Assert
        ensured.IsFailure.Should().BeTrue();
        ensured.Error.Code.Should().Be("NEGATIVE");
        ensured.Error.Message.Should().Be("Value -5 must be positive");
    }

    [Fact]
    public void Ensure_WithErrorFactory_Should_PropagateError_When_ResultIsFailure()
    {
        // Arrange
        var originalError = Error.NotFound("NOT_FOUND", "User not found");
        var result = Result<int>.Failure(originalError);

        // Act
        var ensured = result.Ensure(x => x > 0, x => Error.Validation("NEGATIVE", $"Value {x} must be positive"));

        // Assert
        ensured.IsFailure.Should().BeTrue();
        ensured.Error.Should().Be(originalError);
    }

    [Fact]
    public void Ensure_Should_ChainMultipleValidations()
    {
        // Arrange
        var result = Result<int>.Success(25);

        // Act
        var ensured = result
            .Ensure(x => x >= 0, Error.Validation("NEGATIVE", "Age cannot be negative"))
            .Ensure(x => x <= 150, Error.Validation("TOO_OLD", "Age unrealistic"))
            .Ensure(x => x >= 18, Error.Validation("UNDERAGE", "Must be 18 or older"));

        // Assert
        ensured.IsSuccess.Should().BeTrue();
        ensured.Value.Should().Be(25);
    }

    [Fact]
    public void Ensure_Should_ShortCircuitOnFirstFailure()
    {
        // Arrange
        var result = Result<int>.Success(15);

        // Act
        var ensured = result
            .Ensure(x => x >= 0, Error.Validation("NEGATIVE", "Age cannot be negative"))
            .Ensure(x => x >= 18, Error.Validation("UNDERAGE", "Must be 18 or older"))
            .Ensure(x => x <= 150, Error.Validation("TOO_OLD", "Age unrealistic"));

        // Assert
        ensured.IsFailure.Should().BeTrue();
        ensured.Error.Code.Should().Be("UNDERAGE");
    }

    [Fact]
    public async Task EnsureAsync_Should_ReturnOriginalResult_When_PredicateIsTrue()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var ensured = await result.EnsureAsync(
            async x =>
            {
                await Task.Delay(1);
                return x > 0;
            },
            Error.Validation("NEGATIVE", "Value must be positive")
        );

        // Assert
        ensured.IsSuccess.Should().BeTrue();
        ensured.Value.Should().Be(42);
    }

    [Fact]
    public async Task EnsureAsync_Should_ReturnFailure_When_PredicateIsFalse()
    {
        // Arrange
        var result = Result<int>.Success(-5);
        var error = Error.Validation("NEGATIVE", "Value must be positive");

        // Act
        var ensured = await result.EnsureAsync(
            async x =>
            {
                await Task.Delay(1);
                return x > 0;
            },
            error
        );

        // Assert
        ensured.IsFailure.Should().BeTrue();
        ensured.Error.Should().Be(error);
    }

    [Fact]
    public async Task EnsureAsync_Should_PropagateError_When_ResultIsFailure()
    {
        // Arrange
        var originalError = Error.NotFound("NOT_FOUND", "User not found");
        var result = Result<int>.Failure(originalError);

        // Act
        var ensured = await result.EnsureAsync(
            async x =>
            {
                await Task.Delay(1);
                return x > 0;
            },
            Error.Validation("NEGATIVE", "Value must be positive")
        );

        // Assert
        ensured.IsFailure.Should().BeTrue();
        ensured.Error.Should().Be(originalError);
    }

    [Fact]
    public async Task EnsureAsync_WithErrorFactory_Should_ReturnFailure_When_PredicateIsFalse()
    {
        // Arrange
        var result = Result<int>.Success(-5);

        // Act
        var ensured = await result.EnsureAsync(
            async x =>
            {
                await Task.Delay(1);
                return x > 0;
            },
            x => Error.Validation("NEGATIVE", $"Value {x} must be positive")
        );

        // Assert
        ensured.IsFailure.Should().BeTrue();
        ensured.Error.Code.Should().Be("NEGATIVE");
        ensured.Error.Message.Should().Be("Value -5 must be positive");
    }

    [Fact]
    public async Task EnsureAsync_TaskResult_Should_ValidateWithSyncPredicate()
    {
        // Arrange
        var task = Task.FromResult(Result<int>.Success(42));

        // Act
        var result = await task.EnsureAsync(x => x > 0, Error.Validation("NEGATIVE", "Value must be positive"));

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public async Task EnsureAsync_TaskResult_Should_ReturnFailure_When_PredicateIsFalse()
    {
        // Arrange
        var task = Task.FromResult(Result<int>.Success(-5));
        var error = Error.Validation("NEGATIVE", "Value must be positive");

        // Act
        var result = await task.EnsureAsync(x => x > 0, error);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(error);
    }

    [Fact]
    public async Task EnsureAsync_TaskResult_AsyncPredicate_Should_ValidateCorrectly()
    {
        // Arrange
        var task = Task.FromResult(Result<int>.Success(42));

        // Act
        var result = await task.EnsureAsync(
            async x =>
            {
                await Task.Delay(1);
                return x > 0;
            },
            Error.Validation("NEGATIVE", "Value must be positive")
        );

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(42);
    }

    [Fact]
    public void Ensure_Should_ChainWithOtherOperations()
    {
        // Arrange
        var result = Result<string>.Success("42");

        // Act
        var final = result
            .Try(x => int.Parse(x))
            .Ensure(x => x > 0, Error.Validation("NEGATIVE", "Value must be positive"))
            .Ensure(x => x < 100, Error.Validation("TOO_LARGE", "Value must be less than 100"))
            .Map(x => x * 2)
            .Match(
                success => $"Result: {success}",
                error => $"Error: {error.Message}"
            );

        // Assert
        final.Should().Be("Result: 84");
    }

    [Fact]
    public void Ensure_Should_WorkWithComplexObjects()
    {
        // Arrange
        var user = new User { Name = "John", Age = 25, IsActive = true };
        var result = Result<User>.Success(user);

        // Act
        var ensured = result
            .Ensure(u => u.Age >= 18, u => Error.Validation("UNDERAGE", $"User {u.Name} is only {u.Age}"))
            .Ensure(u => u.IsActive, Error.Unauthorized("INACTIVE", "User is not active"))
            .Ensure(u => !string.IsNullOrEmpty(u.Name), Error.Validation("INVALID_NAME", "Name is required"));

        // Assert
        ensured.IsSuccess.Should().BeTrue();
        ensured.Value.Should().Be(user);
    }

    [Fact]
    public void Ensure_Should_ProvideDynamicErrorMessages()
    {
        // Arrange
        var user = new User { Name = "Jane", Age = 15, IsActive = true };
        var result = Result<User>.Success(user);

        // Act
        var ensured = result.Ensure(u => u.Age >= 18, u => Error.Validation("UNDERAGE", $"User {u.Name} is only {u.Age} years old"));

        // Assert
        ensured.IsFailure.Should().BeTrue();
        ensured.Error.Message.Should().Be("User Jane is only 15 years old");
    }

    private record User
    {
        public string Name { get; init; } = string.Empty;
        public int Age { get; init; }
        public bool IsActive { get; init; }
    }
}
