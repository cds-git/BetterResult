namespace BetterResult.Tests;

public class RecoverTests
{
    [Fact]
    public void Recover_WithErrorType_Should_PreserveSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var recovered = result.Recover(ErrorType.NotFound, err => Result<int>.Success(99));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(42);
    }

    [Fact]
    public void Recover_WithErrorType_Should_Recover_When_ErrorTypeMatches()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "User not found");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = result.Recover(ErrorType.NotFound, err => Result<int>.Success(99));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public void Recover_WithErrorType_Should_PropagateError_When_ErrorTypeDoesNotMatch()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "User not found");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = result.Recover(ErrorType.Validation, err => Result<int>.Success(99));

        // Assert
        recovered.IsFailure.Should().BeTrue();
        recovered.Error.Should().Be(error);
    }

    [Fact]
    public void Recover_WithErrorType_Should_PassErrorToRecoveryFunction()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "User not found");
        var result = Result<int>.Failure(error);
        Error? capturedError = null;

        // Act
        var recovered = result.Recover(ErrorType.NotFound, err =>
        {
            capturedError = err;
            return Result<int>.Success(99);
        });

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        capturedError.Should().Be(error);
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
    public void Recover_WithFallbackValue_Should_PreserveSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var recovered = result.Recover(ErrorType.NotFound, 99);

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(42);
    }

    [Fact]
    public void Recover_WithFallbackValue_Should_Recover_When_ErrorTypeMatches()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "User not found");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = result.Recover(ErrorType.NotFound, 99);

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public void Recover_WithFallbackValue_Should_PropagateError_When_ErrorTypeDoesNotMatch()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "User not found");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = result.Recover(ErrorType.Validation, 99);

        // Assert
        recovered.IsFailure.Should().BeTrue();
        recovered.Error.Should().Be(error);
    }

    [Fact]
    public void Recover_Should_ChainMultipleRecoveries()
    {
        // Arrange
        var error = Error.Timeout("TIMEOUT", "Request timed out");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = result
            .Recover(ErrorType.NotFound, 1)
            .Recover(ErrorType.Timeout, 2)
            .Recover(ErrorType.Validation, 3);

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
        var recovered = result.Recover(ErrorType.NotFound, err => Result<int>.Failure(recoveryError));

        // Assert
        recovered.IsFailure.Should().BeTrue();
        recovered.Error.Should().Be(recoveryError);
    }

    [Fact]
    public async Task RecoverAsync_WithErrorType_Should_PreserveSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var recovered = await result.RecoverAsync(ErrorType.NotFound, err => Task.FromResult(Result<int>.Success(99)));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(42);
    }

    [Fact]
    public async Task RecoverAsync_WithErrorType_Should_Recover_When_ErrorTypeMatches()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "User not found");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = await result.RecoverAsync(ErrorType.NotFound, err => Task.FromResult(Result<int>.Success(99)));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
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
    public async Task RecoverAsync_TaskResult_WithErrorType_Should_Recover()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "User not found");
        var resultTask = Task.FromResult(Result<int>.Failure(error));

        // Act
        var recovered = await resultTask.RecoverAsync(ErrorType.NotFound, err => Result<int>.Success(99));

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
    public async Task RecoverAsync_TaskResult_WithFallbackValue_Should_Recover()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "User not found");
        var resultTask = Task.FromResult(Result<int>.Failure(error));

        // Act
        var recovered = await resultTask.RecoverAsync(ErrorType.NotFound, 99);

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public async Task RecoverAsync_TaskResult_AsyncRecovery_Should_Recover()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "User not found");
        var resultTask = Task.FromResult(Result<int>.Failure(error));

        // Act
        var recovered = await resultTask.RecoverAsync(
            ErrorType.NotFound,
            err => Task.FromResult(Result<int>.Success(99)));

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
    public void Recover_Should_WorkInPipeline_When_ChainedWithOtherOperations()
    {
        // Arrange
        var result = Result<string>.Success("42");

        // Act
        var final = result
            .Try(x => int.Parse(x))
            .Map(x => x * 2)
            .Recover(ErrorType.Unexpected, 0)
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
            .Recover(ErrorType.Unexpected, 0)
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
    public void Recover_WithErrorTypes_Should_PreserveSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var recovered = result.Recover(
            [ErrorType.NotFound, ErrorType.Unauthorized],
            err => Result<int>.Success(99));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(42);
    }

    [Fact]
    public void Recover_WithErrorTypes_Should_Recover_When_ErrorTypeInSet()
    {
        // Arrange
        var error = Error.Unauthorized("UNAUTH", "Access denied");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = result.Recover(
            [ErrorType.NotFound, ErrorType.Unauthorized],
            err => Result<int>.Success(99));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public void Recover_WithErrorTypes_Should_PropagateError_When_ErrorTypeNotInSet()
    {
        // Arrange
        var error = Error.Validation("INVALID", "Bad input");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = result.Recover(
            [ErrorType.NotFound, ErrorType.Unauthorized],
            err => Result<int>.Success(99));

        // Assert
        recovered.IsFailure.Should().BeTrue();
        recovered.Error.Should().Be(error);
    }

    [Fact]
    public void Recover_WithErrorTypes_FallbackValue_Should_PreserveSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var recovered = result.Recover([ErrorType.NotFound, ErrorType.Unauthorized], 99);

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(42);
    }

    [Fact]
    public void Recover_WithErrorTypes_FallbackValue_Should_Recover_When_ErrorTypeInSet()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "Missing");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = result.Recover([ErrorType.NotFound, ErrorType.Unauthorized], 99);

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public void Recover_WithErrorTypes_FallbackValue_Should_PropagateError_When_ErrorTypeNotInSet()
    {
        // Arrange
        var error = Error.Validation("INVALID", "Bad input");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = result.Recover([ErrorType.NotFound, ErrorType.Unauthorized], 99);

        // Assert
        recovered.IsFailure.Should().BeTrue();
        recovered.Error.Should().Be(error);
    }

    [Fact]
    public void Recover_WithErrorTypes_Should_HandleSeveralErrorsWithOneHandler()
    {
        // Arrange — three failures, three different types, all retryable, one shared handler
        ErrorType[] retryable = [ErrorType.Timeout, ErrorType.Unavailable, ErrorType.Conflict];

        var timeout = Result<int>.Failure(Error.Timeout("T", "timed out")).Recover(retryable, _ => 1);
        var unavailable = Result<int>.Failure(Error.Unavailable("U", "down")).Recover(retryable, _ => 1);
        var conflict = Result<int>.Failure(Error.Conflict("C", "conflict")).Recover(retryable, _ => 1);
        var validation = Result<int>.Failure(Error.Validation("V", "invalid")).Recover(retryable, _ => 1);

        // Assert
        timeout.IsSuccess.Should().BeTrue();
        unavailable.IsSuccess.Should().BeTrue();
        conflict.IsSuccess.Should().BeTrue();
        validation.IsFailure.Should().BeTrue(); // not in the retryable set
    }

    [Fact]
    public async Task RecoverAsync_WithErrorTypes_Should_PreserveSuccess_When_ResultIsSuccess()
    {
        // Arrange
        var result = Result<int>.Success(42);

        // Act
        var recovered = await result.RecoverAsync(
            [ErrorType.NotFound, ErrorType.Unauthorized],
            err => Task.FromResult(Result<int>.Success(99)));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(42);
    }

    [Fact]
    public async Task RecoverAsync_WithErrorTypes_Should_Recover_When_ErrorTypeInSet()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "Missing");
        var result = Result<int>.Failure(error);

        // Act
        var recovered = await result.RecoverAsync(
            [ErrorType.NotFound, ErrorType.Unauthorized],
            err => Task.FromResult(Result<int>.Success(99)));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public async Task RecoverAsync_TaskResult_WithErrorTypes_Should_Recover()
    {
        // Arrange
        var error = Error.Unauthorized("UNAUTH", "denied");
        var resultTask = Task.FromResult(Result<int>.Failure(error));

        // Act
        var recovered = await resultTask.RecoverAsync(
            [ErrorType.NotFound, ErrorType.Unauthorized],
            err => Result<int>.Success(99));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public async Task RecoverAsync_TaskResult_WithErrorTypes_FallbackValue_Should_Recover()
    {
        // Arrange
        var error = Error.NotFound("NOT_FOUND", "Missing");
        var resultTask = Task.FromResult(Result<int>.Failure(error));

        // Act
        var recovered = await resultTask.RecoverAsync(
            [ErrorType.NotFound, ErrorType.Unauthorized],
            99);

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }

    [Fact]
    public async Task RecoverAsync_TaskResult_WithErrorTypes_AsyncRecovery_Should_Recover()
    {
        // Arrange
        var error = Error.Timeout("TIMEOUT", "timed out");
        var resultTask = Task.FromResult(Result<int>.Failure(error));

        // Act
        var recovered = await resultTask.RecoverAsync(
            [ErrorType.Timeout, ErrorType.Unavailable],
            err => Task.FromResult(Result<int>.Success(99)));

        // Assert
        recovered.IsSuccess.Should().BeTrue();
        recovered.Value.Should().Be(99);
    }
}
