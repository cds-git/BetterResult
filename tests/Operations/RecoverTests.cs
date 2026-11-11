namespace BetterResult.Tests;

public class RecoverTests
{
    [Fact]
    public void Recover_Unconditional_Should_PreserveSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);
        var called = false;

        // Act
        var recovered = result.Recover(err => { called = true; return Result<int>.Success(99); });

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(42);
        called.Should().BeFalse();
    }

    [Fact]
    public void Recover_Unconditional_Should_Recover_OnAnyFailure()
    {
        // Arrange
        var result = Result<int>.Failure(Error.Validation("V", "invalid"));

        // Act
        var recovered = result.Recover(err => Result<int>.Success(99));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public void Recover_Unconditional_Should_ReturnRecoveryError_When_RecoveryItselfFails()
    {
        // Arrange
        var recoveryError = Error.Unavailable("UNAVAILABLE", "Service unavailable");
        var result = Result<int>.Failure(Error.NotFound("NOT_FOUND", "missing"));

        // Act
        var recovered = result.Recover(err => Result<int>.Failure(recoveryError));

        // Assert
        recovered.IsFailure.Should().BeTrue();
        recovered.Error.Should().Be(recoveryError);
    }

    [Fact]
    public async Task RecoverAsync_Unconditional_Should_Recover_OnAnyFailure()
    {
        // Arrange
        var result = Result<int>.Failure(Error.Timeout("T", "timed out"));

        // Act
        var recovered = await result.RecoverAsync(err => Task.FromResult(Result<int>.Success(99)));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public async Task RecoverAsync_TaskResult_Unconditional_SyncRecovery_Should_Recover()
    {
        // Arrange
        var resultTask = Task.FromResult(Result<int>.Failure(Error.Conflict("C", "conflict")));

        // Act
        var recovered = await resultTask.RecoverAsync(err => Result<int>.Success(99));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public async Task RecoverAsync_TaskResult_Unconditional_AsyncRecovery_Should_Recover()
    {
        // Arrange
        var resultTask = Task.FromResult(Result<int>.Failure(Error.Conflict("C", "conflict")));

        // Act
        var recovered = await resultTask.RecoverAsync(err => Task.FromResult(Result<int>.Success(99)));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public void Recover_WithPredicate_Should_PreserveSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var recovered = result.Recover(err => err.Code == "NOT_FOUND", err => Result<int>.Success(99));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(42);
    }

    [Fact]
    public void Recover_WithPredicate_Should_Recover_When_PredicateIsTrue()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "User not found");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = result.Recover(err => err.Code == "NOT_FOUND", err => Result<int>.Success(99));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public void Recover_WithPredicate_Should_PropagateError_When_PredicateIsFalse()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "User not found");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = result.Recover(err => err.Code == "INVALID", err => Result<int>.Success(99));

        // Assert
        recovered.IsFailure.Should().BeTrue();
        recovered.Error.Should().Be(error);
    }

    [Fact]
    public async Task RecoverAsync_WithPredicate_Should_Recover_When_PredicateIsTrue()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "User not found");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = await result.RecoverAsync(
            err => err.Code == "NOT_FOUND",
            err => Task.FromResult(Result<int>.Success(99)));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public async Task RecoverAsync_TaskResult_WithPredicate_Should_Recover()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "User not found");
        var resultTask = Task.FromResult(Result<int>.Failure(error));

        // Act
        var recovered = await resultTask.RecoverAsync(err => err.Code == "NOT_FOUND", err => Result<int>.Success(99));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public async Task RecoverAsync_TaskResult_AsyncRecoveryWithPredicate_Should_Recover()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "User not found");
        var resultTask = Task.FromResult(Result<int>.Failure(error));

        // Act
        var recovered = await resultTask.RecoverAsync(
            err => err.Code == "NOT_FOUND",
            err => Task.FromResult(Result<int>.Success(99)));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public void Recover_Should_ChainMultipleRecoveries()
    {
        // Arrange
        var error = Error.Timeout("TIMEOUT", "Request timed out");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = result
            .Recover(e => e.Type == ErrorType.NotFound, _ => 1)
            .Recover(e => e.Type == ErrorType.Timeout, _ => 2)
            .Recover(e => e.Type == ErrorType.Validation, _ => 3);

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(2);
    }

    [Fact]
    public void Recover_Should_ReturnRecoveryError_When_RecoveryFails()
    {
        // Arrange
        var originalError = Error.NotFound("NOT_FOUND", "User not found");
        var recoveryError = Error.Unavailable("UNAVAILABLE", "Service unavailable");
        var result = Result<int>.Failure(originalError);

        // Act
        var recovered = result.Recover(e => e.Type == ErrorType.NotFound, _ => Result<int>.Failure(recoveryError));

        // Assert
        recovered.IsFailure.Should().BeTrue();
        recovered.Error.Should().Be(recoveryError);
    }

    [Fact]
    public void Recover_Should_WorkInPipeline_When_ChainedWithOtherOperations()
    {
        // Arrange
        var result = Result<string>.Success("42");

        // Act
        var final = result
            .Try(x => int.Parse(x))
            .Map(x => x * 2)
            .Recover(e => e.Type == ErrorType.Unexpected, _ => 0)
            .Match(
                success => $"Result: {success}",
                error => $"Error: {error.Message}"
            );

        // Assert
        final.Should().Be("Result: 84");
    }

    [Fact]
    public void Recover_Should_HandleParsingError_When_ChainedWithTry()
    {
        // Arrange
        var result = Result<string>.Success("invalid");

        // Act
        var final = result
            .Try(x => int.Parse(x))
            .Recover(e => e.Type == ErrorType.Unexpected, _ => 0)
            .Map(x => x * 2)
            .Match(
                success => $"Result: {success}",
                error => $"Error: {error.Message}"
            );

        // Assert
        final.Should().Be("Result: 0");
    }

    // Multi-error-type overloads

    [Fact]
    public void Recover_WithType_Should_Recover_When_ErrorIsThatType()
    {
        Result<string> result = new UserNotFound(7);

        var recovered = result.Recover((UserNotFound e) => $"guest {e.UserId}");

        recovered.Value.Should().Be("guest 7");
    }

    [Fact]
    public void Recover_WithType_Should_PropagateError_When_ErrorIsAnotherType()
    {
        var error = Error.Conflict("CONFLICT", "Conflict");
        Result<string> result = error;

        var recovered = result.Recover((UserNotFound e) => "guest");

        recovered.Error.Should().Be(error);
    }

    [Fact]
    public void Recover_WithType_Should_PreserveSuccess_When_ResultIsSuccess()
    {
        Result<string> result = "ann";

        var recovered = result.Recover((UserNotFound e) => "guest");

        recovered.Value.Should().Be("ann");
    }

    [Fact]
    public void Recover_WithTypeArgument_Should_Recover_When_ErrorIsThatType()
    {
        Result<string> result = new UserNotFound(7);

        var recovered = result.Recover<UserNotFound>(e => $"guest {e.UserId}");

        recovered.Value.Should().Be("guest 7");
    }

    [Fact]
    public void Recover_WithType_Should_Recover_When_ErrorIsAList()
    {
        Result<string> result = Error.Validation("A", "a").Combine(Error.Validation("B", "b"));

        var recovered = result.Recover((ErrorList e) => $"{e.Errors.Count} problems");

        recovered.Value.Should().Be("2 problems");
    }

    [Fact]
    public async Task RecoverAsync_WithType_Should_Recover_When_ErrorIsThatType()
    {
        Result<string> result = new UserNotFound(7);

        var recovered = await result.RecoverAsync(async (UserNotFound e) =>
        {
            await Task.Yield();
            return $"guest {e.UserId}";
        });

        recovered.Value.Should().Be("guest 7");
    }

    [Fact]
    public async Task RecoverAsync_TaskResult_WithType_Should_Recover_When_RecoveryIsSync()
    {
        var resultTask = Task.FromResult<Result<string>>(new UserNotFound(7));

        var recovered = await resultTask.RecoverAsync((UserNotFound e) => $"guest {e.UserId}");

        recovered.Value.Should().Be("guest 7");
    }

    [Fact]
    public async Task RecoverAsync_TaskResult_WithType_Should_Recover_When_RecoveryIsAsync()
    {
        var resultTask = Task.FromResult<Result<string>>(new UserNotFound(7));

        var recovered = await resultTask.RecoverAsync(async (UserNotFound e) =>
        {
            await Task.Yield();
            return $"guest {e.UserId}";
        });

        recovered.Value.Should().Be("guest 7");
    }

    [Fact]
    public async Task RecoverAsync_TaskResult_WithType_Should_PropagateError_When_ErrorIsAnotherType()
    {
        var error = Error.Conflict("CONFLICT", "Conflict");
        var resultTask = Task.FromResult<Result<string>>(error);

        var recovered = await resultTask.RecoverAsync((UserNotFound e) => "guest");

        recovered.Error.Should().Be(error);
    }

    private sealed record UserNotFound(int UserId)
        : Error(ErrorType.NotFound, "USER_NOT_FOUND", $"User {UserId} was not found");
}
